using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTransitionWithZ : MonoBehaviour
{
    public string sceneName; // String name to the game scene to load
    public GameObject zPrompt; // Interact image
    private bool playerInTrigger = false; // Tracks if the player is standing in the trigger volume
    private DialogueManager dialogueManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        zPrompt.gameObject.SetActive(false);
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    //Running for every frame in the game (OnTriggerStay was skipping frames so I had to switch it)
    void Update()
    {
        //If the user is hitting Z and is within the trigger, then the next scene will play
        if (Input.GetKeyDown(KeyCode.Z) && playerInTrigger == true)
        {
            if(dialogueManager != null && dialogueManager.GetIsInDialouge() == false)
            {
                playerInTrigger = false;
                SceneManager.LoadScene(sceneName);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the player enters the trigger, the prompt appears and the boolean is updated
        if (collision.gameObject.tag == "Player")
        {
            playerInTrigger = true;
            //Will change this to space bar later, when we have a sprite for it
            //This will probably be a canvas overlay or just a floating text saying press space to return
            zPrompt.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //If the player leaves the trigger, the prompt disappears and the boolean is updated accordingly
        if (collision.gameObject.tag == "Player" && zPrompt != null)
        {
            playerInTrigger = false;
            zPrompt.gameObject.SetActive(false);
        }
    }
}
