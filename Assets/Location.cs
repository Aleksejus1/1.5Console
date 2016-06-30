using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class Location{
    public string name = "The Wrong Planet";
    public Vector2 monsterLevelRange = new Vector2(0f, 0f);
    public List<Type> monsterList = new List<Type>();
    public Location(string name, Vector2 monsterLevelRange, List<Type> monsterList = null) {
        this.name = name;
        this.monsterLevelRange = monsterLevelRange;
        if (monsterList == null) this.monsterList = new List<Type>();
        else this.monsterList = monsterList;
    }
    public Location addMonster(Type monsterType) { monsterList.Add(monsterType); return this; }
}