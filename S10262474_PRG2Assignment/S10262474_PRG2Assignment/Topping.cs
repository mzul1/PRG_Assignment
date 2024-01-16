using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10262474_PRG2Assignment
{
    public class Topping
    {
        public string Type { get; set; }
        public double Cost { get; } = 1.00;

        public Topping() { }

        public Topping(string type)
        {
            Type = type;
        }

        /*public double CalculatePrice()
    {
        // Each topping costs $1.
        return 1;
    }*/

        public override string ToString()
        {
            return Type;
        }
    }
}
