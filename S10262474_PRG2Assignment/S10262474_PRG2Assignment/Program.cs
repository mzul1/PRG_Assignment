using S10262474_PRG2Assignment;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

/*string[] Toppings = { "Sprinkles", "Mochi", "Sago", "Oreos" };
string[] regFlavours = { "Vanilla", "Chocolate", "Strawberry" };
string[] premFlavours = { "Durian", "Ube", "Sea Salt" };
string[] waffleFlavours = { "Red Velvet", "Charcoal", "Pandan" };
string[] Options = { "Cup", "Cone", "Waffle" };

Dictionary<int, Customer> customers = new Dictionary<int, Customer>();
Dictionary<int, Order> orders = new Dictionary<int, Order>();
Dictionary<int, int> orderIDCustomerID = new Dictionary<int, int>();

Queue<Order> orderQueue = new Queue<Order>();
Queue<Order> goldQueue = new Queue<Order>();

LoadCustomers("customers.csv");
LoadOrders("orders.csv");
AssignOrdersToCustomers();

void LoadCustomers(string filePath)
{
    string[] CustomerCsvLines = File.ReadAllLines(filePath);
    for (int i = 1; i < CustomerCsvLines.Length; i++)
    {
        string[] line = CustomerCsvLines[i].Split(",");
        int memberId;
        DateTime dob;
        int points;
        int punches;
        if (!int.TryParse(line[1], out memberId))
        {
            //Console.WriteLine($"Invalid member ID format in line {i + 1}.");
            continue; // Skip this line and go to the next one
        }

        if (!DateTime.TryParse(line[2], out dob))
        {
            //Console.WriteLine($"Invalid date format in line {i + 1}.");
            continue; // Skip this line and go to the next one
        }

        if (!int.TryParse(line[4], out points) || !int.TryParse(line[5], out punches))
        {
            //Console.WriteLine($"Invalid point card format in line {i + 1}.");
            continue; // Skip this line and go to the next one
        }
        Customer customer = new Customer(line[0], Convert.ToInt32(line[1]), DateTime.Parse(line[2]));
        PointCard pointCard = new PointCard(Convert.ToInt32(line[4]), Convert.ToInt32(line[5])) { Tier = line[3] };
        customer.Rewards = pointCard;
        customers.Add(customer.MemberId, customer);
    }
}

void LoadOrders(string filePath)
{
    string[] ordersFile = File.ReadAllLines(filePath);
    for (int i = 1; i < ordersFile.Length; i++)
    {
        ProcessOrderLine(ordersFile[i].Split(","));
    }
}

void ProcessOrderLine(string[] line)
{
    int orderID = Convert.ToInt32(line[0]);
    Order order = GetOrCreateOrder(orderID, DateTime.Parse(line[2]), DateTime.Parse(line[3]));
    IceCream iceCream = CreateIceCreamFromLine(line);
    order.AddIceCream(iceCream);

    // Check if the key exists before adding to avoid ArgumentException
    if (!orderIDCustomerID.ContainsKey(orderID))
    {
        orderIDCustomerID.Add(orderID, Convert.ToInt32(line[1]));
    }
    else
    {
        // Handle the case where the key exists, such as updating the customer ID, logging, etc.
        // For example:
        // orderIDCustomerID[orderID] = Convert.ToInt32(line[1]);
    }
}

Order GetOrCreateOrder(int orderID, DateTime orderDate, DateTime fulfilledDate)
{
    if (!orders.ContainsKey(orderID))
    {
        Order newOrder = new Order(orderID, orderDate) { TimeFulfilled = fulfilledDate };
        orders.Add(orderID, newOrder);
        return newOrder;
    }
    return orders[orderID];
}

IceCream CreateIceCreamFromLine(string[] line)
{
    string TypeofIceCream = line[4];
    int ScoopsNum = Convert.ToInt32(line[5]);
    List<Flavour> flavours = GetFlavoursFromLine(line, ScoopsNum);
    List<Topping> toppings = GetToppingsFromLine(line);

    switch (TypeofIceCream)
    {
        case "Waffle":
            return new Waffle("Waffle", ScoopsNum, flavours, toppings, line[7]);
        case "Cone":
            return new Cone("Cone", ScoopsNum, flavours, toppings, line[6] == "TRUE");
        case "Cup":
            return new Cup("Cup", ScoopsNum, flavours, toppings);
        default:
            throw new ArgumentException("Invalid ice cream type");
    }
}

List<Flavour> GetFlavoursFromLine(string[] line, int ScoopsNum)
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

List<Topping> GetToppingsFromLine(string[] line)
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

void AssignOrdersToCustomers()
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
    Console.Write("Enter your choice: ");

    if (!int.TryParse(Console.ReadLine(), out option))
    {
        Console.WriteLine("Invalid input! Please enter a valid input.");
        continue;
    }

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
            //CreateCustomerOrder(customers, orders);
            break;
        case 5:
            //DisplayOrderDetailsOfCustomer(customers);
            break;
        case 6:
            //ModifyOrderDetails(customers);
            break;
        case 7:
            //Queue<Order> activeQueue = goldqueue.Count > 0 ? goldqueue : orderqueue;
            //ProcessOrderAndCheckout(activeQueue, customers);
            break;
        case 8:
            //DisplayChargedAmounts(orders);
            break;
        case 0:
            Console.WriteLine("Exiting...");
            break;
        default:
            Console.WriteLine("Invalid input! Please enter a valid input.");
            break;
    }
    Console.WriteLine();

} while (option != 0);

void ListAllCustomers(Dictionary<int, Customer> customers)
{
    string header = String.Format("{0, -10} {1, -10} {2, -15} {3, -10} {4, -10} {5, -10} {6, 10} {7, 20}",
        "Name", "MemberID", "DateOfBirth", "Points",
        "PunchCard", "Tier", "CurrentOrderID", "OrderHistoryID");
    Console.WriteLine(header);

    StringBuilder customerDetails = new StringBuilder();
    foreach (Customer customer in customers.Values)
    {
        string currentOrderID = customer.CurrentOrder != null ? customer.CurrentOrder.Id.ToString() + 1 : "No Current Order";
        string orderHistoryIDs = string.Join(", ", customer.OrderHistory.Select(order => order.Id));

        customerDetails.AppendFormat("{0, -10} {1, -10} {2, -15:dd/MM/yyyy} {3, -10} {4, -10} {5, -10} {6, -10} {7, 8}\n",
            customer.Name, customer.MemberId, customer.Dob,
            customer.Rewards.Points,
            customer.Rewards.PunchCard, customer.Rewards.Tier, currentOrderID, orderHistoryIDs);
    }

    Console.WriteLine(customerDetails.ToString());
}

void ListAllOrders(Dictionary<int, Customer> customers)
{
    // Local function to print order details, to avoid code duplication.
    void PrintOrderDetails(Queue<Order> queue, string queueName)
    {
        Console.WriteLine();
        if (queue.Count > 0)
        {
            Console.WriteLine($"Orders In {queueName} Queue: ");
            foreach (Order order in queue)
            {
                Console.WriteLine();
                Console.WriteLine($"Order ID: {order.Id}");
                Console.WriteLine($"Time Received: {order.TimeReceived}");
                foreach (IceCream iceCream in order.IceCreamList)
                {
                    Console.WriteLine($"Ice cream option: {iceCream.Option}");
                    Console.WriteLine($"Ice cream scoops: {iceCream.Scoops}");
                    Console.WriteLine("Ice cream flavours: ");
                    foreach (Flavour flavour in iceCream.Flavours)
                    {
                        Console.WriteLine(flavour.ToString());
                    }
                    Console.WriteLine("Ice cream toppings: ");
                    foreach (Topping topping in iceCream.Toppings)
                    {
                        Console.WriteLine(topping.ToString());
                    }
                }
                Console.WriteLine();
            }
        }
        else
        {
            Console.WriteLine($"No orders in {queueName.ToLower()} queue.");
        }
        Console.WriteLine();
    }

    // Use the local function to print details for both queues.
    PrintOrderDetails(goldQueue, "Gold");
    PrintOrderDetails(orderQueue, "Regular");
}

void NewCustomer(Dictionary<int, Customer> customers)
{
    string name;
    int id;
    DateTime dob;

    // Input and validation for name
    while (true)
    {
        Console.Write("Enter Name: ");
        name = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(name))
        {
            break;
        }
        Console.WriteLine("Invalid name. Please enter a valid name.");
    }

    // Input and validation for ID
    while (true)
    {
        Console.Write("Enter ID Number (XXXXXX): ");
        if (int.TryParse(Console.ReadLine(), out id) && id >= 0 && id <= 999999 && !customers.ContainsKey(id))
        {
            break;
        }
        Console.WriteLine("Invalid ID. Please enter a valid integer within range and not already used.");
    }

    // Input and validation for date of birth
    while (true)
    {
        Console.Write("Enter Date of Birth (dd/MM/yyyy): ");
        if (DateTime.TryParse(Console.ReadLine(), out dob) && DateTime.Compare(DateTime.Now, dob) >= 0)
        {
            break;
        }
        Console.WriteLine("Invalid Date of Birth. Please enter a valid date in the past.");
    }

    // Adding the customer
    AddCustomer(customers, name, id, dob);

    // Confirmation of customer addition
    if (customers.ContainsKey(id))
    {
        Console.WriteLine("\nCustomer registered successfully!\n");
    }
    else
    {
        Console.WriteLine("\nCustomer registration failed!\n");
    }
}

// Encapsulate customer creation and addition in a separate function
void AddCustomer(Dictionary<int, Customer> customers, string name, int id, DateTime dob)
{
    Customer customer = new Customer(name, id, dob);
    PointCard pointCard = new PointCard { Tier = "Ordinary" };
    customer.Rewards = pointCard;
    customers.Add(id, customer);

    string data = $"{name},{id},{dob:dd/MM/yyyy}, {customer.Rewards.Tier}, {customer.Rewards.Points}, {customer.Rewards.PunchCard}";

    File.AppendAllText("customers.csv", data + Environment.NewLine);
}*/

