using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity {
    public string name;
    public Health hp = new Health(0, 0);
    public int level, initiativeRoll;
    public Dice initiative = new Dice("1d20");
    public void attack(Entity target) {
        target.takeDamge(dealDamage());
    }
    public void takeDamge(int count) {
        if (hp.state != State.dead) {
            hp.Current -= count;
            UnityEngine.Debug.Log(name + " took " + count + " damage and now has " + hp.ToString() + " health.");
            if (hp.state == State.dead) {//If dead
                UnityEngine.Debug.Log(name + " died.");
            }
        }
    }
    public virtual int dealDamage() {
        return new Dice("1d2").roll();
    }
}
public enum State { alive, injured, dead };
public class Health {
    public bool canBeDamaged = true;
    public State state = State.alive;
    private int max, current;
    public List<Text> updates = new List<Text>();
    public List<Func<Action>> updatesF = new List<Func<Action>>();
    public void addTextUpdate(Text t) {
        updates.Add(t);
        t.text = ToString();
    }
    public void addFunctionUpdate(Func<Action> f) {
        updatesF.Add(f);
        f();
    }
    public int Max {
        get { return max; }
        set {
            if(value<0) value=0;
            max = value;
            updateList();
        }
    }
    public int Current {
        get { return current; }
        set {
            if(!(!canBeDamaged&&value<current)) {
                current = value;
                updateState();
                updateList();
            } 
        }
    }
    private void updateList() {
        foreach(Text t in updates) t.text = ToString();
        foreach(Func<Action> f in updatesF) f();
    }
    public Health(int c, int m) {
        current = c;
        max = m;
    }
    public State updateState() {
        if (current <= 0) {
            if(state!=State.dead) {
                state = State.dead;
                canBeDamaged = false;
            }
        }
        else if (current <= max / 2) {
            if(state!=State.injured) {
                state = State.injured;
                if(!canBeDamaged) canBeDamaged = true;
            }
        } 
        else if(state!=State.alive) {
            state = State.alive;
            if(!canBeDamaged) canBeDamaged = true;
        }
        return state;
    }
    public override string ToString() { return current + "/" + max; }
}
