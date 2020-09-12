using System;
using System.Windows;

namespace LicentaPuzzlePirates.Windows
{
    public partial class ChooseBoatWindow : Window
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

        public ChooseBoatWindow()
        {
            InitializeComponent();
            Loaded += ToolWindow_Loaded;

            gamesEngine = GamesEngine.GetInstance();
            LoadBoats();
        }

        private void LoadBoats()
        {
            BoatStats[] boatStats = gamesEngine.GetBoatStatsFromFile();

            for (int i = 0; i < boatStats.Length; i++)
            {
                BoatsPanel.Children.Add(new ShowBoatControl(boatStats[i]));
            }
        }
    }
}
