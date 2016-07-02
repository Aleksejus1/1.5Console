public class Entity {
    public string name;
    public Health hp = new Health(0, 0);
    public int level, initiativeRoll;
    public Dice initiative = new Dice("1d20");
    public void attack(Entity target) {
        target.takeDamge(dealDamage());
    }
    public void takeDamge(double count) {
        if (hp.state != State.dead) {
            hp.current -= count;
            UnityEngine.Debug.Log(name + " took " + count + " damage and now has " + hp.current + "/" + hp.max + " health.");
            if (hp.updateState() == State.dead) {//If dead
                UnityEngine.Debug.Log(name + " died.");
            }
            else UnityEngine.Debug.Log(name + " is now " + hp.state.ToString());
        }
    }
    public virtual double dealDamage() {
        return new Dice("1d2").roll();
    }
}
public enum State { alive, injured, dead };
public class Health {
    public State state = State.alive;
    public double max, current;
    public Health(double c, double m) {
        current = c;
        max = m;
    }
    public State updateState() {
        if (current <= 0) state = State.dead;
        else if (current <= max / 2) state = State.injured;
        else state = State.alive;
        return state;
    }
    public override string ToString() { return current + "/" + max; }
}
