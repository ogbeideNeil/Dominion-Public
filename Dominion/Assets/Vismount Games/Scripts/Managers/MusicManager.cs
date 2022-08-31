using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    private Sound defaultSound;

    [SerializeField]
    private Sound combatSound;

    private static AudioSource currentlyPlaying;
    private static AudioSource defaultAudio;

    private static Sound DefaultSound { get; set; }
    private static Sound CombatSound { get; set; }

    private static float defaultAudioVolume;

    public void Awake()
    {
        DefaultSound = defaultSound;
        CombatSound = combatSound;
    }

    public static void Play(Sound sound, float timeToFade = 2f)
    {
        AudioSource audio;
        float maxVolume;

        if (defaultAudio == null)
        {
            defaultAudio = SoundManager.PlaySound(DefaultSound);
            currentlyPlaying = defaultAudio;
            defaultAudioVolume = defaultAudio.volume;
        }
        
        if (currentlyPlaying != defaultAudio && sound == DefaultSound)
        {
            audio = defaultAudio;
            audio.Play();
            maxVolume = defaultAudioVolume;
        }
        else
        {
            audio = SoundManager.PlaySound(sound);
            maxVolume = audio.volume;
        }

        
        audio.volume = 0;

        LeanTween.value(currentlyPlaying.gameObject, currentlyPlaying.volume, 0f, timeToFade)
            .setOnUpdate(value =>
            {
                currentlyPlaying.volume = value;
                //currentlyPlaying.volume = (1 - value) * SoundManager.NormalizedMusicVolume;
            });

        LeanTween
            .value(audio.gameObject, 0f, maxVolume, timeToFade)
            .setOnUpdate(value =>
            {
                audio.volume = value;
                
            })
            .setOnComplete(() =>
            {
                if (currentlyPlaying == defaultAudio)
                {
                    currentlyPlaying.Pause();
                }
                else
                {
                    RemoveCurrentAudio();
                }

                currentlyPlaying = audio;
            });
    }

    public static void PlayDefault()
    {
        Play(DefaultSound);
    }

    public static void PlayCombat()
    {
        Play(CombatSound);
    }

    private static void RemoveCurrentAudio()
    {
        currentlyPlaying.Stop();
        Destroy(currentlyPlaying.gameObject);
    }

}
