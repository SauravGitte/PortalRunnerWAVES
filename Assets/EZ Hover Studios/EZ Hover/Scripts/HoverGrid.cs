using System.Collections.Generic;
using UnityEngine;

namespace EZHover
{
    [ExecuteInEditMode]
    public class HoverGrid : MonoBehaviour
    {
        [Header("Hover Settings")]
        [SerializeField] private float maxHoverThrust = 30f;
        public float MaxHoverThrust { get { return maxHoverThrust; } set { maxHoverThrust = value; } }

        [SerializeField] private float targetHeight = 4f;
        public float TargetHeight { get { return targetHeight; } set { targetHeight = value; } }

        [SerializeField] private LayerMask hoverableLayers = 1;
        public LayerMask HoverableLayers { get { return hoverableLayers; } set { hoverableLayers = value; } }

        [Header("Grid Settings")]
        [SerializeField] private Vector3 gridSize = new Vector3(10, 0.2f, 10);
        public Vector3 GridSize { get { return gridSize; } set { gridSize = value; } }

        [Range(1, 100)]
        [SerializeField] private int columnCount = 3;
        public int ColumnCount { get { return columnCount; } set { columnCount = value; } }

        [Range(1, 100)]
        [SerializeField] private int rowCount = 3;
        public int RowCount { get { return rowCount; } set { rowCount = value; } }

        [Header("Stabilization")]
        [SerializeField] private float stabilizeForce = 10f;
        public float StabilizeForce { get { return stabilizeForce; } set { stabilizeForce = value; } }

        [SerializeField] private float stabilizationStrength = 1f;
        public float StabilizationStrength { get { return stabilizationStrength; } set { stabilizationStrength = Mathf.Clamp01(value); } }

        [SerializeField] private bool stabilizeZ = true;
        public bool StabilizeZ { get { return stabilizeZ; } set { stabilizeZ = value; } }

        [SerializeField] private bool stabilizeX = false;
        public bool StabilizeX { get { return stabilizeX; } set { stabilizeX = value; } }

        [Header("Gizmo Settings")]
        [SerializeField] private bool alwaysRenderGizmos = false;
        [SerializeField] private Color gridColour = new Color(0, 1, 1, 0.5f);

        private HoverPoint bestHoverPoint;
        private HoverPoint[,] points;
        private Vector3 usedGridSize;
        private Vector3 gridSquareBounds;
        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponentInParent<Rigidbody>();
            if (rb == null)
            {
                Debug.LogError(nameof(HoverGrid) + " component could not find a rigidbody component on the gameobject " + gameObject.name +
                    " or on any of its parent gameobjects. Please add a rigidbody component so that physics can be applied.");
                return;
            }
            rb.centerOfMass = Vector3.zero;

            GenerateHoverPoints();
        }

        private void FixedUpdate()
        {
            if (rb == null) return;

            FindBestHoverPoint();
            ApplyHoverForce();

            Vector3 surfaceNormal = (bestHoverPoint != null && bestHoverPoint.DistanceFromGround != Mathf.Infinity)
                ? bestHoverPoint.GroundNormal
                : Vector3.up;

            Stabilize(surfaceNormal);
        }

        private void Update()
        {
            if (IsPrefab()) return;

            if (points == null ||
                points.GetLength(0) != columnCount ||
                points.GetLength(1) != rowCount ||
                points[0, 0] == null ||
                usedGridSize != gridSize ||
                points[0, 0].HoverableLayers.value != hoverableLayers.value)
            {
                GenerateHoverPoints();
            }

            if (Application.isEditor)
            {
                FindBestHoverPoint();
            }
        }

        private bool IsPrefab()
        {
            return gameObject.scene.path == "";
        }

