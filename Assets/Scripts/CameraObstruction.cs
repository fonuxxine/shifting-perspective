using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraObstruction : MonoBehaviour
{ 
    public float fadeSpeed = 0.5f; // Adjust the speed of fading

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entered object has a renderer
        Renderer renderer = other.GetComponent<Renderer>();
        Debug.Log("Entered" + other.name);
        if (renderer != null)
        {
            Debug.Log("Renderer found" + other.name);
            // Start a coroutine to gradually change the alpha value
            StartCoroutine(FadeObject(renderer.material, 1f, 0.4f));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the exited object has a renderer
        Renderer renderer = other.GetComponent<Renderer>();
        Debug.Log("Exited" + other.name);
        if (renderer != null)
        {
            // Start a coroutine to gradually change the alpha value
            StartCoroutine(FadeObject(renderer.material, 0.4f, 1f));
        }
    }

    private IEnumerator FadeObject(Material material, float startAlpha, float targetAlpha)
    {
        Color startColor = material.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);

        float elapsedTime = 0f;

        while (elapsedTime < fadeSpeed)
        {
            material.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        material.color = targetColor; // Ensure the target alpha is set accurately
    }
}
