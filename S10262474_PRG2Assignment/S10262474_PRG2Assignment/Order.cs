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
    class Order
    {
        public int Id { get; set; }
        public DateTime TimeReceived { get; set; }
        public DateTime? TimeFulfilled { get; set; }
        public List<IceCream> IceCreamList { get; set; } = new List<IceCream>();
        public Order() { }
        public Order(int id, DateTime timereceived)
        {
            Id = id;
            TimeReceived = timereceived;
        }
        public void ModifyIceCream(int orderid, string[] Options, string[] premFlavours, string[] regFlavours, string[] Toppings, string[] waffleFlavours)
        {
            string option;
            int scoopnum;
            List<Flavour> flavours;
            List<Topping> toppings;

            int orderno = orderid - 1;

            option = GetIceCreamOption(Options);
            scoopnum = GetScoopNumber();
            flavours = GetFlavours(premFlavours, regFlavours, scoopnum);
            toppings = GetToppings(Toppings);

            IceCream iceCream = CreateIceCream(option, scoopnum, flavours, toppings, waffleFlavours);

            if (iceCream != null)
            {
                IceCreamList[orderno] = iceCream;
            }
        }

        private string GetIceCreamOption(string[] Options)
        {
            string option;

            do
            {
                DisplayOptions(Options);
                Console.Write("Enter option: ");
                option = Console.ReadLine().Trim();

                if (string.IsNullOrWhiteSpace(option) || !Options.Contains(option))
                {
                    Console.WriteLine("Invalid option! Please enter a valid option.");
                }
            } while (string.IsNullOrWhiteSpace(option) || !Options.Contains(option));

            return option;
        }

        private void DisplayOptions(string[] Options)
        {
            Console.WriteLine("Options that are available: ");
            foreach (string opt in Options)
            {
                Console.WriteLine(opt);
            }
        }

        private int GetScoopNumber()
        {
            int scoopnum;

            do
            {
                Console.Write("Enter the number of scoops 1/2/3: ");
                if (!int.TryParse(Console.ReadLine(), out scoopnum) || scoopnum < 1 || scoopnum > 3)
                {
                    Console.WriteLine("Invalid scoop number! Please enter a valid option.");
                }
            } while (scoopnum < 1 || scoopnum > 3);

            return scoopnum;
        }

        private List<Flavour> GetFlavours(string[] premFlavours, string[] regFlavours, int scoopnum)
        {
            List<Flavour> flavours = new List<Flavour>();
            int totalflavourquantity = 0;

            while (totalflavourquantity < scoopnum)
            {
                string flavourtype = GetFlavourType(premFlavours, regFlavours);
                int flavourquantity = GetFlavourQuantity(scoopnum, totalflavourquantity);

                totalflavourquantity += flavourquantity;

                Flavour flavour = new Flavour(flavourtype, premFlavours.Contains(flavourtype), flavourquantity);
                flavours.Add(flavour);
            }

            return flavours;
        }

        private string GetFlavourType(string[] premFlavours, string[] regFlavours)
        {
            string flavourtype;

            do
            {
                DisplayFlavours(premFlavours.Concat(regFlavours));
                Console.Write("Enter the flavour type: ");
                flavourtype = Console.ReadLine().Trim();

                if (string.IsNullOrWhiteSpace(flavourtype) || (!premFlavours.Contains(flavourtype) && !regFlavours.Contains(flavourtype)))
                {
                    Console.WriteLine("Invalid Flavour Type! Please enter a valid Flavour Type.");
                }
            } while (string.IsNullOrWhiteSpace(flavourtype) || (!premFlavours.Contains(flavourtype) && !regFlavours.Contains(flavourtype)));

            return flavourtype;
        }

        private void DisplayFlavours(IEnumerable<string> flavours)
        {
            Console.WriteLine("Flavours that are available: ");
            foreach (string flavour in flavours)
            {
                Console.WriteLine(flavour);
            }
        }

        private int GetFlavourQuantity(int scoopnum, int totalflavourquantity)
        {
            int flavourquantity;

            do
            {
                Console.Write("Enter the flavour quantity: ");
                if (!int.TryParse(Console.ReadLine(), out flavourquantity) || flavourquantity < 1 || flavourquantity > 3)
                {
                    Console.WriteLine("Invalid input! Please enter valid Flavour Quantity.");
                }
                else if (flavourquantity + totalflavourquantity > scoopnum)
                {
                    Console.WriteLine("You've exceeded the scoop number. Please try again.");
                }
                else
                {
                    return flavourquantity;
                }
            } while (true);
        }

        private List<Topping> GetToppings(string[] Toppings)
        {
            List<Topping> toppings = new List<Topping>();
            string toppingtype;

            while (toppings.Count < 4)
            {
                toppingtype = GetToppingType(Toppings);

                if (toppingtype == "NIL" || toppings.Count == 4)
                {
                    break;
                }

                Topping topping = new Topping(toppingtype);
                toppings.Add(topping);
            }

            return toppings;
        }

        private string GetToppingType(string[] Toppings)
        {
            string toppingtype;

            do
            {
                DisplayToppings(Toppings);
                Console.Write("Enter the topping (or NIL to stop adding): ");
                toppingtype = Console.ReadLine().Trim();

                if (string.IsNullOrWhiteSpace(toppingtype) || (!Toppings.Contains(toppingtype) && toppingtype != "nil"))
                {
                    Console.WriteLine("Invalid Topping Type! Please enter a valid Topping Type.");
                }
            } while (string.IsNullOrWhiteSpace(toppingtype) || (!Toppings.Contains(toppingtype) && toppingtype != "nil"));

            return toppingtype;
        }

        private void DisplayToppings(string[] Toppings)
        {
            Console.WriteLine("Toppings that are available: ");
            foreach (string topping in Toppings)
            {
                Console.WriteLine(topping);
            }
        }

        private IceCream CreateIceCream(string option, int scoopnum, List<Flavour> flavours, List<Topping> toppings, string[] waffleFlavours)
        {
            switch (option)
            {
                case "Waffle":
                    string waffleflavour = GetWaffleFlavour(waffleFlavours);
                    return new Waffle("Waffle", scoopnum, flavours, toppings, waffleflavour);
                case "Cone":
                    bool dipped = GetDippedStatus();
                    return new Cone("Cone", scoopnum, flavours, toppings, dipped);
                case "Cup":
                    return new Cup("Cup", scoopnum, flavours, toppings);
                default:
                    return null;
            }
        }

        private string GetWaffleFlavour(string[] waffleFlavours)
        {
            string waffleflavour;

            do
            {
                DisplayWaffleFlavours(waffleFlavours);
                Console.Write("Enter the waffle flavour: ");
                waffleflavour = Console.ReadLine().Trim();

                if (string.IsNullOrWhiteSpace(waffleflavour) || !waffleFlavours.Contains(waffleflavour))
                {
                    Console.WriteLine("Waffle Flavour is not on the menu. Please Try again!");
                }
            } while (string.IsNullOrWhiteSpace(waffleflavour) || !waffleFlavours.Contains(waffleflavour));

            return waffleflavour;
        }

        private void DisplayWaffleFlavours(string[] waffleFlavours)
        {
            Console.WriteLine("Waffle Flavours that are available: ");
            foreach (string waffleFlavour in waffleFlavours)
            {
                Console.WriteLine(waffleFlavour);
            }
        }

        private bool GetDippedStatus()
        {
            bool dipped;

            do
            {
                Console.Write("Is the cone dipped? True/False: ");
                if (!bool.TryParse(Console.ReadLine(), out dipped))
                {
                    Console.WriteLine("Entered a invalid input. Please Try again.");
                }
                else
                {
                    return dipped;
                }
            } while (true);
        }
        public void AddIceCream(IceCream iceCream)
        {
            IceCreamList.Add(iceCream);
        }
        public void DeleteIceCream(int num)
        {
            IceCreamList.RemoveAt(num);
        }
        public double CalculateTotal()
        {
            double total = 0;
            foreach (IceCream icecream in IceCreamList)
            {
                total += icecream.CalculatePrice();
            }
            return total;
        }
        public override string ToString()
        {
            return "ID: " + Id + "\tTime Received: " + TimeReceived + "\tTime Fulfilled: " +
                TimeFulfilled + "\tIceCreamList: " + IceCreamList;
        }
    }
}