string[] Toppings = { "Sprinkles", "Mochi", "Sago", "Oreos" };
string[] regFlavours = { "Vanilla", "Chocolate", "Strawberry" };
string[] premFlavours = { "Durian", "Ube", "Sea Salt" };
string[] waffleFlavours = { "Red Velvet", "Charcoal", "Pandan" };
string[] Options = { "Cup", "Cone", "Waffle" };

Dictionary<int, Customer> customers = new Dictionary<int, Customer>();
Dictionary<int, Order> orders = new Dictionary<int, Order>();
Dictionary<int, int> orderIDCustomerID = new Dictionary<int, int>();

Queue<Order> orderqueue = new Queue<Order>();
Queue<Order> goldqueue = new Queue<Order>();

LoadCustomers("customers.csv");
LoadOrders("orders.csv");
AssignOrdersToCustomers();

void LoadCustomers(string filePath)
{
    string[] CustomerCsvLines = File.ReadAllLines(filePath);
    for (int i = 1; i < CustomerCsvLines.Length; i++)
    {
        string[] line = CustomerCsvLines[i].Split(',');
        Customer customer = new Customer(line[0], Convert.ToInt32(line[1]), DateTime.Parse(line[2]));
        PointCard pointCard = new PointCard(Convert.ToInt32(line[4]), Convert.ToInt32(line[5])) { Tier = line[3] };
        customer.Rewards = pointCard;
        customers.Add(customer.MemberId, customer);
    }
}

void LoadOrders(string filePath)
{
    string[] ordersFile = File.ReadAllLines(filePath);
    for (int i = 1; i < ordersFile.Length; i++)
    {
        ProcessOrderLine(ordersFile[i].Split(','));
    }
}

void ProcessOrderLine(string[] line)
{
    int orderID = Convert.ToInt32(line[0]);
    if (!orders.TryGetValue(orderID, out Order order))
    {
        order = new Order(orderID, DateTime.Parse(line[2])) { TimeFulfilled = DateTime.Parse(line[3]) };
        orders.Add(orderID, order);
        orderIDCustomerID.Add(orderID, Convert.ToInt32(line[1]));
    }
    AddIceCreamToOrder(order, line);
}

