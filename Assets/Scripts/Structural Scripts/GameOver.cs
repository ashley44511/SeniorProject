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
    public Camera mainCamera;
    // public string currentScene;
	private HealthBar healthBar;
    private PlayerMovement playerMovement;
    private bool playerAlive;
	private List<Item> initalItemList;
    private GameObject initialInventoryPanel;



    // Start is called before the first frame update
    void Start()
    {
        healthBar = GameObject.Find("PlayerHealthBar").GetComponent<HealthBar>();
        initalItemList = GameObject.Find("Player").GetComponent<PlayerAttack>().itemList;
        initialInventoryPanel = GameObject.Find("GameManagers").GetComponent<HotbarController>().inventoryPanel;
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        playerAlive = healthBar.currentHealth > 0;
        if (!playerAlive) {
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
        // gameOverButton.transform.position = mainCamera.transform.position;
	}

    public void ReloadScene()
    {
        healthBar.SetCurrentHealth(healthBar.maxHealth);
        GameObject.Find("Player").GetComponent<PlayerAttack>().itemList = initalItemList;
        GameObject.Find("GameManagers").GetComponent<HotbarController>().inventoryPanel = initialInventoryPanel;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}


 