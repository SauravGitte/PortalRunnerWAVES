// Modified HoverPoint.cs
using UnityEngine;

namespace EZHover
{
    public class HoverPoint : MonoBehaviour
    {
        public Vector3 HitPos { get; set; } = Vector3.zero;
        public float DistanceFromGround { get; private set; } = Mathf.Infinity;
        public Vector3 GroundNormal { get; private set; } = Vector3.up; // New property
        public LayerMask HoverableLayers { get; set; }

        public void Recalculate(float maxDistance, Rigidbody rb)
        {
            RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.down, maxDistance, HoverableLayers.value);

            float closest = Mathf.Infinity;
            RaycastHit closestHit = new RaycastHit();

            foreach (RaycastHit hit in hits)
            {
                if (hit.rigidbody != rb && hit.distance < closest)
                {
                    closestHit = hit;
                    closest = hit.distance;
                }
            }

            if (closestHit.collider == null)
            {
                DistanceFromGround = Mathf.Infinity;
                HitPos = transform.position + (Vector3.down * maxDistance);
                GroundNormal = Vector3.up; // Default to up when no hit
            }
            else
            {
                DistanceFromGround = closestHit.distance;
                HitPos = closestHit.point;
                GroundNormal = closestHit.normal; // Store the surface normal
            }
        }
    }
}