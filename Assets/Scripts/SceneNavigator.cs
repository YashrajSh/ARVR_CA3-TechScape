using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator: MonoBehaviour
{
    private static readonly string[] TechScapeDetailScenes =
    {
        "Karnataka", "Maharashtra", "Telangana", "Tamil Nadu", "Kerala",
        "Gujarat", "Uttar Pradesh", "West Bengal", "Rajasthan"
    };

    public void GoToScene(string sceneName)
    {
        if (TechScapeStateData.GetState(sceneName) == null && sceneName != "Main" && sceneName != "Onboarding")
            return;
        SceneManager.LoadScene(sceneName);
    }
    public void GoBack()
    {
        // Detail scenes are not arranged geographically in Build Settings; return
        // to the AR map instead of accidentally navigating to another state.
        if (TechScapeStateData.GetState(SceneManager.GetActiveScene().name) != null)
        {
            SceneManager.LoadScene("Main");
            return;
        }

        SceneManager.LoadScene("Main");
    }
    public void NextScene()
    {
        NavigateDetailScene(1);
    }

    public void PreviousScene() => NavigateDetailScene(-1);

    private static void NavigateDetailScene(int direction)
    {
        var current = SceneManager.GetActiveScene().name;
        var index = System.Array.IndexOf(TechScapeDetailScenes, current);
        if (index < 0) return;
        var next = (index + direction + TechScapeDetailScenes.Length) % TechScapeDetailScenes.Length;
        SceneManager.LoadScene(TechScapeDetailScenes[next]);
    }
}
