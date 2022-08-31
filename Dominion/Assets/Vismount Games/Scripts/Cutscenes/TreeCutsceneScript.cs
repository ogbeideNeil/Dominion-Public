using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TreeCutsceneScript : MonoBehaviour
{
    [SerializeField]
    private GameObject CurrentTree;

    [SerializeField]
    private GameObject VisionTree;

    [SerializeField]
    private GameObject WaterBlue;

    [SerializeField]
    private GameObject WaterRed;

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
        if (playableDirector.time > 5)
        {
            SwitchToVision();
        }
        //else if(playableDirector.time)
        if (isPlaying && playableDirector.time > 15)
        {
            SwitchToPresent();
        }
    }

    private void SwitchToVision() 
    {
        WaterBlue.SetActive(false);
        WaterRed.SetActive(true);
        CurrentTree.SetActive(false);
        VisionTree.SetActive(true);
    }

    private void SwitchToPresent() 
    {
        WaterBlue.SetActive(true);
        WaterRed.SetActive(false);
        CurrentTree.SetActive(true);
        VisionTree.SetActive(false);
    }
}
