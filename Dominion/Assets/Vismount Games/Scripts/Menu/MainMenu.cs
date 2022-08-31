using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private CameraManager cameraManager;

    [SerializeField]
    private Sound mainMenuSound;

    [SerializeField]
    private Canvas playerHud;

    private bool mainMenuCamera = true;

    private void Start()
    {
        MusicManager.Play(mainMenuSound, 0f);
    }

    public void StartSinglePlayer ()
    {
        if (mainMenuCamera)
        {
            cameraManager.InitialToPlayer();
            MusicManager.PlayDefault();
            mainMenuCamera = false;
        }
        Debug.Log("SinglePlayer");
        playerHud.enabled = true;

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
