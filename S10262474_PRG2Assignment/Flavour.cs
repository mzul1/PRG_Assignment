using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10262474_PRG2Assignment
{
    internal class Flavour
    {
        public string Type { get; set; }
        public bool Premium { get; set; }
        public int Quantity { get; set; }
        public Flavour() { }
        public Flavour(string type, bool premium, int quantity)
        {
            this.Type = type;
            this.Premium = premium;
            this.Quantity = quantity;
        }
        public override string ToString()
        {
            return Type + Premium + Quantity;
        }
    }
}
