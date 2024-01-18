using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10262474_PRG2Assignment
{
    class Order
    {
        public int Id { get; set; }
        public DateTime TimeReceived { get; set; }
        public DateTime? TimeFulfilled { get; set; }
        public List<IceCream> IceCreamList { get; set; }

        // Constructors
        public Order() { }

        public Order(int id, DateTime timeReceived)
        {
            Id = id;
            TimeReceived = timeReceived;
            IceCreamList = new List<IceCream>();
        }

        // Class methods
        public void ModifyIceCream(int iceCreamIndex)
        {

        }

        public void AddIceCream(IceCream iceCream)
        {
            IceCreamList.Add(iceCream);
        }

        public void DeleteIceCream(int iceCreamIndex)
        {
            IceCreamList.RemoveAt(iceCreamIndex);
        }

        public double CalculateTotal()
        {
            return 0.0;
        }

        public override string ToString()
        {
            return $"Order ID: {Id}, Time Received: {TimeReceived}, Time Fulfilled: {TimeFulfilled}, Total: {CalculateTotal()}";
        }
    }
}
