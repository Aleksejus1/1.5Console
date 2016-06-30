using System;

[Serializable] public class Dice{
    public int count, type, extra = 0;
    public Dice(string value) {
        int idOfD = value.IndexOf('d');
        if (idOfD != -1) {
            count = int.Parse(value.Substring(0, idOfD));
            int idOfP = value.IndexOf('+'), idOfM = value.IndexOf('-'), id;
            if (idOfP == -1 && idOfM == -1) { id = value.Length; }
            else {
                if (idOfP != -1) id = idOfP;
                else id = idOfM;
                extra = int.Parse(value.Substring(id, value.Length - id));
            }
            type = int.Parse(value.Substring(idOfD + 1, id - idOfD - 1));
        }
    }
    public int roll() {
        int retVal = extra;
        for (int i = 0; i < count; i++) retVal += UnityEngine.Random.Range(0, type) + 1;
        return retVal;
    }
    public static implicit operator int(Dice d) { return d.roll(); }
    public static int operator *(int integer, Dice d){
        d.count *= integer;
        int retVal = d;
        d.count /= integer;
        return retVal;
    }
    public static int operator *(Dice di, int i) { return i * di; }
}
