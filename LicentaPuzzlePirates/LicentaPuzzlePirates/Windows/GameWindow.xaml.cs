using System;
using System.Windows;

namespace LicentaPuzzlePirates.Windows
{
    public partial class GameWindow : Window
    {
        #region RemoveCloseButtonRegion
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        void ToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }
        #endregion

        private GamesEngine gamesEngine;

        public GameWindow()
        {
            InitializeComponent();
            Loaded += ToolWindow_Loaded;

            gamesEngine = GamesEngine.GetInstance();
            RefreshLabels();
        }

        private void RefreshLabels()
        {
            HullHealthBlock.Text = "Hull Health: " + (int)gamesEngine.boat.HullHealth;
            SailHealthBlock.Text = "Sail Health: " + (int)gamesEngine.boat.SailHealth;
            FloodPercentBloock.Text = "Flooding Percent: " + (int)gamesEngine.boat.FloodPercent;
            RigPercentBlock.Text = "Rigging Percent: " + (int)gamesEngine.boat.RigginPercent;

            ScoreBlock.Text = "Score: " + gamesEngine.Score;

            SpeedsBlock.Text = "Vertical Speed: \n" + (int)(gamesEngine.boat.verticalSpeed / gamesEngine.boat.BasicVerticalSpeed * 100) + "%" + "\n"
                + "Horizontal Speed: \n" + (int)(gamesEngine.boat.horizontalSpeed / gamesEngine.boat.BasicHorizontalSpeed * 100) + "%" + "\n";

            RepairsLeftBlock.Text = "Repairs Left: " + (gamesEngine.MaxRepairs - gamesEngine.RepairsDone);
        }

        #region ButtonsCliclsRegion
        private void FixHullButton_Click(object sender, RoutedEventArgs e)
        {
            gamesEngine.InitCarpentryGame();
            RefreshLabels();
        }

        private void SewSailButton_Click(object sender, RoutedEventArgs e)
        {
            gamesEngine.InitPatchingGame();
            RefreshLabels();
        }

        private void PumpWaterButton_Click(object sender, RoutedEventArgs e)
        {
            gamesEngine.InitBilgePumpingGame();
            RefreshLabels();
        }

        private void RigSailButton_Click(object sender, RoutedEventArgs e)
        {
            gamesEngine.InitRiggingGame();
            RefreshLabels();
        }

        private void SailOnButton_Click(object sender, RoutedEventArgs e)
        {
            gamesEngine.InitSailingGame();
            RefreshLabels();
        }

        private void ExitGameButton_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menuWindow = new MenuWindow();
            menuWindow.Show();
            this.Close();
        }
        #endregion
    }
}
