using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StateToScene : MonoBehaviour, IPointerClickHandler
{
    public string sceneNameToLoad;

    private void Start()
    {
        // State marker objects contain their image and collider together. Disable
        // unsupported parents so no legacy thumbnail or invisible hit target remains.
        if (!TechScapeStateData.IsSupportedState(sceneNameToLoad))
            gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!TechScapeStateData.IsSupportedState(sceneNameToLoad))
            return;
        var stateId = sceneNameToLoad == "Gujrat" ? "Gujarat" : sceneNameToLoad;
        TechScapeStateData.SelectState(stateId);
        SceneManager.LoadScene(stateId);
    }
}
