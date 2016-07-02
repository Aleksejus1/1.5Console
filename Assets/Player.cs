using System;

[Serializable] public class Player : Entity{
    public Location location;
    public Player() {
        hp.current = hp.max = 1;
        initiative.extra = 2;
        level = 1;
    }
}