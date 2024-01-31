using S10262474_PRG2Assignment;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
//==========================================================
// Student Number : S10262474
// Student Name : Chew Jin Xuan
// Partner Name : Zulhimi
//==========================================================
string[] Toppings = { "Sprinkles", "Mochi", "Sago", "Oreos" };
string[] regFlavours = { "Vanilla", "Chocolate", "Strawberry" };
string[] premFlavours = { "Durian", "Ube", "Sea Salt" };
string[] waffleFlavours = { "Red Velvet", "Charcoal", "Pandan" };
string[] Options = { "Cup", "Cone", "Waffle" };

Dictionary<int, Customer> customers = new Dictionary<int, Customer>();
Dictionary<int, Order> orders = new Dictionary<int, Order>();
Dictionary<int, int> orderIDCustomerID = new Dictionary<int, int>();


    LoadCustomersFromCsv("customers.csv");
    LoadOrdersFromCsv("orders.csv");
    AddOrdersToCustomers();

    // Initialize order queues
    Queue<Order> orderqueue = new Queue<Order>();
    Queue<Order> goldqueue = new Queue<Order>();


void LoadCustomersFromCsv(string filePath)
{
    string[] CustomerCsvLines = File.ReadAllLines(filePath);

    for (int i = 1; i < CustomerCsvLines.Length; i++)
    {
        string[] line = CustomerCsvLines[i].Split(",");
        Customer customer = new Customer(line[0], Convert.ToInt32(line[1]), DateTime.Parse(line[2]));
        PointCard pointCard = new PointCard(Convert.ToInt32(line[4]), Convert.ToInt32(line[5]));
        pointCard.Tier = line[3];
        customer.Rewards = pointCard;
        customers.Add(customer.MemberId, customer);
    }
}

void LoadOrdersFromCsv(string filePath)
{
    string[] ordersFile = File.ReadAllLines(filePath);

    for (int i = 1; i < ordersFile.Length; i++)
    {
        string[] line = ordersFile[i].Split(",");
        int orderID = Convert.ToInt32(line[0]);

        if (!orders.TryGetValue(orderID, out Order order))
        {
            order = new Order(orderID, DateTime.Parse(line[2]));
            order.TimeFulfilled = DateTime.Parse(line[3]);
            orders.Add(orderID, order);
            orderIDCustomerID.Add(orderID, Convert.ToInt32(line[1]));
        }

        string TypeofIceCream = line[4];
        int ScoopsNum = Convert.ToInt32(line[5]);
        List<Flavour> flavours = GetIceCreamFlavours(line, ScoopsNum);
        List<Topping> toppings = GetIceCreamToppings(line);
        IceCream iceCream = CreateIceCream(TypeofIceCream, ScoopsNum, flavours, toppings, line);
        order.AddIceCream(iceCream);
    }
}

List<Flavour> GetIceCreamFlavours(string[] line, int ScoopsNum)
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

List<Topping> GetIceCreamToppings(string[] line)
{
    List<Topping> toppings = new List<Topping>();

    for (int x = 11; x < 15; x++)
    {
        if (!string.IsNullOrEmpty(line[x]))
        {
            toppings.Add(new Topping(line[x]));
        }
    }
    return toppings;
}

IceCream CreateIceCream(string TypeofIceCream, int ScoopsNum, List<Flavour> flavours, List<Topping> toppings, string[] line)
{
    switch (TypeofIceCream)
    {
        case "Waffle":
            return new Waffle("Waffle", ScoopsNum, flavours, toppings, line[7]);
        case "Cone":
            return new Cone("Cone", ScoopsNum, flavours, toppings, line[6] == "TRUE");
        case "Cup":
            return new Cup("Cup", ScoopsNum, flavours, toppings);
        default:
            return null;
    }
}

void AddOrdersToCustomers()
{
    foreach (int orderID in orderIDCustomerID.Keys)
    {
        int customerID = orderIDCustomerID[orderID];
        Customer customer = customers[customerID];
        customer.OrderHistory.Add(orders[orderID]);
    }
}


