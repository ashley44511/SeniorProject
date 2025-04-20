using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadGameWonScene : MonoBehaviour
{
    public bool hitDialouge = false;
    public bool dialogueFinished = true;
    private DialogueManager dialogueManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(dialogueManager.GetIsInDialouge() && !dialogueFinished)
        {
            dialogueFinished = false;
            hitDialouge = true;
            Debug.Log("Hit Dialogue: " + hitDialouge);
        }

        if(hitDialouge && !dialogueManager.GetIsInDialouge())
        {
            dialogueFinished = true;
            Debug.Log("Hit Dialogue: " + dialogueFinished);
        }

        if(hitDialouge && dialogueFinished)
        {
            SceneManager.LoadScene("Credits");
        }
    }
}
