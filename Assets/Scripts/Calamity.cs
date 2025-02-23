using Dreamteck.Forever;
using System.Collections;
using UnityEngine;

public class Calamity : MonoBehaviour
{
    public float minSpeed = 5f;
    public float maxSpeed = 35f;
    public float speedIncreaseDuration = 10f; // Time to reach max speed
    private Runner runner;
    private float currentSpeed;
    private float speedIncrement;
    [SerializeField]
    private SceneLoader loader;
    public GameObject deathBox;
    public GameObject scoreTextGame;
    public GameObject scoreMain;
    public GameObject scoreText;
    private int deathScore;

    private void Start()
    {
        
        runner = GetComponentInParent<Runner>();
        currentSpeed = minSpeed;
        speedIncrement = (maxSpeed - minSpeed) / speedIncreaseDuration;
        StartCoroutine(IncreaseSpeed());
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1;
        Cursor.visible = false;
        deathBox.SetActive(false);
    }

    private IEnumerator IncreaseSpeed()
    {
        float elapsedTime = 0f;
        while (elapsedTime < speedIncreaseDuration)
        {
            currentSpeed += speedIncrement * Time.deltaTime;
            runner.followSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        runner.followSpeed = maxSpeed; // Ensure it caps at max speed
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with " + other.gameObject.name);
        // if (other.gameObject.CompareTag("Player"))
        // {
        //     Cursor.lockState = CursorLockMode.None; // Unlocks cursor
        //     Cursor.visible = true;
        //     loader.LoadScene("Main Menu");
        // }
        
        if (other.gameObject.CompareTag("Player"))
        {
            // getting last score from the scoretext object
            deathScore = int.Parse(scoreTextGame.GetComponent<TMPro.TextMeshProUGUI>().text);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // setting the scoretext to deathScore
            scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = "Score: " + deathScore.ToString();
            scoreTextGame.SetActive(false);
            scoreMain.SetActive(false);
            Time.timeScale = 0;
            deathBox.SetActive(true);
        }
    }
}