int option;

do
{
    option = DisplayMenu();

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
        case 7:
            if (goldqueue.Count > 0)
            {
                ProcessOrderAndCheckout(goldqueue, customers);
            }
            else
            {
                ProcessOrderAndCheckout(orderqueue, customers);
            }
            break;
        case 8:
            DisplayChargedAmounts(orders);
            break;
        case 0:
            Console.WriteLine("Exit from Program...");
            break;
        default:
            Console.WriteLine("This is a Invalid input! Please enter a valid option.");
            break;
    }

    Console.WriteLine();

} while (option != 0);
    

    static int DisplayMenu()
{
    int option;

    Console.WriteLine("-------------Menu-------------");
    Console.WriteLine("[1] List all customers");
    Console.WriteLine("[2] List all current orders");
    Console.WriteLine("[3] Register a new customer");
    Console.WriteLine("[4] Create a customer's order");
    Console.WriteLine("[5] Display order details of a customer");
    Console.WriteLine("[6] Modify order details");
    Console.WriteLine("[7] Process an order and checkout");
    Console.WriteLine("[8] Display monthly charged amounts breakdown & total charged amounts for the year");
    Console.WriteLine("[0] Exit");
    Console.WriteLine();

    do
    {
        Console.Write("Enter your choice: ");
        if (!int.TryParse(Console.ReadLine(), out option))
        {
            Console.WriteLine("This is a Invalid input! Please try again.");
        }
    } while (option < 0 || option > 8);

    return option;
}

//basic feature 1 - List all customers (Chew Jin Xuan)
void ListAllCustomers(Dictionary<int, Customer> customers)
{
    Console.WriteLine("{0, -10} {1, -10} {2, -15} {3, -10} {4, -10} {5, -10} {6, 10} {7,20}",
        "Name", "MemberID", "DateOfBirth", "Points",
        "PunchCard", "Tier", "CurrentOrderID", "OrderHistoryID");

    foreach (Customer customer in customers.Values)
    {
        Console.WriteLine(CustomerToString(customer));
    }

    Console.WriteLine();
}

string CustomerToString(Customer customer)
{
    string currentOrderId = customer.CurrentOrder != null ? customer.CurrentOrder.Id.ToString() : "No Current Order";
    string orderHistoryIds = string.Join(", ", customer.OrderHistory.Select(order => order.Id));

    return string.Format("{0, -10} {1, -10} {2, -15:dd/MM/yyyy} {3, -10} {4, -10} {5, -10} {6, -10} {7, 8}",
        customer.Name, customer.MemberId, customer.Dob,
        customer.Rewards.Points, customer.Rewards.PunchCard, customer.Rewards.Tier, currentOrderId, orderHistoryIds);
}

//basic feature 2 - List all current orders (zulhimi)
void ListAllOrders(Dictionary<int, Customer> customers)
{
    Console.WriteLine();

    ListOrders("Gold Queue", goldqueue);
    ListOrders("Regular Queue", orderqueue);

    Console.WriteLine();
}

void ListOrders(string queueName, Queue<Order> orders)
{
    Console.WriteLine($"{queueName}:");

    if (orders.Count > 0)
    {
        foreach (Order order in orders)
        {
            DisplayOrderDetails(order);
        }
    }
    else
    {
        Console.WriteLine($"No orders in {queueName.ToLower()}.");
    }
}

void DisplayOrderDetails(Order order)
{
    Console.WriteLine();
    Console.WriteLine($"Order ID: {order.Id}");
    Console.WriteLine($"Time Received: {order.TimeReceived}");

    foreach (IceCream iceCream in order.IceCreamList)
    {
        DisplayIceCreamDetails(iceCream);
    }

    Console.WriteLine();
}

