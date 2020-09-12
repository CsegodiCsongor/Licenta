using System;
using System.Windows;

namespace LicentaPuzzlePirates.Windows
{
    public partial class MenuWindow : Window
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


        public MenuWindow()
        {
            InitializeComponent();
            Loaded += ToolWindow_Loaded;

            GamesEngine.GetInstance();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            ChooseBoatWindow chooseBoatWindow = new ChooseBoatWindow();
            chooseBoatWindow.Show();
            this.Close();
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow helpWindow = new HelpWindow();
            helpWindow.Show();

            this.Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
