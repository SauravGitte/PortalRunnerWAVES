using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // if collided with gameover collider, load the main menu or the gameeoer scene
    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Collided with " + other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            Cursor.lockState = CursorLockMode.None; // Unlocks cursor
            Cursor.visible = true;
            SceneManager.LoadScene("Main Menu");
        }
    }
}
