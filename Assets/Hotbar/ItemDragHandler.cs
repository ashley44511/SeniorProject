using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform originalParent;
    CanvasGroup canvasGroup;
    private PauseMenu pauseMenu;
    private AudioSource audioSource;
    private PlayerAttack playerAttack;


    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        pauseMenu = FindObjectOfType<PauseMenu>();
        originalParent = transform.parent;
        audioSource = GameObject.FindWithTag("WorldAudio").GetComponent<AudioSource>();
        playerAttack = GameObject.FindWithTag("Player").GetComponent<PlayerAttack>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Prevents dragging from starting when the pause menu is open
        if(pauseMenu.getOpen())
        {
            Debug.Log("Trying to drag item while pause menu is open");
            eventData.pointerDrag = null;
            return;
        }

        originalParent = transform.parent;
        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
        //Semi-transparent during drag
        canvasGroup.alpha = 0.6f; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        //If the pause menu is open, it snaps back to the original item slot
        if(pauseMenu.getOpen())
        {
            transform.SetParent(originalParent);
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
         
            return;
        }

        //Follow the mouse
        transform.position = eventData.position; 
    }

    public void OnEndDrag(PointerEventData eventData)
    {
         //If the pause menu is open, it snaps back to the original item slot
        if(pauseMenu.getOpen())
        {
            transform.SetParent(originalParent);
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
            
            return;
        }

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        //Slot where item dropped
        Slot dropSlot = eventData.pointerEnter?.GetComponent<Slot>();

        if (dropSlot == null)
        {
            GameObject dropItem = eventData.pointerEnter;

            if (dropItem != null)
            {
                dropSlot = dropItem.GetComponentInParent<Slot>();
            }
        }

        Slot originalSlot = originalParent.GetComponent<Slot>();

        if (dropSlot != null && pauseMenu.getOpen() == false)
        {
            //Is a slot under drop point
            //There's an item in the slot
            if (dropSlot.currentItem != null)
            {
                //Slot has an item so you want to swap items
                playerAttack.MoveItem(dropSlot.currentItem, originalSlot.currentItem);
                dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                //Drop slot is empty
                originalSlot.currentItem = null;
            }

            //Move item into drop slot
            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
        }
        else
        {
            //If the item is outside of the hotbar, drop it on the ground
            //Also have to check if it's a bear trap (since that's the only item we want to place now)
            if (!MouseOverInventory(eventData.position) && pauseMenu.getOpen() == false)
            {
                playerAttack.RemoveItem(originalSlot.currentItem);
                DropItem(originalSlot, eventData);
            }
            else
            {
                //No slot under drop point
                transform.SetParent(originalParent);
            }
        }

        //Centers the item in the slot
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    public bool MouseOverInventory(Vector2 mousePosition)
    {
        //Useful tutorial: https://www.youtube.com/watch?v=L5phEpQooxw
        RectTransform inventoryRect = originalParent.parent.GetComponent<RectTransform>();

        //Checks if the mouse is over the inventory bar
        return RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, mousePosition);
    }

    //TODO: Limit drop area (if we want to)
    public void DropItem(Slot originalSlot, PointerEventData eventData)
    {
        Item item = originalSlot.currentItem.GetComponent<Item>();
        
        //Only drop the item if it's the bear trap
        if(item.itemName == "BearTrap")
        {
            originalSlot.currentItem = null;

            if(item.placeInWorldSound != null)
            {
                Debug.Log("Playing " + item.itemName + " placing sound");
                audioSource.PlayOneShot(item.placeInWorldSound);
            }

            //Gets where the mouse is in the world
            Vector3 dropPosition = Camera.main.ScreenToWorldPoint(eventData.position);
            //Failsafe to ensure item is visible to the camera
            dropPosition.z = 0;

            //Creates the item on the ground and removes it from the hotbar. Also resizes it so it looks normal
            GameObject droppedItem = Instantiate(gameObject, dropPosition, Quaternion.identity);
            droppedItem.transform.localScale = new Vector3(item.imageScale, item.imageScale, item.imageScale);
            Destroy(gameObject);
        }
        else
        {
            //Back to original slot
            transform.SetParent(originalParent);
        }
    }
}
