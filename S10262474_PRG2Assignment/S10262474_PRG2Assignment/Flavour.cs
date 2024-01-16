using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10262474_PRG2Assignment
{
    public class Flavour
    {
        public string Type { get; set; }
        public bool Premium { get; set; }
        public int Quantity { get; set; }
        public double Cost => Premium ? 2.00 : 0.00;

        public Flavour() { }

        public Flavour(string type, bool premium, int quantity)
        {
            Type = type;
            Premium = premium;
            Quantity = quantity;
        }

        /*public double CalculatePrice()
        {
            // Premium flavours cost an additional $2 per scoop.
            return Quantity * (Premium ? 2 : 0);
        }*/

        public override string ToString()
        {
            return $"{Type}{(Premium ? " (Premium)" : "")} x {Quantity}";
        }
    }
}
