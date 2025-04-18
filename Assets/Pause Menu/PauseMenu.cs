using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    private bool isOpen = false;
    public GameObject pauseMenu;
    //private AudioSource worldAudio;
    private Scene currentScene;

    void Start()
    {
        //worldAudio = GameObject.Find("WorldAudio").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //https://discussions.unity.com/t/how-to-check-which-scene-is-loaded-and-write-if-code-for-it/163399
        //Have to check if the main menu is open, and it should close the pause menu if it is
        currentScene = SceneManager.GetActiveScene();

        if(currentScene.name == "TitleScreen")
        {
            if(isOpen)
            {
                ClosePauseMenu();
            }

            return;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isOpen)
            {
                ClosePauseMenu();
            }
            else
            {
                OpenPauseMenu();
            }
        }
    }

    private void OpenPauseMenu()
    {
        isOpen = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    private void ClosePauseMenu()
    {
        isOpen = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public bool getOpen()
    {
        return isOpen;
    }

    public void setOpen(bool open)
    {
        if(open)
        {
            OpenPauseMenu();
        }
        else
        {
            ClosePauseMenu();
        }
    }
}
