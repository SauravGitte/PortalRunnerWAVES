using EZHover;
using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private float mult = 0.75f; // Speed multiplier
    [SerializeField] private float cooldown = 3f; // Cooldown duration
    private bool canAffectPlayer = true;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hi");
        if (canAffectPlayer && collision.gameObject.CompareTag("Player"))
        {
            HoverMovement hoverMovement = collision.gameObject.GetComponentInChildren<HoverMovement>();
            if (hoverMovement != null)
            {
                hoverMovement.MoveSpeed *= mult;
                StartCoroutine(CooldownRoutine(hoverMovement));
            }
        }
    }

    private IEnumerator CooldownRoutine(HoverMovement hoverMovement)
    {
        canAffectPlayer = false;
        yield return new WaitForSeconds(cooldown);
        hoverMovement.MoveSpeed /= mult; // Reset speed to normal
        canAffectPlayer = true;
    }
}
