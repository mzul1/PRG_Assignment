//==========================================================
// Student Number : S10262474
// Student Name : Chew Jin Xuan
// Partner Name : Zulhimi
//==========================================================

// See https://aka.ms/new-console-template for more information
using S10262474_PRG2Assignment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


string[] Toppings = { "Sprinkles", "Mochi", "Sago", "Oreos" };
string[] regFlavours = { "Vanilla", "Chocolate", "Strawberry" };
string[] premFlavours = { "Durian", "Ube", "Sea Salt" };

// Customers
Dictionary<int, Customer> customers = LoadCustomers("customers.csv");

// Orders
var orderData = LoadOrders("orders.csv");
Dictionary<int, Order> orders = orderData.Item1;
Dictionary<int, int> orderIDCustomerID = orderData.Item2;

// Add Order to Customer
LinkOrdersToCustomers(customers, orderIDCustomerID, orders);

// Order Queues
Queue<Order> orderqueue = new Queue<Order>();
Queue<Order> goldqueue = new Queue<Order>();

// Methods
Dictionary<int, Customer> LoadCustomers(string filePath)
{
    Dictionary<int, Customer> customers = new Dictionary<int, Customer>();
    string[] CustomerCsvLines = File.ReadAllLines(filePath);
    for (int i = 1; i < CustomerCsvLines.Length; i++)
    {
        string[] line = CustomerCsvLines[i].Split(",");
        if (line.Length < 6)
        {
            // Handle the error for lines with insufficient data
            //Console.WriteLine($"Line {i} is malformed with {line.Length} values.");
            continue;
        }

        try
        {
            Customer customer = new Customer(line[0], Convert.ToInt32(line[1]), DateTime.Parse(line[2]));
            PointCard pointCard = new PointCard(Convert.ToInt32(line[4]), Convert.ToInt32(line[5]));
            pointCard.Tier = line[3];
            customer.Rewards = pointCard;
            customers.Add(customer.MemberId, customer);
        }
        catch (Exception ex)
        {
            // Log or handle the exception
            //Console.WriteLine($"Error processing line {i}: {ex.Message}");
        }
    }
    return customers;
}

Tuple<Dictionary<int, Order>, Dictionary<int, int>> LoadOrders(string filePath)
{
    Dictionary<int, Order> orders = new Dictionary<int, Order>();
    Dictionary<int, int> orderIDCustomerID = new Dictionary<int, int>();
    string[] ordersFile = File.ReadAllLines(filePath);
    for (int i = 1; i < ordersFile.Length; i++)
    {
        string[] line = ordersFile[i].Split(",");
        int orderID = Convert.ToInt32(line[0]);
        ProcessOrderLine(orders, orderIDCustomerID, line, orderID);
    }
    return new Tuple<Dictionary<int, Order>, Dictionary<int, int>>(orders, orderIDCustomerID);
}

Order ProcessOrderLine(Dictionary<int, Order> orders, Dictionary<int, int> orderIDCustomerID, string[] line, int orderID)
{
    Order order;
    if (!orders.ContainsKey(orderID))
    {
        order = new Order(orderID, DateTime.Parse(line[2]));
        order.TimeFulfilled = DateTime.Parse(line[3]);
        orders.Add(orderID, order);
        orderIDCustomerID.Add(orderID, Convert.ToInt32(line[1]));
    }
    else
    {
        order = orders[orderID];
    }

    IceCream iceCream = CreateIceCream(line);
    order.AddIceCream(iceCream);
    return order;
}

IceCream CreateIceCream(string[] line)
{
    string TypeofIceCream = line[4];
    int ScoopsNum = Convert.ToInt32(line[5]);
    List<Flavour> flavours = GetFlavours(line, ScoopsNum);
    List<Topping> toppings = GetToppings(line);

    IceCream iceCream = null;
    switch (TypeofIceCream)
    {
        case "Waffle":
            iceCream = new Waffle("Waffle", ScoopsNum, flavours, toppings, line[7]);
            break;
        case "Cone":
            iceCream = new Cone("Cone", ScoopsNum, flavours, toppings, line[6] == "TRUE");
            break;
        case "Cup":
            iceCream = new Cup("Cup", ScoopsNum, flavours, toppings);
            break;
    }
    return iceCream;
}

