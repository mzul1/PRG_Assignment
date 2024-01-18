using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10262474_PRG2Assignment
{
    class Cone : IceCream
    {
        public bool Dipped { get; set; }
        public Cone() { }
        public Cone(string option, int scoops, List<Flavour> flavour, List<Topping> topping, bool dipped) : base(option, scoops, flavour, topping)
        {
            this.Dipped = dipped;
        }

        public override double CalculatePrice()
        {
            double price = 0;
            // scoops price
            if (this.Scoops == 1)
            {
                price = 4.00;
            }
            else if (this.Scoops == 2)
            {
                price = 5.50;
            }
            else
            {
                price = 6.50;
            }

            // flavour price
            foreach (Flavour flavour in Flavours)
            {
                if (flavour.Premium)
                {
                    price += 2;
                }
            }

            //toppings
            price += this.Toppings.Count();

            //dipped
            if (this.Dipped) { price += 2; }

            return price;
        }

        public override string ToString()
        {
            return "Option: " + Option + " Scoops: " + Scoops + " Flavours: " + Flavours + " Toppings: " + Toppings + " Dipped: " + Dipped;
        }
    }
}
