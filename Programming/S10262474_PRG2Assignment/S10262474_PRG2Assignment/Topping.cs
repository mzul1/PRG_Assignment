using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10262474_PRG2Assignment
{
    class Topping
    {
        public string Type { get; set; }
        public Topping() { }
        public Topping(string type)
        {
            this.Type = type;
        }
        public override string ToString()
        {
            return Type;
        }
    }
}
