using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadGameWonScene : MonoBehaviour
{
    public bool hitDialouge = false;
    public bool hasBeenInDialogue = false;
    public bool dialogueFinished = false;
    private DialogueManager dialogueManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(dialogueManager.GetIsInDialouge() == true && dialogueFinished == false)
        {
            hasBeenInDialogue = true;
        }

        if(hitDialouge && dialogueManager.GetIsInDialouge() == false && hasBeenInDialogue == true)
        {
            dialogueFinished = true;
        }

        if(hitDialouge && dialogueFinished && hasBeenInDialogue)
        {
            SceneManager.LoadScene("Credits");
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            hitDialouge = true;
        }
    }
}