void DisplayIceCreamDetails(IceCream iceCream)
{
    Console.WriteLine($"Ice cream option: {iceCream.Option}");
    Console.WriteLine($"Ice cream scoops: {iceCream.Scoops}");
    Console.WriteLine("Ice cream flavours: ");

    foreach (Flavour flav in iceCream.Flavours)
    {
        Console.WriteLine(flav.ToString());
    }

    Console.WriteLine("Ice cream toppings: ");

    if (iceCream.Toppings.Count > 0)
    {
        foreach (Topping topping in iceCream.Toppings)
        {
            Console.WriteLine(topping.ToString());
        }
    }
    else
    {
        Console.WriteLine("No toppings");
    }

    Console.WriteLine();
}

//basic feature 3 - Register a new customer (Chew Jin Xuan)
void NewCustomer(Dictionary<int, Customer> customers)
{
    string name;
    int id;
    DateTime dob;

    do
    {
        name = GetValidName();
    } while (string.IsNullOrWhiteSpace(name));

    do
    {
        id = GetValidID(customers);
    } while (id < 0 || id > 999999 || customers.ContainsKey(id));

    do
    {
        dob = GetValidDateOfBirth();
    } while (DateTime.Compare(DateTime.Now, dob) < 0);

    Customer customer = new Customer(name, id, dob);
    PointCard pointCard = new PointCard();
    pointCard.Tier = "Ordinary";
    customer.Rewards = pointCard;
    customers.Add(id, customer);
    string data = $"{name},{id},{dob:dd/MM/yyyy}, {customer.Rewards.Tier}, {customer.Rewards.Points}, {customer.Rewards.PunchCard}";

    using (StreamWriter sw = new StreamWriter("customers.csv", true))
    {
        sw.WriteLine(data);
    }

    Console.WriteLine();
    if (customers.ContainsKey(id))
    {
        Console.WriteLine("Customer is registered successfully!");
    }
    else
    {
        Console.WriteLine("Customer registration failed!");
    }
    Console.WriteLine();
}

string GetValidName()
{
    string name;
    do
    {
        Console.Write("Enter Name: ");
        name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("This is a invalid name. Please enter a valid name.");
        }
    } while (string.IsNullOrWhiteSpace(name));
    return name;
}

int GetValidID(Dictionary<int, Customer> customers)
{
    int id;
    do
    {
        Console.Write("Enter ID Number (XXXXXX): ");
        if (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("This is a invalid ID format. Please enter a valid integer.");
        }
    } while (!IsValidID(id) || customers.ContainsKey(id));
    return id;
}

bool IsValidID(int id)
{
    return id >= 0 && id <= 999999;
}

DateTime GetValidDateOfBirth()
{
    DateTime dob;
    do
    {
        Console.Write("Enter Date of Birth (dd/MM/yyyy): ");
        if (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dob))
        {
            Console.WriteLine("This is a invalid Date of Birth format. Please enter a valid date (dd/MM/yyyy).");
        }
    } while (DateTime.Compare(DateTime.Now, dob) < 0);
    return dob;
}

//basic feature 4 - Create customer's order (Chew Jin Xuan)
void CreateCustomerOrder(Dictionary<int, Customer> customers, Dictionary<int, Order> orders)
{
    bool InvalidID = false;
    int id;
    Customer customer = null;

    ListAllCustomers(customers);
    Console.WriteLine();
    do
    {
        Console.Write("Enter your customer ID: ");
        if (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("This is a invalid ID format. Please enter a valid integer.");
            InvalidID = true;
        }
        else
        {
            InvalidID = false;
            if (customers.ContainsKey(id))
            {
                customer = customers[id];
            }
            else
            {
                Console.WriteLine("ID entered is invalid! Please try again.");
                InvalidID = true;
            }
        }
    } while (InvalidID);

    Order order = customer.MakeOrder(customers, orders, Toppings, regFlavours, premFlavours, waffleFlavours, Options);
    orders.Add(orders.Count + 1, order);

    if (customer.Rewards.Tier == "Gold")
    {
        goldqueue.Enqueue(order);
    }
    else
    {
        orderqueue.Enqueue(order);
    }

    if (orders.ContainsValue(order))
    {
        Console.WriteLine("Order is made successfully!");
    }
    else
    {
        Console.WriteLine("Order made failed!");
    }
}

