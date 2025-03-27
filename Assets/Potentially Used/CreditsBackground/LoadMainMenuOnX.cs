using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainMenuOnX : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            LoadSceneByName("TitleScreen");
        }
    }

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
