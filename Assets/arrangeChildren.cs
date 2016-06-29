using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]

public class arrangeChildren : MonoBehaviour {
	void Start () {
        arrange();
    }
    public void arrange() {
        float height = 0f, heightPlaced = 0f;
        foreach (Transform child in transform) height += child.GetComponent<RectTransform>().rect.height;
        height /= 2;
        for (int i = 0; i<transform.childCount; i++){
            Transform child = transform.GetChild(i);
            RectTransform rt = child.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = new Vector2(0f, height - heightPlaced - rt.rect.height/2);
            heightPlaced += rt.rect.height;
        }
    }
}

[CustomEditor(typeof(arrangeChildren))]
public class arrangeChildrenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        arrangeChildren ac = (arrangeChildren)target;
        if (GUILayout.Button("Arrange"))
        {
            ac.arrange();
        }
    }
}