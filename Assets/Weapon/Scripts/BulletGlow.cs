using UnityEngine;

public class BulletGlow : MonoBehaviour {
    [Header("Glow Settings")]
    // Color of the glow light
    public Color glowColor = Color.cyan;
    // Intensity of the light
    public float intensity = 5f;
    // How far the light reaches
    public float range = 3f;

    void Start() {
        Debug.Log("hello world");
        // Check if a Light component is already attached
        Light bulletLight = GetComponent<Light>();
        if (bulletLight == null) {
            // Add a Light component if not present
            bulletLight = gameObject.AddComponent<Light>();
        }
        
        // Configure the light's properties
        bulletLight.color = glowColor;
        bulletLight.intensity = intensity;
        bulletLight.range = range;
        // Optionally, adjust other light properties (like shadows) as needed
    }
}
