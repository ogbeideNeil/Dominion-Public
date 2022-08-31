using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GD.ScriptableTypes;
using UnityEditor;
using UnityEngine;

public enum Sound
{
	Default,
	EmpireCutscene,
	CombatMusic,
	MainMenuMusic,
	NonCombatMusic,
	OphiAttack1,
	OphiAttack2,
	OphiAttack3,
	OphiBigAttack,
	OphiAirAttack,
	OphiBigSwing,
	OphiJump,
	OphiStep1,
	OphiStep2,
	OphiStep3,
	TreeLine1,
	TreeLineAttack,
	TreeLine3,
	MineLine,
	CampLine,
	EndLine1,
	EndLine2,
	EndLine3,
	BirdSong,
	Waterfall,
	OceanAmbience,
	GemSong,
	RuneSound,
	ConstantWind,
	RiverWater,
	NewForestSong,
	RuneStart,
	RiverFlow,
	Coughing,
	DoorOpen,
	Battle,
	Screams,
	Crying,
	Mining,
	OphiSkillAttack,
	BarkAndNoBite,
	OrderKnows,
	TheyreDown,
	WordForYou,
	PottedPlant,
	SummonSpirit,
	ToHelp,
	GolemStep,
	TreantStep,
	OphiGetHit,
	TreantHit1,
	TreantHit2,
	GolemRoar,
	GolemCliffJump,
	IntroCutscene,
}

public class SoundManager : MonoBehaviour, IHandleTicks
{
#pragma warning disable CS0649
    [SerializeField]
    private string soundMangerPath = "Assets/Vismount Games/Scripts/Managers/SoundManager.cs";

    [SerializeField]
    private bool generateEnums;

    [SerializeField]
    private IntReference masterVolume;

    [SerializeField]
    private IntReference effectsVolume;

    [SerializeField]
    private IntReference musicVolume;

    [SerializeField]
    private ListNamedClipVariable clips;


#pragma warning restore CS0649

    private static Dictionary<Sound, NamedClip> sounds;
    private static Dictionary<Sound, float> soundsNextPlayTime;
    private static LinkedList<PlayingSound> currentlyPlaying;
    private static float deltaTime;

    public static int MasterVolume => MasterVolumeRef.Value;
    public static int EffectsVolume => EffectsVolumeRef.Value;
    public static int MusicVolume => MusicVolumeRef.Value;

    public static float NormalizedEffectsVolume { get; private set; }
    public static float NormalizedMusicVolume { get; private set; }
    public static float NormalizedMasterVolume { get; private set; }

    private static IntReference MasterVolumeRef { get; set; }
    private static IntReference EffectsVolumeRef { get; set; }
    private static IntReference MusicVolumeRef { get; set; }

    public static AudioSource PlaySound(Sound sound, bool playOnce = false)
    {
        try
        {
            if (!sounds.ContainsKey(sound))
            {
                Debug.LogError($"SoundManager - Sounds does not contain key {sound}");
                return null;
            }

            Debug.Log("Play sound: " + sound);
            NamedClip clip = sounds[sound];
            if (!CanPlay(sound, clip, playOnce))
            {
                return null;
            }

            GameObject soundObject = new GameObject("Sound Object");
            AudioSource audio = AddAudioSource(soundObject, clip);

            audio.Play();

            currentlyPlaying.AddLast(new PlayingSound(sound, audio, !audio.loop));

            return audio;
        }
        catch (Exception e)
        {
            Debug.LogError("SoundManager - " + e);
            return null;
        }
    }

    public static AudioSource PlaySound(Sound sound, Vector3 position, bool playOnce = false)
    {
        try
        {
            if (!sounds.ContainsKey(sound))
            {
                Debug.LogError($"SoundManager - Sounds does not contain key {sound}");
                return null;
            }

            NamedClip clip = sounds[sound];
            if (!CanPlay(sound, clip, playOnce))
            {
                return null;
            }

            GameObject soundObject = new GameObject("Sound Object");
            soundObject.transform.position = position;

            AudioSource audio = AddAudioSource(soundObject, clip);
            audio.Play();

            currentlyPlaying.AddLast(new PlayingSound(sound, audio, !audio.loop));

            return audio;
        }
        catch (Exception e)
        {
            Debug.LogError("SoundManager - " + e);
            return null;
        }
    }

    public static void EndCurrentlyPlayingSounds()
    {
        var action = new Action<LinkedListNode<PlayingSound>>(node =>
        {
            if (node.Value.Audio.isPlaying)
            {
                node.Value.PlayTime = node.Value.Audio.clip.length + 10;
            }
        });

        ApplyActionToCurrentlyPlaying(action);

        var updateAction = new Action<LinkedListNode<PlayingSound>>(UpdateSoundNode);
        ApplyActionToCurrentlyPlaying(updateAction);
    }

