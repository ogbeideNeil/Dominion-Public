using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    [SerializeField] private KatanaAttackDetails[] attackDetails;

    public KatanaAttackDetails[] AttackDetails { get => attackDetails; private set => attackDetails = value; }
    public int amountOfAttacks { get; protected set; }
    public float[] movementSpeed { get; protected set; }

    private void OnEnable()
    {
        amountOfAttacks = attackDetails.Length;
    }
}
