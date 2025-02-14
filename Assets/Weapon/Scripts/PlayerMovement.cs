using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start() {
        controller = GetComponent<CharacterController>();
    }

    void Update() {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        float moveX = Input.GetAxis("Horizontal");

        Vector3 move = transform.right * moveX;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
