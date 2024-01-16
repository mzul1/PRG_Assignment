using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10262474_PRG2Assignment
{
    public class Cup : IceCream
    {
        /*public Cup(string option, int scoops, List<Flavour> flavours, List<Topping> toppings)
            : base(option, scoops, flavours, toppings)
        {
        }*/

        public Cup(int scoops, List<Flavour> flavours, List<Topping> toppings)
        : base("Cup", scoops, flavours, toppings)
        {
        }

        public override double CalculatePrice()
        {
            return base.CalculatePrice(); 
        }
    }
}
