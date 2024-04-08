using UnityEngine;

public class TimedSceneTransition : MonoBehaviour
{

    public float delayTime = 1000f;
    public bool skipCredits = true; // true for Main Menu, false for credits
    
    void Start()
    {
        if (skipCredits) {
            Invoke("LoadMainMenu", delayTime);
        } else {
            Invoke("LoadCredits", delayTime);
        }
    }

    void LoadMainMenu()
    {
        SceneLoader.LoadScene(SceneID.MainMenu);
    }


    void LoadCredits()
    {
        SceneLoader.LoadScene(SceneID.ToBeContinued);
    }
}