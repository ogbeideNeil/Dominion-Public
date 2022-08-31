using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyDirector : MonoBehaviour, IHandleTicks
{
    [SerializeField]
    private Transform player;

    [SerializeField]
    private Transform playerCamera;

    [SerializeField]
    private GameEvent onCombatStarted;

    [SerializeField]
    private GameEvent onCombatEnded;

    [SerializeField]
    private int maxEngagedTreants = 3;

    [SerializeField]
    private int maxEngagedGolems = 2;

    public static Transform Player { get; private set; }
    public static Transform PlayerCamera { get; private set; }

    private static GameEvent OnCombatStarted { get; set; }
    private static GameEvent OnCombatEnded { get; set; }

    private static Dictionary<ZoneObjectData, Dictionary<EnemyType, List<Enemy>>> zones;
    private static HashSet<ZoneObjectData> alertedZones;
    private static Vector3 trackPosition;
    private static bool combatStarted;

    public static void RegisterEnemy(Enemy enemy)
    {
        EnemyType type = enemy.EnemyType;
        ZoneObjectData zoneData = enemy.ZoneData;

        if (zones.ContainsKey(zoneData))
        {
            Dictionary<EnemyType, List<Enemy>> zoneEnemies = zones[zoneData];
            if (zoneEnemies.ContainsKey(type))
            {
                zoneEnemies[type].Add(enemy);
            }
            else
            {
                zoneEnemies.Add(type, new List<Enemy> { enemy });
            }
        }
        else
        {
            var zoneEnemies = new Dictionary<EnemyType, List<Enemy>>
                {{
                    enemy.EnemyType,
                    new List<Enemy> {enemy}
                }};

            zones.Add(zoneData, zoneEnemies);
        }
    }

    public static void UnRegisterEnemy(Enemy enemy)
    {
        EnemyType type = enemy.EnemyType;
        ZoneObjectData zoneData = enemy.ZoneData;

        if (zones.ContainsKey(zoneData))
        {
            Dictionary<EnemyType, List<Enemy>> zoneEnemies = zones[zoneData];
            if (zoneEnemies.ContainsKey(type))
            {
                if (zoneEnemies[type].Remove(enemy))
                {
                    if (!zoneEnemies[type].Any())
                    {
                        zoneEnemies.Remove(type);

                        if (!zoneEnemies.Any())
                        {
                            alertedZones.Remove(zoneData);
                            zones.Remove(zoneData);

                            if (!alertedZones.Any())
                            {
                                EndCombat();
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"EnemyDirector -> UnRegisterEnemy {enemy.name}, Could not find enemy in list");
                }
            }
            else
            {
                Debug.LogWarning($"EnemyDirector -> UnRegisterEnemy {enemy.name}, {type}, Could not find enemy type");
            }
        }
        else
        {
            Debug.LogWarning($"EnemyDirector -> UnRegisterEnemy {enemy.name}, {zoneData.Name}, Could not zoneData");
        }
    }

    public void HandleTick()
    {
        if (!AnyAlertedEnemies()) return;

        IEnumerable<List<Enemy>> alertedEnemies =
            alertedZones
                .Select(zoneData => zones[zoneData])
                .SelectMany(zoneEnemies => zoneEnemies.Values);

        foreach (List<Enemy> enemies in alertedEnemies)
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.TrackPlayerPosition(trackPosition);
                enemy.EngagePlayer = false;
            }

            enemies.ForEach(enemy => enemy.TrackPlayerPosition(trackPosition));
        }

        SetClosestEnemiesToEngaged(EnemyType.Treant, maxEngagedTreants);
        SetClosestEnemiesToEngaged(EnemyType.Golem, maxEngagedGolems);
    }

    private void SetClosestEnemiesToEngaged(EnemyType enemyType, int maxEngaged)
    {
        Enemy[] closestEnemies = GetClosestAlertedEnemiesToPlayer(enemyType, maxEngaged);

        foreach (Enemy enemy in closestEnemies)
        {
            enemy.EngagePlayer = true;
        }
    }

    public static void AlertZone(ZoneObjectData zoneData)
    {
        if (!AnyAlertedEnemies())
        {
            StartCombat();
        }

        alertedZones.Add(zoneData);
    }

    public static void TrackPosition(Vector3 position)
    {
        trackPosition = position;
    }

    public static bool AnyAlertedEnemies()
    {
        return alertedZones.Any();
    }

    private void Awake()
    {
        OnCombatStarted = onCombatStarted;
        OnCombatEnded = onCombatEnded;

        Player = player;
        PlayerCamera = playerCamera;
        zones = new Dictionary<ZoneObjectData, Dictionary<EnemyType, List<Enemy>>>();
        alertedZones = new HashSet<ZoneObjectData>();

        TimeTickSystem.Instance.RegisterListener(TimeTickSystem.TickRateMultiplierType.Four, HandleTick);
    }

    private static Enemy[] GetClosestAlertedEnemiesToPlayer(EnemyType type, int count)
    {
        List<Tuple<Enemy, float>> distances = new List<Tuple<Enemy, float>>();

        foreach (ZoneObjectData zoneData in alertedZones)
        {
            if (!zones[zoneData].ContainsKey(type)) return Array.Empty<Enemy>();

            List<Enemy> enemies = zones[zoneData][type];

            foreach (Enemy enemy in enemies)
            {
                distances.Add(
                    new Tuple<Enemy, float>(
                        enemy,
                        Vector3.Distance(enemy.transform.position, trackPosition)));
            }
        }

        if (!distances.Any()) return Array.Empty<Enemy>();

        distances.Sort((pairA, pairB) => pairA.Item2.CompareTo(pairB.Item2));

        if (distances.Count < count)
        {
            count = distances.Count;
        }

        return distances.GetRange(0, count).Select(pair => pair.Item1).ToArray();
    }

    private static void EndCombat()
    {
        if (!combatStarted) return;

        combatStarted = false;
        OnCombatEnded.Raise();
    }

    private static void StartCombat()
    {
        if (combatStarted) return;

        combatStarted = true;
        OnCombatStarted.Raise();
    }
}
