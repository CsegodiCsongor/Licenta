using System;
using System.Windows;
using System.Windows.Media.Imaging;
using LicentaPuzzlePirates.Games;
using LicentaPuzzlePirates.Games.GameResources;
using LicentaPuzzlePirates.Helpers;
using LicentaPuzzlePirates.Windows;

namespace LicentaPuzzlePirates
{
    public class GamesEngine
    {
        private static GamesEngine instance;

        public static GamesEngine GetInstance()
        {
            if(instance == null)
            {
                instance = new GamesEngine();
            }

            return instance;
        }

        private GamesEngine()
        {
            fileHandler = new FileHandler();
        }


        public char[] boatSplitChars = new char[] { '|' };
        public FileHandler fileHandler;

        public static Random random = new Random();

        public int RepairsDone;
        public int MaxRepairs = 2;
        public int Score;
        public Boat boat;


        public void InitGame(BoatStats boatStats)
        {
            Score = 0;
            RepairsDone = 0;
            boat = new Boat(boatStats);

            GameWindow gameWindow = new GameWindow();
            gameWindow.Show();
        }


        public BoatStats[] GetBoatStatsFromFile()
        {
            string[] boats = fileHandler.GetBoats();
            BoatStats[] boatStats = new BoatStats[boats.Length];

            for (int i = 0; i < boats.Length; i++)
            {
                string[] stats = fileHandler.GetBoatStas(boats[i]).Split(boatSplitChars, StringSplitOptions.RemoveEmptyEntries);
                boatStats[i] = new BoatStats(stats);
            }

            return boatStats;
        }

        public BitmapImage[] GetImagesFromFile()
        {
            string[] imagePaths = fileHandler.GetHelpImages();
            BitmapImage[] images = new BitmapImage[imagePaths.Length];

            for (int i = 0; i < images.Length; i++)
            {
                Uri uri = new Uri(imagePaths[i]);
                images[i] = new BitmapImage(uri);
            }

            return images;
        }


        #region PuzzlezRegion
        public void InitRiggingGame()
        {
            if (RepairsDone < MaxRepairs && boat.HullHealth > 0)
            {
                RepairsDone++;
                RiggingGame riggingGame = new RiggingGame(9, 9);
            }
        }

        public void InitBilgePumpingGame()
        {
            if (RepairsDone < MaxRepairs && boat.HullHealth > 0)
            {
                RepairsDone++;
                BilgePumpingGame bilgePumpingGame = new BilgePumpingGame(4, 8);
            }
        }

        public void InitCarpentryGame()
        {
            if (RepairsDone < MaxRepairs && boat.HullHealth > 0)
            {
                RepairsDone++;
                CarpentryGame carpentryGame = new CarpentryGame(2, 3);
            }
        }

        public void InitPatchingGame()
        {
            if (RepairsDone < MaxRepairs && boat.HullHealth > 0)
            {
                RepairsDone++;
                PatchingGame patchingGame = new PatchingGame(6, 6);
            }
        }

        public void InitSailingGame()
        {
            if (boat.HullHealth > 0)
            {
                SailingGame sailingGame = new SailingGame(boat);
                sailingGame.gameTimer.Stop();

                if (boat.HullHealth <= 0)
                {
                    MessageBox.Show("You Lost...");
                }

                RepairsDone = 0;
            }
        }
        #endregion
    }
}
