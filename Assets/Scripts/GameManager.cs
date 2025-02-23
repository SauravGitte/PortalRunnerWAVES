using System.ComponentModel.Design.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // gettign the text UI object for score calc
    public GameObject score;
    public int levelSpeed = 1;
    private int startTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = (int)Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // setting the score to be incremented every second
        score.GetComponent<TextMeshProUGUI>().text = ((int)Time.time - startTime).ToString();

        // if the escape key is pressed, the cursor will be unlocked and the main menu will be loaded
        if (Input.GetKeyDown(KeyCode.Escape) == true) {
            Cursor.lockState = CursorLockMode.None; // Unlocks cursor
            Cursor.visible = true;
            SceneManager.LoadScene("Main Menu");
        }
    }

    
}