    public static void PauseAllSounds()
    {
        var action = new Action<LinkedListNode<PlayingSound>>(node =>
        {
            node.Value.WasPlaying = node.Value.Audio.isPlaying;
            node.Value.Audio.Pause();
        });

        ApplyActionToCurrentlyPlaying(action);
    }

    public static void ResumeAllSounds()
    {
        var action = new Action<LinkedListNode<PlayingSound>>(node =>
        {
            if (node.Value.WasPlaying)
            {
                node.Value.Audio.UnPause();
            }
        });

        ApplyActionToCurrentlyPlaying(action);
    }

    public static void SetMasterVolume(int volume)
    {
        MasterVolumeRef.Variable.Value = volume;
        NormalizedMasterVolume = volume * 0.01f;

        SetEffectsVolume(EffectsVolumeRef.Value);
        SetMusicVolume(MusicVolumeRef.Value);
    }

    public static void SetEffectsVolume(int volume)
    {
        EffectsVolumeRef.Variable.Value = volume;
        NormalizedEffectsVolume = NormalizedMasterVolume * volume * 0.01f;

        var action = new Action<LinkedListNode<PlayingSound>>(node =>
        {
            if (!node.Value.Audio.loop)
            {
                node.Value.Audio.volume = sounds[node.Value.Sound].volume * NormalizedEffectsVolume;
            }
        });

        ApplyActionToCurrentlyPlaying(action);
    }

    public static void SetMusicVolume(int volume)
    {
        MusicVolumeRef.Variable.Value = volume;
        NormalizedMusicVolume = NormalizedMasterVolume * volume * 0.01f;

        var action = new Action<LinkedListNode<PlayingSound>>(node =>
        {
            if (node.Value.Audio.loop)
            {
                node.Value.Audio.volume = sounds[node.Value.Sound].volume * NormalizedMusicVolume;
            }
        });

        ApplyActionToCurrentlyPlaying(action);
    }

    private static void ApplyActionToCurrentlyPlaying(Action<LinkedListNode<PlayingSound>> action)
    {
        LinkedListNode<PlayingSound> currentNode = currentlyPlaying.First;

        while (currentNode != null)
        {
            LinkedListNode<PlayingSound> nextNode = currentNode.Next;

            if (currentNode.Value.Audio == null)
            {
                currentlyPlaying.Remove(currentNode);
            }
            else
            {
                action(currentNode);
            }

            currentNode = nextNode;
        }
    }

    private static void UpdateSoundNode(LinkedListNode<PlayingSound> node)
    {
        node.Value.UpdateTime(deltaTime);

        if (node.Value.ShouldDestroy())
        {
            Destroy(node.Value.Audio.gameObject);
            currentlyPlaying.Remove(node);
        }
    }

    private static AudioSource AddAudioSource(GameObject soundObject, NamedClip clip)
    {
        AudioSource audio = soundObject.AddComponent<AudioSource>();

        audio.clip = clip.clip;
        audio.pitch = clip.pitch;
        audio.minDistance = clip.minDistance;
        audio.maxDistance = clip.maxDistance;
        audio.loop = clip.loop;
        audio.volume = clip.volume;

        if (audio.loop)
        {
            audio.volume *= NormalizedMusicVolume;
        }
        else
        {
            audio.volume *= NormalizedEffectsVolume;
        }

        if (!clip.useDefaultSettings)
        {
            audio.mute = clip.mute;
            audio.bypassEffects = clip.bypassEffects;
            audio.bypassListenerEffects = clip.bypassListenerEffects;
            audio.playOnAwake = clip.playOnAwake;
            audio.priority = clip.priority;
            audio.panStereo = clip.stereoPan;
            audio.spatialBlend = clip.spatialBlend;
            audio.reverbZoneMix = clip.reverbZoneMix;
            audio.dopplerLevel = clip.dopplerLevel;
            audio.spread = clip.spread;
            audio.rolloffMode = clip.volumeRolloff;
        }

        return audio;
    }

    private static bool CanPlay(Sound sound, NamedClip clip, bool playOnce)
    {
        if (playOnce)
        {
            if (soundsNextPlayTime.ContainsKey(sound))
            {
                // If the clip has finished playing
                if (soundsNextPlayTime[sound] > Time.time)
                {
                    return false;
                }

                soundsNextPlayTime[sound] = Time.time + clip.clip.length;
            }
            else
            {
                soundsNextPlayTime.Add(sound, Time.time + clip.clip.length);
            }
        }

        return true;
    }

