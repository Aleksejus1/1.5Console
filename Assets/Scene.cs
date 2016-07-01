using UnityEngine;

public class Scene {
    public string name = "TheEndOfTheWorld";
    public InfoBar infoBar = null;
    public Transform scene;
    public Scene(string name) {
        this.name = name;
    }
    public virtual Transform load() {
        unload(false);
        /*
        Transform retVal = GC._Instantiate(GC.defaultScene, Canvas);
        options = retVal.FindChild("Options");
        return retVal;*/
        return null;
    }
    public virtual void unload(bool keepInfoBar) {
        if (!keepInfoBar && infoBar != null) infoBar.destroy();
        if (scene) Object.Destroy(scene.gameObject);
    }
    protected Transform addInfoBar(){
        if (infoBar != null){
            Transform retVal = GC._Instantiate(GC.ScenePrefabs.infoBarPrefab, scene);
            retVal.rt().anchoredPosition = Vector2.zero;
            infoBar = new InfoBar(retVal);
            return retVal;
        }
        else return null;
    }
    protected Transform addButton(Transform parent, string text)
    {
        Transform button = GC._Instantiate(GC.ScenePrefabs.buttonPrefab, parent);
        button.text().text = text;
        return button;
    }
    protected Transform addInputField(Transform parent, string placeHolderText)
    {
        Transform inputField = GC._Instantiate(GC.ScenePrefabs.inputFieldPrefab, parent);
        inputField.GetChild(0).text().text = placeHolderText;
        return inputField;
    }
}

public class defaultScene : Scene{
    public Transform options;
    public defaultScene(string name) : base(name){}
    protected void addFightButton(){
        addButton(options, "Fight").button().onClick.AddListener(() => Scenes.load(typeof(Scenes.fight), true));
    }
}