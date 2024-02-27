using UnityEngine.SceneManagement;

public enum SceneID {
    MainMenu,
    ExtLevel,
    IntLevel1,
    ToBeContinued
}

public static class SceneLoader {
    // get scene name based on SceneID
    public static string GetSceneName(SceneID sceneID) {
        return sceneID.ToString();
    }
    
    // load a scene based on its enum value
    public static void LoadScene(SceneID sceneID) {
        SceneManager.LoadScene(GetSceneName(sceneID));
    }

    // load a scene based on its build index
    public static void LoadSceneByBuildIndex(int buildIndex) {
        SceneManager.LoadScene(buildIndex);
    }
}
