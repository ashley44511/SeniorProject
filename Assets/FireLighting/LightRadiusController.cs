using UnityEngine;
using UnityEngine.Rendering.Universal;
public class LightRadiusController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Light2D light2D; // Assign in Inspector or find dynamically
    public float radiusIncrease = 1.0f; // How much to increase the radius

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
            if (light2D != null)
            {
                light2D.pointLightOuterRadius += radiusIncrease;
            }
        }
    }
}
