using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Monsters {
    public class worm : Monster{
        public double HP, HPMax = 0;
        public worm(int level) : base("Worm", new Dice("2d2"), new Dice("1d2")){
            HP = HPMax += HPD + HD * (level - 1);
            MonoBehaviour.print(HP + " " + HPMax);
        }
    }
    public static List<Type> monsterList = typeof(Monsters).GetNestedTypes().OfType<Type>().ToList();
}