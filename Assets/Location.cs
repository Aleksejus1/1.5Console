using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class Location{
    public string name = "The Wrong Planet";
    public Vector2 monsterLevelRange = new Vector2(0f, 0f);
    public List<Monster> monsterList = new List<Monster>();
    public Location(string name, Vector2 monsterLevelRange, List<Monster> monsterList = null) {
        this.name = name;
        this.monsterLevelRange = monsterLevelRange;
        if (monsterList != null) this.monsterList = monsterList;
    }
    public Location addMonster(Monster monster) { monsterList.Add(monster); return this; }
    public Monster getMonster() {
        foreach(Monster m in monsterList) Debug.Log(m.name);
        return monsterList[UnityEngine.Random.Range(0,monsterList.Count)].New(getLevel());
    }
    public int getLevel() { return UnityEngine.Random.Range((int)monsterLevelRange.x, (int)monsterLevelRange.y); }
}