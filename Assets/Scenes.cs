using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

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
                buttons.Add(child.newButton().button());
                child.setButton(()=> { selectEnemy(child); }).button().targetGraphic = child.text();
            }
        }
        public void selectEnemy(Transform enemy) {
            Debug.Log("Enemy has been selected");
            getButton("Attack").transform.enable();
            getButton("Cancel").transform.disable();
            getText("SelectInfo").transform.disable();
            foreach(Transform child in enemies) if(child.button()) GameObject.Destroy(child.button());
            if(enemy!=null) { Battle.attack(Battle.getTarget(enemy.name));}
        }
        public override Transform load() {
            base.load().setName("Fight Scene");
            Battle.newBattle(GC.player,GC.player.location.getMonster());
            combat = sp.combatPrefab.instantiate(scene);
            party = combat.FindChild("Party");
            enemies = combat.FindChild("Enemies");
            foreach(Entity e in Battle.party) {
                Transform name = addText(party, e.name).rt().Anchor(AnchorPoint.Left).Move(10,0).TextAdjustWidth().setName(e.name);
                e.hp.addTextUpdate(addText(name, e.hp.ToString()).bestFit(false).fontSize(-4, true).rt().Anchor(AnchorPoint.BotCenter).Move(0,-10).TextAdjustWidth().setName("Health").text());
            }
            foreach(Entity e in Battle.enemies) {
                Transform name = addText(enemies, e.name).rt().Anchor(AnchorPoint.Right).Move(-10,0).TextAdjustWidth().setName(e.name);
                e.hp.addTextUpdate(addText(name, e.hp.ToString()).bestFit(false).fontSize(-4, true).rt().Anchor(AnchorPoint.BotCenter).Move(0,-10).TextAdjustWidth().setName("Health").text());
            }
            addButton(options, "Attack", "Attack").setButton(() => {
                if(GC.player.hp.state!=State.dead) selectEnemy();
                else {
                    int id = infoText(scene,"You're dead",2,"DeadInfo");
                    if(id!=-1) texts[id].transform.rt().Anchor(AnchorPoint.TopCenter).Move(0, -10);
                }
            });
            addButton(scene, "Cancel", "Cancel").disable().setButton(() => { selectEnemy(null); }).rt().Anchor(AnchorPoint.BotCenter).Move(0,10);
            addText(scene, "Select target", "SelectInfo").rt().Anchor(AnchorPoint.Center).Move(0, 0).disable();
            return scene;
        }
    }
    //startMenu, newGame, play, fight
    public static void load(Type type, bool keepInfoBar = false) {
        currentScene = (Activator.CreateInstance(type) as Scene).load();
    }
}