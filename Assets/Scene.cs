using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scene {
    public bool hasInfoBar = false;
    public Transform scene;
    protected GC.ScenePrefabs sp;
    protected List<Text> IF = new List<Text>();
    public Scene(GC.ScenePrefabs sp) {
        this.sp = sp;
    }
    public virtual Transform load() {
        unload();
        if (hasInfoBar && Scenes.infoBar == null) addInfoBar();
        return null;
    }
    public virtual void unload() {
        if (!hasInfoBar && Scenes.infoBar != null) Scenes.infoBar.destroy();
        if (Scenes.currentScene) GameObject.Destroy(Scenes.currentScene.gameObject);
    }
    protected Transform loadScene(Transform scenePrefab, Transform parent) { return scene = scenePrefab.instantiate(parent); }
    protected Transform addInfoBar(){
        if (Scenes.infoBar == null){
            Transform retVal = sp.infoBarPrefab.instantiate(GC.Canvas);
            retVal.rt().anchoredPosition = Vector2.zero;
            Scenes.infoBar = new InfoBar(retVal);
            return retVal;
        }
        else return null;
    }
    protected Transform addButton(Transform parent, string text)
    {
        Transform button = sp.buttonPrefab.instantiate(parent);
        button.text().text = text;
        return button;
    }
    protected Transform addInputField(Transform parent, string IFName, string placeHolderText){
        IF.Add(sp.inputFieldPrefab.instantiate(parent).setIFPHText(placeHolderText).setName(IFName).text());
        return IF[IF.Count - 1].transform;
    }
    protected Text getIF(string IFName) { foreach (Text t in IF) if (t.name == IFName) return t; return null; }
}

public class defaultScene : Scene{
    public override Transform load(){
        base.load();
        scene = loadScene(sp.defaultScene, GC.Canvas);
        options = scene.FindChild("Options");
        return scene;
    }
    public Transform options;
    public defaultScene(GC.ScenePrefabs sp) : base(sp) {}
    protected void addFightButton(){
        addButton(options, "Fight").button().onClick.AddListener(() => Scenes.load(typeof(Scenes.fight), true));
    }
}