        private void GenerateHoverPoints()
        {
            Vector3 min = -gridSize / 2;
            Vector3 max = gridSize / 2;
            Vector3 range = max - min;

            gridSquareBounds = new Vector3(range.x / columnCount, gridSize.y, range.z / rowCount);

            points = new HoverPoint[columnCount, rowCount];

            List<HoverPoint> reserves = new List<HoverPoint>(GetComponentsInChildren<HoverPoint>());

            bestHoverPoint = null;

            for (int i = 0; i < columnCount; i++)
            {
                float xPos = ((i * gridSquareBounds.x) + ((i + 1) * gridSquareBounds.x)) / 2;

                for (int j = 0; j < rowCount; j++)
                {
                    HoverPoint point;
                    if (reserves.Count < 1)
                    {
                        GameObject instance = new GameObject("Auto Generated Hover Point");
                        instance.transform.rotation = transform.rotation;
                        instance.transform.parent = transform;
                        point = instance.AddComponent<HoverPoint>();
                    }
                    else
                    {
                        point = reserves[0];
                        reserves.RemoveAt(0);
                    }

                    point.HoverableLayers = hoverableLayers;

                    float zPos = ((j * gridSquareBounds.z) + ((j + 1) * gridSquareBounds.z)) / 2;

                    point.transform.localPosition = new Vector3(min.x + xPos, 0.0f, min.z + zPos);

                    points[i, j] = point;
                }
            }

            while (reserves.Count > 0)
            {
                HoverPoint toDestroy = reserves[0];
                reserves.RemoveAt(0);
                DestroyImmediate(toDestroy.gameObject);
            }

            usedGridSize = gridSize;
        }

        private void FindBestHoverPoint()
        {
            if (bestHoverPoint != null)
            {
                bestHoverPoint.Recalculate(targetHeight, rb);
            }

            for (int i = 0; i < points.GetLength(0); i++)
            {
                for (int j = 0; j < points.GetLength(1); j++)
                {
                    HoverPoint point = points[i, j];
                    if (point == null) continue;

                    point.Recalculate(targetHeight, rb);

                    if (bestHoverPoint == null || point.DistanceFromGround < bestHoverPoint.DistanceFromGround)
                    {
                        bestHoverPoint = point;
                    }
                }
            }
        }

        private void ApplyHoverForce()
        {
            if (bestHoverPoint == null || bestHoverPoint.DistanceFromGround == Mathf.Infinity) return;

            rb.AddForce(maxHoverThrust * (1f - (bestHoverPoint.DistanceFromGround / targetHeight)) * Vector3.up, ForceMode.Acceleration);
        }

        private void Stabilize(Vector3 surfaceNormal)
        {
            if (!stabilizeX && !stabilizeZ) return;

            Vector3 cross = Vector3.Cross(transform.up, surfaceNormal);
            Vector3 torque = new Vector3(
                stabilizeX ? cross.x : 0f,
                0f,
                stabilizeZ ? cross.z : 0f
            );

            rb.AddTorque(torque * stabilizeForce * stabilizationStrength, ForceMode.Acceleration);
        }

        // New method to get a point on the grid bounds in a specific direction
        public Vector3 GetDirectionPointOnGridBounds(Vector3 direction)
        {
            Vector3 localDirection = transform.InverseTransformDirection(direction);
            Vector3 gridExtents = gridSize / 2f;
            Vector3 localPoint = new Vector3(
                Mathf.Clamp(localDirection.x, -gridExtents.x, gridExtents.x),
                0f,
                Mathf.Clamp(localDirection.z, -gridExtents.z, gridExtents.z)
            );
            return transform.TransformPoint(localPoint);
        }

        private void OnDrawGizmos()
        {
            if (alwaysRenderGizmos) RenderGizmos();
        }

        private void OnDrawGizmosSelected()
        {
            if (!alwaysRenderGizmos) RenderGizmos();
        }

        private void RenderGizmos()
        {
            if (points == null || IsPrefab()) return;

            for (int i = 0; i < points.GetLength(0); i++)
            {
                for (int j = 0; j < points.GetLength(1); j++)
                {
                    HoverPoint point = points[i, j];
                    if (point == null) continue;

                    DrawRaycastLine(point);
                    DrawHitSphere(point);
                    DrawGridSquare(point);
                }
            }
        }

        private void DrawRaycastLine(HoverPoint point)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(point.transform.position, point.HitPos);
        }

        private void DrawHitSphere(HoverPoint point)
        {
            Gizmos.color = (point == bestHoverPoint && point.DistanceFromGround != Mathf.Infinity) ? Color.green : Color.yellow;
            Gizmos.DrawSphere(point.HitPos, 0.1f);
        }

        private void DrawGridSquare(HoverPoint point)
        {
            Gizmos.color = gridColour;
            Matrix4x4 currentMatrix = Gizmos.matrix;
            Gizmos.matrix = point.transform.localToWorldMatrix;
            Gizmos.DrawCube(Vector3.zero, gridSquareBounds * 0.95f);
            Gizmos.matrix = currentMatrix;
        }
    }
}