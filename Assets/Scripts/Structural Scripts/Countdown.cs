using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class Countdown : MonoBehaviour
{
    public Slider timerBar;
    public Gradient gradient;
    public Image fill;
    public int totalTime;
    private float timeLeft;
    private bool timerActive = true;
    public Image sceneFadeImage;

    public Color fadeOutColor;
    public float fadeOutDuration;
    public string NextScene;
    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        timeLeft = totalTime;
        fill.color = gradient.Evaluate(1f);
        timerBar.maxValue = totalTime;
        timerBar.value = totalTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft > 0) {
            timeLeft -= Time.deltaTime;
            timerBar.value = timeLeft;
            // fill.color = gradient.Evaluate(timerBar.normalizedValue);
        } else {
            timerActive = false; 
            timeLeft = 0;
            playerMovement.disabled = true;
            StartCoroutine(FadeOut());
            StartCoroutine(waitForFadeOut());
        }

    }

    bool getTimerStatus() {
        return timerActive;
    }

    float getTimeLeft() {
        return timeLeft;
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
        SceneManager.LoadScene(NextScene);
	}
}


 