using UnityEngine;
using UnityEngine.EventSystems;

public class EnableCubeOnClick : MonoBehaviour, IPointerClickHandler
{
    public GameObject cubeToEnable;

    public void OnPointerClick(PointerEventData eventData)
    {
        cubeToEnable.SetActive(true);
    }
}
