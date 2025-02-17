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

    private void Start()
    {
        runner = GetComponentInParent<Runner>();
        currentSpeed = minSpeed;
        speedIncrement = (maxSpeed - minSpeed) / speedIncreaseDuration;
        StartCoroutine(IncreaseSpeed());
        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;
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
        if (other.gameObject.CompareTag("Player"))
        {
            Cursor.lockState = CursorLockMode.None; // Unlocks cursor
            Cursor.visible = true;
            loader.LoadScene("Main Menu");
        }
    }
}
