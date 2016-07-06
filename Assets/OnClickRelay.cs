using UnityEngine;
using UnityEngine.EventSystems;

public class OnClickRelay : MonoBehaviour, IPointerClickHandler {
    public void OnPointerClick(PointerEventData eventData) {
        GC.lastButton = eventData.pointerPress.transform;
    }
}