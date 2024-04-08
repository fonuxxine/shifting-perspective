using UnityEngine.SceneManagement;

public enum SceneID {
    MainMenu,
    Intro1,
    ExtLevel,
    Intro2,
    IntTutLevel,
    IntLevel1,
    FinalLevel,
    EndCredits
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
