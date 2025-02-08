using UnityEngine;

public class HotbarController : MonoBehaviour
{
    //2 video tutorials that helped create the inventory system:
    // https://www.youtube.com/watch?v=CcfYUYgaBTw
    // https://www.youtube.com/watch?v=wlBJ0yZOYfM

    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;
    public Color defaultBackgroundColor;
    public Color selectedBackgroundColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < slotCount; i++)
        {
            Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();

            if (i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                item.transform.localScale = new Vector3(1, 1, 1);
                slot.currentItem = item;
            }
        }

        SelectSlot(0);
    }

    public bool AddItem(GameObject itemPrefab)
    {
        foreach(Transform slotTransform in inventoryPanel.transform)
        {
            Debug.Log("Checking slot");
            Slot slot = slotTransform.GetComponent<Slot>();
            if(slot != null && slot.currentItem == null)
            {
                Debug.Log("Slot is empty");
                GameObject newItem = Instantiate(itemPrefab, slot.transform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                newItem.transform.localScale = new Vector3(1, 1, 1);
                slot.currentItem = newItem;
                return true;
            }
        }
        Debug.Log("Inventory is full");
        return false;
    }

    public void SelectSlot(int index)
    {
        //Hides currently selected item border and changes it to the new one
        foreach(Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if(slot != null && slot.backgroundImage != null)
            {
                slot.backgroundImage.color = defaultBackgroundColor;
            }
            if(slot != null && slot.borderFrame != null)
            {
                slot.borderFrame.SetActive(false);
            }
        }

        //Enables the border of the selected slot
        Slot selectedSlot = inventoryPanel.transform.GetChild(index).GetComponent<Slot>();

        if (selectedSlot != null && selectedSlot.backgroundImage != null)
        {
            selectedSlot.backgroundImage.color = selectedBackgroundColor;
        }
        
        if(selectedSlot != null && selectedSlot.borderFrame != null)
        {
            selectedSlot.borderFrame.SetActive(true);
        }

        Debug.Log("Selecting slot " + index);
    }

public void UseItem(int currentIndex)
{
    Slot selectedSlot = inventoryPanel.transform.GetChild(currentIndex).GetComponent<Slot>();
    
    if(selectedSlot != null && selectedSlot.currentItem != null)
    {
        Debug.Log("Using item in slot " + currentIndex);
        Destroy(selectedSlot.currentItem);
        selectedSlot.currentItem = null;
    }
}
}