List<Flavour> GetFlavours(string[] line, int ScoopsNum)
{
    List<Flavour> flavours = new List<Flavour>();
    for (int x = 8; x < 8 + ScoopsNum; x++)
    {
        Flavour TypeofFlavour = flavours.Find(f => f.Type == line[x]);
        if (TypeofFlavour == null)
        {
            flavours.Add(new Flavour(line[x], premFlavours.Contains(line[x]), 1));
        }
        else
        {
            TypeofFlavour.Quantity += 1;
        }
    }
    return flavours;
}

List<Topping> GetToppings(string[] line)
{
    List<Topping> toppings = new List<Topping>();
    for (int x = 11; x < 15; x++)
    {
        if (line[x] != "")
        {
            toppings.Add(new Topping(line[x]));
        }
    }
    return toppings;
}

void LinkOrdersToCustomers(Dictionary<int, Customer> customers, Dictionary<int, int> orderIDCustomerID, Dictionary<int, Order> orders)
{
    foreach (int orderID in orderIDCustomerID.Keys)
    {
        int customerID = orderIDCustomerID[orderID];
        Customer customer = customers[customerID];
        customer.OrderHistory.Add(orders[orderID]);
    }
}

int DisplayMenu()
{
    Console.WriteLine("-------------Menu-------------");
    Console.WriteLine("[1] List all customers");
    Console.WriteLine("[2] List all current orders");
    Console.WriteLine("[3] Register a new customer");
    Console.WriteLine("[4] Create a customer's order");
    Console.WriteLine("[5] Display order details of a customer");
    Console.WriteLine("[6] Modify order details");
    Console.WriteLine("[0] Exit");
    Console.WriteLine();
    Console.Write("Enter your choice: ");
    int option;
    while (!int.TryParse(Console.ReadLine(), out option) || option < 0 || option > 6)
    {
        Console.Write("Invalid input. Please enter a number between 0 and 6: ");
    }
    return option;
}

bool isRunning = true;
while (isRunning)
{
    int option = DisplayMenu();
    switch (option)
    {
        case 1:
            ListAllCustomers(customers);
            break;
        case 2:
            ListAllOrders(customers);
            break;
        case 3:
            NewCustomer(customers);
            break;
        case 4:
            CreateCustomerOrder(customers, orders);
            break;
        case 5:
            DisplayOrderDetailsOfCustomer(customers);
            break;
        case 6:
            ModifyOrderDetails(customers);
            break;
        case 0:
            isRunning = false;
            break;
        default:
            Console.WriteLine("Invalid option, please try again.");
            break;
    }
    Console.WriteLine();
}

//basic feature 1 - List all customers
void ListAllCustomers(Dictionary<int, Customer> customers)
{
    Console.WriteLine("{0, -10} {1, -10} {2, -15} {3, -10} {4, -10} {5, -10} {6, 10} {7, 20}",
        "Name", "MemberID", "DateOfBirth", "Points",
        "PunchCard", "Tier", "CurrentOrderID", "OrderHistoryID");

    foreach (Customer customer in customers.Values)
    {
        string currentOrderID = customer.CurrentOrder != null ? (customer.CurrentOrder.Id + 1).ToString() : "No Current Order";
        string orderHistoryIDs = string.Join(", ", customer.OrderHistory.Select(order => order.Id));

        Console.WriteLine("{0, -10} {1, -10} {2, -15:dd/MM/yyyy} {3, -10} {4, -10} {5, -10} {6, -10} {7, 8}", customer.Name, customer.MemberId, customer.Dob, customer.Rewards.Points, customer.Rewards.PunchCard, customer.Rewards.Tier, currentOrderID, orderHistoryIDs);
    }

    Console.WriteLine();
}

//basic feature 2 - List all current orders
void ListAllOrders(Dictionary<int, Customer> customers)
{
    Console.WriteLine("Current Orders:");
    bool hasOrders = false;

    foreach (var customer in customers.Values)
    {
        if (customer.CurrentOrder == null) continue;

        hasOrders = true;
        Console.WriteLine($"{customer.Name}'s Order:");
        Console.WriteLine($"Order ID: {customer.CurrentOrder.Id}");
        Console.WriteLine($"Time Received: {customer.CurrentOrder.TimeReceived}");

        foreach (var iceCream in customer.CurrentOrder.IceCreamList)
        {
            Console.WriteLine($"Ice cream option: {iceCream.Option}");
            Console.WriteLine($"Ice cream scoops: {iceCream.Scoops}");
            Console.Write("Ice cream flavours: ");
            Console.WriteLine(string.Join(", ", iceCream.Flavours));
            Console.Write("Ice cream toppings: ");
            Console.WriteLine(string.Join(", ", iceCream.Toppings));
        }
        Console.WriteLine();
    }

    if (!hasOrders)
    {
        Console.WriteLine("No current orders.");
    }

    Console.WriteLine();
}

