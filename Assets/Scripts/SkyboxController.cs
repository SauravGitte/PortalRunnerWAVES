using UnityEngine;
using System.Collections;

public class SkyboxController : MonoBehaviour
{
    public Material skyboxBlendMaterial; // Material using the custom SkyboxBlend shader
    public Cubemap skyboxB; // Target skybox
    public float transitionDuration = 3f; // Time in seconds
    public float smoothEndFactor = 0.1f; // Additional time to ease out

    private Cubemap skyboxA; // Stores initial skybox dynamically

    private void Start()
    {
        if (skyboxBlendMaterial == null)
        {
            Debug.LogError("Skybox Blend Material is missing!");
            return;
        }

        // Capture the current skybox dynamically
        if (RenderSettings.skybox != null && RenderSettings.skybox.HasProperty("_Tex"))
        {
            skyboxA = RenderSettings.skybox.GetTexture("_Tex") as Cubemap;
        }

        if (skyboxA == null)
        {
            Debug.LogWarning("No initial skybox found! Using default cubemap.");
            skyboxA = skyboxB; // Fallback to avoid errors
        }

        // Assign both skyboxes to the blend material
        skyboxBlendMaterial.SetTexture("_CubemapA", skyboxA);
        skyboxBlendMaterial.SetTexture("_CubemapB", skyboxB);
        skyboxBlendMaterial.SetFloat("_BlendFactor", 0);

        RenderSettings.skybox = skyboxBlendMaterial;
    }

    public void ChangeSkybox()
    {
        StopAllCoroutines();
        StartCoroutine(TransitionSkybox());
    }

    private IEnumerator TransitionSkybox()
    {
        float elapsedTime = 0f;
        float totalDuration = transitionDuration * (1 + smoothEndFactor); // Extend transition time slightly

        while (elapsedTime < totalDuration)
        {
            float t = elapsedTime / transitionDuration;

            // Use an ease-out curve to slow down the transition at the end
            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            skyboxBlendMaterial.SetFloat("_BlendFactor", smoothT);

            // Optional: Slight exposure/tint fade for extra smoothness
            if (skyboxBlendMaterial.HasProperty("_Exposure"))
            {
                float exposure = Mathf.Lerp(1.0f, 1.2f, smoothT); // Slight brightness boost
                skyboxBlendMaterial.SetFloat("_Exposure", exposure);
            }

            DynamicGI.UpdateEnvironment();
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Instead of forcing 1, let it settle naturally
        float finalValue = skyboxBlendMaterial.GetFloat("_BlendFactor");
        if (finalValue > 0.98f) // If already close to 1, don't force it
        {
            skyboxBlendMaterial.SetFloat("_BlendFactor", finalValue);
        }

        DynamicGI.UpdateEnvironment();
    }
}
