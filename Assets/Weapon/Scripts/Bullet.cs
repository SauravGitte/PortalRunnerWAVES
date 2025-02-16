using UnityEngine;

public class Bullet : MonoBehaviour {
    void Start() {
        // Log that the bullet has been spawned
        // Automatically destroy the bullet after 3 seconds to prevent clutter
        Destroy(gameObject, 3f);
    }

    // Called when the bullet's collider makes contact with another collider
    void OnCollisionEnter(Collision collision) {

        // Check if the collided object is tagged as "Enemy"
        if (collision.gameObject.CompareTag("Enemy")) {
            // Destroy the enemy
            print(collision.gameObject.name);
            Destroy(collision.gameObject);
            // Destroy the bullet immediately upon impact
            Destroy(gameObject);
        }
    }
}
