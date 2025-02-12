using UnityEngine;
using Dreamteck.Forever;

public class Basic_Movement : MonoBehaviour
{
    public float jumpForce = 10f, moveSpeed = 5;
    private Runner runner;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        runner = GetComponent<Runner>();
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 pos = runner.motion.offset;
        float horizontalInput = Input.GetAxis("Horizontal");
        pos.x += horizontalInput * moveSpeed * Time.deltaTime;
        runner.motion.offset.Set(pos.x, pos.y);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.GetContact(0).otherCollider.CompareTag("Obstacle"))
    //    {
    //        Debug.Log("Collided with obstacle");
    //        runner.follow = false;
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.GetContact(0).otherCollider.CompareTag("Obstacle"))
    //    {
    //        runner.follow = true;
    //    }
    //}
}
