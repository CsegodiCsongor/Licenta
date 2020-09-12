using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace LicentaPuzzlePirates.Windows
{
    public partial class HelpWindow : Window
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


        BitmapImage[] images;
        int currentImageIndex;

        public HelpWindow()
        {
            InitializeComponent();
            Loaded += ToolWindow_Loaded;

            LoadImages();
        }


        private void LoadImages()
        {
            images = GamesEngine.GetInstance().GetImagesFromFile();
            currentImageIndex = 0;

            ShowImage();
        }

        private void ShowImage()
        {
            if (images.Length > 0) { HelpImage.Source = images[currentImageIndex]; }
        }


        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentImageIndex == 0) { currentImageIndex = images.Length - 1; }
            else { currentImageIndex--; }
            ShowImage();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menuWindow = new MenuWindow();
            menuWindow.Show();

            this.Close();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            currentImageIndex = (currentImageIndex + 1) % images.Length;
            ShowImage();
        }
    }
}
