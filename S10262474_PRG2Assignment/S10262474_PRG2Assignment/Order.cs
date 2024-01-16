using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10262474_PRG2Assignment
{
    public enum IceCreamOption
    {
        Cup,
        Cone,
        Waffle
    }
    public class Order
    {
        public int Id { get; set; }
        public DateTime TimeReceived { get; set; }
        public DateTime? TimeFulfilled { get; set; }
        public List<IceCream> IceCreamList { get; set; }

        public Order()
        {
            IceCreamList = new List<IceCream>();
        }

        public Order(int id, DateTime timeReceived) : this()
        {
            Id = id;
            TimeReceived = timeReceived;
        }

        public void AddIceCream(IceCream iceCream)
        {
            IceCreamList.Add(iceCream);
        }

        public void ModifyIceCream(int index)
        {
            
            throw new NotImplementedException();
        }

        public void DeleteIceCream(int index)
        {
            if (index >= 0 && index < IceCreamList.Count)
            {
                IceCreamList.RemoveAt(index);
            }
            else
            {
                throw new IndexOutOfRangeException("Invalid ice cream index.");
            }
        }

        public double CalculateTotal()
        {
            return IceCreamList.Sum(iceCream => iceCream.CalculatePrice());
        }

        public override string ToString()
        {
            string iceCreamsText = string.Join("\n", IceCreamList.Select(iceCream => iceCream.ToString()));
            return $"Order ID: {Id}, Time Received: {TimeReceived}, Ice Creams:\n{iceCreamsText}, Total Price: {CalculateTotal():C2}";
        }
    }
}
