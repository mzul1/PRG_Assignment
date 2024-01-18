using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10262474_PRG2Assignment
{
    class Customer
    {
        // Initiliaze
        public string Name { get; set; }
        public int MemberId { get; set; }
        public DateTime Dob { get; set; }
        public Order CurrentOrder { get; set; }
        public List<Order> OrderHistory { get; set; }
        public PointCard Rewards { get; set; }

        // Constructors
        public Customer() { }

        public Customer(string name, int memberId, DateTime dob)
        {
            Name = name;
            MemberId = memberId;
            Dob = dob;
            OrderHistory = new List<Order>();
            Rewards = new PointCard();
        }

        // Class methods
        public Order MakeOrder()
        {
            CurrentOrder = new Order();
            return CurrentOrder;
        }

        public bool IsBirthday()
        {
            return DateTime.Today.Month == Dob.Month && DateTime.Today.Day == Dob.Day;
        }

        public override string ToString()
        {
            // Ensure that the Rewards.Tier is included in the string
            return $"Customer: {Name}, Member ID: {MemberId}, DOB: {Dob.ToShortDateString()}, Rewards Points: {Rewards.Points}";
        }
    }
    }
