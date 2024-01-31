using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//==========================================================
// Student Number : S10262474
// Student Name : Chew Jin Xuan
// Partner Name : Zulhimi
//==========================================================
namespace S10262474_PRG2Assignment
{
    class PointCard
    {
        public int Points { get; set; }
        public int PunchCard { get; set; }
        public string Tier { get; set; }
        public PointCard() { }
        public PointCard(int points, int punchCard)
        {
            Points = points;
            PunchCard = punchCard;
        }
        public void AddPoints(int points)
        {
            Points += points;
        }
        public void RedeemPoints(int points)
        {
            Points -= points;
        }
        public void Punch()
        {
            if (PunchCard < 11)
            {
                PunchCard++;
            }
            else
            {
                PunchCard = 10;
            }
        }
        public override string ToString()
        {
            return "Points: " + Points + "\tPunch Card: " + PunchCard +
                "\tTier: " + Tier;
        }
    }
}
