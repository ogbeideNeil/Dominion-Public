using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private Canvas mainMenu;

    [SerializeField]
    private GameObject PauseMenuEmpty;

    [SerializeField]
    private GameObject PauseMenuOptionsEmpty;

    [SerializeField]
    private Canvas playerHud;

    [SerializeField]
    private Button selectFirst;

    [SerializeField]
    private List<GameEvent> onPauseEvents;

    [SerializeField]
    private List<GameEvent> onResumeEvents;

    private PlayerActions playerActions;
    private bool paused;

    public bool PauseResumeEvents { get; set; }

    private void Awake()
    {
        playerActions = new PlayerActions();

        playerActions.IngameUI.Pause.performed += PerformedPause;
        playerActions.IngameUI.Enable();
    }

    public void Resume()
    {
        mainMenu.enabled = false;
        PauseMenuEmpty.SetActive(false);
        PauseMenuOptionsEmpty.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
        SoundManager.ResumeAllSounds();
        playerHud.enabled = true;

        if (!PauseResumeEvents)
        {
            foreach (GameEvent gameEvent in onResumeEvents)
            {
                gameEvent.Raise();
            }
        }
    }

    public void Pause()
    {
        if (mainMenu.enabled == false) 
        {
            PauseMenuEmpty.SetActive(true);
            Time.timeScale = 0f;
            paused = true;
            SoundManager.PauseAllSounds();
            playerHud.enabled = false;
            selectFirst.Select();

            foreach (GameEvent gameEvent in onPauseEvents)
            {
                gameEvent.Raise();
            }
        }
        
    }

    public void QuitToMainMenu()
    {
        paused = false;
        //canvas.enabled = false;
        Time.timeScale = 1f;
        mainMenu.enabled = true;
        playerHud.enabled = false;
    }

    public void EnableInGameUI(bool enable)
    {
        if (enable)
        {
            playerActions.IngameUI.Enable();
        }
        else
        {
            playerActions.IngameUI.Disable();
        }
    }

    private void PerformedPause(InputAction.CallbackContext context)
    {
        if (!PauseMenuEmpty.activeInHierarchy)
        {
            if (paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        else if (PauseMenuEmpty.activeInHierarchy)
        {
            Resume();
        }
        else if (PauseMenuOptionsEmpty.activeInHierarchy)
        {
            Resume();
        }
    }
}
