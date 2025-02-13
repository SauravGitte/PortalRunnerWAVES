using UnityEngine;
using System.Collections;

public class Dissolve : MonoBehaviour
{
    public Material dissolveMaterial; // The dissolve material
    public float dissolveSpeed = 1.5f; // Speed of dissolve animation

    public Renderer gateRenderer;
    private float dissolveAmount = 0f;
    private bool isDissolving = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isDissolving)
        {
            isDissolving = true;
            gateRenderer.material = dissolveMaterial; // Switch to dissolve material
            StartCoroutine(DissolveEffect());
        }
    }

    private IEnumerator DissolveEffect()
    {
        while (dissolveAmount < 1f)
        {
            dissolveAmount += Time.deltaTime * dissolveSpeed;
            gateRenderer.material.SetFloat("_DissolveAmount", dissolveAmount);
            yield return null;
        }

        gameObject.SetActive(false); // Disable the gate after full dissolve
    }
}
