using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using LicentaPuzzlePirates.Helpers;
using LicentaPuzzlePirates.Windows;

namespace LicentaPuzzlePirates.Games
{
    public abstract class Game
    {
        #region PropertiesRegion
        protected GamesEngine gamesEngine;

        //Controlls
        protected PuzzlesWindow gameWindow;
        protected StackPanel stackPanel;
        protected DrawingCanvas drawingCanvas;
        protected Graphics graphics;
        protected DispatcherTimer timer;

        protected DrawingVisual timerVisual;

        private static Brush timerBrush = Brushes.Red;


        //Event&GameplayStats
        protected int timeToPlaySeconds;
        protected int currentlyElpasedTime;
        #endregion


        public Game()
        {
            gamesEngine = GamesEngine.GetInstance();
            timeToPlaySeconds = 30;
            currentlyElpasedTime = 0;
            InitTimer();
        }


        #region InitRegion
        protected void Init()
        {
            InitGameWindow();
            InitStackPanel();
            InitCanvas();
            InitGameStats();
            CreateControlls();
        }

        protected void InitGameWindow()
        {
            gameWindow = new PuzzlesWindow();
            gameWindow.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            gameWindow.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            gameWindow.Left = 0;
            gameWindow.Top = 0;
            gameWindow.Closed += GameWindowCloseEvent;
        }

        protected void InitStackPanel()
        {
            stackPanel = new StackPanel();
            stackPanel.Height = gameWindow.Height;
            stackPanel.Width = gameWindow.Width;
            gameWindow.Content = stackPanel;
        }

        protected void InitCanvas()
        {
            drawingCanvas = new DrawingCanvas();
            drawingCanvas.Height = stackPanel.Height;
            drawingCanvas.Width = stackPanel.Width;
            stackPanel.Children.Add(drawingCanvas);
            graphics = new Graphics(drawingCanvas);
        }

        protected abstract void InitGameStats();

        protected abstract void CreateControlls();

        private void InitTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerTickEvent;
        }
        #endregion


        #region EventsRegion
        private void TimerTickEvent(object sender, EventArgs e)
        {
            DrawTime();
            currentlyElpasedTime += 1;
            if (currentlyElpasedTime > timeToPlaySeconds && !(this is SailingGame))
            {
                gameWindow.Close();
                timer.Stop();

            }
        }

        private void GameWindowCloseEvent(object sender, EventArgs e)
        {
            timer.Stop();
        }
        #endregion


        #region DrawRegion
        protected void DrawTime()
        {
            if (timerVisual != null)
            {
                drawingCanvas.DeleteVisual(timerVisual);
            }
            int seconds = timeToPlaySeconds - currentlyElpasedTime >= 0 ? timeToPlaySeconds - currentlyElpasedTime : 0;
            if (seconds > 0)
            {
                timerVisual = graphics.DrawText("Time left: " + seconds, 100, 100, timerBrush);
            }
            else if (seconds <= 0)
            {
                timerVisual = graphics.DrawText("You can rest now.", 100, 100, timerBrush);
            }
        }
        #endregion
    }
}