void AddIceCreamToOrder(Order order, string[] line)
{
    int ScoopsNum = Convert.ToInt32(line[5]);
    List<Flavour> flavours = GetFlavoursFromLine(line, ScoopsNum);
    List<Topping> toppings = GetToppingsFromLine(line);

    IceCream iceCream = CreateIceCream(line, ScoopsNum, flavours, toppings);
    order.AddIceCream(iceCream);
}

List<Flavour> GetFlavoursFromLine(string[] line, int scoopsNum)
{
    List<Flavour> flavours = new List<Flavour>();
    for (int x = 8; x < 8 + scoopsNum; x++)
    {
        string flavourType = line[x];
        Flavour existingFlavour = flavours.Find(f => f.Type == flavourType);
        if (existingFlavour == null)
        {
            bool isPremium = premFlavours.Contains(flavourType);
            flavours.Add(new Flavour(flavourType, isPremium, 1));
        }
        else
        {
            existingFlavour.Quantity++;
        }
    }
    return flavours;
}

List<Topping> GetToppingsFromLine(string[] line)
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

IceCream CreateIceCream(string[] line, int scoopsNum, List<Flavour> flavours, List<Topping> toppings)
{
    string TypeofIceCream = line[4];
    switch (TypeofIceCream)
    {
        case "Waffle":
            return new Waffle("Waffle", scoopsNum, flavours, toppings, line[7]);
        case "Cone":
            return new Cone("Cone", scoopsNum, flavours, toppings, line[6] == "TRUE");
        case "Cup":
            return new Cup("Cup", scoopsNum, flavours, toppings);
        default:
            throw new ArgumentException("Invalid type of ice cream");
    }
}

void AssignOrdersToCustomers()
{
    foreach (var pair in orderIDCustomerID)
    {
        customers[pair.Value].OrderHistory.Add(orders[pair.Key]);
    }
}

int DisplayMenu()
{
    int option;
    bool invalidOption;

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
        invalidOption = !int.TryParse(Console.ReadLine(), out option);
        if (invalidOption)
        {
            Console.WriteLine("Invalid input! Please try again.");
        }
    } while (invalidOption);

    return option;
}

int option = DisplayMenu();
while (option != 0)
{
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
            Queue<Order> queueToProcess = goldqueue.Count > 0 ? goldqueue : orderqueue;
            ProcessOrderAndCheckout(queueToProcess, customers);
            break;
        case 8:
            DisplayChargedAmounts(orders);
            break;
        default:
            Console.WriteLine("Invalid input! Please enter a valid input.");
            break;
    }
    Console.WriteLine();
    option = DisplayMenu();
}

void ListAllCustomers(Dictionary<int, Customer> customers)
{
    bool isFirstIteration = true;

    foreach (Customer customer in customers.Values)
    {
        if (isFirstIteration)
        {
            Console.WriteLine("{0, -10} {1, -10} {2, -15} {3, -10} {4, -10} {5, -10} {6, 10} {7, 20}",
                "Name", "MemberID", "DateOfBirth", "Points",
                "PunchCard", "Tier", "CurrentOrderID", "OrderHistoryID");
            isFirstIteration = false;
        }

        string currentOrderId = customer.CurrentOrder != null ? (customer.CurrentOrder.Id + 1).ToString() : "No Current Order";
        string orderHistoryIds = string.Join(", ", customer.OrderHistory.Select(order => order.Id.ToString()));

        Console.WriteLine("{0, -10} {1, -10} {2, -15:dd/MM/yyyy} {3, -10} {4, -10} {5, -10} {6, -10} {7, 8}",
            customer.Name, customer.MemberId, customer.Dob,
            customer.Rewards.Points, customer.Rewards.PunchCard,
            customer.Rewards.Tier, currentOrderId, orderHistoryIds);
    }

    if (customers.Count == 0)
    {
        Console.WriteLine("No customers found.");
    }

    Console.WriteLine();
}

void ListAllOrders(Dictionary<int, Customer> customers)
{
    Console.WriteLine();

    // Handle both queues in a single, unified structure
    PrintQueueDetails(goldqueue, "Gold");
    PrintQueueDetails(orderqueue, "Regular");

    Console.WriteLine();
}

void PrintQueueDetails(Queue<Order> queue, string queueName)
{
    if (queue.Count == 0)
    {
        Console.WriteLine($"No orders in {queueName.ToLower()} queue.");
        return;
    }

    Console.WriteLine($"Orders In {queueName} Queue: ");
    foreach (Order order in queue)
    {
        Console.WriteLine($"\nOrder ID: {order.Id}\nTime Received: {order.TimeReceived}");
        foreach (IceCream iceCream in order.IceCreamList)
        {
            Console.WriteLine($"Ice cream option: {iceCream.Option}\nIce cream scoops: {iceCream.Scoops}\nIce cream flavours: ");
            Console.WriteLine(iceCream.Flavours.Count > 0 ? string.Join("\n", iceCream.Flavours.Select(flav => flav.ToString())) : "No flavours");
            Console.WriteLine("Ice cream toppings: ");
            Console.WriteLine(iceCream.Toppings.Count > 0 ? string.Join("\n", iceCream.Toppings.Select(topping => topping.ToString())) : "No toppings");
            Console.WriteLine();
        }
    }
}

