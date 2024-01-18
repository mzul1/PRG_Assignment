using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using PRG2_Assignment;

public class Program
{
    private static Dictionary<int, int> orderToMemberMapping = new Dictionary<int, int>();
    private static Dictionary<int, Customer> customers = new Dictionary<int, Customer>();
    private static Dictionary<string, double> toppings = new Dictionary<string, double>();
    private static Dictionary<string, double> flavours = new Dictionary<string, double>();
    private static Dictionary<string, double> options = new Dictionary<string, double>();
    public static void Main()
    {
        LoadCustomers("customers.csv");
        LoadToppings("toppings.csv");
        LoadFlavours("flavours.csv");
        LoadOptions("options.csv");

        var allOrders = ReadOrdersFromCSV("orders.csv");

        var goldMemberQueue = new Queue<Order>();
        var regularQueue = new Queue<Order>();

        foreach (var order in allOrders)
        {
            int memberId;
            if (orderToMemberMapping.ContainsKey(order.Id))
            {
                memberId = orderToMemberMapping[order.Id];
            }
            else
            {
                Console.WriteLine($"Order ID {order.Id} not found in member mapping."); // Debug line
                continue;  // Skip this order if no member mapping is found
            }

            if (IsGoldMember(memberId))
            {
                goldMemberQueue.Enqueue(order);
                Console.WriteLine($"Order {order.Id} enqueued in Gold Member Queue.");  // Debug line
            }
            else
            {
                regularQueue.Enqueue(order);
                Console.WriteLine($"Order {order.Id} enqueued in Regular Queue.");  // Debug line
            }
        }

        DisplayOrders("Gold Member Orders:", goldMemberQueue);
        DisplayOrders("Regular Orders:", regularQueue);
    }

    private static void LoadCustomers(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        foreach (var line in lines.Skip(1))
        {
            var parts = line.Split(',');
            string name = parts[0];
            int memberId = int.Parse(parts[1]);
            DateTime dob = DateTime.ParseExact(parts[2], "dd/MM/yyyy", CultureInfo.InvariantCulture);

            customers[memberId] = new Customer(name, memberId, dob);
        }
    }

    private static void LoadToppings(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        foreach (var line in lines.Skip(1))
        {
            var parts = line.Split(',');
            string toppingName = parts[0];
            double cost = double.Parse(parts[1]);

            toppings[toppingName] = cost;
        }
    }

    private static void LoadFlavours(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        foreach (var line in lines.Skip(1))
        {
            var parts = line.Split(',');
            string flavourName = parts[0];
            double cost = double.Parse(parts[1]);

            flavours[flavourName] = cost;
        }
    }

    private static void LoadOptions(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        foreach (var line in lines.Skip(1))
        {
            var parts = line.Split(',');
            string key = $"{parts[0]}_{parts[1]}_{parts[2]}_{parts[3]}";
            double cost = double.Parse(parts[4]);

            options[key] = cost;
        }
    }

    private static bool IsGoldMember(int memberId)
    {
        return customers.ContainsKey(memberId) && customers[memberId].Rewards.Tier == MembershipStatus.Gold;
    }

    private static List<Order> ReadOrdersFromCSV(string filePath)
    {
        var orders = new List<Order>();
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines.Skip(1)) // Skip the header
        {
            var parts = line.Split(',');
            int orderId = int.Parse(parts[0]);
            int memberId = int.Parse(parts[1]);
            DateTime timeReceived = DateTime.ParseExact(parts[2], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            string option = parts[4];
            int scoops = int.Parse(parts[5]);
            bool dipped = parts[6].ToLower() == "true";
            string waffleFlavour = parts[7];
            List<Flavour> iceCreamFlavours = new List<Flavour>(); // Assuming Flavour class has a constructor that takes a string
            for (int i = 8; i <= 10; i++)
            {
                if (!string.IsNullOrWhiteSpace(parts[i]))
                {
                    iceCreamFlavours.Add(new Flavour(parts[i], flavours.ContainsKey(parts[i]), 1));
                }
            }

            List<Topping> iceCreamToppings = new List<Topping>(); // Assuming Topping class has a constructor that takes a string
            for (int i = 11; i <= 14; i++)
            {
                if (!string.IsNullOrWhiteSpace(parts[i]))
                {
                    iceCreamToppings.Add(new Topping(parts[i]));
                }
            }

            IceCream iceCream;
            switch (option)
            {
                case "Cup":
                    iceCream = new Cup(scoops, iceCreamFlavours, iceCreamToppings);
                    break;
                case "Cone":
                    iceCream = new Cone(scoops, iceCreamFlavours, iceCreamToppings, dipped);
                    break;
                case "Waffle":
                    iceCream = new Waffle(scoops, iceCreamFlavours, iceCreamToppings, waffleFlavour);
                    break;
                default:
                    continue; // Skip if the option is not recognized
            }

            Order order = new Order(orderId, timeReceived);
            order.AddIceCream(iceCream); // Assuming Order has an AddIceCream method
            orderToMemberMapping[orderId] = memberId;

            orders.Add(order);
        }

        return orders;
    }

    private static void DisplayOrders(string title, Queue<Order> queue)
    {
        Console.WriteLine(title);
        if (!queue.Any())
        {
            Console.WriteLine("No orders in this queue.");
            return;
        }

        foreach (var order in queue)
        {
            if (orderToMemberMapping.ContainsKey(order.Id))
            {
                int memberId = orderToMemberMapping[order.Id];
                var customer = customers[memberId];
                Console.WriteLine($"Member ID: {customer.MemberId}, Member Name: {customer.Name}, Order: {order}");
            }
            else
            {
                Console.WriteLine($"Warning: Order ID {order.Id} not found in member mapping.");
            }
        }
    }
}
