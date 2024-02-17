using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuLogic : MonoBehaviour
{
    public void StartGame() {
        SceneManager.LoadScene(3); // load the 'Interior 1' level
    }
}
