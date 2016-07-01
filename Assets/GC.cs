using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEditor;

public class ReadOnlyAttribute : PropertyAttribute{
}
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label){
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
    public override void OnGUI(Rect position,SerializedProperty property,GUIContent label){
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}

public static class TransformEx {
    /// <summary>Destroys all children within this Transform</summary>
    public static Transform Clear(this Transform transform) {
        foreach (Transform child in transform) GameObject.Destroy(child.gameObject);
        return transform;
    }
    /// <summary>Instantiates inside of the parent</summary>
    public static Transform instantiate(this Transform Prefab, Transform parent){
        Transform newTransform = UnityEngine.Object.Instantiate(Prefab);
        newTransform.SetParent(parent);
        newTransform.localPosition = Vector3.zero;
        return newTransform;
    }
    /// <summary>Changes the transform's name</summary>
    public static Transform setName(this Transform transform, string newName) {
        transform.name = newName;
        return transform;
    }
    /// <summary>Returns RectTransform Component</summary>
    public static RectTransform rt(this Transform transform) { return transform.GetComponent<RectTransform>(); }
    /// <summary>Returns Button Component</summary>
    public static Button button(this Transform transform) { return transform.GetComponent<Button>(); }
    /// <summary>Returns InputField Component</summary>
    public static InputField inputField(this Transform transform) { return transform.GetComponent<InputField>(); }
    /// <summary>Returns Text Component</summary>
    public static Text text(this Transform transform) { return transform.GetComponent<Text>(); }
}
public static class RectTransformEx {
    public enum AnchorPoint { TopLeft, TopCenter, TopRight, Left, Center, Right, BotLeft, BotCenter, BotRight }
    /// <summary>Anchors to a different Anchor Point</summary>
    public static RectTransform Anchor(this RectTransform rt, AnchorPoint ap) {
        Vector2 min = Vector2.zero, max = Vector2.zero, pivot = Vector2.zero;
        switch (ap){
            case AnchorPoint.TopLeft:
                min = new Vector2(0f, 1f);
                max = new Vector2(0f, 1f);
                pivot = new Vector2(0f, 1f);
                break;
            case AnchorPoint.TopCenter:
                min = new Vector2(0.5f, 1f);
                max = new Vector2(0.5f, 1f);
                pivot = new Vector2(0.5f, 1f);
                break;
            case AnchorPoint.TopRight:
                min = new Vector2(1f, 1f);
                max = new Vector2(1f, 1f);
                pivot = new Vector2(1f, 1f);
                break;
            case AnchorPoint.Left:
                min = new Vector2(0f, 0.5f);
                max = new Vector2(0f, 0.5f);
                pivot = new Vector2(0f, 0.5f);
                break;
            case AnchorPoint.Center:
                min = new Vector2(0.5f, 0.5f);
                max = new Vector2(0.5f, 0.5f);
                pivot = new Vector2(0.5f, 0.5f);
                break;
            case AnchorPoint.Right:
                min = new Vector2(1f, 0.5f);
                max = new Vector2(1f, 0.5f);
                pivot = new Vector2(1f, 0.5f);
                break;
            case AnchorPoint.BotLeft:
                min = new Vector2(0f, 0f);
                max = new Vector2(0f, 0f);
                pivot = new Vector2(0f, 0f);
                break;
            case AnchorPoint.BotCenter:
                min = new Vector2(0.5f, 0f);
                max = new Vector2(0.5f, 0f);
                pivot = new Vector2(0.5f, 0f);
                break;
            case AnchorPoint.BotRight:
                min = new Vector2(1f, 0f);
                max = new Vector2(1f, 0f);
                pivot = new Vector2(1f, 0f);
                break;
        }
        rt.anchorMin = min;
        rt.anchorMax = max;
        rt.pivot = pivot;
        return rt;
    }
    public static RectTransform Move(this RectTransform rt, float x, float y) {
        rt.anchoredPosition = new Vector2(x, y);
        return rt;
    }
}

[Serializable]
public class GC : MonoBehaviour {
    [Serializable] public class Camera_ {
        [HideInInspector]
        public Camera c;
        public Color From, To;
        private bool side = true;
        public float time = 1f;
        private float progress =0f;
        public void flash() {
            if (side) {
                progress += Time.deltaTime;
                if (progress > time) {
                    progress = time * 2 - progress;
                    side = false;
                }
            }
            else {
                progress -= Time.deltaTime;
                if (progress < 0) {
                    progress *= -1;
                    side = true;
                }
            }
            c.backgroundColor = Color.Lerp(From, To, progress / time);
        }
    }
    [Serializable] public static class ScenePrefabs {
        public static Transform defaultScene, startMenu;
        public static Transform infoBarPrefab, buttonPrefab, inputFieldPrefab;
    }
    public new Camera_ camera;
    public static Player player = new Player();
    public static Transform Canvas;
    void Start(){
        Canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        camera.c = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Scenes.load(typeof(Scenes.startMenu));
    }
    void Update() {
        camera.flash();
    }
    public void CreateNewCharacter(string name){
        player.name = name;
        player.hp.max = 1;
        player.hp.current = 1;
        Scenes.load(typeof(Scenes.play));
    }
}
