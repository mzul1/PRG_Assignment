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
    internal class Flavour
    {
        public string Type { get; set; }
        public bool Premium { get; set; }
        public int Quantity { get; set; }
        public Flavour() { }
        public Flavour(string type, bool premium, int quantity)
        {
            Type = type;
            Premium = premium;
            Quantity = quantity;
        }
        public override string ToString()
        {
            return "Type:" + Type + "\tPremium: " + Premium + "\tQuantity: " + Quantity;
        }
    }
}
