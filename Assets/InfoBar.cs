using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable] public class InfoBar{
    public Transform transform;
    public Text health, name, location;
    public InfoBar(Transform t){
        transform = t;
        health = t.FindChild("Health").text();
        name = t.FindChild("Name").text();
        location = t.FindChild("Location").text();
        updateAll();
    }
    public void destroy(){
        if (transform) UnityEngine.Object.Destroy(transform.gameObject);
        transform = null;
        health = null;
        name = null;
        location = null;
    }
    public void updateAll() { updateHealth(); updateName(); updateLocation(); }
    public void updateHealth() { health.text = "Health: " + GC.player.hp.ToString(); }
    public void updateName() { name.text = GC.player.name; }
    public void updateLocation() { location.text = GC.player.location.ToString(); }
}