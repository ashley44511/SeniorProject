using UnityEngine;

public class PlayerItemPickup : MonoBehaviour
{
    private HotbarController inventoryController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryController = FindObjectOfType<HotbarController>();
    }

    private void onTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Debug.Log("Item collided with!");

            Item item = collision.GetComponent<Item>();
            if(item != null)
            {
                //Adding item into the inventory
                bool itemAdded = inventoryController.AddItem(collision.gameObject);

                if(itemAdded)
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