//basic feature 3 - Register a new customer
void NewCustomer(Dictionary<int, Customer> customers)
{
    Console.Write("Enter Name: ");
    string name = Console.ReadLine();
    Console.Write("Enter ID Number: ");
    int id;
    while (!int.TryParse(Console.ReadLine(), out id))
    {
        Console.Write("Invalid input. Please enter a valid ID Number: ");
    }
    Console.Write("Enter Date of Birth (e.g., MM/dd/yyyy): ");
    DateTime dob;
    while (!DateTime.TryParse(Console.ReadLine(), out dob))
    {
        Console.Write("Invalid input. Please enter a valid Date of Birth (e.g., MM/dd/yyyy): ");
    }

    if (customers.ContainsKey(id))
    {
        Console.WriteLine("\nA customer with this ID already exists.\n");
    }
    else
    {
        Customer customer = new Customer(name, id, dob);
        customer.Rewards = new PointCard { Tier = "Ordinary" };
        customers.Add(id, customer);
        string data = $"{name},{id},{dob:yyyy-MM-dd}, {customer.Rewards.Tier}, {customer.Rewards.Points}, {customer.Rewards.PunchCard}";

        File.AppendAllText("customers.csv", data + Environment.NewLine);

        Console.WriteLine("\nCustomer registered successfully!\n");
    }
}

//basic feature 4 - Create customer's order
void CreateCustomerOrder(Dictionary<int, Customer> customers, Dictionary<int, Order> orders)
{
    ListAllCustomers(customers);
    Console.WriteLine();

    Console.Write("Enter your customer ID: ");
    if (!int.TryParse(Console.ReadLine(), out int id) || !customers.ContainsKey(id))
    {
        Console.WriteLine("Invalid Customer ID.");
        return;
    }

    Customer customer = customers[id];
    Order order = new Order();
    string input;
    do
    {
        IceCream iceCream = GetIceCreamDetailsFromUser();
        if (iceCream != null)
        {
            order.AddIceCream(iceCream);
            Console.WriteLine("Ice cream added to order.");
        }

        Console.Write("Add Another Ice Cream? [Y/N]: ");
        input = Console.ReadLine().Trim().ToUpper();
    } while (input == "Y");

    int newOrderId = orders.Keys.Any() ? orders.Keys.Max() + 1 : 1;
    orders.Add(newOrderId, order);
    customer.CurrentOrder = order;
    Console.WriteLine("Order created with ID " + newOrderId);
}

IceCream GetIceCreamDetailsFromUser()
{
    Console.Write("Enter option (Waffle/Cone/Cup): ");
    string option = Console.ReadLine();
    Console.Write("Enter number of scoops: ");
    if (!int.TryParse(Console.ReadLine(), out int scoops) || scoops <= 0)
    {
        Console.WriteLine("Invalid number of scoops.");
        return null;
    }

    var flavours = GetFlavoursFromUser();
    var toppings = GetToppingsFromUser();
    return CreateIceCreams(option, scoops, flavours, toppings);
}

List<Flavour> GetFlavoursFromUser()
{
    List<Flavour> flavours = new List<Flavour>();
    string flavourType;
    do
    {
        Console.Write("Enter flavour type (or 'NIL' to stop adding): ");
        flavourType = Console.ReadLine();
        if (flavourType == "NIL") break;

        Console.Write("Is it premium? (True/False): ");
        bool isPremium = Convert.ToBoolean(Console.ReadLine());
        Console.Write("Enter flavour quantity: ");
        int quantity = Convert.ToInt32(Console.ReadLine());

        flavours.Add(new Flavour(flavourType, isPremium, quantity));
    } while (flavourType != "NIL");

    return flavours;
}

List<Topping> GetToppingsFromUser()
{
    List<Topping> toppings = new List<Topping>();
    string toppingType;
    do
    {
        Console.Write("Enter topping (or 'NIL' to stop adding): ");
        toppingType = Console.ReadLine();
        if (toppingType == "NIL") break;

        toppings.Add(new Topping(toppingType));
    } while (toppingType != "NIL");

    return toppings;
}

