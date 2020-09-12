using System.Windows;
using System.Windows.Controls;

namespace LicentaPuzzlePirates.Windows
{
    public partial class ShowBoatControl : UserControl
    {
        private BoatStats boatStats;

        public ShowBoatControl(BoatStats boatStats)
        {
            InitializeComponent();

            this.boatStats = boatStats;
            PopulateLabels();
        }

        private void PopulateLabels()
        {
            HullHealthBlock.Text = boatStats.BasicHullHealth.ToString();
            SailHealthBlock.Text = boatStats.BasicSailHealth.ToString();
            TotalSpeedBlock.Text = (boatStats.BasicVerticalSpeed + boatStats.BasicHorizontalSpeed).ToString();
            TotalDeficitBlock.Text = ((int)(((boatStats.FloodDeficit + boatStats.RiggingDeficit + boatStats.SailDeficit) * 100 / 3))).ToString();
        }

        private void SelectBoatButton_Click(object sender, RoutedEventArgs e)
        {
            GamesEngine.GetInstance().InitGame(boatStats);
            Window.GetWindow(this).Close();
        }
    }
}
