using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractDialogueTrigger : MonoBehaviour
{
    public SpriteRenderer InteractImage;
    private HotbarController inventoryController;

    private bool playerInTrigger = false;
    DialogueManager manager;
    public TextAsset TextFileAsset; // your imported text file for your NPC
    private Queue<string> dialogue = new Queue<string>(); // stores the dialogue (Great Performance!)
    public float waitTime = 0.5f; // lag time for advancing dialogue so you can actually read it
    private float nextTime = 0f; // used with waitTime to create a timer system
    public bool singleUseDialogue = false;
    public bool deleteWhenFinished = false;
    [HideInInspector]
    public bool hasBeenUsed = false;
    bool inArea = false;
    bool triggered = false;

    public bool pickupItem = false;
    public GameObject itemPickupPrefab;
    private HotbarController itemHotbar;
    private bool itemAdded = false;
    void Start()
    {
        InteractImage.gameObject.SetActive(false);
        manager = FindObjectOfType<DialogueManager>();
        itemHotbar = GameObject.FindGameObjectWithTag("GameController").GetComponent<HotbarController>();
    }

    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E) && triggered == false)
        {
            InteractImage.gameObject.SetActive(false);
            triggered = true;
            manager.currentTriggerInteract = this;
            TriggerDialogue();
        }
        else if (!hasBeenUsed && inArea && Input.GetKeyDown(KeyCode.E) && nextTime < Time.timeSinceLevelLoad && dialogue.Count > 0)
        {
            //Debug.Log("Advance");
            nextTime = Time.timeSinceLevelLoad + waitTime;
            manager.AdvanceDialogue();
        }
     
        //If the queue is empty, the dialogue has finished
        if (dialogue.Count == 0 && singleUseDialogue && hasBeenUsed)
        {
            //manager.EndDialogue();

            if (deleteWhenFinished)
            {
                Destroy(gameObject);
            }

            //Checks if there is an item that has to be picked up
            if(pickupItem)
            {
                //Will add the item to the inventory if it's added to the script
                if(itemPickupPrefab != null && !itemAdded)
                {
                    itemAdded = true;
                    itemHotbar.AddItem(itemPickupPrefab);
                }
            }
        }
    }

    /* Called when you want to start dialogue */
    void TriggerDialogue()
    {
        ReadTextFile(); // loads in the text file
        manager.StartDialogue(dialogue); // Accesses Dialogue Manager and Starts Dialogue
    }

    /* loads in your text file */
    private void ReadTextFile()
    {
        string txt = TextFileAsset.text;

        string[] lines = txt.Split(System.Environment.NewLine.ToCharArray()); // Split dialogue lines by newline

        SearchForTags(lines);

        dialogue.Enqueue("EndQueue");
    }

    /*Version 2: Introduces the ability to have multiple tags on a single line! Allows for more functions to be programmed
     * to unique text strings or general functions. 
     */
    private void SearchForTags(string[] lines)
    {
        foreach (string line in lines) // for every line of dialogue
        {
            if (!string.IsNullOrEmpty(line))// ignore empty lines of dialogue
            {
                if (line.StartsWith("[")) // e.g [NAME=Michael] Hello, my name is Michael
                {
                    string special = line.Substring(0, line.IndexOf(']') + 1); // special = [NAME=Michael]
                    string curr = line.Substring(line.IndexOf(']') + 1); // curr = Hello, ...
                    dialogue.Enqueue(special); // adds to the dialogue to be printed
                    string[] remainder = curr.Split(System.Environment.NewLine.ToCharArray());
                    SearchForTags(remainder);
                    //dialogue.Enqueue(curr);
                }

                else
                {
                    dialogue.Enqueue(line); // adds to the dialogue to be printed
                }
            }
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInTrigger = true;
            inArea = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !hasBeenUsed && InteractImage != null)
        {
            InteractImage.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            InteractImage.gameObject.SetActive(false);
            playerInTrigger = false;
            
/*              if(triggered)
            {
                manager.EndDialogue();
            }  */

/*             if(singleUseDialogue && deleteWhenFinished && triggered)
            {
                Destroy(gameObject);
            } */
        }

        inArea = false;

        if(triggered == true && singleUseDialogue == false)
        {
            triggered = false;
        }
    }
}