IceCream CreateIceCreams(string option, int scoops, List<Flavour> flavours, List<Topping> toppings)
{
    switch (option)
    {
        case "Waffle":
            Console.Write("Enter waffle flavour: ");
            return new Waffle(Console.ReadLine(), scoops, flavours, toppings, Console.ReadLine());
        case "Cone":
            Console.Write("Is cone dipped? (True/False): ");
            return new Cone("Cone", scoops, flavours, toppings, Convert.ToBoolean(Console.ReadLine()));
        case "Cup":
            return new Cup("Cup", scoops, flavours, toppings);
        default:
            Console.WriteLine("Invalid option.");
            return null;
    }
}

//basic feature 5 - Display order details of a customer
void DisplayOrderDetailsOfCustomer(Dictionary<int, Customer> customers)
{
    ListAllCustomers(customers);
    Console.WriteLine();
    Console.Write("Enter your customer ID: ");
    if (!int.TryParse(Console.ReadLine(), out int id) || !customers.TryGetValue(id, out Customer customer))
    {
        Console.WriteLine("Invalid customer ID or customer not found.");
        return;
    }

    if (customer.OrderHistory.Any())
    {
        foreach (Order order in customer.OrderHistory)
        {
            DisplayOrder(order);
        }
    }
    else
    {
        Console.WriteLine("No previous orders.");
    }

    if (customer.CurrentOrder != null)
    {
        Console.WriteLine("Current Order:");
        DisplayOrder(customer.CurrentOrder);
    }
    else
    {
        Console.WriteLine("No current orders.");
    }
}

void DisplayOrder(Order order)
{
    Console.WriteLine($"Order ID: {order.Id}");
    Console.WriteLine($"Time Received: {order.TimeReceived}");
    foreach (IceCream iceCream in order.IceCreamList)
    {
        Console.WriteLine($"Ice cream option: {iceCream.Option}");
        Console.WriteLine($"Ice cream scoops: {iceCream.Scoops}");
        DisplayFlavours(iceCream.Flavours);
        DisplayToppings(iceCream.Toppings);
    }
    Console.WriteLine();
}

void DisplayFlavours(List<Flavour> flavours)
{
    Console.WriteLine("Ice cream flavours: ");
    foreach (Flavour flavour in flavours)
    {
        Console.WriteLine($"{flavour}");
    }
}

void DisplayToppings(List<Topping> toppings)
{
    Console.WriteLine("Ice cream toppings: ");
    foreach (Topping topping in toppings)
    {
        Console.WriteLine($"{topping}");
    }
}

