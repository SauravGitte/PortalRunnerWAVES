using UnityEngine;

public class AttachLight : MonoBehaviour {
    [Header("Light Settings")]
    public LightType lightType = LightType.Point;
    public float range = 10f;
    public float intensity = 1f;
    public Color lightColor = Color.white;

    // Optional: set shadow settings (None, Hard, or Soft)
    public LightShadows shadows = LightShadows.None;

    void Start() {
        // Create a new GameObject to hold the light
        GameObject lightGameObject = new GameObject("AttachedLight");

        // Make the new light a child of the ball
        lightGameObject.transform.parent = transform;
        // Optionally, set the light's local position
        lightGameObject.transform.localPosition = Vector3.zero;

        // Add a Light component to the new GameObject
        Light lightComp = lightGameObject.AddComponent<Light>();
        lightComp.type = lightType;
        lightComp.range = range;
        lightComp.intensity = intensity;
        lightComp.color = lightColor;
        lightComp.shadows = shadows;
    }
}
