using System;

[Serializable] public class Monster : Entity{
    public Dice HPD, HD;
    public Monster(string name, Dice HPD, Dice HD) {
        this.name = name;
        this.HPD = HPD;
        this.HD = HD;
    }
}