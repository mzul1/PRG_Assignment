// See https://aka.ms/new-console-template for more information

using S10262474_PRG2Assignment;
using System.Globalization;

string customersFilePath = "customers.csv";
/*List<Customer> customers = new List<Customer>();


LoadCustomersFromFile("customers.csv");
    DisplayMenu();


void LoadCustomersFromFile(string filePath)
{
    var lines = File.ReadAllLines(filePath);
    foreach (var line in lines)
    {
        var parts = line.Split(',');
        if (parts.Length < 3) // Checks if there are enough parts after splitting
        {
            continue; // Skip this iteration if not
        }

        if (!int.TryParse(parts[1], out int memberId)) // Safely try to parse MemberId
        {
            //Console.WriteLine($"Warning: MemberId '{parts[1]}' is not in a correct format and will be skipped.");
            continue; // Skip this iteration if parsing fails
        }

        if (!DateTime.TryParse(parts[2], out DateTime dob)) // Safely try to parse DateOfBirth
        {
            Console.WriteLine($"Warning: DateOfBirth '{parts[2]}' is not in a correct format and will be skipped.");
            continue; // Skip this iteration if parsing fails
        }

        var name = parts[0];
        customers.Add(new Customer(name, memberId, dob));
    }
}*/

void DisplayMenu()
{
    bool exitMenu = false;
    while (!exitMenu)
    {
        Console.WriteLine("Please choose an option:");
        Console.WriteLine("1) List all customers");
        Console.WriteLine("3) Register a new customer"); // New option
        Console.WriteLine("4) Exit");
        var input = Console.ReadLine();

        switch (input)
        {
            case "1":
                ListAllCustomers();
                break;
            case "3":
                RegisterNewCustomer(); // Handle the registration process
                break;
            case "4":
                exitMenu = true;
                break;
            default:
                Console.WriteLine("Invalid option, please try again.");
                break;
        }
    }
}

/*void ListAllCustomers()
{
    foreach (var customer in customers)
    {
        Console.WriteLine(customer.ToString());
    }
}

void RegisterNewCustomer()
{
    Console.WriteLine("Enter the name of the new customer:");
    string name = Console.ReadLine();

    Console.WriteLine("Enter the member ID of the new customer:");
    int memberId;
    while (!int.TryParse(Console.ReadLine(), out memberId))
    {
        Console.WriteLine("Invalid input for member ID. Please enter a valid number:");
    }

    Console.WriteLine("Enter the date of birth of the new customer (MM/dd/yyyy):");
    DateTime dob;
    while (!DateTime.TryParse(Console.ReadLine(), out dob))
    {
        Console.WriteLine("Invalid input for date of birth. Please enter a valid date (MM/dd/yyyy):");
    }

    // Create a new Customer and PointCard objects
    var newCustomer = new Customer(name, memberId, dob);
    var pointCard = new PointCard(); // Assuming default constructor sets up a new card correctly
    newCustomer.Rewards = pointCard; // Assign the PointCard to the customer

    // Add the new customer to the list
    customers.Add(newCustomer);

    // Append the new customer to the 'customers.csv' file
    AppendCustomerToFile(newCustomer);

    Console.WriteLine("New customer registered successfully.");
}

void AppendCustomerToFile(Customer customer)
{
    string newLine = $"{customer.Name},{customer.MemberId},{customer.DateOfBirth.ToString("MM/dd/yyyy")}";
    // Append the new customer information to the 'customers.csv' file
    File.AppendAllText("customers.csv", newLine + Environment.NewLine);
}
*/

List<Customer> customers = new List<Customer>();


LoadCustomersData();
DisplayMenu();


void LoadCustomersData()
{
    if (File.Exists(customersFilePath))
    {
        string[] lines = File.ReadAllLines("customers.csv");
        foreach (string line in lines.Skip(1)) // Skip the header line
        {
            string[] tokens = line.Split(',');
            string name = tokens[0];
            if (!int.TryParse(tokens[1].Trim(), out int memberId))
            {
                // Handle the error, e.g., log it, skip this line, etc.

                continue; // Skip this iteration and move to the next line
            }
            DateTime dob = DateTime.Parse(tokens[2]);
            MembershipStatus status = (MembershipStatus)Enum.Parse(typeof(MembershipStatus), tokens[3]);
            int points = int.Parse(tokens[4]);
            int punchCard = int.Parse(tokens[5]);

            Customer customer = new Customer(name, memberId, dob);
            customer.Rewards = new PointCard()
            {
                Points = points,
                PunchCard = punchCard,
                Tier = status
            };

            customers.Add(customer);

        }
    }
    else
    {
        Console.WriteLine("Customers data file not found.");
    }
}


void ListAllCustomers()
{
    foreach (Customer customer in customers)
    {
        Console.WriteLine(customer);
    }
}

void RegisterNewCustomer()
{
    Console.WriteLine("Enter customer name:");
    string name = Console.ReadLine();

    Console.WriteLine("Enter customer ID number:");
    int memberId = int.Parse(Console.ReadLine());

    Console.WriteLine("Enter customer date of birth (dd/MM/yyyy):");
    DateTime dob = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

    Customer newCustomer = new Customer(name, memberId, dob);
    PointCard newPointCard = new PointCard();
    newCustomer.Rewards = newPointCard;

    customers.Add(newCustomer);
    AppendCustomerToFile(newCustomer);
    Console.WriteLine($"Customer {name} registered successfully with ID {memberId}.");

    
}

void AppendCustomerToFile(Customer customer)
{
    // Build the new customer data line.
    string newLine = $"{customer.Name},{customer.MemberId},{customer.DateOfBirth:dd/MM/yyyy},{customer.Rewards.Tier},{customer.Rewards.Points},{customer.Rewards.PunchCard}";

    // Read all lines of the file into a list, including any blank lines.
    var allLines = File.ReadAllLines(customersFilePath).ToList();

    // Identify the last non-empty line index.
    int lastNonEmptyLineIndex = allLines.FindLastIndex(line => !string.IsNullOrWhiteSpace(line));

    // If there are empty lines at the end, remove them.
    if (lastNonEmptyLineIndex < allLines.Count - 1)
    {
        allLines = allLines.Take(lastNonEmptyLineIndex + 1).ToList();
    }

    // Append the new customer line to the list.
    allLines.Add(newLine);

    // Write all lines back to the file, overwriting it.
    File.WriteAllLines(customersFilePath, allLines);
}