void NewCustomer(Dictionary<int, Customer> customers)
{
    string name = "";
    int id = 0;
    DateTime dob = DateTime.MinValue;

    // Input and validate name
    while (true)
    {
        Console.Write("Enter Name: ");
        name = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(name))
        {
            break;
        }
        Console.WriteLine("Invalid name. Please enter a valid name.");
    }

    // Input and validate ID
    while (true)
    {
        Console.Write("Enter ID Number (XXXXXX): ");
        if (int.TryParse(Console.ReadLine(), out id) && id >= 0 && id <= 999999 && !customers.ContainsKey(id))
        {
            break;
        }
        Console.WriteLine("Invalid ID. It must be a unique integer between 0 and 999999.");
    }

    // Input and validate date of birth
    while (true)
    {
        Console.Write("Enter Date of Birth (dd/MM/yyyy): ");
        if (DateTime.TryParse(Console.ReadLine(), out dob) && dob <= DateTime.Now)
        {
            break;
        }
        Console.WriteLine("Invalid Date of Birth. Enter a valid past date in format dd/MM/yyyy.");
    }

    // Create customer and add to dictionary
    Customer customer = new Customer(name, id, dob);
    PointCard pointCard = new PointCard { Tier = "Ordinary" };
    customer.Rewards = pointCard;
    customers[id] = customer;

    // Save customer data to file
    string data = $"{name},{id},{dob:dd/MM/yyyy}, {customer.Rewards.Tier}, {customer.Rewards.Points}, {customer.Rewards.PunchCard}";
    try
    {
        using (StreamWriter sw = new StreamWriter("customers.csv", true))
        {
            sw.WriteLine(data);
        }
        Console.WriteLine("\nCustomer registered successfully!\n");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\nFailed to save customer data: {ex.Message}\n");
    }
}

