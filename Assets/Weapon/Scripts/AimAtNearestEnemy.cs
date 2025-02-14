using UnityEngine;

public class AimAtNearestEnemy : MonoBehaviour {
    // Rotation speed for smooth aiming
    public float rotationSpeed = 5f;

    void Update() {
        // Find the nearest enemy with the "Enemy" tag
        GameObject nearestEnemy = FindNearestEnemy();

        if (nearestEnemy != null) {
            // Compute the direction from the gun to the enemy
            Vector3 direction = nearestEnemy.transform.position - transform.position;
            // Determine the rotation required to look at the enemy
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            // Smoothly rotate the gun towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    // Helper method to find the nearest enemy GameObject
    GameObject FindNearestEnemy() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
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
