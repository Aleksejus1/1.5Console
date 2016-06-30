using System;

[Serializable] public class Monster{
    public string name;
    public Dice HPD, HD;
    public int level;
    public Monster(string name, Dice HPD, Dice HD) {
        this.name = name;
        this.HPD = HPD;
        this.HD = HD;
    }
}