//basic feature 5 - Display order details of a customer (Zulhimi)
void DisplayOrderDetailsOfCustomer(Dictionary<int, Customer> customers)
{
    ListAllCustomers(customers);

    Console.Write("Enter your customer ID: ");
    if (!int.TryParse(Console.ReadLine(), out int id) || !customers.TryGetValue(id, out Customer customer))
    {
        Console.WriteLine("This is a invalid ID format or the ID does not exist. Please enter a valid integer.");
        return;
    }

    DisplayOrders(customer.OrderHistory, "Order History");
    DisplayCurrentOrder(customer.CurrentOrder, $"{customer.Name}'s Current Order");
}

void DisplayOrders(List<Order> orders, string title)
{
    Console.WriteLine();
    Console.WriteLine(title);

    if (orders.Count == 0)
    {
        Console.WriteLine("There is no orders found.");
        return;
    }

    foreach (Order order in orders)
    {
        Console.WriteLine($"Time Received: {order.TimeReceived}");

        foreach (IceCream iceCream in order.IceCreamList)
        {
            DisplayIceCreamInfo(iceCream);
        }
        Console.WriteLine();
    }
}

void DisplayCurrentOrder(Order currentOrder, string title)
{
    if (currentOrder != null)
    {
        Console.WriteLine();
        Console.WriteLine(title);
        Console.WriteLine($"Order ID: {currentOrder.Id}");
        Console.WriteLine($"Time Received: {currentOrder.TimeReceived}");

        foreach (IceCream iceCream in currentOrder.IceCreamList)
        {
            DisplayIceCreamInfo(iceCream);
        }
        Console.WriteLine();
    }
    else
    {
        Console.WriteLine("There is no current orders.");
    }
}

void DisplayIceCreamInfo(IceCream iceCream)
{
    Console.WriteLine($"Ice cream option: {iceCream.Option}");
    Console.WriteLine($"Ice cream scoops: {iceCream.Scoops}");
    Console.WriteLine("Ice cream flavours: ");

    foreach (Flavour flav in iceCream.Flavours)
    {
        Console.WriteLine(flav.ToString());
    }

    Console.WriteLine("Ice cream toppings: ");

    if (iceCream.Toppings.Count > 0)
    {
        foreach (Topping topping in iceCream.Toppings)
        {
            Console.WriteLine(topping.ToString());
        }
    }
    else
    {
        Console.WriteLine("There is no toppings");
    }
}

//basic feature 6 - Modify order details (Zulhimi)
void ModifyOrderDetails(Dictionary<int, Customer> customers)
{
    int id;
    Customer customer = null;
    int option;
    int iceCreamNo = 0;
    string iceCreamOption;
    int scoopNum;
    string flavourType;
    bool flavourPremium = false;
    int flavourQuantity = 0;
    int totalFlavourQuantity = 0;
    string toppingType = null;
    string waffleFlavour;
    bool dipped = false;

    ListAllCustomers(customers);
    Console.WriteLine();

    bool isValidCustomer = GetValidCustomer(customers, out id, out customer);

    if (!isValidCustomer)
    {
        Console.WriteLine("There is no current orders.");
        return;
    }

    DisplayCustomerOrder(customer);

    bool isValidOption = GetValidOption(out option);

    if (!isValidOption)
    {
        Console.WriteLine("This Option does not exist.");
        return;
    }

    Console.WriteLine();

    if (option == 1)
    {
        bool isValidIceCreamNo = GetValidIceCreamNumber(out iceCreamNo, customer.CurrentOrder);

        if (!isValidIceCreamNo)
        {
            Console.WriteLine("This is a invalid ice cream number.");
            return;
        }

        customer.CurrentOrder.ModifyIceCream(iceCreamNo, Options, premFlavours, regFlavours, Toppings, waffleFlavours);
    }
    else if (option == 2)
    {
        iceCreamOption = GetValidIceCreamOption();

        scoopNum = GetValidScoopNumber();

        List<Flavour> flavours = GetValidFlavours(scoopNum);

        List<Topping> toppings = GetValidToppings();

        IceCream iceCream = CreateIceCreams(iceCreamOption, scoopNum, flavours, toppings);

        customer.CurrentOrder.AddIceCream(iceCream);
    }
    else if (option == 3)
    {
        bool isValidIceCreamNo = GetValidIceCreamNumber(out iceCreamNo, customer.CurrentOrder);

        if (!isValidIceCreamNo)
        {
            Console.WriteLine("This is a invalid ice cream number.");
            return;
        }

        iceCreamNo -= 1;

        if (customer.CurrentOrder.IceCreamList.Count > 1)
        {
            customer.CurrentOrder.DeleteIceCream(iceCreamNo);
        }
        else
        {
            Console.WriteLine("Cannot have any zero ice creams in an order.");
        }
    }

    DisplayCustomerOrder(customer);
}

