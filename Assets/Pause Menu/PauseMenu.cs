using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private bool isOpen = false;
    public GameObject pauseMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
}
