using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scene {
    public bool hasInfoBar = false;
    public Transform scene;
    protected GC.ScenePrefabs sp;
    public List<Text> texts = new List<Text>();
    public List<Button> buttons = new List<Button>();
    public List<Text> IF = new List<Text>();
    public Scene(GC.ScenePrefabs sp) {
        this.sp = sp;
    }
    public virtual Transform load() {
        unload();
        if (hasInfoBar && Scenes.infoBar == null) addInfoBar();
        return null;
    }
    public virtual void unload() {
        if (!hasInfoBar && Scenes.infoBar != null) { Scenes.infoBar.destroy(); Scenes.infoBar = null; }
        if (Scenes.currentScene) GameObject.Destroy(Scenes.currentScene.gameObject);
    }
    protected Transform loadScene(Transform scenePrefab, Transform parent) { return scene = scenePrefab.instantiate(parent); }
    public Transform addInfoBar(){
        if (Scenes.infoBar == null){
            Transform retVal = sp.infoBarPrefab.instantiate(GC.Canvas);
            retVal.rt().anchoredPosition = Vector2.zero;
            Scenes.infoBar = new InfoBar(retVal);
            return retVal;
        }
        else return null;
    }
    public Transform addButton(Transform parent, string text, string ButtonName = ""){
        if(ButtonName=="")ButtonName=text+" Button";
        buttons.Add(sp.buttonPrefab.instantiate(parent).setText(text).setName(ButtonName).button());
        return buttons[buttons.Count-1].transform;
    }
    public Transform addText(Transform parent, string text, string textName = ""){
        if(textName=="") textName = text;
        texts.Add(sp.textPrefab.instantiate(parent).setText(text).setName(textName).text());
        return texts[texts.Count-1].transform;
    }
    public Transform addInputField(Transform parent, string placeHolderText, string IFName = ""){
        if(IFName=="")IFName=placeHolderText+" Input Field";
        IF.Add(sp.inputFieldPrefab.instantiate(parent).setIFPHText(placeHolderText).setName(IFName).text());
        return IF[IF.Count - 1].transform;
    }
    public int infoText(Transform parent, string text, float fadeOutTimer, bool timerIsrunning=true, string textName="") {
        if(textName=="")textName=text;
        Text t = getText(textName);
        int retVal = -1;
        if(t==null) {
            t = addText(parent,text,textName).addTimer(fadeOutTimer,()=> {
                t = getText(textName);
                texts.Remove(t);
                GameObject.Destroy(t.gameObject);
                return null;
            },()=>{
                t= getText(textName);
                if(t.color.a>0) t.setColor(new Color(t.color.r,t.color.g,t.color.b,t.transform.timer().progress()));
                return null;
            },()=> {
                t.setColor(new Color(t.color.r, t.color.g, t.color.b, 1));
                return null;
            }).text();
            t.transform.timer().active = timerIsrunning;
            retVal = texts.Count-1;
        }
        else t.transform.timer().reset(timerIsrunning);
        return retVal;
    }
    public Text      getIF       (string IFName)     { foreach (Text t in IF)        if (t.name == IFName)       return t; return null; }
    public Button getButton   (string ButtonName) { foreach (Button b in buttons) if (b.name == ButtonName)   return b; return null; }
    public Text      getText     (string textName)   { foreach (Text t in texts)     if (t.name == textName)     return t; return null; }
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
    public void addFightButton(){
        addButton(options, "Fight").setButton(() => Scenes.load(typeof(Scenes.fight)));
    }
    public void addExploreButton() {
        addButton(options, "Explore").setButton(() => { Scenes.load(typeof(Scenes.explore)); });
    }
}