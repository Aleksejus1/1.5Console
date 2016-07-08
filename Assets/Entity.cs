using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity : Level {
    public string name;
    public Health hp = new Health(0, 0);
    public int initiativeRoll;
    public Dice initiative = new Dice("1d20");
    public void attack(Entity target) {
        target.takeDamge(dealDamage());
    }
    public void takeDamge(int count) {
        if (hp.state != State.dead) {
            hp.Current -= count;
            if (hp.state == State.dead) {//If dead
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
public class Level {
    private int XpCurrent, XpMax;
    private int LeveL;

    public int level {
        get { return LeveL; }
        set {
            LeveL = value;
            switch (level){
                case 1: xpMax = 300; break;
                case 2: xpMax = 900; break;
                case 3: xpMax = 2700; break;
                case 4: xpMax = 6500; break;
                case 5: xpMax = 14000; break;
                case 6: xpMax = 23000; break;
                case 7: xpMax = 34000; break;
                case 8: xpMax = 48000; break;
                case 9: xpMax = 64000; break;
                case 10: xpMax = 85000; break;
                case 11: xpMax = 100000; break;
                case 12: xpMax = 120000; break;
                case 13: xpMax = 140000; break;
                case 14: xpMax = 165000; break;
                case 15: xpMax = 195000; break;
                case 16: xpMax = 225000; break;
                case 17: xpMax = 265000; break;
                case 18: xpMax = 305000; break;
                case 19: xpMax = 355000; break;
            }
        }
    }
    public int xpCurrent {
        get { return XpCurrent; }
        set {
            XpCurrent = value;
            if (xpCurrent >= xpMax && level < 20) {
                level++;
            }
        }
    }
    public int xpMax {
        get { return XpMax; }
        set { XpMax = value; }
    }
    public string xpToString() { return xpCurrent + "/" + xpMax; }
}