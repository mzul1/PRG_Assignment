using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//==========================================================
// Student Number : S10262474
// Student Name : Chew Jin Xuan
// Partner Name : Zulhimi
//==========================================================
namespace S10262474_PRG2Assignment
{
    internal class Topping
    {
        public string Type { get; set; }
        public Topping() { }
        public Topping(string type)
        {
            Type = type;
        }
        public override string ToString()
        {
            return "Type: " + Type;
        }
    }
}
