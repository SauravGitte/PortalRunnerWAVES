using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoGun : MonoBehaviour {
    [Header("Bullet Settings")]
    // Assign your bullet prefab in the Inspector
    public GameObject bulletPrefab;
    // The point where bullets spawn (for example, a child "FirePoint")
    public Transform firePoint;
    // Speed of the bullet when fired
    public float bulletSpeed = 20f;
    // Time between shots
    public float fireRate = 0.5f;

    // Internal timer to control auto-fire rate
    private float nextFireTime = 0f;

    // Update is called once per frame
    void Update() {
        // Find the nearest enemy (with tag "Enemy")
        GameObject nearestEnemy = GetNearestEnemy();

        if (nearestEnemy != null) {
            // Calculate direction from firePoint to enemy
            Vector3 direction = nearestEnemy.transform.position - firePoint.position;
            // Determine the rotation needed to look at the enemy
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            // Smoothly rotate the firePoint toward the enemy
            firePoint.rotation = Quaternion.Slerp(firePoint.rotation, lookRotation, Time.deltaTime * 5f);

            // Auto-fire: check fire rate timer and ensure there are less than 5 bullets in the scene
            if (Time.time >= nextFireTime && GameObject.FindGameObjectsWithTag("Bullet").Length < 5) {
                FireBullet();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    // Instantiates the bullet and sets its velocity
    void FireBullet() {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        // Tag the bullet so we can count it (make sure the prefab�s tag is set to "Bullet" or assign it here)
        bullet.tag = "Bullet";

        // Try to get the Rigidbody component to apply velocity
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null) {
            rb.linearVelocity = firePoint.forward * bulletSpeed;
        }
    }

    // Returns the nearest GameObject with tag "Enemy", or null if none exist
    GameObject GetNearestEnemy() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies) {
            float distance = Vector3.Distance(firePoint.position, enemy.transform.position);
            if (distance < minDistance) {
                minDistance = distance;
                nearest = enemy;
            }
        }

        return nearest;
    }
}
