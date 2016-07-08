using System;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void ActionPointer(PointerEventData e);

public class mouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private ActionPointer enter = null, exit = null;
    public void OnPointerEnter(PointerEventData eventData) { if(enter!=null) enter(eventData); }
    public void OnPointerExit(PointerEventData eventData) { if (exit != null) exit(eventData); }
    public void setFunctions(ActionPointer enter, ActionPointer exit) { this.enter = enter; this.exit = exit; }
}