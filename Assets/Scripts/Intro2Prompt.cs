using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro2Prompt : MonoBehaviour
{
    public string sceneName; // name of scene to transition to
    public GameObject targetObject; // Reference to the object whose rotation you want to check
    private void Update()
    {
        // Check if the parent object exists and its rotation is 180 degrees in the Y direction
        if (targetObject.transform.eulerAngles.z == 180f)
        {
            // Enable this child object
            gameObject.GetComponent<Canvas>().enabled = true;
        }
        if (gameObject.GetComponent<Canvas>().isActiveAndEnabled &&
            (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}