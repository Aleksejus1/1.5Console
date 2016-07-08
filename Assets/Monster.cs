using System;

[Serializable] public class Monster : Entity{
    public Dice HPD, HD;
    public float CR;
    public Monster(string name, Dice HPD, Dice HD, float CR) {
        this.name = name;
        this.HPD = HPD;
        this.HD = HD;
        this.CR = CR;
        hp.Current = hp.Max = hp.Current = hp.Max += this.HPD + this.HD * (level - 1);;
    }
    public int getXP() {
        if (CR == 0) return UnityEngine.Random.Range(0, 10);
        else if (CR == 1 / 8) return 25;
        else if (CR == 1 / 4) return 50;
        else if (CR == 1 / 2) return 100;
        else if (CR == 1) return 200;
        else if (CR == 2) return 450;
        else if (CR == 3) return 700;
        else if (CR == 4) return 1100;
        else if (CR == 5) return 1800;
        else if (CR == 6) return 2300;
        else if (CR == 7) return 2900;
        else if (CR == 8) return 3900;
        else if (CR == 9) return 5000;
        else if (CR == 10) return 5900;
        else if (CR == 11) return 7200;
        else if (CR == 12) return 8400;
        else if (CR == 13) return 10000;
        else if (CR == 14) return 11500;
        else if (CR == 15) return 13000;
        else if (CR == 16) return 15000;
        else if (CR == 17) return 18000;
        else if (CR == 18) return 20000;
        else if (CR == 19) return 22000;
        else if (CR == 20) return 25000;
        else if (CR == 21) return 33000;
        else if (CR == 22) return 41000;
        else if (CR == 23) return 50000;
        else if (CR == 24) return 62000;
        else if (CR == 30) return 155000;
        else return 0;
    }
    public Monster setName(string name) { this.name = name; return this; }
    public virtual Monster New(int level){
        return new Monster(name,HPD,HD,CR);
    }
}