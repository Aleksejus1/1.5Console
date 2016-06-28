using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public static class TransformEx {
    public static Transform Clear(this Transform transform) {
        foreach (Transform child in transform) GameObject.Destroy(child.gameObject);
        return transform;
    }
}

[Serializable]
public class GC : MonoBehaviour {
    public enum Scenes { startMenu, newGame, none }
    [Serializable]
    public class Scenes_ {
        public Scenes current = Scenes.none;
        public Transform startMenu, newGame;
        public List<Button> buttons;
        public void load(Scenes sceneID){
            unloadCurrentScene();
            switch (sceneID) {
                case Scenes.startMenu:
                    _Instantiate(startMenu, Canvas).FindChild("NewGame").GetComponent<Button>().onClick.AddListener(() => load(Scenes.newGame));
                    break;
                case Scenes.newGame:
                    _Instantiate(newGame, Canvas);
                    break;
            }
            current = sceneID;
        }
    }
    public Scenes_ scenes;
    [Serializable]
    public class Camera_ {
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
    public static Transform Canvas;
    void Start(){
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
    private static void unloadCurrentScene() { Canvas.Clear(); }
}