//basic feature 6 - Modify order details
void ModifyOrderDetails(Dictionary<int, Customer> customers)
{
    ListAllCustomers(customers);
    Console.WriteLine();
    Console.Write("Enter your customer ID: ");
    int id = Convert.ToInt32(Console.ReadLine());
    Customer customer = customers[id];
    if (customer.CurrentOrder != null)
    {
        Order currentOrder = customer.CurrentOrder;
        foreach (IceCream iceCream in customer.CurrentOrder.IceCreamList)
        {
            Console.WriteLine($"Ice cream {customer.CurrentOrder.IceCreamList.IndexOf(iceCream) + 1}");
            Console.WriteLine($"Ice cream option: {iceCream.Option}");
            Console.WriteLine($"Ice cream scoops: {iceCream.Scoops}");
            Console.WriteLine("Ice cream flavours: ");
            foreach (Flavour flav in iceCream.Flavours)
            {
                Console.WriteLine(flav.ToString());
            }
            Console.WriteLine("Ice cream toppings: ");
            foreach (Topping topping in iceCream.Toppings)
            {
                Console.WriteLine(topping.ToString());
            }
        }
        Console.WriteLine();
        Console.WriteLine("Choose one of the following options: ");
        Console.WriteLine("[1] Choose an existing ice cream object to modify.");
        Console.WriteLine("[2] Add an entirely new ice cream object to the order.");
        Console.WriteLine("[3] Choose an existing ice cream object to delete from the order.");
        Console.Write("Enter option: ");
        int option = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine();
        if (option == 1)
        {
            Console.WriteLine("Enter which ice cream to modify: ");
            int IceCreamNo = Convert.ToInt32(Console.ReadLine());
            currentOrder.ModifyIceCream(IceCreamNo);
        }
        else if (option == 2)
        {
            Console.WriteLine("Enter ice cream option: ");
            string icecreamoption = Console.ReadLine();
            Console.WriteLine("Enter number of scoops: ");
            int scoopnum = Convert.ToInt32(Console.ReadLine());
            List<Flavour> flavours = new List<Flavour>();
            Console.WriteLine("Enter flavour type (or NIL to stop adding): ");
            string flavourtype = Console.ReadLine();
            while (flavourtype != "NIL")
            {
                Console.WriteLine("Is it premium? (True/False): ");
                bool flavourpremium = Convert.ToBoolean(Console.ReadLine());
                Console.WriteLine("Enter flavour quantity: ");
                int flavourquantity = Convert.ToInt32(Console.ReadLine());
                Flavour flavour = new Flavour(flavourtype, flavourpremium, flavourquantity);
                flavours.Add(flavour);
                Console.WriteLine("Enter flavour (or NIL to stop adding): ");
                flavourtype = Console.ReadLine();
            }
            List<Topping> toppings = new List<Topping>();
            Console.WriteLine("Enter topping (or NIL to stop adding): ");
            string toppingtype = Console.ReadLine();
            while (toppingtype != "NIL")
            {
                Topping topping = new Topping(toppingtype);
                toppings.Add(topping);
                Console.WriteLine("Enter topping (or NIL to stop adding): ");
                toppingtype = Console.ReadLine();
            }

            IceCream iceCream = null;
            switch (icecreamoption)
            {
                case "Waffle":
                    Console.Write("Enter waffle flavour: ");
                    string waffleflavour = Console.ReadLine();
                    iceCream = new Waffle("Waffle", scoopnum, flavours, toppings, waffleflavour);
                    break;
                case "Cone":
                    Console.Write("Is cone dipped? (True/False): ");
                    bool dipped = Convert.ToBoolean(Console.ReadLine());
                    iceCream = new Cone("Cone", scoopnum, flavours, toppings, dipped);
                    break;
                case "Cup":
                    iceCream = new Cup("Cup", scoopnum, flavours, toppings);
                    break;
            }
            currentOrder.AddIceCream(iceCream);
        }
        else if (option == 3)
        {
            Console.WriteLine("Enter which ice cream to delete: ");
            int IceCreamNo = Convert.ToInt32(Console.ReadLine()) - 1;
            if (currentOrder.IceCreamList.Count > 1)
            {
                currentOrder.DeleteIceCream(IceCreamNo);
            }
            else
            {
                Console.WriteLine("Cannot have zero ice creams in an order.");
            }
        }
        else
        {
            Console.WriteLine("Option does not exist.");
        }
    }
    else
    {
        Console.WriteLine("No current orders.");
    }
    foreach (IceCream iceCream in customer.CurrentOrder.IceCreamList)
    {
        Console.WriteLine($"Ice cream {customer.CurrentOrder.IceCreamList.IndexOf(iceCream) + 1}");
        Console.WriteLine($"Ice cream option: {iceCream.Option}");
        Console.WriteLine($"Ice cream scoops: {iceCream.Scoops}");
        Console.WriteLine("Ice cream flavours: ");
        foreach (Flavour flav in iceCream.Flavours)
        {
            Console.WriteLine(flav.ToString());
        }
        Console.WriteLine("Ice cream toppings: ");
        foreach (Topping topping in iceCream.Toppings)
        {
            Console.WriteLine(topping.ToString());
        }
    }
}



//advanced features - a

//advanced features - b - display monthly charged amounts breakdown & total charged amounts for the year
void DisplayChargedAmounts(Dictionary<int, Order> orders)
{
    Console.Write("Enter the year: ");
    int year = Convert.ToInt32(Console.ReadLine());
    double[] monthlyTotals = new double[12];
    double overallTotal = 0;

    foreach (Order order in orders.Values)
    {
        if (order.TimeFulfilled.HasValue && order.TimeFulfilled.Value.Year == year)
        {
            int monthIndex = order.TimeFulfilled.Value.Month - 1;
            monthlyTotals[monthIndex] += order.CalculateTotal();
        }
    }

    string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
    for (int i = 0; i < monthlyTotals.Length; i++)
    {
        Console.WriteLine($"{months[i]} {year}: ${monthlyTotals[i]:0.00}");
        overallTotal += monthlyTotals[i];
    }

    Console.WriteLine();
    Console.WriteLine("Total: ${0:0.00}", overallTotal);
}

DisplayChargedAmounts(orders);