void CreateCustomerOrder(Dictionary<int, Customer> customers, Dictionary<int, Order> orders)
{
    bool InvalidID = false, InvalidOption = false, InvalidScoopNum = false, InvalidFlavourType = false, InvalidFlavourQuantity = false, InvalidToppingType = false, InvalidWaffleFlavour = false, InvalidDipped = false, InvalidAddIceCream = false;
    int id, scoopnum;
    string option, flavourtype, waffleflavour, addicecream;
    bool flavourpremium = false;
    int flavourquantity = 0;
    int totalflavourquantity = 0;
    string toppingtype = null;
    bool dipped = false;
    Customer customer = null;

    Order order = new Order();
    List<Flavour> flavours = new List<Flavour>();
    List<Topping> toppings = new List<Topping>();
    totalflavourquantity = 0;
    flavours = new List<Flavour>();
    totalflavourquantity = 0;
    toppings = new List<Topping>();
    toppingtype = null;

    ListAllCustomers(customers);
    Console.WriteLine();
    do
    {
        Console.Write("Enter your customer ID: ");
        if (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Invalid ID format. Please enter a valid integer.");
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


    do
    {
        Console.WriteLine("Options available: ");
        Console.WriteLine("Cup");
        Console.WriteLine("Cone");
        Console.WriteLine("Waffle");
        Console.Write("Enter option: ");
        option = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(option))
        {
            Console.WriteLine("Invalid option! Please enter a valid option.");
            InvalidOption = true;
        }
        else
        {
            InvalidOption = false;
            if (Options.Contains(option))
            {
                continue;
            }
            else
            {
                Console.WriteLine("Option does not exist in menu! Please enter a valid option.");
                InvalidOption = true;
            }
        }
    } while (InvalidOption);

    do
    {
        Console.Write("Enter number of scoops(1/2/3): ");
        if (!int.TryParse(Console.ReadLine(), out scoopnum))
        {
            Console.WriteLine("Invalid option! Please enter a valid option.");
            InvalidScoopNum = true;
        }
        else
        {
            InvalidScoopNum = false;
            if (scoopnum > 3 || scoopnum < 1)
            {
                Console.WriteLine("Invalid scoop number! Please enter a valid option.");
                InvalidScoopNum = true;
            }
            else continue;
        }
    } while (InvalidScoopNum);


    while (totalflavourquantity < scoopnum)
    {
        do
        {
            Console.WriteLine("Flavours available: ");
            Console.WriteLine("Vanilla");
            Console.WriteLine("Chocolate");
            Console.WriteLine("Strawberry");
            Console.WriteLine("Durian");
            Console.WriteLine("Ube");
            Console.WriteLine("Sea Salt");
            Console.Write("Enter flavour type: ");
            flavourtype = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(flavourtype))
            {
                Console.WriteLine("Invalid Flavour Type! Please enter a valid Flavour Type.");
                InvalidFlavourType = true;
            }
            else
            {
                InvalidFlavourType = false;
                if (premFlavours.Contains(flavourtype))
                {
                    flavourpremium = true;
                }
                else if (regFlavours.Contains(flavourtype))
                {
                    flavourpremium = false;
                }
                else
                {
                    Console.WriteLine("Flavour entered not in menu! Please enter a different flavour.");
                    InvalidFlavourType = true;
                }
            }
        } while (InvalidFlavourType);

        do
        {
            Console.Write("Enter flavour quantity: ");
            if (!int.TryParse(Console.ReadLine(), out flavourquantity))
            {
                Console.WriteLine("Invalid input! Please enter valid Flavour Quantity.");
                InvalidFlavourQuantity = true;
            }
            else
            {
                InvalidFlavourQuantity = false;
                if (flavourquantity > scoopnum)
                {
                    Console.WriteLine("Entered quantity more than scoop number. Please try again.");
                    InvalidFlavourQuantity = true;
                }
                totalflavourquantity += flavourquantity;

                if (totalflavourquantity > scoopnum)
                {
                    Console.WriteLine("You've exceeded the scoop number. Please try again.");
                    InvalidFlavourQuantity = true;
                    totalflavourquantity -= flavourquantity;
                }
            }
        } while (InvalidFlavourQuantity);

        Flavour flavour = new Flavour(flavourtype, flavourpremium, flavourquantity);
        flavours.Add(flavour);
    }
    while (toppingtype != "nil")
    {
        do
        {
            Console.WriteLine("Toppings available: ");
            Console.WriteLine("Sprinkles");
            Console.WriteLine("Mochi");
            Console.WriteLine("Sago");
            Console.WriteLine("Oreos");
            Console.Write("Enter topping (or nil to stop adding): ");
            toppingtype = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(toppingtype))
            {
                Console.WriteLine("Invalid Topping Type! Please enter a valid Topping Type.");
                InvalidToppingType = true;
            }
            else
            {
                InvalidToppingType = false;
                if (Toppings.Contains(toppingtype))
                {
                    continue;
                }
                else if (toppingtype == "nil")
                {
                    continue;
                }
                else
                {
                    Console.WriteLine("Topping entered not in menu. Try again!");
                    InvalidToppingType = true;
                }
            }
        } while (InvalidToppingType);
        if (toppingtype != "nil")
        {
            Topping topping = new Topping(toppingtype);
            toppings.Add(topping);
        }
    }

    IceCream iceCream = null;
    switch (option)
    {
        case "Waffle":
            do
            {
                Console.WriteLine("Waffle Flavours available: ");
                Console.WriteLine("Red Velvet");
                Console.WriteLine("Charcoal");
                Console.WriteLine("Pandan");
                Console.Write("Enter waffle flavour: ");
                waffleflavour = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(waffleflavour))
                {
                    Console.WriteLine("Waffle Flavour entered invalid. Try again!");
                    InvalidWaffleFlavour = true;
                }
                else
                {
                    InvalidWaffleFlavour = false;
                    if (waffleFlavours.Contains(waffleflavour))
                    {
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Waffle Flavour entered is not on the menu. Try again!");
                        InvalidWaffleFlavour = true;
                    }
                }
            } while (InvalidWaffleFlavour);

            iceCream = new Waffle("Waffle", scoopnum, flavours, toppings, waffleflavour);
            break;
        case "Cone":
            do
            {
                Console.Write("Is cone dipped? (True/False): ");
                if (!bool.TryParse(Console.ReadLine(), out dipped))
                {
                    Console.WriteLine("Entered invalid input. Try again.");
                    InvalidDipped = true;
                }
                else
                {
                    InvalidDipped = false;
                }
            } while (InvalidDipped);
            iceCream = new Cone("Cone", scoopnum, flavours, toppings, dipped);
            break;
        case "Cup":
            iceCream = new Cup("Cup", scoopnum, flavours, toppings);
            break;
    }
    order.AddIceCream(iceCream);

    Console.Write("Add Another Ice Cream? [Y/N]: ");
    addicecream = Console.ReadLine();
    while (addicecream == "Y")
    {
        do
        {
            Console.WriteLine("Options available: ");
            Console.WriteLine("Cup");
            Console.WriteLine("Cone");
            Console.WriteLine("Waffle");
            Console.Write("Enter option: ");
            option = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(option))
            {
                Console.WriteLine("Invalid option! Please enter a valid option.");
                InvalidOption = true;
            }
            else
            {
                InvalidOption = false;
                if (Options.Contains(option))
                {
                    continue;
                }
                else
                {
                    Console.WriteLine("Option does not exist in menu! Please enter a valid option.");
                    InvalidOption = true;
                }
            }
        } while (InvalidOption);

        do
        {
            Console.Write("Enter number of scoops(1/2/3): ");
            if (!int.TryParse(Console.ReadLine(), out scoopnum))
            {
                Console.WriteLine("Invalid option! Please enter a valid option.");
                InvalidScoopNum = true;
            }
            else
            {
                InvalidScoopNum = false;
                if (scoopnum > 3 || scoopnum < 1)
                {
                    Console.WriteLine("Invalid scoop number! Please enter a valid option.");
                    InvalidScoopNum = true;
                }
                else continue;
            }
        } while (InvalidScoopNum);

        while (totalflavourquantity < scoopnum)
        {
            do
            {
                Console.WriteLine("Flavours available: ");
                Console.WriteLine("Vanilla");
                Console.WriteLine("Chocolate");
                Console.WriteLine("Strawberry");
                Console.WriteLine("Durian");
                Console.WriteLine("Ube");
                Console.WriteLine("Sea Salt");
                Console.Write("Enter flavour type: ");
                flavourtype = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(flavourtype))
                {
                    Console.WriteLine("Invalid Flavour Type! Please enter a valid Flavour Type.");
                    InvalidFlavourType = true;
                }
                else
                {
                    InvalidFlavourType = false;
                    if (premFlavours.Contains(flavourtype))
                    {
                        flavourpremium = true;
                    }
                    else if (regFlavours.Contains(flavourtype))
                    {
                        flavourpremium = false;
                    }
                    else
                    {
                        Console.WriteLine("Flavour entered not in menu! Please enter a different flavour.");
                        InvalidFlavourType = true;
                    }
                }
            } while (InvalidFlavourType);

            do
            {
                Console.Write("Enter flavour quantity: ");
                if (!int.TryParse(Console.ReadLine(), out flavourquantity))
                {
                    Console.WriteLine("Invalid input! Please enter valid Flavour Quantity.");
                    InvalidFlavourQuantity = true;
                }
                else
                {
                    InvalidFlavourQuantity = false;
                    if (flavourquantity > scoopnum)
                    {
                        Console.WriteLine("Entered quantity more than scoop number. Please try again.");
                        InvalidFlavourQuantity = true;
                    }
                    totalflavourquantity += flavourquantity;

                    if (totalflavourquantity > scoopnum)
                    {
                        Console.WriteLine("You've exceeded the scoop number. Please try again.");
                        InvalidFlavourQuantity = true;
                        totalflavourquantity -= flavourquantity;
                    }
                }
            } while (InvalidFlavourQuantity);

            Flavour flavour = new Flavour(flavourtype, flavourpremium, flavourquantity);
            flavours.Add(flavour);
        }
        
        while (toppingtype != "nil")
        {
            do
            {
                Console.WriteLine("Toppings available: ");
                Console.WriteLine("Sprinkles");
                Console.WriteLine("Mochi");
                Console.WriteLine("Sago");
                Console.WriteLine("Oreos");
                Console.Write("Enter topping (or nil to stop adding): ");
                toppingtype = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(toppingtype))
                {
                    Console.WriteLine("Invalid Topping Type! Please enter a valid Topping Type.");
                    InvalidToppingType = true;
                }
                else
                {
                    InvalidToppingType = false;
                    if (Toppings.Contains(toppingtype))
                    {
                        continue;
                    }
                    else if (toppingtype == "nil")
                    {
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Topping entered not in menu. Try again!");
                        InvalidToppingType = true;
                    }
                }
            } while (InvalidToppingType);
            if (toppingtype != "nil")
            {
                Topping topping = new Topping(toppingtype);
                toppings.Add(topping);
            }
        }

        iceCream = null;
        switch (option)
        {
            case "Waffle":
                do
                {
                    Console.WriteLine("Waffle Flavours available: ");
                    Console.WriteLine("Red Velvet");
                    Console.WriteLine("Charcoal");
                    Console.WriteLine("Pandan");
                    Console.Write("Enter waffle flavour: ");
                    waffleflavour = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(waffleflavour))
                    {
                        Console.WriteLine("Waffle Flavour entered invalid. Try again!");
                        InvalidWaffleFlavour = true;
                    }
                    else
                    {
                        InvalidWaffleFlavour = false;
                        if (waffleFlavours.Contains(waffleflavour))
                        {
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("Waffle Flavour entered is not on the menu. Try again!");
                            InvalidWaffleFlavour = true;
                        }
                    }
                } while (InvalidWaffleFlavour);

                iceCream = new Waffle("Waffle", scoopnum, flavours, toppings, waffleflavour);
                break;
            case "Cone":
                do
                {
                    Console.Write("Is cone dipped? (True/False): ");
                    if (!bool.TryParse(Console.ReadLine(), out dipped))
                    {
                        Console.WriteLine("Entered invalid input. Try again.");
                        InvalidDipped = true;
                    }
                    else
                    {
                        InvalidDipped = false;
                    }
                } while (InvalidDipped);
                iceCream = new Cone("Cone", scoopnum, flavours, toppings, dipped);
                break;
            case "Cup":
                iceCream = new Cup("Cup", scoopnum, flavours, toppings);
                break;
        }
        order.AddIceCream(iceCream);

        Console.Write("Add Another Ice Cream? [Y/N]: ");
        addicecream = Console.ReadLine();
    }

    customer.CurrentOrder = order;
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
        Console.WriteLine("Order made successfully!");
    }
    else
    {
        Console.WriteLine("Order made failed!");
    }
}

