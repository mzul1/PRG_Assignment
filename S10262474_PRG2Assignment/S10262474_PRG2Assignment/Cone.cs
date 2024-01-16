using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10262474_PRG2Assignment
{
    public class Cone : IceCream
    {
        public bool Dipped { get; set; }

        /*public Cone(string option, int scoops, List<Flavour> flavours, List<Topping> toppings, bool dipped)
            : base(option, scoops, flavours, toppings)
        {
            Dipped = dipped;
        }*/

        public Cone(int scoops, List<Flavour> flavours, List<Topping> toppings, bool dipped)
        : base("Cone", scoops, flavours, toppings)
        {
            Dipped = dipped;
        }

        public override double CalculatePrice()
        {
            double price = base.CalculatePrice();
            if (Dipped)
            {
                price += 2; 
            }
            return price;
        }

        public override string ToString()
        {
            return base.ToString() + (Dipped ? ", Chocolate-Dipped" : "");
        }
    }
}
