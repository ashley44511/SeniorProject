using UnityEngine;

public class HotbarController : MonoBehaviour
{
    //2 video tutorials that I think are exactly what we want:
    // https://www.youtube.com/watch?v=CcfYUYgaBTw
    // https://www.youtube.com/watch?v=wlBJ0yZOYfM

    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;

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
                slot.currentItem = item;
            }
        }
    }

}
