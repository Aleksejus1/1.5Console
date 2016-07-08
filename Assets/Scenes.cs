using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public static class Scenes {
    public static Scene scene;
    public static InfoBar infoBar = null;
    public static GC.ScenePrefabs sp;
    public static Transform currentScene = null;
    public class startMenu : Scene {
        public startMenu() : base(Scenes.sp) { }
        public override Transform load() {
            base.load();
            loadScene(sp.startMenu, GC.Canvas).FindChild("NewGame").setButton(() => Scenes.load(typeof(newGame)));
            scene.setName("StartMenu Scene");
            return scene;
        }
    }
    public class newGame : defaultScene {
        public newGame() : base(Scenes.sp) { }
        public override Transform load() {
            base.load().setName("NewGame Scene");
            addInputField(options, "Character Name", "Name").setIFLimit(16);
            addButton(scene,"Create").rt().Anchor(AnchorPoint.BotCenter).Move(0f, 10f).setButton(() => {
                GC.CreateNewCharacter(getIF("Name").text);
                Scenes.load(typeof(play));
            });
            return scene;
        }
    }
    public class play : defaultScene {
        public play() : base(Scenes.sp) { }
        public override Transform load() {
            hasInfoBar = true;
            base.load().setName("Play Scene");
            addFightButton();
            return scene;
        }
    }
    public class fight : defaultScene {
        public Transform combat, party, enemies;
        public fight() : base(Scenes.sp) { }
        public void selectEnemy() {
            getButton("Attack").transform.disable();
            getButton("Cancel").transform.enable();
            getText("SelectInfo").transform.enable();
            foreach(Transform child in enemies) {
                if (Battle.getTarget(child.name).hp.state != State.dead) {
                    buttons.Add(child.newButton().button());
                    child.setButton(delegate() {
                    selectEnemy(GC.lastButton);
                }).button().targetGraphic = child.text();
                }
            }
        }
        public override void unload() {
            if (Scenes.scene == this) GC.player.hp.updates.Remove(getText(GC.player.name + "HP"));
            base.unload();
        }
        public void selectEnemy(Transform enemy) {
            getButton("Attack").transform.enable();
            getButton("Cancel").transform.disable();
            getText("SelectInfo").transform.disable();
            foreach(Transform child in enemies) if(child.button()) GameObject.Destroy(child.button());
            if (enemy!=null) { Battle.attack(Battle.getTarget(enemy.name));}
        }
        public override Transform load() {
            base.load().setName("Fight Scene");
            Battle.newBattle(new List<Entity>() { GC.player }, new List<Entity>() { GC.player.location.getMonster() , GC.player.location.getMonster() });
            combat = sp.combatPrefab.instantiate(scene);
            party = combat.FindChild("Party");
            enemies = combat.FindChild("Enemies");
            createFighters(Battle.party, party, AnchorPoint.Left);
            createFighters(Battle.enemies, enemies, AnchorPoint.Right);
            addButton(options, "Attack", "Attack").setButton(() => {
                if(GC.player.hp.state!=State.dead) selectEnemy();
                else {
                    int id = infoText(scene,"You're dead",2,true,"DeadInfo");
                    if(id!=-1) texts[id].transform.rt().Anchor(AnchorPoint.TopCenter).Move(0, -10);
                }
            });
            addButton(scene, "Cancel", "Cancel").disable().setButton(() => { selectEnemy(null); }).rt().Anchor(AnchorPoint.BotCenter).Move(0,10);
            addText(scene, "Select target", "SelectInfo").rt().Anchor(AnchorPoint.Center).Move(0, 0).disable();
            return scene;
        }
        private void createFighters(List<Entity> list, Transform parent, AnchorPoint side) {
            foreach (Entity e in list) {
                Transform name = addText(parent, e.name).rt().Anchor(side).TextAdjustWidth().setName(e.name);
                int toSide = 10; if (side == AnchorPoint.Right) toSide *= -1;
                switch (parent.childCount) {
                    case 1: name.rt().Move(toSide, 0);      break;
                    case 2: name.rt().Move(toSide, 50);     break;
                    case 3: name.rt().Move(toSide, 100);    break;
                    case 4: name.rt().Move(toSide, -50);    break;
                    case 5: name.rt().Move(toSide, -100);   break;
                }
                e.hp.addTextUpdate(addText(name, e.hp.ToString(), e.name+"HP").bestFit(false).fontSize(-4, true).rt().Anchor(AnchorPoint.BotCenter).Move(0, -10).TextAdjustWidth().setName("Health").text());
            }
        }
    }
    //startMenu, newGame, play, fight
    public static void load(Type type) {
        scene = Activator.CreateInstance(type) as Scene;
        currentScene = scene.load();
    }
}