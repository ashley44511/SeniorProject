using UnityEngine;
using UnityEngine.EventSystems;
public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //Tutorial I followed:https://www.youtube.com/watch?v=wlBJ0yZOYfM&t=168s
    //Existing bug: Currently dragging the item makes it invisible. Need to fix this.

    Transform originalParent;
    CanvasGroup canvasGroup;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Orignal parent saved
        originalParent = transform.parent;
        //Above previous canvas
        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
        //Somewhat see through during drag
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Goes where the mouse goes
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Enable raycast
        canvasGroup.blocksRaycasts = true;
        //No longer transparent
        canvasGroup.alpha = 1f;

        Slot dropSlot = eventData.pointerEnter?.GetComponent<Slot>();
        if (dropSlot != null)
        {
            GameObject item = eventData.pointerEnter;

            if (item != null)
            {
                dropSlot = item.GetComponentInParent<Slot>();
            }
        }
        Slot originalSlot = originalParent.GetComponent<Slot>();

        if (dropSlot != null)
        {
            if (dropSlot.currentItem != null)
            {
                //Slot has an item so items are swapped
                dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                originalSlot.currentItem = null;
            }

            //Moving the item to the drop slot
            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
        }
        else
        {
            //No slot under the drop point
            transform.SetParent(originalParent);
        }

        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
}
