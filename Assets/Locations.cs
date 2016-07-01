using System;
using System.Collections.Generic;
using UnityEngine;

public static class locations{
    public class Wilderness : Location{
        public Wilderness() : base("Wilderness", new Vector2(1, 5), new List<Type>() { typeof(Monsters.worm)}) {
        }
    }
}
