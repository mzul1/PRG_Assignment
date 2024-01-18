// See https://aka.ms/new-console-template for more information
using S10262474_PRG2Assignment;
using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Globalization;

string customersFilePath = "customers.csv";
List<Customer> customers = new List<Customer>();


    LoadCustomersData();
    DisplayMenu();


void LoadCustomersData()
{
    try
    {
        var lines = File.ReadAllLines(customersFilePath);
        for (int i = 1; i < lines.Length; i++) // Skip header line
        {
            var data = lines[i].Split(','); // Replace ',' with the actual delimiter used in the CSV file
            if (data.Length < 6)
            {
                Console.WriteLine("Skipping line due to insufficient data: " + lines[i]);
                continue;
            }

            var customer = new Customer(data[0], int.Parse(data[1]), DateTime.Parse(data[2]));
            customer.Rewards = new PointCard(int.Parse(data[4]), int.Parse(data[5]));
            customer.Rewards.Tier = data[3]; // Make sure this corresponds to the 'MembershipStatus' column from your CSV
            customers.Add(customer);

        }
        Console.WriteLine("Loaded " + customers.Count + " customers.");
    }
    catch { }
   
}

void ListAllCustomers()
{
    // Check if the file exists before trying to read it
    if (!File.Exists(customersFilePath))
    {
        Console.WriteLine("The customers file was not found.");
        return;
    }

    try
    {
        var lines = File.ReadAllLines(customersFilePath);

        // Skip the header line and print each customer's details
        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i].Trim(); // Trim whitespace from the beginning and end of the line
            if (string.IsNullOrEmpty(line)) continue; // Skip empty lines

            var data = line.Split(','); // Change to comma for CSV files
            if (data.Length != 6 || data.Any(field => string.IsNullOrEmpty(field)))
            {
                // Skip lines that do not have exactly 6 data fields or contain empty fields
                continue;
            }

            // Extract each piece of information
            string name = data[0];
            string memberId = data[1];
            string dob = data[2]; // Assume date is in the correct format for display
            string membershipStatus = data[3];
            string membershipPoints = data[4];
            string punchCard = data[5];

            // Print the customer's details
            Console.WriteLine($"Customer: {name}, Member ID: {memberId}, DOB: {dob}, Membership: {membershipStatus}, Rewards Points: {membershipPoints}, Punch Card: {punchCard}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("An error occurred while reading customer data: " + ex.Message);
    }
}

void RegisterNewCustomer()
{
    Console.WriteLine("Please enter the customer's name:");
    string name = Console.ReadLine();

    Console.WriteLine("Please enter the customer's member ID:");
    int memberId;
    while (!int.TryParse(Console.ReadLine(), out memberId))
    {
        Console.WriteLine("Invalid input for member ID. Please enter a valid number:");
    }

    Console.WriteLine("Please enter the customer's date of birth (dd/MM/yyyy):");
    DateTime dob;
    while (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dob))
    {
        Console.WriteLine("Invalid input for date of birth. Please enter the date in the format dd/MM/yyyy:");
    }

    // Initialize a new PointCard for the customer with 0 points and punch card
    var pointCard = new PointCard(0, 0);

    // Create a new customer object
    var newCustomer = new Customer(name, memberId, dob) { Rewards = pointCard };

    // Append the new customer to the list
    customers.Add(newCustomer);

    // Append the customer information to the customers.csv file
    AppendCustomerToFile(newCustomer);

    Console.WriteLine("Customer registered successfully.");
}

void AppendCustomerToFile(Customer customer)
{
    string newLine = $"{customer.Name},{customer.MemberId},{customer.Dob:dd/MM/yyyy},{customer.Rewards.Tier},{customer.Rewards.Points},{customer.Rewards.PunchCard}";
    File.AppendAllText(customersFilePath, newLine + Environment.NewLine);
}

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