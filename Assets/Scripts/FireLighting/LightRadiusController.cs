using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;
public class LightRadiusController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Light2D light2D; // Assign in Inspector or find dynamically
    public float radiusIncrease = 2.0f; // How much to increase the radius
    public float maxOuterRadius = 18.0f;
    public float minOuterRadius = 0.0f;
    public float secondsToWait = 1f;
    private bool isReducing = false; // Track if it's already reducing

    void Start()
    {
        if (light2D == null)
        {
            light2D = GetComponent<Light2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            if (light2D != null && light2D.pointLightOuterRadius < maxOuterRadius)
            {
                light2D.pointLightOuterRadius += radiusIncrease;
                light2D.pointLightInnerRadius += (radiusIncrease/2);
            }
        }
        if (!isReducing)
        {
            StartCoroutine(DecreaseRadius());
        }
    }

    IEnumerator DecreaseRadius()
    {
        isReducing = true; // Prevent multiple coroutines running
        while (light2D.pointLightOuterRadius > minOuterRadius)
        {
            light2D.pointLightOuterRadius -= radiusIncrease; // Decrease radius
            light2D.pointLightInnerRadius -= (radiusIncrease / 2);
            yield return new WaitForSeconds(secondsToWait); // Wait x second(s)
        }
        isReducing = false; // Allow re-triggering if needed
    }
}
