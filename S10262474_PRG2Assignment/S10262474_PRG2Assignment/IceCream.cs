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
    abstract class IceCream
    {
        public string Option { get; set; }
        public int Scoops { get; set; }
        public List<Flavour> Flavours { get; set; }
        public List<Topping> Toppings { get; set; }
        public IceCream() { }
        public IceCream(string option, int scoops, List<Flavour> flavours,
            List<Topping> toppings)
        {
            Option = option;
            Scoops = scoops;
            Flavours = flavours;
            Toppings = toppings;
        }
        public abstract double CalculatePrice();
        public override string ToString()
        {
            string flavourList = string.Join(", ", Flavours.Select(f => f.Type));
            string toppingList = Toppings.Any() ? string.Join(", ", Toppings.Select(t => t.Type)) : "None";

            string details = $"Option: {Option} Scoops: {Scoops} Flavours: {flavourList} Toppings: {toppingList}";

            if (this is Cone cone)
            {
                details += $" Dipped: {cone.Dipped}";
            }
            else if (this is Waffle waffle)
            {
                details += $" Waffle Flavour: {waffle.WaffleFlavour}";
            }

            return details;
        }
    }


}
