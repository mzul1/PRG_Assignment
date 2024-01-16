using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10262474_PRG2Assignment
{
    public class Customer
    {
        public string Name { get; set; }
        public int MemberId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Order CurrentOrder { get; set; }
        public List<Order> OrderHistory { get; set; }
        public PointCard Rewards { get; set; }

        public Customer()
        {
            OrderHistory = new List<Order>();
            Rewards = new PointCard();
        }

        public Customer(string name, int memberId, DateTime dob) : this()
        {
            Name = name;
            MemberId = memberId;
            DateOfBirth = dob;
            OrderHistory = new List<Order>();
            Rewards = new PointCard();
        }

        public Order MakeOrder()
        {
            CurrentOrder = new Order();
            return CurrentOrder;
        }

        public bool IsBirthday()
        {
            return DateTime.Today.Month == DateOfBirth.Month && DateTime.Today.Day == DateOfBirth.Day;
        }

        public override string ToString()
        {
            return $"Customer: {Name}, Member ID: {MemberId}, Date of Birth: {DateOfBirth.ToShortDateString()}, {Rewards}";
        }

        
    }
}
