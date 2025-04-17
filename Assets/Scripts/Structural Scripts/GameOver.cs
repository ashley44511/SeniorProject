using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class GameOver : MonoBehaviour
{
    public Image sceneFadeImage;

    public Color fadeOutColor;
    public Button gameOverButton;
    public float fadeOutDuration;
	private HealthBar healthBar;
    private PlayerMovement playerMovement;
    private bool playerAlive;
    private List<GameObject> initialInventoryPanel = new List<GameObject>();
    private HotbarController hotbarController;



    // Start is called before the first frame update
    void Start()
    {
        hotbarController = GameObject.Find("GameManagers").GetComponent<HotbarController>();
        healthBar = GameObject.Find("PlayerHealthBar").GetComponent<HealthBar>();
        foreach(Transform slotTransform in hotbarController.inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot.currentItem != null)
            {
                GameObject newItem = Instantiate(slot.currentItem);
                Debug.Log("New Item " + newItem);
                newItem.SetActive(false);
                initialInventoryPanel.Add(newItem);
            } 
        }
            
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        playerAlive = healthBar.currentHealth > 0;
        if (!playerAlive) {
            if (playerMovement != null)
                playerMovement.disabled = true;
            StartCoroutine(FadeOut());
            StartCoroutine(waitForFadeOut());
        }

    }

    public IEnumerator FadeOut() {
        yield return FadeCoroutine();
    }

    private IEnumerator FadeCoroutine() {
        float elapsedTime = 0;
        float elapesdPercentage = 0;
        Color startColor = new Color(sceneFadeImage.color.r, sceneFadeImage.color.g, sceneFadeImage.color.b, 0);

        while (elapesdPercentage < 1) {
            elapesdPercentage = elapsedTime / fadeOutDuration;
            sceneFadeImage.color = Color.Lerp(startColor, fadeOutColor, elapesdPercentage);
            yield return null;
            elapsedTime += Time.deltaTime;
        }
    }

    private IEnumerator waitForFadeOut()
	{
		yield return new WaitForSeconds(fadeOutDuration + 1);
        gameOverButton.gameObject.SetActive(true);
	}

    public void ReloadScene()
    {
        Debug.Log("Reloading Scene");
        playerMovement.disabled = false;

        hotbarController.ClearInventory();
        int index = 0;
        foreach (GameObject item in initialInventoryPanel) {
            if (item != null) {
                item.SetActive(true);
                hotbarController.AddItem(item);
            } 
            index ++;
        }
        healthBar.SetCurrentHealth(healthBar.maxHealth);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}


 