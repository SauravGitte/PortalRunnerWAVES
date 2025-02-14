using UnityEngine;

public class Bullet : MonoBehaviour {
    void Start() {
        // Log that the bullet has been spawned
        Debug.Log("Bullet spawned: " + gameObject.name);
        // Automatically destroy the bullet after 3 seconds to prevent clutter
        Destroy(gameObject, 3f);
    }

    // Called when the bullet's collider makes contact with another collider
    void OnCollisionEnter(Collision collision) {
        Debug.Log("Bullet collided with: " + collision.gameObject.name);

        // Check if the collided object is tagged as "Enemy"
        if (collision.gameObject.CompareTag("Enemy")) {
            Debug.Log("Hit enemy: " + collision.gameObject.name + ". Destroying enemy.");
            // Destroy the enemy
            Destroy(collision.gameObject);
            // Destroy the bullet immediately upon impact
            Destroy(gameObject);
        }
    }
}
