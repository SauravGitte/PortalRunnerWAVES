using UnityEngine;

public class Portal : MonoBehaviour
{
    private SkyboxController skyboxController;
    private DirectionalLightController directionalLightController;

    private void Awake()
    {
        // Get components attached to the same GameObject
        skyboxController = GetComponent<SkyboxController>();
        directionalLightController = GetComponent<DirectionalLightController>();

        // Ensure components exist
        if (skyboxController == null)
            Debug.LogWarning("SkyboxController is missing on Portal object!");

        if (directionalLightController == null)
            Debug.LogWarning("DirectionalLightController is missing on Portal object!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            skyboxController?.ChangeSkybox(); // Null-safe call
            directionalLightController?.ApplySettings(); // Uses public fields now
        }
    }
}
