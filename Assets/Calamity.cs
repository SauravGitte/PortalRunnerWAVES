using UnityEngine;

public class Calamity : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with " + other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {

            Debug.Log("Game Over");
            Time.timeScale = 0f;
        }
    }
}
