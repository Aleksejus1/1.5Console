using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/*
Battle system:
    party vs enemies:
        max 5 on each team - Not hardcoded, simply don't add more than 5 to a battle and you should be fine
        each member has a designated spot for their info to be displayed - not done
        each member can attack any one from the opposing part - kind of
    Turn:
        If not dead:
            Attck:
                Player:
                    can choose a target from the enemie team - done
                    target has to be not dead - not done
                Else:
                    Attacks a random character from the enemy team - done
                    target has to be not dead - not done
        Dead don't get turns.
     
*/

public static class Battle {
    private static int PartyAlive = 0, EnemiesAlive = 0;
    public static int partyAlive { get { return PartyAlive; } set {
            if (value == 0 && PartyAlive > value) lose();
            PartyAlive = value;
        }
    }
    public static int enemiesAlive { get { return EnemiesAlive; } set {
            if (value == 0 && EnemiesAlive > value) win();
            EnemiesAlive = value;
        }
    }
    private static int TurnHolder = 0;
    public static int turn = 0;
    public static int turnHolder{
        get { return TurnHolder; }
        set {
            TurnHolder = value;
            TurnHolder %= combatants.Count;
            if (TurnHolder < value) nextTurn();
        }
    }
    public static bool inProgress = false;
    public static List<Entity> party, enemies, combatants = new List<Entity>(), graveyard = new List<Entity>();
    public static void newBattle(Entity party, Entity enemies) { newBattle(new List<Entity>() { party }, new List<Entity>() { enemies }); }
    public static void nextTurn() {
        turn++;
    }
    public static void attack(Entity target) {
        takeTurns();
        combatants[turnHolder].attack(target);
        if (target.hp.state == State.dead) enemiesAlive--;
        turnHolder++;
        takeTurns();
    }
    public static Entity getTarget(string targetName) {
        Entity target = null;
        foreach (Entity e in enemies.Concat(party)) if (e.name == targetName) target = e;
        return target;
    }
    public static void takeTurns() {
        while (combatants[turnHolder] != GC.player) {
            List<Entity> targetList;
            bool tlist = false;
            if (party.IndexOf(combatants[turnHolder]) > -1) { targetList = enemies; tlist = true; } else targetList = party;
            if (takeTurn(combatants[turnHolder], targetList).hp.state == State.dead) {
                if (tlist) enemiesAlive--;
                else partyAlive--;
            };
            turnHolder++;
        }
    }
    public static Entity takeTurn(Entity turnee, List<Entity> targetList) {
        Entity target = targetList[Random.Range(0, targetList.Count)];
        if (turnee.hp.state!=State.dead) turnee.attack(target);
        return target;
    }
    public static void renameDuplicates(List<Entity> list) {
        List<Entity> duplicates = new List<Entity>();
        for (int i = 0; i < list.Count; i++) {
            duplicates.Clear();
            for (int o = i + 1; o < list.Count; o++) {
                if (list[i].name == list[o].name) {
                    if (duplicates.Count == 0) duplicates.Add(list[i]);
                    duplicates.Add(list[o]);
                }
            }
            for (int o = 1; o < duplicates.Count; o++) duplicates[o].name += " " + (o+1).ToString();
        }
    }
    public static void newBattle(List<Entity> party, List<Entity> enemies) {
        if (inProgress) resetBattle();
        inProgress = true;
        Battle.party = party;
        combatants.AddRange(Battle.party);
        renameDuplicates(Battle.party);
        partyAlive = party.Count;
        Battle.enemies = enemies;
        combatants.AddRange(Battle.enemies);
        renameDuplicates(Battle.enemies);
        enemiesAlive = enemies.Count;
        rollInitiative();
        nextTurn();
    }
    public static void resetBattle() {
        turn = 0;
        foreach (Entity e in combatants) e.initiativeRoll = 0;
        party.Clear();
        enemies.Clear();
        combatants.Clear();
    }
    private static void rollInitiative() {
        foreach (Entity e in combatants) e.initiativeRoll = e.initiative.roll();
        combatants = combatants.OrderBy(o => -o.initiativeRoll).ToList();
    }
    private static void win() {
        Debug.Log("You win!");
        int xpAward = 0;
        foreach (Entity e in enemies) { xpAward += (e as Monster).getXP(); }
        xpAward /= party.Count;
        foreach (Entity e in party) e.xpCurrent += xpAward;
        //Get loot or what not
        //Return to play scene
        Scenes.load(typeof(Scenes.play));
    }
    private static void lose() {
        Debug.Log("You lose.");
    }
}