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
    class Cup : IceCream
    {
        public Cup() : base() { }
        public Cup(string option, int scoops, List<Flavour> flavours,
            List<Topping> toppings) : base(option, scoops, flavours, toppings) { }
        public override double CalculatePrice()
        {
            double price;
            if (base.Scoops == 1)
            {
                price = 4.00;
            }
            else if (base.Scoops == 2)
            {
                price = 5.50;
            }
            else
            {
                price = 6.50;
            }

            foreach (Flavour flavour in base.Flavours)
            {
                if (flavour.Premium)
                {
                    price += 2 * flavour.Quantity;
                }
            }

            if (base.Toppings.Count > 0)
            {
                price += base.Toppings.Count * 1;
            }
            return price;
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
