using UnityEngine;

public class MainMenuLogic : MonoBehaviour
{
    public void StartGame() {
        SceneLoader.LoadScene(SceneID.ExtLevel); // load the first level
    }
}
