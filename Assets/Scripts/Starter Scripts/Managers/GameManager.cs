using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;  // Singleton instance
    [HideInInspector]
    public Vector3 RespawnPlace;
    public GameObject Player;

    // Track game state
    public int currentDay = 1;
    public HashSet<string> triggeredDialogues = new HashSet<string>(); // Track which dialogues have been triggered

    private void Awake()
    {
        // Singleton pattern to ensure only one GameManager exists
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Set initial respawn position
        if (Player == null)
        {
            Player = FindObjectOfType<PlayerMovement>().gameObject;
        }
        RespawnPlace = Player.transform.position;
    }

    private void Start()
    {
        // Optional: You could load the game state from a saved file here if you want
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGameState();
        }
    }


    // Save and Load could be added here, depending on how persistent you want the data across sessions
    public void SaveGameState()
    {
        // Save your game state, such as currentDay, triggered dialogues, etc.
        PlayerPrefs.SetInt("CurrentDay", currentDay);
        PlayerPrefs.SetString("TriggeredDialogues", string.Join(",", triggeredDialogues));
    }

    public void LoadGameState()
    {
        // Load game state (example)
        currentDay = PlayerPrefs.GetInt("CurrentDay", 1); // Default to day 1 if not saved
        string savedDialogues = PlayerPrefs.GetString("TriggeredDialogues", "");
        triggeredDialogues = new HashSet<string>(savedDialogues.Split(','));
    }

    public void Respawn(GameObject player)
    {
        player.transform.position = RespawnPlace;
    }

    public void SetNewRespawnPlace(GameObject newPlace)
    {
        RespawnPlace = newPlace.transform.position;
    }

    public void DisablePlayerMovement(bool isDisabled)
    {
        PlayerMovement playerMovement = Player.GetComponent<PlayerMovement>();
        PlayerAudio playerAudio = Player.GetComponent<PlayerAudio>();

        if (playerMovement)
            playerMovement.DisablePlayer(isDisabled);
        if (playerAudio)
            playerAudio.StopAll();
    }

    public void ResetGameState()
    {
        triggeredDialogues.Clear();
        currentDay = 1;
        RespawnPlace = Player.transform.position;

        Debug.Log("GameManager reset for testing.");
    }


    // You can add more functions to handle other game states as needed, like health, inventory, etc.
}