bool GetValidCustomer(Dictionary<int, Customer> customers, out int id, out Customer customer)
{
    while (true)
    {
        Console.Write("Enter your customer ID: ");
        if (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("This is a invalid ID format. Please enter a valid integer.");
            continue;
        }

        if (customers.ContainsKey(id))
        {
            customer = customers[id];
            return true;
        }
        else
        {
            Console.WriteLine("The ID entered is invalid! Please try again.");
        }
    }
}

void DisplayCustomerOrder(Customer customer)
{
    Order currentOrder = customer.CurrentOrder;

    foreach (IceCream iceCream in currentOrder.IceCreamList)
    {
        Console.WriteLine($"Ice cream {currentOrder.IceCreamList.IndexOf(iceCream) + 1}");
        Console.WriteLine($"Ice cream option: {iceCream.Option}");
        Console.WriteLine($"Ice cream scoops: {iceCream.Scoops}");
        Console.WriteLine("Ice cream flavours: ");

        foreach (Flavour flav in iceCream.Flavours)
        {
            Console.WriteLine(flav.ToString());
        }

        Console.WriteLine("Ice cream toppings: ");

        if (iceCream.Toppings.Count > 0)
        {
            foreach (Topping topping in iceCream.Toppings)
            {
                Console.WriteLine(topping.ToString());
            }
        }
        else
        {
            Console.WriteLine("There is no toppings");
        }

        Console.WriteLine();
    }
}

bool GetValidOption(out int option)
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("Choose one of the following options: ");
        Console.WriteLine("[1] Choose an existing ice cream object to modify.");
        Console.WriteLine("[2] Add an entirely new ice cream object to the order.");
        Console.WriteLine("[3] Choose an existing ice cream object to delete from the order.");
        Console.Write("Enter option: ");

        if (int.TryParse(Console.ReadLine(), out option))
        {
            if (option >= 1 && option <= 3)
            {
                return true;
            }
        }

        Console.WriteLine("The Option entered is invalid. Please Try again!");
    }
}

bool GetValidIceCreamNumber(out int iceCreamNo, Order currentOrder)
{
    while (true)
    {
        Console.Write("Enter which ice cream to modify: ");

        if (!int.TryParse(Console.ReadLine(), out iceCreamNo))
        {
            Console.WriteLine("The Value that is inputted is not an integer. Please Try again.");
            continue;
        }

        if (iceCreamNo < 1 || iceCreamNo > currentOrder.IceCreamList.Count)
        {
            Console.WriteLine("This is a invalid ice cream number. Please Try again.");
            continue;
        }

        return true;
    }
}

string GetValidIceCreamOption()
{
    while (true)
    {
        Console.WriteLine("Options available: Cup, Cone, Waffle");
        Console.Write("Enter option: ");
        string iceCreamOption = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(iceCreamOption))
        {
            Console.WriteLine("This is a invalid option! Please enter a valid option.");
            continue;
        }

        if (Options.Contains(iceCreamOption))
        {
            return iceCreamOption;
        }
        else
        {
            Console.WriteLine("This Option does not exist in the menu! Please enter a valid option.");
        }
    }
}

