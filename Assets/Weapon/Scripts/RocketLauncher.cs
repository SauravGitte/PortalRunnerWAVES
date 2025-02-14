using UnityEngine;
using BigRookGames.Weapons;

public class RocketLauncher : MonoBehaviour
{
    Quaternion originalRot;
    public int  radius = 20;
    GunfireController fireScript;

    [Header("Shooting")]
    public float reloadTime = 2f;
    private float nextShootTime = 0f;

    void Start()
    {
        originalRot = transform.rotation;
        fireScript = GetComponentInChildren<GunfireController>();
    }

    void Update()
    {
        GameObject nearestEnemy = FindNearestWall();
        // Debug.Log(nearestEnemy.name);
        if (nearestEnemy != null) {
            // Calculate the direction from the gun to the enemy
            Vector3 direction = nearestEnemy.transform.position - transform.position;
            // Determine the target rotation to face the enemy
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            // Smoothly rotate the gun towards the enemy
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            if (Time.time > nextShootTime) {
                nextShootTime = Time.time + reloadTime;
                fireScript.FireWeapon();
            }
        } else {
            // smoothly rotates back to original rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRot, Time.deltaTime * 5f);
        }
    } 

    GameObject FindNearestWall() {
        // to find a gameobject in a sphere of radius with the tag "Wall"
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        if (hitColliders.Length == 0) {
            return null;
        }

        GameObject nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider hitCollider in hitColliders) {
            if (hitCollider.tag != "Obstacle") {
                continue;
            }
            float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
            if (distance < minDistance) {
                minDistance = distance;
                nearest = hitCollider.gameObject;
            }
        }
        // if (nearest != null){
        // Debug.Log(nearest.name + " is near");}
        return nearest;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
