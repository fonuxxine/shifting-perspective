using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraObstruction : MonoBehaviour
{ 
    public float fadeSpeed = 0.5f; // Adjust the speed of fading
    public float fadeAmount = 0.4f;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entered object has a renderer
        Renderer renderer = other.GetComponent<Renderer>();
        if (renderer != null)
        {
            Debug.Log("Entered" + other.name);
            Debug.Log("Renderer found" + other.name);
            // Start a coroutine to gradually change the alpha value
            StartCoroutine(FadeOut(renderer.material));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the exited object has a renderer
        Renderer renderer = other.GetComponent<Renderer>();
        if (renderer != null)
        {
            // Start a coroutine to gradually change the alpha value
            Debug.Log("Exited" + other.name);
            StartCoroutine(FadeIn(renderer.material));
        }
    }

    private IEnumerator FadeOut(Material mat)
    {
        StandardShaderUtils.ChangeTransparency(mat, true);

        float steps = (1 - fadeAmount) / 0.01f;
        double timeStep = fadeSpeed / steps;

        float opacity = 1f;
        while (opacity >= fadeAmount)
        {
            opacity -= 0.01f;

            Color c1 = mat.color;
            c1.a = opacity;
            mat.color = c1;

            yield return new WaitForSeconds((float)timeStep);
        }
        Color c = mat.color;
        c.a = opacity;
        mat.color = c;
    }

    private IEnumerator FadeIn(Material mat)
    {

        float steps = (1 - fadeAmount) / 0.01f;
        double timeStep = fadeSpeed / steps;

        float opacity = fadeAmount;
        while (opacity < 1f)
        {
            opacity += 0.01f;
            Color c1 = mat.color;
            c1.a = opacity;
            mat.color = c1;

            yield return new WaitForSeconds((float)timeStep);
        }

        Color c = mat.color;
        c.a = opacity;
        mat.color = c;

        StandardShaderUtils.ChangeTransparency(mat, false);

    }
}
