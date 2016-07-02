using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Battle {
    public static int turn = 0, turnHolder = 0;
    public static bool inProgress = false;
    public static List<Entity> party, enemies, combatants = new List<Entity>(), graveyard = new List<Entity>();
    public static void newBattle(Entity party, Entity enemies) { newBattle(new List<Entity>() { party }, new List<Entity>() { enemies }); }
    public static void attack(Entity target) {
        turn++;
        takeTurns();
        combatants[turnHolder].attack(target); turnHolder++;
        takeTurns();
    }
    public static Entity getTarget(string targetName) {
        Entity target = null;
        foreach (Entity e in enemies) if (e.name == targetName) target = e;
        return target;
    }
    public static void takeTurns() {
        turnHolder %= combatants.Count;
        while (combatants[turnHolder] != GC.player) {
            takeTurn(combatants[turnHolder]);
            turnHolder++;
            turnHolder %= combatants.Count;
        }
    }
    public static void takeTurn(Entity turnee) {
        turnee.attack(party[Random.Range(0, party.Count)]);
    }
    public static void newBattle(List<Entity> party, List<Entity> enemies) {
        if (inProgress) resetBattle();
        inProgress = true;
        Battle.party = party;
        combatants.AddRange(Battle.party);
        Battle.enemies = enemies;
        combatants.AddRange(Battle.enemies);
        rollInitiative();
        foreach (Entity e in combatants) Debug.Log(e.name + " " + e.initiativeRoll);
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
}
