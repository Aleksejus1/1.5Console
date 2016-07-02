using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Monsters {
    public class worm : Monster{
        public worm(int level) : base("Worm", new Dice("2d2"), new Dice("1d2")){
            hp.current = hp.max += HPD + HD * (level - 1);
        }
        public override double dealDamage() {
            Debug.Log(name + " is dealing damage. Type = Overriden");
            return new Dice("1d2").roll();
        }
    }
    public static List<Type> monsterList = typeof(Monsters).GetNestedTypes().OfType<Type>().ToList(); //List of all monster types
}