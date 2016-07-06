using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable] public class InfoBar{
    public Transform transform;
    public Text health, name, location;
    public InfoBar(Transform t){
        transform = t;
        health = t.FindChild("Health").text();
        GC.player.hp.addFunctionUpdate(updateHealth);
        name = t.FindChild("Name").text();
        location = t.FindChild("Location").text();
        updateAll();
    }
    public void destroy(){
        if (transform) UnityEngine.Object.Destroy(transform.gameObject);
        GC.player.hp.updatesF.Remove(updateHealth);
        transform = null;
        health = null;
        name = null;
        location = null;
    }
    public void updateAll() { updateName(); updateLocation(); }
    public Action updateHealth() { health.text = "Health: " + GC.player.hp.ToString(); return null; }
    public void updateName() { name.text = GC.player.name; }
    public void updateLocation() { location.text = GC.player.location.name; }
}