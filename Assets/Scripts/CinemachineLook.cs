using UnityEngine;
using Unity.Cinemachine;

public class CinemachineLook : MonoBehaviour
{
    [Header("Player Settings")]
    [Tooltip("Drag your player GameObject here (must have a Rigidbody).")]
    public Transform player;

    [Header("Camera Base Settings")]
    [Tooltip("Base vertical offset from the player.")]
    public float baseHeight = 3f;
    [Tooltip("Base distance behind the player.")]
    public float baseDistance = 6f;

    [Header("Dynamic Effect Settings")]
    [Tooltip("How much extra height is added based on speed.")]
    public float speedHeightFactor = 0.05f;
    [Tooltip("How much extra distance (zoom) is added based on speed.")]
    public float speedDistanceFactor = 0.1f;
    [Tooltip("Additional vertical bobbing amplitude for cinematic effect.")]
    public float bobbingAmplitude = 0.2f;
    [Tooltip("Speed multiplier for bobbing effect.")]
    public float bobbingSpeed = 0.5f;
    [Tooltip("How smoothly the camera transitions to its target offset.")]
    public float smoothSpeed = 5f;

    private CinemachineCamera virtualCam;
    private CinemachinePositionComposer positionComposer;
    private Rigidbody playerRigidbody;

    private Vector3 targetOffset;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player Transform not assigned in CinematicCameraController.");
            return;
        }

        // Get the player's Rigidbody component
        playerRigidbody = player.GetComponent<Rigidbody>();
        if (playerRigidbody == null)
        {
            Debug.LogError("Player GameObject does not have a Rigidbody component.");
        }

        // Get the Cinemachine Camera component attached to this GameObject
        virtualCam = GetComponent<CinemachineCamera>();
        if (virtualCam == null)
        {
            Debug.LogError("No CinemachineCamera component found on this GameObject.");
            return;
        }

        // Get the Position Composer component (used to offset the camera relative to the target)
        positionComposer = virtualCam.GetComponent<CinemachinePositionComposer>();
        if (positionComposer == null)
        {
            Debug.LogError("No CinemachinePositionComposer component found on the virtual camera.");
            return;
        }

        // Initialize offset using base settings
        targetOffset = new Vector3(0, baseHeight, -baseDistance);
        positionComposer.TargetOffset = targetOffset;
    }

    void Update()
    {
        if (playerRigidbody == null) return;

        // Get current speed of the player
        float speed = playerRigidbody.linearVelocity.magnitude;

        // Calculate desired offsets based on speed
        float desiredHeight = baseHeight + (speed * speedHeightFactor);
        float desiredDistance = baseDistance + (speed * speedDistanceFactor);

        // Add a bobbing effect based on time and speed
        float bobbingOffset = Mathf.Sin(Time.time * bobbingSpeed * speed) * bobbingAmplitude;

        // Update target offset vector
        targetOffset = new Vector3(0, desiredHeight + bobbingOffset, -desiredDistance);

        // Smoothly update the tracked object offset of the position composer
        positionComposer.TargetOffset = Vector3.Lerp(positionComposer.TargetOffset, targetOffset, smoothSpeed * Time.deltaTime);
    }
}
