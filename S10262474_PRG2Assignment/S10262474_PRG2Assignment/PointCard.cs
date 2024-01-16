using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10262474_PRG2Assignment
{
    public enum MembershipStatus
    {
        Ordinary,
        Silver,
        Gold
    }
    public class PointCard
    {
        public int Points { get; set; }
        public int PunchCard { get; set; }
        public MembershipStatus Tier { get; set; }

        public PointCard()
        {
            Points = 0;
            PunchCard = 0;
            Tier = MembershipStatus.Ordinary;
        }

        public void AddPoints(int pointsToAdd)
        {
            Points += pointsToAdd;
            UpdateTier();
        }

        public void RedeemPoints(int pointsToRedeem)
        {
            if (Tier == MembershipStatus.Silver || Tier == MembershipStatus.Gold)
            {
                Points -= pointsToRedeem;
                if (Points < 0)
                {
                    Points = 0;
                }
            }
            else
            {
                throw new InvalidOperationException("Only Silver and Gold members can redeem points.");
            }
        }

        public void Punch()
        {
            PunchCard++;
            if (PunchCard >= 10)
            {
                PunchCard = 0; 
            }
        }

        private void UpdateTier()
        {
            if (Points >= 100 && Tier != MembershipStatus.Gold)
            {
                Tier = MembershipStatus.Gold;
            }
            else if (Points >= 50 && Tier == MembershipStatus.Ordinary)
            {
                Tier = MembershipStatus.Silver;
            }
            
        }

        public override string ToString()
        {
            //return $"Points: {Points}, PunchCard: {PunchCard}, Tier: {Tier}";
            return $"Membership Status: {Tier}, Membership Points: {Points}, PunchCard: {PunchCard}";
        }
    }
}
