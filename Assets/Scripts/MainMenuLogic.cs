using UnityEngine;

public class MainMenuLogic : MonoBehaviour
{
    public void StartGame() {
        SceneLoader.LoadScene(SceneID.ExtLvl); // load the first level
    }
}
