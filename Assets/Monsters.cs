using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Monsters {
    public class Worm : Monster{
        public Worm() : base("Worm", new Dice("2d2"), new Dice("1d2"), 0){}
        public override int dealDamage() { return new Dice("1d1").roll(); }
    }
    public static Worm worm = new Worm();
    public static List<Monster> monsterList = new List<Monster>() { worm }; //List of all monster types
}