void DisplayOrderDetailsOfCustomer(Dictionary<int, Customer> customers)
{
    ListAllCustomers(customers);
    Console.WriteLine();

    int id = GetValidCustomerId(customers);
    Customer customer = customers[id];

    Console.WriteLine();
    DisplayOrders(customer.OrderHistory, "previous");
    DisplayCurrentOrder(customer.CurrentOrder);
}

int GetValidCustomerId(Dictionary<int, Customer> customers)
{
    while (true)
    {
        Console.Write("Enter your customer ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID format. Please enter a valid integer.");
        }
        else if (!customers.ContainsKey(id))
        {
            Console.WriteLine("ID entered is invalid! Please try again.");
        }
        else
        {
            return id;
        }
    }
}

void DisplayOrders(List<Order> orders, string orderType)
{
    if (orders == null || orders.Count == 0)
    {
        Console.WriteLine($"No {orderType} orders.");
        return;
    }

    foreach (Order order in orders)
    {
        if (order == null) break;
        DisplayOrderDetails(order);
    }
}

void DisplayCurrentOrder(Order currentOrder)
{
    if (currentOrder == null)
    {
        Console.WriteLine("No current orders.");
        return;
    }

    Console.WriteLine("Current Order: ");
    DisplayOrderDetails(currentOrder);
}