int GetValidScoopNumber()
{
    while (true)
    {
        Console.Write("Enter number of scoops (1/2/3): ");
        if (int.TryParse(Console.ReadLine(), out int scoopNum))
        {
            if (scoopNum >= 1 && scoopNum <= 3)
            {
                return scoopNum;
            }
        }
        Console.WriteLine("This is a invalid scoop number! Please enter a valid option.");
    }
}

List<Flavour> GetValidFlavours(int scoopNum)
{
    List<Flavour> flavours = new List<Flavour>();
    int totalFlavourQuantity = 0;

    while (totalFlavourQuantity < scoopNum)
    {
        Console.WriteLine("Flavours available: Vanilla, Chocolate, Strawberry, Durian, Ube, Sea Salt");
        Console.Write("Enter flavour type: ");
        string flavourType = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(flavourType))
        {
            Console.WriteLine("This is a invalid Flavour Type! Please enter a valid Flavour Type.");
            continue;
        }

        bool flavourPremium = premFlavours.Contains(flavourType);
        bool isValidFlavourQuantity = GetValidFlavourQuantity(scoopNum, out int flavourQuantity);

        if (!isValidFlavourQuantity)
        {
            Console.WriteLine("This is a invalid flavour quantity. Please try again.");
            continue;
        }

        totalFlavourQuantity += flavourQuantity;

        if (totalFlavourQuantity > scoopNum)
        {
            Console.WriteLine("You've exceeded the scoop number. Please try again.");
            totalFlavourQuantity -= flavourQuantity;
            continue;
        }

        Flavour flavour = new Flavour(flavourType, flavourPremium, flavourQuantity);
        flavours.Add(flavour);
    }

    return flavours;
}

bool GetValidFlavourQuantity(int scoopNum, out int flavourQuantity)
{
    while (true)
    {
        Console.Write("Enter flavour quantity: ");
        if (int.TryParse(Console.ReadLine(), out flavourQuantity))
        {
            if (flavourQuantity >= 1 && flavourQuantity <= scoopNum)
            {
                return true;
            }
        }
        Console.WriteLine("This is a invalid input! Please enter valid Flavour Quantity.");
    }
}

List<Topping> GetValidToppings()
{
    List<Topping> toppings = new List<Topping>();
    string toppingType = "";

    while (toppingType != "nil" && toppings.Count < 4)
    {
        Console.WriteLine("Toppings available: Sprinkles, Mochi, Sago, Oreos");
        Console.Write("Enter topping (or NIL to stop adding): ");
        toppingType = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(toppingType))
        {
            Console.WriteLine("This is a invalid Topping Type! Please enter a valid Topping Type.");
            continue;
        }

        if (Toppings.Contains(toppingType) || toppingType == "nil")
        {
            if (toppingType != "nil")
            {
                Topping topping = new Topping(toppingType);
                toppings.Add(topping);
            }
        }
        else
        {
            Console.WriteLine("The Topping entered not in menu. Please Try again!");
        }
    }

    return toppings;
}

IceCream CreateIceCreams(string iceCreamOption, int scoopNum, List<Flavour> flavours, List<Topping> toppings)
{
    IceCream iceCream = null;

    switch (iceCreamOption)
    {
        case "Waffle":
            string waffleFlavour = GetValidWaffleFlavour();

            iceCream = new Waffle("Waffle", scoopNum, flavours, toppings, waffleFlavour);
            break;
        case "Cone":
            bool dipped = GetValidConeDipped();

            iceCream = new Cone("Cone", scoopNum, flavours, toppings, dipped);
            break;
        case "Cup":
            iceCream = new Cup("Cup", scoopNum, flavours, toppings);
            break;
    }

    return iceCream;
}

