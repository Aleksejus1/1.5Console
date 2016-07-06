using System;

[Serializable] public class Monster : Entity{
    public Dice HPD, HD;
    public Monster(string name, Dice HPD, Dice HD, int level = 1) {
        this.name = name;
        this.HPD = HPD;
        this.HD = HD;
        this.level = level;
        hp.Current = hp.Max = hp.Current = hp.Max += this.HPD + this.HD * (level - 1);;
    }
    public Monster setName(string name) { this.name = name; return this; }
    public virtual Monster New(int level){
        return new Monster(name,HPD,HD,level);
    }
}