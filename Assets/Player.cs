using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable] public class Player{
    public string name = "Alek";
    public class Health{
        public double max, current;
        public Health(double c, double m){
            current = c;
            max = m;
        }
        public override string ToString() { return current + "/" + max; }
    }
    public Health hp = new Health(0, 0);
    public Location location;
}