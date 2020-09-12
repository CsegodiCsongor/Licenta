using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using LicentaPuzzlePirates.Games.GameResources;

namespace LicentaPuzzlePirates.Games
{
    public class SailingGame : Game
    {
        #region PropertiesRegion
        //GridStats
        private static Brush boatColor = Brushes.Brown;
        private static Brush rockColor = Brushes.Black;
        private static Brush backgroundColor = Brushes.Aquamarine;
        private static Brush textColor = Brushes.OrangeRed;


        //Controlls
        public DispatcherTimer gameTimer;


        //Event&GamePlayStats
        private List<Rock> rocks;
        private static int maxRockSize;
        private static int minRockSize;

        private static int maxRockNum = 10;
        private static int spawnChancePrecent = 15;
        private static int millisecondsBetweenSpawn = 100;
        private static int currentElpasedTimeForSpawn = 0;

        private static int currentElpasedTimeForBoatUpdate = 0;
        private static int millisecondsBetweenBoatUpdates = 1000;

        private Boat boat;

        private int verticalMovement;
        private int horizontalMovement;

        private int ticksInMilliseconds;

        private static int FPS = 60;
        #endregion


        public SailingGame(Boat boat) : base()
        {
            this.boat = boat;
            InitTimer();
            Init();
            timer.Start();

            gameWindow.Background = backgroundColor;
            gameWindow.ShowDialog();
        }


        #region InitRegion
        private void InitTimer()
        {
            ticksInMilliseconds = 1000 / FPS;
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(ticksInMilliseconds);
        }

        protected override void InitGameStats()
        {
            rocks = new List<Rock>();
            minRockSize = 20;
            maxRockSize = 100;
            Rock.speed = 15;

            boat.location = new Point((int)(drawingCanvas.Width / 10), (int)(drawingCanvas.Height / 2));

            verticalMovement = 0;
            horizontalMovement = 0;
        }

        protected override void CreateControlls()
        {
            gameWindow.KeyDown += KeyDownEvent;
            gameWindow.KeyUp += KeyUpEvent;

            gameTimer.Tick += GameTimerTickEvent;
            gameTimer.Start();
        }
        #endregion


        #region EventsRegion
        public void KeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A || e.Key == Key.Left)
            {
                horizontalMovement = -1;
            }
            else if (e.Key == Key.D || e.Key == Key.Right)
            {
                horizontalMovement = 1;
            }
            else if (e.Key == Key.W || e.Key == Key.Up)
            {
                verticalMovement = -1;
            }
            else if (e.Key == Key.S || e.Key == Key.Down)
            {
                verticalMovement = 1;
            }
            else if (e.Key == Key.Escape && currentlyElpasedTime >= timeToPlaySeconds)
            {
                CalcScore(currentlyElpasedTime);
                gameWindow.Close();
            }
        }

        public void KeyUpEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A || e.Key == Key.Left || e.Key == Key.D || e.Key == Key.Right)
            {
                horizontalMovement = 0;
            }
            else if (e.Key == Key.W || e.Key == Key.Up || e.Key == Key.S || e.Key == Key.Down)
            {
                verticalMovement = 0;
            }
        }

        public void GameTimerTickEvent(object sender, EventArgs e)
        {
            drawingCanvas.RemoveVisuals();
            UpdateBoat();
            UpdateRocks();
            SpawRocks();
            CheckForCol();
            DrawHealth();
            DrawTime();
        }
        #endregion


        #region DrawRegion
        public void DrawRocks()
        {
            foreach (Rock rock in rocks)
            {
                graphics.FillRectangle(rock.location.X - rock.size / 2, rock.location.Y - rock.size / 2, rock.size, rock.size, rockColor);
            }
        }

        public void DrawBoat()
        {
            graphics.FillRectangle(boat.location.X - boat.size / 2, boat.location.Y - boat.size / 2, boat.size, boat.size, boatColor);
        }

        public void DrawHealth()
        {
            graphics.DrawText("Hull Health: " + (int)boat.HullHealth + "\n" + "Sail Health: " + (int)boat.SailHealth, drawingCanvas.Width - 200, 100, textColor);
        }
        #endregion


        #region GameplayRegion
        private void UpdateRocks()
        {
            foreach (Rock rock in rocks)
            {
                rock.UpdateRock();
            }
            DestroyOutOfBoundsRocks();
            DrawRocks();
        }

        private void UpdateBoat()
        {
            if (!((boat.location.X < boat.size && horizontalMovement < 0) || (boat.location.X + boat.size > drawingCanvas.Width && horizontalMovement > 0)
                   || (boat.location.Y < boat.size && verticalMovement < 0) || (boat.location.Y + boat.size > drawingCanvas.Height && verticalMovement > 0)))
            {
                boat.location.X += (int)(horizontalMovement * boat.horizontalSpeed);
                boat.location.Y += (int)(verticalMovement * boat.verticalSpeed);
            }
            DrawBoat();

            currentElpasedTimeForBoatUpdate += gameTimer.Interval.Milliseconds;
            if (currentElpasedTimeForBoatUpdate >= millisecondsBetweenBoatUpdates)
            {
                currentElpasedTimeForBoatUpdate = 0;
                boat.UpdateBoat();
            }
        }


        private void DestroyOutOfBoundsRocks()
        {
            for (int i = 0; i < rocks.Count; i++)
            {
                if (rocks[i].location.X + rocks[i].size < 0)
                {
                    rocks.RemoveAt(i);
                    i--;
                }
            }
        }

        private void SpawRocks()
        {
            currentElpasedTimeForSpawn += gameTimer.Interval.Milliseconds;

            if (currentElpasedTimeForSpawn >= millisecondsBetweenSpawn)
            {
                currentElpasedTimeForSpawn = 0;

                if (rocks.Count < maxRockNum)
                {
                    int toTrySpawn = maxRockNum - rocks.Count;
                    for (int i = 0; i < toTrySpawn; i++)
                    {
                        if (GamesEngine.random.Next(100) < spawnChancePrecent)
                        {
                            int size = GamesEngine.random.Next(minRockSize, maxRockSize + 1);
                            rocks.Add(new Rock(new Point((int)(drawingCanvas.Width + size), GamesEngine.random.Next((int)(drawingCanvas.Height))), size));
                        }
                    }
                }
            }
        }


        private void CheckForCol()
        {
            for (int i = 0; i < rocks.Count; i++)
            {
                if (CalcDistBetweenBoatAndRock(i) < Math.Pow(boat.size / 2 + rocks[i].size / 2, 2))
                {
                    Crash(rocks[i]);

                    rocks.RemoveAt(i);
                    i--;
                }
            }
        }

        private void Crash(Rock rock)
        {
            boat.HullHealth -= rock.size / 2;
            if (boat.HullHealth <= 0)
            {
                gameWindow.Close();
            }
            boat.FloodPercent += rock.size / 4;
            boat.UpdateSpeed();
        }

        private double CalcDistBetweenBoatAndRock(int rockIndex)
        {
            return (Math.Pow(boat.location.X - rocks[rockIndex].location.X, 2)
                + Math.Pow(boat.location.Y - rocks[rockIndex].location.Y, 2));
        }

        private void CalcScore(int elpasedTime)
        {
            gamesEngine.Score += 100 + 10 * (elpasedTime - timeToPlaySeconds);
        }
        #endregion
    }
}
