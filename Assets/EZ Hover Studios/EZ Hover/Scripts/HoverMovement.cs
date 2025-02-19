using UnityEngine;

namespace EZHover
{
    public class HoverMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private bool enableInput = true;
        public bool EnableInput { get { return enableInput; } set { enableInput = value; } }

        [SerializeField] private float moveSpeed = 10f;
        public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

        [Header("Steering Settings")]
        [SerializeField] private float turnSpeed = 100f; // Speed at which the hoverboard turns
        [SerializeField] private float maxTiltAngle = 30f; // Maximum tilt angle when steering
        [SerializeField] private float tiltSpeed = 5f; // Speed at which the hoverboard tilts

        [Header("Obstacle Avoidance")]
        [SerializeField] private bool enableObstacleAvoidance = true;

        [SerializeField] LayerMask obstacleLayers = 1;
        public LayerMask ObstacleLayers { get { return obstacleLayers; } }

        [Tooltip("Amount of vertical force applied to rise above oncoming obstacles.")]
        [SerializeField] private float hoverBoost = 30f;
        public float HoverBoost { get { return hoverBoost; } set { hoverBoost = value; } }

        [Tooltip("Detection distance to oncoming obstacle before applying hover boost and repulsion.")]
        [SerializeField] private float obstacleDetectionRange = 10f;
        public float ObstacleDetectionRange { get { return obstacleDetectionRange; } set { obstacleDetectionRange = value; } }

        [Tooltip("Amount of repulsion force applied to prevent collision with oncoming obstacle within detection range.")]
        [SerializeField] private float repulsionSpeed = 10f;
        public float RepulsionSpeed { get { return repulsionSpeed; } set { repulsionSpeed = value; } }

        [Header("Gizmo Settings")]
        [SerializeField] bool drawMoveDirectionLine = true;

        Rigidbody rb;
        HoverGrid hoverGrid;

        Vector3 moveDir; // Stores input direction (x for steering, y for throttle)

        private void Awake()
        {
            rb = GetComponentInParent<Rigidbody>();
            if (rb == null)
            {
                Debug.LogError(nameof(HoverMovement) + " component could not find a rigidbody component on the gameobject " + gameObject.name +
                    " or on any of its parent gameobjects. Please add a rigidbody component so that physics can be applied.");
                return;
            }
            else
            {
                rb.centerOfMass = Vector3.zero;
            }

            hoverGrid = GetComponent<HoverGrid>();
            if (hoverGrid == null)
            {
                Debug.LogError(nameof(HoverMovement) + " component could not find a " + nameof(HoverGrid) + " component on the gameobject " + gameObject.name
                    + ". Please add a " + nameof(HoverGrid) + " component so that movement can occur.");
            }
        }

        private void FixedUpdate()
        {
            if (hoverGrid == null || rb == null || !enableInput)
            {
                return;
            }

            // Get movement direction based on vertical input (throttle)
            Vector3 moveForce = GetMoveDirection() * moveSpeed;

            // Handle obstacle avoidance
            if (enableObstacleAvoidance)
            {
                Vector3 start = hoverGrid.GetDirectionPointOnGridBounds(moveForce.normalized);

                if (drawMoveDirectionLine)
                {
                    Debug.DrawLine(start, start + (moveForce.normalized * obstacleDetectionRange), Color.red);
                }

                bool isHit = Physics.Raycast(start, moveForce.normalized, out RaycastHit hit, obstacleDetectionRange, obstacleLayers);

                if (isHit)
                {
                    // Apply more repulsion and hover boost when closer to obstacle
                    float closenessMult = (1 - (hit.distance / obstacleDetectionRange));

                    // More repulsion force applied when facing a steep incline
                    float steepnessMult = 1 - Mathf.Clamp(Vector3.Dot(hit.normal, Vector3.up), 0.0f, 1.0f);

                    Vector3 repulsionForce = -moveForce.normalized * closenessMult * repulsionSpeed * steepnessMult;
                    Vector3 hoverForce = new Vector3(0, hoverBoost * closenessMult, 0);

                    rb.AddForce(repulsionForce + hoverForce + moveForce, ForceMode.Acceleration);
                }
                else
                {
                    rb.AddForce(moveForce, ForceMode.Acceleration);
                }
            }
            else
            {
                rb.AddForce(moveForce, ForceMode.Acceleration);
            }

            // Handle steering and tilt based on horizontal input
            float steerInput = moveDir.x;

            // Calculate new rotation angles
            float newYaw = rb.rotation.eulerAngles.y + steerInput * turnSpeed * Time.fixedDeltaTime;
            float newRoll = -steerInput * maxTiltAngle;

            // Create target rotation with new yaw and roll
            Quaternion targetRotation = Quaternion.Euler(newRoll, newYaw, rb.rotation.eulerAngles.z);

            // Smoothly interpolate towards the target rotation
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, tiltSpeed * Time.fixedDeltaTime));
        }

        private Vector3 GetMoveDirection()
        {
            // Use vertical input (moveDir.y) for forward/backward direction
            Vector3 forwardDir = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
            return forwardDir * moveDir.y;
        }

        public void Move(Vector2 moveInput)
        {
            if (!enableInput) return;

            // Separate input into steering (x) and throttle (y)
            moveDir = new Vector3(moveInput.x, moveInput.y, 0f);
        }
    }
}