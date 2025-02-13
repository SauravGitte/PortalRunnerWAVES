using UnityEngine;
using System.Collections;

public class DirectionalLightController : MonoBehaviour
{
    public Light directionalLight; // Automatically assigned if null

    [Header("Target Light Settings")]
    public float targetIntensity = 1.5f;
    public Color targetColor = Color.white;
    public Vector3 targetRotation = new Vector3(50, -30, 0);
    public LightShadows targetShadowType = LightShadows.Soft;
    public float transitionDuration = 3f; // Transition time in seconds

    private void Awake()
    {
        if (directionalLight == null)
        {
            directionalLight = RenderSettings.sun;
        }

        if (directionalLight == null)
        {
            Debug.LogWarning("No Sun found in the scene!");
        }
    }

    public void ApplySettings()
    {
        if (directionalLight == null)
        {
            Debug.LogWarning("Sun not found!");
            return;
        }
        StartCoroutine(TransitionLight());
    }

    private IEnumerator TransitionLight()
    {
        float elapsedTime = 0f;
        float startIntensity = directionalLight.intensity;
        Color startColor = directionalLight.color;
        Quaternion startRotation = directionalLight.transform.rotation;
        LightShadows startShadows = directionalLight.shadows;

        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            directionalLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, t);
            directionalLight.color = Color.Lerp(startColor, targetColor, t);
            directionalLight.transform.rotation = Quaternion.Lerp(startRotation, Quaternion.Euler(targetRotation), t);
            directionalLight.shadows = (t > 0.5f) ? targetShadowType : startShadows; // Change shadows midway

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Finalize values
        directionalLight.intensity = targetIntensity;
        directionalLight.color = targetColor;
        directionalLight.transform.rotation = Quaternion.Euler(targetRotation);
        directionalLight.shadows = targetShadowType;
    }
}
