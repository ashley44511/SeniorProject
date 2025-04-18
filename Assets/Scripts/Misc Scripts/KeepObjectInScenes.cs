using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeepObjectInScenes : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
        Scene currentScene = SceneManager.GetActiveScene();
        
        if (currentScene.name == "TitleScreen")
        {
            Destroy(gameObject);
        }
	
        if (currentScene.name == "Credits")
        {
            Destroy(gameObject);
        }
	}
}