void DisplayOrderDetails(Order order)
{
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

void ModifyOrderDetails(Dictionary<int, Customer> customers)
{
    bool InvalidID = false;
    bool InvalidOption = false;
    bool InvalidIceCreamNo = false;
    bool InvalidIceCreamOption = false;
    bool InvalidScoopNum = false;
    bool InvalidFlavourType = false;
    bool InvalidFlavourQuantity = false;
    bool InvalidToppingType = false;
    bool InvalidWaffleFlavour = false;
    bool InvalidDipped = false;
    int id;
    Customer customer = null;
    int option = 0;
    int IceCreamNo = 0;
    string icecreamoption;
    int scoopnum;
    string flavourtype;
    bool flavourpremium = false;
    int flavourquantity = 0;
    int totalflavourquantity = 0;
    string toppingtype = null;
    string waffleflavour;
    bool dipped = false;

    ListAllCustomers(customers);
    Console.WriteLine();
    do
    {
        Console.Write("Enter your customer ID: ");
        if (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Invalid ID format. Please enter a valid integer.");
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

        do
        {
            Console.WriteLine();
            Console.WriteLine("Choose one of the following options: ");
            Console.WriteLine("[1] Choose an existing ice cream object to modify.");
            Console.WriteLine("[2] Add an entirely new ice cream object to the order.");
            Console.WriteLine("[3] Choose an existing ice cream object to delete from the order.");
            Console.Write("Enter option: ");
            if (!int.TryParse(Console.ReadLine(), out option))
            {
                Console.WriteLine("Option entered is invalid. Try again!");
                InvalidOption = true;
            }
            else
            {
                InvalidOption = false;
                if (option != 1 || option != 2 || option != 3)
                {
                    Console.WriteLine("Option entered is invalid. Try again!");
                    InvalidOption = true;
                }
                else continue;
            }

        } while (InvalidOption);


        Console.WriteLine();
        if (option == 1)
        {
            do
            {
                Console.WriteLine("Enter which ice cream to modify: ");
                if (!int.TryParse(Console.ReadLine(), out IceCreamNo))
                {
                    Console.WriteLine("Value inputted is not an integer. Try again.");
                    InvalidIceCreamNo = true;
                }
                else
                {
                    InvalidIceCreamNo = false;
                }

            } while (InvalidIceCreamNo);
            currentOrder.ModifyIceCream(IceCreamNo, Options, premFlavours, regFlavours, Toppings, waffleFlavours);
        }
        else if (option == 2)
        {
            do
            {
                Console.WriteLine("Options available: ");
                Console.WriteLine("Cup");
                Console.WriteLine("Cone");
                Console.WriteLine("Waffle");
                Console.Write("Enter option: ");
                icecreamoption = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(icecreamoption))
                {
                    Console.WriteLine("Invalid option! Please enter a valid option.");
                    InvalidOption = true;
                }
                else
                {
                    InvalidOption = false;
                    if (Options.Contains(icecreamoption))
                    {
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Option does not exist in menu! Please enter a valid option.");
                        InvalidOption = true;
                    }
                }
            } while (InvalidOption);

            do
            {
                Console.Write("Enter number of scoops(1/2/3): ");
                if (!int.TryParse(Console.ReadLine(), out scoopnum))
                {
                    Console.WriteLine("Invalid option! Please enter a valid option.");
                    InvalidScoopNum = true;
                }
                else
                {
                    InvalidScoopNum = false;
                    if (scoopnum > 3 || scoopnum < 1)
                    {
                        Console.WriteLine("Invalid scoop number! Please enter a valid option.");
                        InvalidScoopNum = true;
                    }
                    else continue;
                }
            } while (InvalidScoopNum);

            List<Flavour> flavours = new List<Flavour>();
            totalflavourquantity = 0;

            while (totalflavourquantity < scoopnum)
            {
                do
                {
                    Console.WriteLine("Flavours available: ");
                    Console.WriteLine("Vanilla");
                    Console.WriteLine("Chocolate");
                    Console.WriteLine("Strawberry");
                    Console.WriteLine("Durian");
                    Console.WriteLine("Ube");
                    Console.WriteLine("Sea Salt");
                    Console.Write("Enter flavour type: ");
                    flavourtype = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(flavourtype))
                    {
                        Console.WriteLine("Invalid Flavour Type! Please enter a valid Flavour Type.");
                        InvalidFlavourType = true;
                    }
                    else
                    {
                        InvalidFlavourType = false;
                        if (premFlavours.Contains(flavourtype))
                        {
                            flavourpremium = true;
                        }
                        else if (regFlavours.Contains(flavourtype))
                        {
                            flavourpremium = false;
                        }
                        else
                        {
                            Console.WriteLine("Flavour entered not in menu! Please enter a different flavour.");
                            InvalidFlavourType = true;
                        }
                    }
                } while (InvalidFlavourType);

                do
                {
                    Console.Write("Enter flavour quantity: ");
                    if (!int.TryParse(Console.ReadLine(), out flavourquantity))
                    {
                        Console.WriteLine("Invalid input! Please enter valid Flavour Quantity.");
                        InvalidFlavourQuantity = true;
                    }
                    else
                    {
                        InvalidFlavourQuantity = false;
                        if (flavourquantity > scoopnum)
                        {
                            Console.WriteLine("Entered quantity more than scoop number. Please try again.");
                            InvalidFlavourQuantity = true;
                        }

                        totalflavourquantity += flavourquantity;

                        if (totalflavourquantity > scoopnum)
                        {
                            Console.WriteLine("You've exceeded the scoop number. Please try again.");
                            InvalidFlavourQuantity = true;
                            totalflavourquantity -= flavourquantity;
                        }
                    }
                } while (InvalidFlavourQuantity);

                Flavour flavour = new Flavour(flavourtype, flavourpremium, flavourquantity);
                flavours.Add(flavour);
            }
            List<Topping> toppings = new List<Topping>();
            while (toppingtype != "nil")
            {
                do
                {
                    Console.WriteLine("Toppings available: ");
                    Console.WriteLine("Sprinkles");
                    Console.WriteLine("Mochi");
                    Console.WriteLine("Sago");
                    Console.WriteLine("Oreos");
                    Console.Write("Enter topping (or nil to stop adding): ");
                    toppingtype = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(toppingtype))
                    {
                        Console.WriteLine("Invalid Topping Type! Please enter a valid Topping Type.");
                        InvalidToppingType = true;
                    }
                    else
                    {
                        InvalidToppingType = false;
                        if (Toppings.Contains(toppingtype))
                        {
                            continue;
                        }
                        else if (toppingtype == "nil")
                        {
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("Topping entered not in menu. Try again!");
                            InvalidToppingType = true;
                        }
                    }
                } while (InvalidToppingType);
                if (toppingtype != "nil")
                {
                    Topping topping = new Topping(toppingtype);
                    toppings.Add(topping);
                }
            }

            IceCream iceCream = null;
            switch (icecreamoption)
            {
                case "Waffle":
                    do
                    {
                        Console.WriteLine("Waffle Flavours available: ");
                        Console.WriteLine("Red Velvet");
                        Console.WriteLine("Charcoal");
                        Console.WriteLine("Pandan");
                        Console.Write("Enter waffle flavour: ");
                        waffleflavour = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(waffleflavour))
                        {
                            Console.WriteLine("Waffle Flavour entered invalid. Try again!");
                            InvalidWaffleFlavour = true;
                        }
                        else
                        {
                            InvalidWaffleFlavour = false;
                            if (waffleFlavours.Contains(waffleflavour))
                            {
                                continue;
                            }
                            else
                            {
                                Console.WriteLine("Waffle Flavour entered is not on the menu. Try again!");
                                InvalidWaffleFlavour = true;
                            }
                        }
                    } while (InvalidWaffleFlavour);

                    iceCream = new Waffle("Waffle", scoopnum, flavours, toppings, waffleflavour);
                    break;
                case "Cone":
                    do
                    {
                        Console.Write("Is cone dipped? (True/False): ");
                        if (!bool.TryParse(Console.ReadLine(), out dipped))
                        {
                            Console.WriteLine("Entered invalid input. Try again.");
                            InvalidDipped = true;
                        }
                        else
                        {
                            InvalidDipped = false;
                        }
                    } while (InvalidDipped);
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
            do
            {
                Console.WriteLine("Enter which ice cream to delete: ");
                if (!int.TryParse(Console.ReadLine(), out IceCreamNo))
                {
                    Console.WriteLine("Value inputted is not an integer. Try again.");
                    InvalidIceCreamNo = true;
                }
                else
                {
                    InvalidIceCreamNo = false;
                }

            } while (InvalidIceCreamNo);

            IceCreamNo -= 1;
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
        return;
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
}

void ProcessOrderAndCheckout(Queue<Order> queue, Dictionary<int, Customer> customers)
{
    if (!queue.Any())
    {
        Console.WriteLine("No current orders in queue.");
        return;
    }

    Order order = queue.Dequeue();
    PrintOrderDetails(order);

    double billAmount = order.CalculateTotal();
    Console.WriteLine($"Total Bill Amount: ${billAmount.ToString("0.00")}");

    Customer customer = customers.Values.FirstOrDefault(c => c.CurrentOrder == order);
    if (customer != null)
    {
        ProcessCustomerOrder(customer, order, ref billAmount);
    }
}

void PrintOrderDetails(Order order)
{
    foreach (IceCream iceCream in order.IceCreamList)
    {
        Console.WriteLine($"Ice cream {order.IceCreamList.IndexOf(iceCream) + 1}");
        Console.WriteLine($"Ice cream option: {iceCream.Option}");
        Console.WriteLine($"Ice cream scoops: {iceCream.Scoops}");
        Console.WriteLine("Ice cream flavours: ");
        foreach (Flavour flavour in iceCream.Flavours)
        {
            Console.WriteLine(flavour.ToString());
        }
        PrintToppings(iceCream.Toppings);
        Console.WriteLine();
    }
}

void PrintToppings(List<Topping> toppings)
{
    Console.WriteLine("Ice cream toppings: ");
    if (toppings.Any())
    {
        foreach (Topping topping in toppings)
        {
            Console.WriteLine(topping.ToString());
        }
    }
    else
    {
        Console.WriteLine("No toppings");
    }
}

void ProcessCustomerOrder(Customer customer, Order order, ref double billAmount)
{
    Console.WriteLine($"Membership status: {customer.Rewards.Tier}");
    Console.WriteLine($"Points: {customer.Rewards.Points}");

    ApplyBirthdayDiscount(customer, order, ref billAmount);
    ApplyPunchCardDiscount(customer, order, ref billAmount);
    RedeemRewardPoints(customer, ref billAmount);

    Console.WriteLine($"Final Bill Amount: ${billAmount.ToString("0.00")}");
    Console.Write("Press any key to make payment: ");
    Console.ReadLine();

    UpdateCustomerRewards(customer, billAmount, order);
}

void ApplyBirthdayDiscount(Customer customer, Order order, ref double billAmount)
{
    if (customer.IsBirthday() && order.IceCreamList.Count > 1)
    {
        billAmount -= order.IceCreamList.Max(i => i.CalculatePrice());
    }
    else if (customer.IsBirthday())
    {
        billAmount = 0;
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

void RedeemRewardPoints(Customer customer, ref double billAmount)
{
    if ((customer.Rewards.Tier == "Gold" || customer.Rewards.Tier == "Silver") && customer.Rewards.Points > 0)
    {
        int redeemPoints = GetRedeemPointsFromCustomer();
        customer.Rewards.RedeemPoints(redeemPoints);
        billAmount -= redeemPoints * 0.02;
    }
}

int GetRedeemPointsFromCustomer()
{
    while (true)
    {
        Console.Write("Enter number of points to redeem: ");
        if (int.TryParse(Console.ReadLine(), out int redeemPoints))
        {
            return redeemPoints;
        }
        Console.WriteLine("Enter a valid integer.");
    }
}

void UpdateCustomerRewards(Customer customer, double billAmount, Order order)
{
    foreach (IceCream ic in order.IceCreamList)
    {
        customer.Rewards.Punch();
    }

    int earnedPoints = Convert.ToInt32(Math.Floor(billAmount * 0.72));
    customer.Rewards.AddPoints(earnedPoints);
    UpdateCustomerTier(customer);

    order.TimeFulfilled = DateTime.Now;
    customer.CurrentOrder = null;
    customer.OrderHistory.Add(order);
}

void UpdateCustomerTier(Customer customer)
{
    if (customer.Rewards.Points >= 100)
    {
        customer.Rewards.Tier = "Gold";
    }
    else if (customer.Rewards.Points >= 50 && customer.Rewards.Tier != "Gold")
    {
        customer.Rewards.Tier = "Silver";
    }
}
void DisplayChargedAmounts(Dictionary<int, Order> orders)
{
    int year = GetValidYearFromUserInput();
    Dictionary<DateTime, Order> ordersInYear = GetOrdersInYear(orders, year);

    Dictionary<int, double> monthlyTotals = CalculateMonthlyTotals(ordersInYear);
    double overallTotal = monthlyTotals.Values.Sum();

    PrintMonthlyTotals(year, monthlyTotals);
    Console.WriteLine($"Total: ${overallTotal.ToString("0.00")}");
}

int GetValidYearFromUserInput()
{
    int currentYear = DateTime.Now.Year;
    while (true)
    {
        Console.Write("Enter the year: ");
        if (int.TryParse(Console.ReadLine(), out int year) && year <= currentYear)
        {
            return year;
        }
        Console.WriteLine("Invalid input. Please enter a valid year.");
    }
}

Dictionary<DateTime, Order> GetOrdersInYear(Dictionary<int, Order> orders, int year)
{
    return orders.Values
        .Where(order => order.TimeFulfilled?.Year == year)
        .ToDictionary(order => order.TimeFulfilled ?? DateTime.MinValue, order => order);
}

Dictionary<int, double> CalculateMonthlyTotals(Dictionary<DateTime, Order> ordersInYear)
{
    Dictionary<int, double> monthlyTotals = new Dictionary<int, double>();

    for (int month = 1; month <= 12; month++)
    {
        double monthlyTotal = ordersInYear
            .Where(pair => pair.Key.Month == month)
            .Sum(pair => pair.Value.CalculateTotal());

        monthlyTotals[month] = monthlyTotal;
    }

    return monthlyTotals;
}

void PrintMonthlyTotals(int year, Dictionary<int, double> monthlyTotals)
{
    Console.WriteLine();
    foreach (var pair in monthlyTotals)
    {
        string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(pair.Key);
        Console.WriteLine($"{monthName} {year}: ${pair.Value.ToString("0.00")}");
    }
    Console.WriteLine();
}
