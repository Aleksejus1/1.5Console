using System;
using UnityEngine;

[Serializable] public static class Scenes
{
    public class startMenu : Scene{
        public startMenu() : base("startMenu") { }
        public override Transform load(){
            loadScene(GC.ScenePrefabs.startMenu);
            scene = GC.ScenePrefabs.startMenu.instantiate(GC.Canvas);
            return base.load();
        }
        private void loadScene(Transform scenePrefab){ scene = scenePrefab.instantiate(GC.Canvas); }
    }
    public class play : Scene {
        public play() : base("play") { }
        public override Transform load(){
            MonoBehaviour.print("Hi Loader, wanna play?");
            return base.load();
        }
    }
    public class fight : defaultScene {
        public fight() : base("play") { }
        public override Transform load(){
            MonoBehaviour.print("Hi Loader, wanna fight?");
            return base.load();
        }
    }
    //startMenu, newGame, play, fight
    public static void load(Type type, bool keepInfoBar = false){
        (Activator.CreateInstance(type) as Scene).load();
    }
}