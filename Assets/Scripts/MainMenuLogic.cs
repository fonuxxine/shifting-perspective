using UnityEngine;

public class MainMenuLogic : MonoBehaviour
{
    public void StartGame() {
        SceneLoader.LoadScene(SceneID.SampleScene); // load the first level
    }
}
