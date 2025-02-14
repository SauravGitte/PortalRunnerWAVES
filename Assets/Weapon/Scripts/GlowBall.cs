using UnityEngine;

public class GlowBall : MonoBehaviour {
    [Header("Glow Settings")]
    public Color glowColor = Color.yellow;
    public float glowIntensity = 5f; // Adjust to control brightness

    void Start() {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null) {
            // Get the material attached to the ball
            Material mat = renderer.material;

            // Check if the material supports emission
            if (mat.HasProperty("_EmissionColor")) {
                // Enable emission on the material
                mat.EnableKeyword("_EMISSION");
                // Set the emission color multiplied by the intensity
                mat.SetColor("_EmissionColor", glowColor * glowIntensity);
            } else {
                Debug.LogWarning("The material on this object does not support emission.");
            }
        }
    }
}
