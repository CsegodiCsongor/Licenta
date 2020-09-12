namespace LicentaPuzzlePirates
{
    public class BoatStats
    {
        #region PropertiesRegion
        private int basicSize;
        private int basicHullHealth;
        private int basicSailHealth;
        public int BasicSize { get { return basicSize; } }
        public int BasicHullHealth { get { return basicHullHealth; } }
        public int BasicSailHealth { get { return basicSailHealth; } }

        private int basicHorizontalSpeed;
        private int basicVerticalSpeed;
        public int BasicHorizontalSpeed { get { return basicHorizontalSpeed; } }
        public int BasicVerticalSpeed { get { return basicVerticalSpeed; } }

        private int sailHealthHorizonatlPercent;
        private int sailHealthVerticalPercent;
        public int SailHealthHorizonatlPercent { get { return sailHealthHorizonatlPercent; } }
        public int SailHealthVerticalPercent { get { return sailHealthVerticalPercent; } }

        private int floodVerticalPercent;
        private int floodHorizontalPercent;
        public int FloodVerticalPercent { get { return floodVerticalPercent; } }
        public int FloodHorizontalPercent { get { return floodHorizontalPercent; } }

        private int rigVerticalPercent;
        private int rigHorizontalPercent;
        public int RigVerticalPercent { get { return rigVerticalPercent; } }
        public int RigHorizontalPercent { get { return rigHorizontalPercent; } }

        private double riggingDeficit;
        private double sailDeficit;
        private double floodDeficit;
        public double RiggingDeficit { get { return riggingDeficit; } }
        public double SailDeficit { get { return sailDeficit; } }
        public double FloodDeficit { get { return floodDeficit; } }
        #endregion

        public BoatStats(BoatStats boatStats)
        {
            basicSize = boatStats.BasicSize;
            basicHullHealth = boatStats.BasicHullHealth;
            basicSailHealth = boatStats.BasicSailHealth;

            basicHorizontalSpeed = boatStats.BasicHorizontalSpeed;
            basicVerticalSpeed = boatStats.BasicVerticalSpeed;


            sailHealthHorizonatlPercent = boatStats.SailHealthHorizonatlPercent;
            sailHealthVerticalPercent = boatStats.SailHealthVerticalPercent;

            floodVerticalPercent = boatStats.FloodVerticalPercent;
            floodHorizontalPercent = boatStats.FloodHorizontalPercent;

            rigVerticalPercent = boatStats.RigVerticalPercent;
            rigHorizontalPercent = boatStats.RigHorizontalPercent;


            riggingDeficit = boatStats.RiggingDeficit;
            sailDeficit = boatStats.SailDeficit;
            floodDeficit = boatStats.FloodDeficit;
        }

        public BoatStats(string[] boatStats)
        {
            this.basicSize = int.Parse(boatStats[0]);
            this.basicHullHealth = int.Parse(boatStats[1]);
            this.basicSailHealth = int.Parse(boatStats[2]);
            this.basicHorizontalSpeed = int.Parse(boatStats[3]);
            this.basicVerticalSpeed = int.Parse(boatStats[4]);
            this.sailHealthHorizonatlPercent = int.Parse(boatStats[5]);
            this.sailHealthVerticalPercent = int.Parse(boatStats[6]);
            this.floodVerticalPercent = int.Parse(boatStats[7]);
            this.floodHorizontalPercent = int.Parse(boatStats[8]);
            this.rigVerticalPercent = int.Parse(boatStats[9]);
            this.rigHorizontalPercent = int.Parse(boatStats[10]);
            this.riggingDeficit = double.Parse(boatStats[11]);
            this.sailDeficit = double.Parse(boatStats[12]);
            this.floodDeficit = double.Parse(boatStats[13]);

        }
    }
}


