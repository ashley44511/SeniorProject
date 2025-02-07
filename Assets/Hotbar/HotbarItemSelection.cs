using UnityEngine;

public class HotbarItemSelection : MonoBehaviour
{
    private HotbarController hotbarController;
    public int currentIndex = 0;

    void Start()
    {
        hotbarController = FindObjectOfType<HotbarController>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            currentIndex = 9;
            hotbarController.SelectSlot(currentIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentIndex = 0;
            hotbarController.SelectSlot(currentIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentIndex = 1;
            hotbarController.SelectSlot(currentIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentIndex = 2;
            hotbarController.SelectSlot(currentIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentIndex = 3;
            hotbarController.SelectSlot(currentIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentIndex = 4;
            hotbarController.SelectSlot(currentIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            currentIndex = 5;
            hotbarController.SelectSlot(currentIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            currentIndex = 6;
            hotbarController.SelectSlot(currentIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            currentIndex = 7;
            hotbarController.SelectSlot(currentIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            currentIndex = 8;
            hotbarController.SelectSlot(currentIndex);
        }

        //Scroll wheel up
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            currentIndex++;

            if (currentIndex > 9)
            {
                currentIndex = 0;
            }
            hotbarController.SelectSlot(currentIndex);
        }
        //Scroll wheel down
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            currentIndex--;

            if (currentIndex < 0)
            {
                currentIndex = 9;
            }
            hotbarController.SelectSlot(currentIndex);
        }
    }



}