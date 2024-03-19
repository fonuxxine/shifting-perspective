using UnityEngine;

public class MainMenuLogic : MonoBehaviour
{
    public void StartGame() {
        SceneLoader.LoadScene(SceneID.Intro1); // load the first level
    }
}
