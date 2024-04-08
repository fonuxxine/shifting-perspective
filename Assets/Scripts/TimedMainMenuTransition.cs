using UnityEngine;

public class TimedMainMenuTransition : MonoBehaviour
{
    public float delayTime = 1000f;
    
    void Start()
    {
        Invoke("LoadMainMenu", delayTime);
    }

    void LoadMainMenu()
    {
        SceneLoader.LoadScene(SceneID.MainMenu);
    }
}