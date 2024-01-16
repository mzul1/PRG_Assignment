using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10262474_PRG2Assignment
{
    public enum Flavor
    {
        Vanilla,
        Chocolate,
        Strawberry,
        Durian,
        Ube,
        SeaSalt
    }

     public enum Toppings
     {
         Sprinkles,
         Mochi,
         Sago,
         Oreos
     }

    public class IceCream
    {
        public string Option { get; set; }
        public int Scoops { get; set; }
        public List<Flavour> Flavours { get; set; }
        public List<Topping> Toppings { get; set; }

        public IceCream(string option, int scoops, List<Flavour> flavours, List<Topping> toppings)
        {
            Option = option;
            Scoops = scoops;
            Flavours = flavours ?? new List<Flavour>();
            Toppings = toppings ?? new List<Topping>();
        }

        public virtual double CalculatePrice()
        {
            double price = 0;
            // Base price calculation
            switch (Scoops)
            {
                case 1:
                    price += 4.00; // Single scoop
                    break;
                case 2:
                    price += 5.50; // Double scoops
                    break;
                case 3:
                    price += 6.50; // Triple scoops
                    break;
                default:
                    throw new ArgumentException("Invalid number of scoops.");
            }
            // Add premium flavour costs and toppings
            price += Flavours.Sum(flavour => flavour.Cost * flavour.Quantity) + Toppings.Sum(topping => topping.Cost);
            return price;
        }

        //public virtual double CalculatePrice()
        //{
            //double price = 4.00 + (Scoops - 1) * 1.50;
            //price += Flavours.Where(f => f.Premium).Sum(f => f.Quantity * 2);
            //price += Toppings.Count;
            //return price;
        //}

        public override string ToString()
        {
            var flavoursString = string.Join(", ", Flavours.Select(f => f.ToString()));
            var toppingsString = string.Join(", ", Toppings.Select(t => t.ToString()));
            return $"{Option} - {Scoops} scoop(s) with Flavours: {flavoursString} and Toppings: {toppingsString}";
        }
    }
}
