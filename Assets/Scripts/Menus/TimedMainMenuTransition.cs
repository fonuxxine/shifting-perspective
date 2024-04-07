using UnityEngine;

public class TimedMainMenuTransition : MonoBehaviour
{
    public float delayTime = 3f;
    
    void Start()
    {
        Invoke("LoadMainMenu", delayTime);
    }

    void LoadMainMenu()
    {
        SceneLoader.LoadScene(SceneID.MainMenu);
    }
}