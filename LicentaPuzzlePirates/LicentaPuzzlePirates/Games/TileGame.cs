using System.Windows.Media;
using LicentaPuzzlePirates.Games.GameResources;

namespace LicentaPuzzlePirates.Games
{
    public abstract class TileGame : Game
    {
        #region PropertiesRegion
        //GridStats
        protected static Brush borderColor = Brushes.Black;
        protected static int borderWidth = 3;


        ////PositionStats
        protected double tileSize;

        protected int margin = 25;
        protected int tileOffsetX;
        protected int tileOffsetY;

        protected int maxValX;
        protected int maxValY;


        //TilesForGameplay
        protected Tile[,] tiles;
        #endregion

        public TileGame() : base() { }

        public TileGame(int tilesWidth, int tilesHeight) : base()
        {
            tiles = new Tile[tilesHeight, tilesWidth];
            Init();
        }
    }
}
