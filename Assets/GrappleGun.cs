using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class GrappleGun : MonoBehaviour
{
    public LayerMask canGrapple;
    public Transform Player;
    public float maxD, minD, spring, damper, massF;

    private LineRenderer lineRenderer;
    private Vector3 grapplePoint;
    private SpringJoint joint;
    private Color defaultColor = Color.green; // Default color for laser
    private Color grappleColor = Color.red; // Color when grappling

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startColor = defaultColor;
        lineRenderer.endColor = defaultColor;
        lineRenderer.startWidth = 0.1f; // Set width for laser effect
        lineRenderer.positionCount = 2;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }
        if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }
    }
    private void LateUpdate()
    {
        DrawLine();
    }

    void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 1000f, canGrapple))
        {
            Debug.Log(hit.collider.gameObject.name);
            grapplePoint = hit.point;
            joint = Player.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distancePoint = Vector3.Distance(Player.position, grapplePoint);

            joint.maxDistance = distancePoint * maxD;
            joint.minDistance = distancePoint * minD;
            joint.spring = spring;
            joint.damper = damper;
            joint.massScale = massF;

            // Change line color to grapple color
            lineRenderer.startColor = grappleColor;
            lineRenderer.endColor = grappleColor;
        }
    }

    void StopGrapple()
    {
        Destroy(joint);

        // Reset line color to default
        lineRenderer.startColor = defaultColor;
        lineRenderer.endColor = defaultColor;
    }

    void DrawLine()
    {
        if (joint)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, grapplePoint);
        }
        else
        {
            // Render the line renderer in the forward direction of the camera
            Vector3 endPosition = Camera.main.transform.position + Camera.main.transform.forward * 100f; // 100f is the distance
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, endPosition);
        }
    }
}
