using UnityEngine;

public class GunAutoAimReturn : MonoBehaviour {
    [Header("Rotation Settings")]
    public float aimRotationSpeed = 5f;    // Speed for auto-aim rotation
    public float returnRotationSpeed = 5f; // Speed for returning to original rotation

    // Variable to store the original rotation of the gun
    private Quaternion originalRotation;

    void Start() {
        // Save the gun's original rotation at startup
        originalRotation = transform.rotation;
    }

    void Update() {
        // Find the nearest enemy in the scene
        GameObject nearestEnemy = FindNearestEnemy();

        if (nearestEnemy != null) {
            // Calculate the direction from the gun to the enemy
            Vector3 direction = nearestEnemy.transform.position - transform.position;
            // Determine the target rotation to face the enemy
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            // Smoothly rotate the gun towards the enemy
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * aimRotationSpeed);
        } else {
            // No enemy found: smoothly rotate the gun back to its original rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, Time.deltaTime * returnRotationSpeed);
        }
    }

    // Helper method to find the nearest enemy with the tag "Enemy"
    GameObject FindNearestEnemy() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) {
            return null;
        }

        GameObject nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies) {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance) {
                minDistance = distance;
                nearest = enemy;
            }
        }

        return nearest;
    }
}
