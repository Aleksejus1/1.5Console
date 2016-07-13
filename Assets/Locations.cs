using System;
using System.Collections.Generic;
using UnityEngine;

public static class Locations{
    public class Wilderness : Location{
        public Wilderness() : base("Wilderness", new Vector2(1, 2), new List<Monster>() { Monsters.worm, Monsters.worm.New(1).setName("notAWorm") }) {}
        public override void load(defaultScene s) {
            s.addExploreButton();
            s.addFightButton();
        }
    }
}
