using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro1Prompt : MonoBehaviour
{
    public string sceneName; // name of scene to transition to
    private void Update()
    {
        // Check if the parent object exists and its rotation is 180 degrees in the Y direction
        if (transform.parent.eulerAngles.y == 180f)
        {
            // Enable this child object
            gameObject.GetComponent<Canvas>().enabled = true;
        }
        if (gameObject.GetComponent<Canvas>().isActiveAndEnabled &&
            (Input.GetButtonDown("Submit") || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
