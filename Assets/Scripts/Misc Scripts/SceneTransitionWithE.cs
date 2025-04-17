using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTransitionWithE : MonoBehaviour
{
    public string sceneName; // String name to the game scene to load
    public GameObject ePrompt; // Interact image
    private bool playerInTrigger = false; // Tracks if the player is standing in the trigger volume
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ePrompt.gameObject.SetActive(false);
    }

    //Running for every frame in the game (OnTriggerStay was skipping frames so I had to switch it)
    void Update()
    {
        //If the user is hitting E and is within the trigger, then the next scene will play
        if (Input.GetKeyDown(KeyCode.E) && playerInTrigger == true)
        {
            playerInTrigger = false;
            SceneManager.LoadScene(sceneName);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the player enters the trigger, the prompt appears and the boolean is updated
        if (collision.gameObject.tag == "Player" && ePrompt != null)
        {
            playerInTrigger = true;
            //Will change this to space bar later, when we have a sprite for it
            //This will probably be a canvas overlay or just a floating text saying press space to return
            ePrompt.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //If the player leaves the trigger, the prompt disappears and the boolean is updated accordingly
        if (collision.gameObject.tag == "Player" && ePrompt != null)
        {
            playerInTrigger = false;
            ePrompt.gameObject.SetActive(false);
        }
    }
}
