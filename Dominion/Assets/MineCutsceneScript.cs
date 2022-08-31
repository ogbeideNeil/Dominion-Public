using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class MineCutsceneScript : MonoBehaviour
{
    [SerializeField]
    private GameObject CurrentScene;

    [SerializeField]
    private GameObject FutureScene;

    bool isPlaying = false;
    private PlayableDirector playableDirector;

    void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    void Update()
    {
        if (!isPlaying && playableDirector.time > 0)
        {
            isPlaying = true;
        }
        if (playableDirector.time > 4)
        {
            SwitchToFuture();
        }
        //else if(playableDirector.time)
        if (isPlaying && playableDirector.time > 14)
        {
            SwitchToPresent();
        }
    }

    private void SwitchToFuture() 
    {
        CurrentScene.SetActive(false);
        FutureScene.SetActive(true);
    }

    private void SwitchToPresent() 
    {
        CurrentScene.SetActive(true);
        FutureScene.SetActive(false);
    }
}
