using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
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
    public enum Scenes { startMenu, newGame, play, fight, none }
    [Serializable] public class Player {
        public string name = "Alek";
        public class Health {
            public double max, current;
            public Health(double c, double m) {
                current = c;
                max = m;
            }
            public override string ToString(){ return current + "/" + max; }
        }
        public Health hp = new Health(0, 0);
        public Location location;
    }
    [Serializable] public class Scenes_{
        [Serializable]
        public class InfoBar {
            public Transform transform;
            public Text health, name, location;
            private Player player;
            public InfoBar(Transform t, Player player) {
                transform = t;
                health = t.FindChild("Health").text();
                name = t.FindChild("Name").text();
                location = t.FindChild("Location").text();
                this.player = player;
                updateAll();
            }
            public void destroy() {
                if(transform) Destroy(transform.gameObject);
                transform = null;
                health = null;
                name = null;
                location = null;
            }
            public void updateAll() { updateHealth(); updateName(); updateLocation(); }
            public void updateHealth() { health.text = "Health: " + player.hp.ToString(); }
            public void updateName() { name.text = player.name; }
            public void updateLocation() { location.text = player.location.ToString(); }
        }
        [ReadOnly] public Scenes current = Scenes.none;
        public Transform buttonPrefab,inputFieldPrefab,infoBarPrefab;
        public Transform startMenu, defaultScene;
        [HideInInspector] public List<InputField> inputFields;
        private Transform scene, options;
        public InfoBar infoBar = null;
        [HideInInspector] public GC gc;
        [HideInInspector] public Player player;
        public void init(GC g, Player p) {
            gc = g;
            player = p;
        }
        public void load(Scenes sceneID, bool keepInfoBar = false){
            unloadCurrentScene(keepInfoBar);
            switch (sceneID) {
                case Scenes.startMenu:
                    (scene = _Instantiate(startMenu, Canvas)).FindChild("NewGame").button().onClick.AddListener(() => load(Scenes.newGame));
                    break;
                case Scenes.newGame:
                    scene = instantiateDefault();
                    addInputField(options, "Character Name...").setName("Name").inputField().characterLimit = 18;
                    addButton(scene, "Create").rt().Anchor(RectTransformEx.AnchorPoint.BotCenter).Move(0f, 10f).transform.setName("Create").button().onClick.AddListener(() => gc.CreateNewCharacter(inputFields[0].text));
                    break;
                case Scenes.play:
                    scene = instantiateDefault();
                    addInfoBar();
                    break;
                case Scenes.fight:
                    scene = instantiateDefault();
                    
                    break;
            }
            current = sceneID;
        }
        private void addFightButton() {
            addButton(options, "Fight").button().onClick.AddListener(() =>load(Scenes.fight, true));
        }
        private Transform addInfoBar() {
            if (infoBar != null){
                Transform retVal = _Instantiate(infoBarPrefab, scene);
                retVal.rt().anchoredPosition = Vector2.zero;
                infoBar = new InfoBar(retVal, player);
                return retVal;
            }
            else return null;
        }
        private Transform instantiateDefault(){
            Transform retVal = _Instantiate(defaultScene, Canvas);
            options = retVal.FindChild("Options");
            return retVal;
        }
        private Transform addButton(Transform parent, string text){
            Transform button = _Instantiate(buttonPrefab, parent);
            button.text().text = text;
            return button;
        }
        private Transform addInputField(Transform parent, string placeHolderText){
            Transform inputField = _Instantiate(inputFieldPrefab, parent);
            inputField.GetChild(0).text().text = placeHolderText;
            inputFields.Add(inputField.GetComponent<InputField>());
            return inputField;
        }
        private void unloadCurrentScene(bool keepInfoBar) {
            if (!keepInfoBar&&infoBar!=null) infoBar.destroy();
            inputFields.Clear();
            if(scene)Destroy(scene.gameObject);
        }
    }
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
    public new Camera_ camera;
    public Scenes_ scenes;
    public Player player = new Player();
    public static Transform Canvas;
    void Start(){
        scenes.init(this, player);
        Canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        camera.c = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        scenes.load(Scenes.startMenu);
    }
    void Update() {
        camera.flash();
    }
    private static Transform _Instantiate(Transform Prefab, Transform parent) {
        Transform newTransform = Instantiate(Prefab);
        newTransform.SetParent(parent);
        newTransform.localPosition = Vector3.zero;
        return newTransform;
    }
    public void CreateNewCharacter(string name){
        player.name = name;
        player.hp.max = 1;
        player.hp.current = 1;
        scenes.load(Scenes.play);
    }
}
