using System.Windows;

namespace LicentaPuzzlePirates.Games.GameResources
{
    public class Boat : BoatStats
    {
        #region PropertiesRegion
        private int GivenHorizontalSpeed;
        private int GivenVerticalSpeed;


        public Point location;

        public int size;

        private double hullHealth;
        public double HullHealth
        {
            get { return hullHealth; }
            set
            {
                if (value > BasicHullHealth) { hullHealth = BasicHullHealth; }
                else if (value < 0) { hullHealth = 0; }
                else { hullHealth = value; }
            }
        }

        private double sailHealth;
        public double SailHealth
        {
            get { return sailHealth; }
            set
            {
                if (value < 0) { sailHealth = 0; }
                else if (value > BasicSailHealth) { sailHealth = BasicSailHealth; }
                else { sailHealth = value; }
            }
        }

        private double floodPercent;
        public double FloodPercent
        {
            get { return floodPercent; }
            set
            {
                if (value < 0) { floodPercent = 0; }
                else if (value > 100) { floodPercent = 100; }
                else { floodPercent = value; }
            }
        }

        private double riggingPercent;
        public double RigginPercent
        {
            get { return riggingPercent; }
            set
            {
                if (value < 0) { riggingPercent = 0; }
                else if (value > 100) { riggingPercent = 100; }
                else { riggingPercent = value; }
            }
        }

        public double horizontalSpeed;
        public double verticalSpeed;
        #endregion


        public Boat(BoatStats boatStats) : base(boatStats)
        {
            location = new Point(0, 0);

            horizontalSpeed = BasicHorizontalSpeed;
            verticalSpeed = BasicVerticalSpeed;

            size = BasicSize;

            HullHealth = BasicHullHealth;
            SailHealth = BasicSailHealth;

            RigginPercent = 100;
            FloodPercent = 0;

            GivenHorizontalSpeed = (100 - (SailHealthHorizonatlPercent + RigHorizontalPercent + FloodHorizontalPercent)) * BasicHorizontalSpeed / 100;
            GivenVerticalSpeed = (100 - (SailHealthVerticalPercent + RigVerticalPercent + FloodVerticalPercent)) * BasicVerticalSpeed / 100;
        }


        public void UpdateBoat()
        {
            RigginPercent -= RiggingDeficit;
            SailHealth -= SailDeficit;
            if (HullHealth < BasicHullHealth / 2)
            {
                FloodPercent += FloodDeficit;
            }
            UpdateSpeed();
        }

        public void UpdateSpeed()
        {
            double bonusVerticalSpeed = BasicVerticalSpeed * (
                (((SailHealth / BasicSailHealth) * 100) * SailHealthVerticalPercent) +
                ((100 - FloodPercent) * FloodVerticalPercent) +
                (RigginPercent * RigVerticalPercent)) / 10000;

            double bonusHorizontalSpeed = BasicHorizontalSpeed * (
                (((SailHealth / BasicSailHealth) * 100) * SailHealthHorizonatlPercent) +
                ((100 - FloodPercent) * FloodHorizontalPercent) +
                (RigginPercent * RigHorizontalPercent)) / 10000;

            horizontalSpeed = GivenHorizontalSpeed + bonusHorizontalSpeed;
            verticalSpeed = GivenVerticalSpeed + bonusVerticalSpeed;
        }

        public void FixHull(int fixingValue)
        {
            if (fixingValue > 0)
            {
                HullHealth += fixingValue;
            }
        }
    }
}
