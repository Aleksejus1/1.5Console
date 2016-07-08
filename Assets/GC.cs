using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;

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

public static class TextEx {
    ///<summary>Sets the state of Best Fit</summary>
    public static Text bestFit(this Text text, bool state) { text.resizeTextForBestFit = state; return text; }
    ///<summary>Sets the color of text</summary>
    public static Text setColor(this Text text, Color color) { text.color = color; return text; }
    ///<summary>Sets the color of text</summary>
    public static Text setColor(this Text text, int r, int g, int b, int a) { return setColor(text, new Color(r / 255f, g / 255f, b / 255f, a / 255f)); }
    ///<summary>Sets the font Size</summary>
    public static Text fontSize(this Text text, int size, bool additive = false) { if(additive) text.fontSize+=size; else text.fontSize = size; return text; }
}
public static class TransformEx {
    public static Transform addTimer(this Transform t, float time, Func<Action> endTimeFunction, Func<Action> updateTimeFunction = null, Func<Action> resetFunction = null) {
        Timer ti = t.gameObject.AddComponent<Timer>();
        ti.time = time;
        ti.endFunction = endTimeFunction;
        ti.updateFunction = updateTimeFunction;
        ti.resetFunction = resetFunction;
        return t;
    }
    public static Timer timer(this Transform t) {
        return t.GetComponent<Timer>();
    }
    ///<summary>Enables GameObject</summary>
    public static Transform newButton(this Transform t, Graphic targetGraphic = null) {
        t.gameObject.AddComponent<OnClickRelay>();
        Button b = t.gameObject.AddComponent<Button>();
        b.targetGraphic = targetGraphic;
        Color hexColor = new Color();
        ColorBlock cb = b.colors;
        if(ColorUtility.TryParseHtmlString("#FFFFFFFF", out hexColor)) cb.normalColor       = hexColor;
        if(ColorUtility.TryParseHtmlString("#C8C8C8FF", out hexColor)) cb.highlightedColor  = hexColor;
        if(ColorUtility.TryParseHtmlString("#919191FF", out hexColor)) cb.pressedColor      = hexColor;
        if(ColorUtility.TryParseHtmlString("#5A5A5AFF", out hexColor)) cb.disabledColor     = hexColor;
        b.colors = cb;
        Navigation n = b.navigation; n.mode = Navigation.Mode.None; b.navigation = n;
        return t;
    }
    ///<summary>Enables GameObject</summary>
    public static Transform enable(this Transform transform) { transform.gameObject.SetActive(true); return transform; }
    ///<summary>Disables GameObject</summary>
    public static Transform disable(this Transform transform) { transform.gameObject.SetActive(false); return transform; }
    ///<summary>Sets the Text Component' state of Best Fit</summary>
    public static Transform bestFit(this Transform transform, bool state) { transform.text().bestFit(state); return transform; }
    ///<summary>Sets the Text Component' font Size</summary>
    public static Transform fontSize(this Transform transform, int size, bool additive = false) { transform.text().fontSize(size,additive); return transform; }
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
        transform.gameObject.name = transform.name = newName;
        return transform;
    }
    /// <summary>Returns RectTransform Component</summary>
    public static RectTransform rt(this Transform transform) { return transform.GetComponent<RectTransform>(); }
    /// <summary>Returns Button Component</summary>
    public static Button button(this Transform transform) { return transform.GetComponent<Button>(); }
    /// <summary>Sets the Listener for Button Component</summary>
    public static Transform setButton(this Transform transform, UnityEngine.Events.UnityAction call) { transform.button().onClick.AddListener(call); return transform; }
    /// <summary>Returns InputField Component</summary>
    public static InputField inputField(this Transform transform) { return transform.GetComponent<InputField>(); }
    /// <summary>Sets InputField's Placeholder Text</summary>
    public static Transform setIFPHText(this Transform transform, string text) { transform.GetChild(0).setText(text); return transform; }
    /// <summary>Sets InputField's Input Limit</summary>
    public static Transform setIFLimit(this Transform transform, int limit) { transform.inputField().characterLimit = limit; return transform; }
    /// <summary>Sets Text component's text</summary>
    public static Transform setText(this Transform transform, string text) { transform.text().text = text; return transform; }
    /// <summary>Returns Text Component</summary>
    public static Text text(this Transform transform) { return transform.GetComponent<Text>(); }
}
public enum AnchorPoint { TopLeft, TopCenter, TopRight, Left, Center, Right, BotLeft, BotCenter, BotRight }
public static class RectTransformEx {
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
    public static RectTransform TextAdjustWidth(this RectTransform rt) {
        rt.gameObject.AddComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
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
    [Serializable] public class ScenePrefabs{
        public Transform defaultScene, startMenu, combatPrefab;
        public Transform infoBarPrefab, buttonPrefab, inputFieldPrefab, textPrefab;
    }
    public ScenePrefabs scenePrefabs;
    public static Transform lastButton;
    public new Camera_ camera;
    public static Player player = new Player();
    public static Transform Canvas;
    void Start(){
        Scenes.sp = scenePrefabs;
        Canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        camera.c = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        CreateNewCharacter("Alek");
        Scenes.load(typeof(Scenes.play));
    }
    void Update() {
        camera.flash();
        if(Input.GetKeyDown(KeyCode.M)) player.hp.Current--;
    }
    public static void CreateNewCharacter(string name){
        player.name = name;
        player.hp.Current = player.hp.Max = 100;
        player.location = new Locations.Wilderness();
    }
}