string GetValidWaffleFlavour()
{
    while (true)
    {
        Console.WriteLine("Waffle Flavours available: Red Velvet, Charcoal, Pandan");
        Console.Write("Enter the waffle flavour: ");
        string waffleFlavour = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(waffleFlavour))
        {
            Console.WriteLine("The Waffle Flavour entered is invalid. Please Try again!");
            continue;
        }

        if (waffleFlavours.Contains(waffleFlavour))
        {
            return waffleFlavour;
        }
        else
        {
            Console.WriteLine("The Waffle Flavour entered is not on the menu. Please Try again!");
        }
    }
}

bool GetValidConeDipped()
{
    while (true)
    {
        Console.Write("Is the cone dipped? True/False: ");
        if (bool.TryParse(Console.ReadLine(), out bool dipped))
        {
            return dipped;
        }
        Console.WriteLine("You have Entered a invalid input. Try again.");
    }
}

//advanced features - a - Process an order and checkout (Chew Jin Xuan)
void ProcessOrderAndCheckout(Queue<Order> queue, Dictionary<int, Customer> customers)
{
    if (queue.Count == 0)
    {
        Console.WriteLine("There is no current orders in the queue.");
        return;
    }

    Order order = queue.Dequeue();
    DisplayOrderDetail(order);

    double billAmount = CalculateTotalBill(order);

    foreach (Customer customer in customers.Values)
    {
        if (customer.CurrentOrder == order)
        {
            DisplayCustomerRewardsInfo(customer);
            ApplyBirthdayDiscount(customer, order, ref billAmount);
            ApplyPunchCardDiscount(customer, order, ref billAmount);
            ApplyPointsRedemption(customer, ref billAmount);

            Console.WriteLine($"Final Bill Amount: ${billAmount.ToString("0.00")}");
            Console.Write("press P to make the payment: ");
            var payment = Console.ReadLine();

            UpdateCustomerRewards(customer, order, billAmount);
            order.TimeFulfilled = DateTime.Now;
            customer.CurrentOrder = null;
            customer.OrderHistory.Add(order);
        }
    }
}

void DisplayOrderDetail(Order order)
{
    foreach (IceCream iceCream in order.IceCreamList)
    {
        Console.WriteLine($"Ice cream {order.IceCreamList.IndexOf(iceCream) + 1}");
        Console.WriteLine($"Ice cream option: {iceCream.Option}");
        Console.WriteLine($"Ice cream scoops: {iceCream.Scoops}");
        Console.WriteLine("Ice cream flavours: ");

        foreach (Flavour flav in iceCream.Flavours)
        {
            Console.WriteLine(flav.ToString());
        }

        Console.WriteLine("Ice cream toppings: ");

        if (iceCream.Toppings.Count > 0)
        {
            foreach (Topping topping in iceCream.Toppings)
            {
                Console.WriteLine(topping.ToString());
            }
        }
        else
        {
            Console.WriteLine("There is No toppings");
        }

        Console.WriteLine();
    }
}

double CalculateTotalBill(Order order)
{
    double billAmount = order.CalculateTotal();
    Console.WriteLine($"Total Bill Amount: ${billAmount.ToString("0.00")}");
    return billAmount;
}

void DisplayCustomerRewardsInfo(Customer customer)
{
    Console.WriteLine($"Membership status: {customer.Rewards.Tier}");
    Console.WriteLine($"Points: {customer.Rewards.Points}");
}

void ApplyBirthdayDiscount(Customer customer, Order order, ref double billAmount)
{
    if (customer.IsBirthday())
    {
        if (order.IceCreamList.Count > 1)
        {
            double mostExpensiveIceCreamPrice = order.IceCreamList.Max(ic => ic.CalculatePrice());
            billAmount -= mostExpensiveIceCreamPrice;
        }
        else
        {
            billAmount = 0;
        }
    }
}

void ApplyPunchCardDiscount(Customer customer, Order order, ref double billAmount)
{
    if (customer.Rewards.PunchCard == 10)
    {
        billAmount -= order.IceCreamList[0].CalculatePrice();
        customer.Rewards.PunchCard = 0;
    }
}

