using UnityEngine;

public class BulletLaserGlow : MonoBehaviour {
    [Header("Laser Glow Settings")]
    public Color glowColor = Color.cyan;
    public float glowIntensity = 5f; // Multiplier for brightness

    void Start() {
        // Get the Renderer component
        Renderer rend = GetComponent<Renderer>();
        if (rend != null) {
            // Use the instance material so changes don't affect the shared material
            Material mat = rend.material;
            // Multiply the color by the intensity for HDR-like brightness
            Color finalColor = glowColor * glowIntensity;
            // Set and enable the emission color on the material
            mat.SetColor("_EmissionColor", finalColor);
            mat.EnableKeyword("_EMISSION");
        }
    }
}