    public void HandleTick()
    {
        var action = new Action<LinkedListNode<PlayingSound>>(UpdateSoundNode);
        ApplyActionToCurrentlyPlaying(action);
        deltaTime = 0;
    }

    private void Awake()
    {
        sounds = new Dictionary<Sound, NamedClip>(clips.Count);
        soundsNextPlayTime = new Dictionary<Sound, float>(clips.Count);
        currentlyPlaying = new LinkedList<PlayingSound>();

        MasterVolumeRef = masterVolume;
        EffectsVolumeRef = effectsVolume;
        MusicVolumeRef = musicVolume;
        SetMasterVolume(masterVolume.Value);
        SetEffectsVolume(effectsVolume.Value);
        SetMusicVolume(musicVolume.Value);

        foreach (NamedClip pair in clips.List)
        {
            Sound sound = (Sound)Enum.Parse(typeof(Sound), pair.name);
            sounds.Add(sound, pair);
        }

        TimeTickSystem.Instance.RegisterListener(TimeTickSystem.TickRateMultiplierType.Eight, HandleTick);
    }

    private void Update()
    {
        deltaTime += Time.deltaTime;
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (generateEnums)
        {
            generateEnums = false;
            List<string> lines = File.ReadAllLines(soundMangerPath).ToList();

            //Find start and end of the Sound enum lines
            FindStartAndEndLines(lines, out int startLine, out int endLine);
            lines.RemoveRange(startLine, endLine - startLine);

            List<string> names = new List<string>(clips.Count);
            foreach (NamedClip namedClip in clips.List)
            {
                if (!string.IsNullOrEmpty(namedClip.name))
                {
                    names.Add($"\t{namedClip.name},");
                }
            }

            lines.InsertRange(startLine, names);
            lines.Insert(startLine, "\tDefault,");

            File.WriteAllLines(soundMangerPath, lines);

            AssetDatabase.Refresh();
        }
#endif
    }

    private void FindStartAndEndLines(List<string> lines, out int startLine, out int endLine)
    {
        startLine = -1;
        endLine = -1;

        for (int i = 0; i < lines.Count; i++)
        {
            if (startLine != -1)
            {
                if (lines[i].Contains("}"))
                {
                    endLine = i;
                    return;
                }
            }
            else if (lines[i].Contains("public enum Sound"))
            {
                startLine = i + 2;
                i++;
            }
        }
    }
}

public class PlayingSound
{
    public AudioSource Audio { get; }
    public bool DestroyOnEnd { get; }
    public Sound Sound { get; }

    public bool WasPlaying { get; set; }
    public float PlayTime { get; set; }

    public PlayingSound(Sound sound, AudioSource audio, bool destroyOnEnd)
    {
        Audio = audio;
        DestroyOnEnd = destroyOnEnd;
        Sound = sound;
    }

    public void UpdateTime(float time)
    {
        PlayTime += time;
    }

    public bool ShouldDestroy()
    {
        return DestroyOnEnd && PlayTime > Audio.clip.length + 2;
    }
}

[Serializable]
public class NamedClip
{
    public NamedClip()
    {
        volume = 1f;
        pitch = 1f;
        minDistance = 1f;
        maxDistance = 500f;
        useDefaultSettings = true;
        mute = false;
        bypassEffects = false;
        bypassListenerEffects = false;
        bypassReverbZones = false;
        playOnAwake = true;
        loop = false;
        priority = 128;
        stereoPan = 0;
        spatialBlend = 0;
        reverbZoneMix = 1;
        dopplerLevel = 1;
        spread = 0;
        volumeRolloff = AudioRolloffMode.Logarithmic;
    }

    public string name;
    public AudioClip clip;

    [Range(0, 1)] public float volume = 1f;
    [Range(-3, 3)] public float pitch = 1f;
    public float minDistance = 1f;
    public float maxDistance = 500f;
    public bool loop = false;

    public bool useDefaultSettings = true;

    [Header("Ignored if UseDefault")]
    public bool mute = false;
    public bool bypassEffects = false;
    public bool bypassListenerEffects = false;
    public bool bypassReverbZones = false;
    public bool playOnAwake = true;

    [Range(0, 256)] public int priority = 128;
    [Range(-1, 1)] public float stereoPan = 0;
    [Range(0, 256)] public float spatialBlend = 0;
    [Range(0, 1.1f)] public float reverbZoneMix = 1;

    [Range(0, 5)] public float dopplerLevel = 1;
    [Range(0, 360)] public int spread = 0;
    public AudioRolloffMode volumeRolloff = AudioRolloffMode.Logarithmic;
}