void ApplyPointsRedemption(Customer customer, ref double billAmount)
{
    if (customer.Rewards.Tier == "Gold" || customer.Rewards.Tier == "Silver")
    {
        if (customer.Rewards.Points > 0)
        {
            bool isValidRedeemPoints = false;
            int redeemPoints = 0;

            do
            {
                Console.Write("Enter number of points to redeem: ");

                if (int.TryParse(Console.ReadLine(), out redeemPoints))
                {
                    if (redeemPoints <= customer.Rewards.Points && redeemPoints >= 0)
                    {
                        isValidRedeemPoints = true;
                    }
                    else
                    {
                        Console.WriteLine("It is a invalid points. Points that is redeemed cannot exceed points the customer has or be negative.");
                    }
                }
                else
                {
                    Console.WriteLine("Please Enter a valid integer.");
                }
            } while (!isValidRedeemPoints);

            customer.Rewards.RedeemPoints(redeemPoints);
            billAmount -= redeemPoints * 0.02;
        }
    }
}

void UpdateCustomerRewards(Customer customer, Order order, double billAmount)
{
    foreach (IceCream iceCream in order.IceCreamList)
    {
        customer.Rewards.Punch();
    }

    int earnedPoints = Convert.ToInt32(Math.Floor(billAmount * 0.72));
    customer.Rewards.AddPoints(earnedPoints);

    if (customer.Rewards.Points >= 100)
    {
        customer.Rewards.Tier = "Gold";
    }
    else if (customer.Rewards.Points >= 50 && customer.Rewards.Tier != "Gold")
    {
        customer.Rewards.Tier = "Silver";
    }
}

//advanced features - b - display monthly charged amounts breakdown & total charged amounts for the year (Zulhimi)
void DisplayChargedAmounts(Dictionary<int, Order> orders)
{
    int year = GetValidYear();

    Dictionary<int, double> monthlyTotals = CalculateMonthlyTotals(orders, year);
    double overallTotal = CalculateOverallTotal(monthlyTotals);

    PrintMonthlyTotals(monthlyTotals, year);
    Console.WriteLine($"Total: ${overallTotal.ToString("0.00")}");
}

int GetValidYear()
{
    bool invalidYear = false;
    int year = 0;
    int currentYear = DateTime.Now.Year;

    do
    {
        Console.Write("Enter the year: ");
        if (!int.TryParse(Console.ReadLine(), out year))
        {
            Console.WriteLine("Please Enter a valid integer.");
            invalidYear = true;
        }
        else if (year > currentYear)
        {
            Console.WriteLine("The date cannot be in the future. Please try again.");
            invalidYear = true;
        }
        else if (year < 2023)
        {
            Console.WriteLine("There is no orders before this date. Please Try Again.");
            invalidYear = true;
        }
        else
        {
            invalidYear = false;
        }
    } while (invalidYear);

    return year;
}

Dictionary<int, double> CalculateMonthlyTotals(Dictionary<int, Order> orders, int year)
{
    Dictionary<int, double> monthlyTotals = new Dictionary<int, double>();

    foreach (Order order in orders.Values)
    {
        DateTime orderDateTime = (DateTime)order.TimeFulfilled;
        if (orderDateTime.Year == year)
        {
            int month = orderDateTime.Month;
            double total = order.CalculateTotal();

            if (monthlyTotals.ContainsKey(month))
            {
                monthlyTotals[month] += total;
            }
            else
            {
                monthlyTotals[month] = total;
            }
        }
    }

    return monthlyTotals;
}

double CalculateOverallTotal(Dictionary<int, double> monthlyTotals)
{
    double overallTotal = 0;

    foreach (double total in monthlyTotals.Values)
    {
        overallTotal += total;
    }

    return overallTotal;
}

void PrintMonthlyTotals(Dictionary<int, double> monthlyTotals, int year)
{
    Console.WriteLine();

    for (int month = 1; month <= 12; month++)
    {
        string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
        double total = monthlyTotals.ContainsKey(month) ? monthlyTotals[month] : 0.0;

        Console.WriteLine($"{monthName} {year}: ${total.ToString("0.00")}");
    }

    Console.WriteLine();
}
