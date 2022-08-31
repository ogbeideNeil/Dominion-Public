using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaSound : MonoBehaviour
{
    [SerializeField]
    private List<Sound> sounds;

    [SerializeField]
    private float delayBetweenSounds = 1f;

    [SerializeField]
    private GameEvent pauseAllSoundsEvent;

    [SerializeField]
    private GameEvent resumeAllSoundsEvent;

    private float timePassed;
    private float playSoundAfterTime;

    private void Awake()
    {
        UnityEvent pauseResponse = new UnityEvent();
        pauseResponse.AddListener(OnPauseAllSounds);

        GameEventListener pauseListener = gameObject.AddComponent<GameEventListener>();
        pauseListener.SetParameters(pauseAllSoundsEvent, pauseResponse);


        UnityEvent resumeResponse = new UnityEvent();
        resumeResponse.AddListener(OnResumeAllSounds);

        GameEventListener resumeListener = gameObject.AddComponent<GameEventListener>();
        resumeListener.SetParameters(resumeAllSoundsEvent, resumeResponse);
    }

    private void OnPauseAllSounds()
    {
        enabled = false;
    }

    private void OnResumeAllSounds()
    {
        enabled = true;
    }

    private void Update()
    {
        timePassed += Time.deltaTime;

        if (timePassed >= playSoundAfterTime)
        {
            timePassed = 0;
            PlaySound();
        }
    }

    private void PlaySound()
    {
        int index = Random.Range(0, sounds.Count);
        AudioSource audio = SoundManager.PlaySound(sounds[index], transform.position);

        playSoundAfterTime = audio.clip.length + delayBetweenSounds;
    }
}