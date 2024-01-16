using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10262474_PRG2Assignment
{
    public class Waffle : IceCream
    {
        public string WaffleFlavour { get; set; }

        /*public Waffle(string option, int scoops, List<Flavour> flavours, List<Topping> toppings, string waffleFlavour)
            : base(option, scoops, flavours, toppings)
        {
            WaffleFlavour = waffleFlavour;
        }

        public override double CalculatePrice()
        {
            double price = base.CalculatePrice() + 3; 
            if (new[] { "Red velvet", "Charcoal", "Pandan" }.Contains(WaffleFlavour))
            {
                price += 3; 
            }
            return price;
        }*/

        public Waffle(int scoops, List<Flavour> flavours, List<Topping> toppings, string waffleFlavour)
        : base("Waffle", scoops, flavours, toppings)
        {
            WaffleFlavour = waffleFlavour;
        }

        public override double CalculatePrice()
        {
            double price = base.CalculatePrice() + 3; // Base cost for waffle option.
            if (WaffleFlavour == "Red Velvet" || WaffleFlavour == "Charcoal" || WaffleFlavour == "Pandan")
            {
                price += 3; // Additional cost for special waffle flavours.
            }
            return price;
        }

        public override string ToString()
        {
            return base.ToString() + $", Waffle Flavour: {WaffleFlavour}";
        }
    }
}
