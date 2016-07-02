using System;
using System.Reflection;
using UnityEngine;

[Serializable]
public static class Scenes {
    public static InfoBar infoBar = null;
    public static GC.ScenePrefabs sp;
    public static Transform currentScene = null;
    public class startMenu : Scene {
        public startMenu() : base(Scenes.sp) { }
        public override Transform load() {
            base.load();
            loadScene(sp.startMenu, GC.Canvas).FindChild("NewGame").setButton(() => Scenes.load(typeof(newGame)));
            return scene;
        }
    }
    public class newGame : defaultScene {
        public newGame() : base(Scenes.sp) { }
        public override Transform load() {
            base.load();
            addInputField(options, "Name", "Character Name").setIFLimit(16);
            sp.buttonPrefab.instantiate(scene).setText("Create").rt().Anchor(AnchorPoint.BotCenter).Move(0f, 10f)
                .setButton(() => { GC.CreateNewCharacter(getIF("Name").text); Scenes.load(typeof(play)); });
            return scene;
        }
    }
    public class play : defaultScene {
        public play() : base(Scenes.sp) { }
        public override Transform load() {
            hasInfoBar = true;
            base.load();
            addFightButton();
            return scene;
        }
    }
    public class fight : defaultScene {
        public fight() : base(Scenes.sp) { }
        public override Transform load() {
            //MonoBehaviour.print("Hi Loader, wanna fight?");
            //Yes i do want to fight and i can see that there is a worm on the ground there, but how Do i fight?
            //Start simple Loader, just have a button to attack and make it do damage to the worm and then if worm is still alive let it attack the player.
            //Sounds simple enough, good idea Console.
            hasInfoBar = true;
            base.load();
            addButton(options, "Attack").setButton(() => {
                Debug.Log(">----Attack----<");
                if (!Battle.inProgress) Battle.newBattle(GC.player, GC.player.location.getMonster());
                Battle.attack(Battle.getTarget("Worm"));
            });
            return scene;
        }
    }
    //startMenu, newGame, play, fight
    public static void load(Type type, bool keepInfoBar = false) {
        currentScene = (Activator.CreateInstance(type) as Scene).load();
    }
}