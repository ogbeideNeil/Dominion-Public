using System;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineFreeLook playerCamera;

    [SerializeField]
    private PlayableDirector initialDirector;

    [SerializeField]
    private GameEvent onCutsceneFinish;

    private const int InactivePriority = 0;
    private const int PlayerPriority = 1;
    private const int CutscenePriority = 2;

    private PlayableDirector activeDirector;
    private DirectorData activeDirectorData;
    private int currentSoundIndex;
    private int currentEventIndex;
    private bool useSounds;
    private bool useEvents;
    private bool loop;
    private bool playing;

    private void Awake()
    {
        playerCamera.Priority = PlayerPriority;
        SetActiveDirector(initialDirector);

        initialDirector.stopped += director =>
        {
            if (loop)
            {
                director.initialTime = 0;
                director.Play();
            }
        };

        loop = true;
    }
    public void SkipCutscene()
    {
        while (useEvents)
        {
            activeDirectorData.EventTimes.List[currentEventIndex].GameEvent.Raise();
            currentEventIndex++;

            if (currentEventIndex >= activeDirectorData.EventTimes.List.Count)
            {
                useEvents = false;
            }
        }

        SoundManager.EndCurrentlyPlayingSounds();
        activeDirector.time = activeDirector.duration;
    }

    public void SetActiveDirector(DirectorData data)
    {
        activeDirectorData = data;
        SetActiveDirector(data.Director);
    }

    private void SetActiveDirector(PlayableDirector director)
    {
        director.stopped += director =>
        {
            onCutsceneFinish.Raise();
        };

        activeDirector = director;
        activeDirector.Play();
        playing = true;

        currentSoundIndex = 0;
        currentEventIndex = 0;

        if (activeDirectorData.SoundTimes != null)
        {
            useSounds = activeDirectorData.SoundTimes.List.Any();
        }
        if (activeDirectorData.EventTimes != null)
        {
            useEvents = activeDirectorData.EventTimes.List.Any();
        }
    }

    private void Update()
    {
        if (playing)
        {
            UpdateEvents();
            UpdateSounds();
        }
    }

    private void UpdateEvents()
    {
        if (useEvents && activeDirector.time >= activeDirectorData.EventTimes.List[currentEventIndex].Time)
        {
            activeDirectorData.EventTimes.List[currentEventIndex].GameEvent.Raise();
            currentEventIndex++;

            if (currentEventIndex >= activeDirectorData.EventTimes.List.Count)
            {
                useEvents = false;
            }
        }
    }

    private void UpdateSounds()
    {
        if (useSounds && activeDirector.time >= activeDirectorData.SoundTimes.List[currentSoundIndex].Time)
        {
            _ = SoundManager.PlaySound(activeDirectorData.SoundTimes.List[currentSoundIndex].Sound);
            currentSoundIndex++;

            if (currentSoundIndex >= activeDirectorData.SoundTimes.List.Count)
            {
                useSounds = false;
            }
        }
    }

    public void InitialToPlayer()
    {
        loop = false;
        initialDirector.Stop();
    }

    [Serializable]
    private class DirectorEventPair
    {
        [SerializeField]
        private PlayableDirector director;

        [SerializeField]
        private GameEvent gameEvent;

        public PlayableDirector Director => director;
        public GameEvent GameEvent => gameEvent;
    }
}
