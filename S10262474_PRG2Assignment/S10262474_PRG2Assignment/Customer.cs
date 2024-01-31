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
    class Customer
    {
        public string Name { get; set; }
        public int MemberId { get; set; }
        public DateTime Dob { get; set; }
        public Order CurrentOrder { get; set; }
        public List<Order> OrderHistory { get; set; } = new List<Order>();
        public PointCard Rewards { get; set; }
        public Customer() { }
        public Customer(string name, int memberId, DateTime dob)
        {
            Name = name;
            MemberId = memberId;
            Dob = dob;
        }
        public Order MakeOrder(Dictionary<int, Customer> customers, Dictionary<int, Order> orders, string[] Toppings,
    string[] regFlavours, string[] premFlavours, string[] waffleFlavours,
    string[] Options)
        {
            if (CurrentOrder != null)
            {
                Console.WriteLine("There is already a current order for this customer.");
                return null;
            }

            int largestOrderIndex = GetLargestOrderIndex(customers);

            Order order = CreateNewOrder(largestOrderIndex);

            string option;
            int scoopnum;

            do
            {
                option = GetIceCreamOption(Options);
            } while (string.IsNullOrWhiteSpace(option));

            do
            {
                scoopnum = GetScoopNumber();
            } while (scoopnum < 1 || scoopnum > 3);

            List<Flavour> flavours = GetFlavours(regFlavours, premFlavours, scoopnum);
            List<Topping> toppings = GetToppings(Toppings);

            IceCream iceCream = CreateIceCream(option, scoopnum, flavours, toppings, waffleFlavours);
            order.AddIceCream(iceCream);

            while (AddAnotherIceCream())
            {
                do
                {
                    option = GetIceCreamOption(Options);
                } while (string.IsNullOrWhiteSpace(option));

                do
                {
                    scoopnum = GetScoopNumber();
                } while (scoopnum < 1 || scoopnum > 3);

                flavours = GetFlavours(regFlavours, premFlavours, scoopnum);
                toppings = GetToppings(Toppings);

                iceCream = CreateIceCream(option, scoopnum, flavours, toppings, waffleFlavours);
                order.AddIceCream(iceCream);
            }

            CurrentOrder = order;
            order.TimeReceived = DateTime.Now;
            return order;
        }

        int GetLargestOrderIndex(Dictionary<int, Customer> customers)
        {
            int largestOrderIndex = 0;
            foreach (Customer c in customers.Values)
            {
                foreach (Order o in c.OrderHistory)
                {
                    largestOrderIndex = Math.Max(largestOrderIndex, o.Id);
                }

                if (c.CurrentOrder != null)
                {
                    largestOrderIndex = Math.Max(largestOrderIndex, c.CurrentOrder.Id);
                }
            }
            return largestOrderIndex;
        }

        Order CreateNewOrder(int largestOrderIndex)
        {
            Order order = new Order();
            order.Id = largestOrderIndex + 1;
            return order;
        }

        string GetIceCreamOption(string[] Options)
        {
            Console.WriteLine("Options that are available: ");
            foreach (string option in Options)
            {
                Console.WriteLine(option);
            }

            Console.Write("Enter option: ");
            return Console.ReadLine();
        }

        int GetScoopNumber()
        {
            int scoopnum;
            do
            {
                Console.Write("Enter number of scoops 1/2/3: ");
            } while (!int.TryParse(Console.ReadLine(), out scoopnum) || scoopnum < 1 || scoopnum > 3);

            return scoopnum;
        }

        List<Flavour> GetFlavours(string[] regFlavours, string[] premFlavours, int scoopnum)
        {
            List<Flavour> flavours = new List<Flavour>();
            int totalflavourquantity = 0;

            while (totalflavourquantity < scoopnum)
            {
                string flavourtype = GetFlavourType(regFlavours, premFlavours);
                int flavourquantity = GetFlavourQuantity(scoopnum);
                totalflavourquantity += flavourquantity;
                Flavour flavour = new Flavour(flavourtype, premFlavours.Contains(flavourtype), flavourquantity);
                flavours.Add(flavour);
            }

            return flavours;
        }

        string GetFlavourType(string[] regFlavours, string[] premFlavours)
        {
            string flavourtype;
            do
            {
                Console.WriteLine("Flavours that are available: ");
                foreach (string flavour in regFlavours.Concat(premFlavours))
                {
                    Console.WriteLine(flavour);
                }
                Console.Write("Enter the flavour type: ");
                flavourtype = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(flavourtype) || !(regFlavours.Contains(flavourtype) || premFlavours.Contains(flavourtype)));

            return flavourtype;
        }

       int GetFlavourQuantity(int scoopnum)
        {
            int flavourquantity;
            do
            {
                Console.Write("Enter the flavour quantity: ");
            } while (!int.TryParse(Console.ReadLine(), out flavourquantity) || flavourquantity < 1 || flavourquantity > scoopnum);

            return flavourquantity;
        }

        List<Topping> GetToppings(string[] Toppings)
        {
            List<Topping> toppings = new List<Topping>();
            string toppingtype = null;

            while (toppingtype != "NIL" && toppings.Count < 4)
            {
                toppingtype = GetToppingType(Toppings);

                if (toppingtype != "NIL")
                {
                    Topping topping = new Topping(toppingtype);
                    toppings.Add(topping);
                }
                else if (toppings.Count == 4)
                {
                    Console.WriteLine("Max topping amount reached.");
                }
            }

            return toppings;
        }

        string GetToppingType(string[] Toppings)
        {
            string toppingtype;
            do
            {
                Console.WriteLine("Toppings that are available: ");
                foreach (string topping in Toppings)
                {
                    Console.WriteLine(topping);
                }
                Console.Write("Enter the topping (or NIL to stop adding): ");
                toppingtype = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(toppingtype) || (!toppingtype.Equals("nil", StringComparison.OrdinalIgnoreCase) && !Toppings.Contains(toppingtype)));

            return toppingtype;
        }

        IceCream CreateIceCream(string option, int scoopnum, List<Flavour> flavours, List<Topping> toppings, string[] waffleFlavours)
        {
            switch (option)
            {
                case "Waffle":
                    string waffleflavour = GetWaffleFlavour(waffleFlavours);
                    return new Waffle("Waffle", scoopnum, flavours, toppings, waffleflavour);
                case "Cone":
                    bool dipped = GetConeDipped();
                    return new Cone("Cone", scoopnum, flavours, toppings, dipped);
                case "Cup":
                    return new Cup("Cup", scoopnum, flavours, toppings);
                default:
                    return null;
            }
        }

        string GetWaffleFlavour(string[] waffleFlavours)
        {
            string waffleflavour;
            do
            {
                Console.WriteLine("Waffle Flavours that are available: ");
                foreach (string flavour in waffleFlavours)
                {
                    Console.WriteLine(flavour);
                }
                Console.Write("Enter the waffle flavour: ");
                waffleflavour = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(waffleflavour) || !waffleFlavours.Contains(waffleflavour));

            return waffleflavour;
        }

        bool GetConeDipped()
        {
            bool dipped;
            do
            {
                Console.Write("Is the cone dipped? True/False: ");
            } while (!bool.TryParse(Console.ReadLine(), out dipped));

            return dipped;
        }

        bool AddAnotherIceCream()
        {
            string addicecream;
            do
            {
                Console.Write("Add another Ice Cream? Y/N: ");
                addicecream = Console.ReadLine().ToUpper();
            } while (string.IsNullOrWhiteSpace(addicecream) || (addicecream != "Y" && addicecream != "N"));

            return addicecream == "Y";
        }
        public bool IsBirthday()
        {
            int month = Dob.Month;
            int day = Dob.Year;
            if (month == DateTime.Now.Month && day == DateTime.Now.Day)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override string ToString()
        {
            return "Name: " + Name + "\tMember ID: " + MemberId + "\tDate Of Birth: " + Dob;
        }
    }

}

