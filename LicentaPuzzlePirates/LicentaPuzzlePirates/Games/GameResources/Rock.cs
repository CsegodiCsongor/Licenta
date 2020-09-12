using System.Windows;

namespace LicentaPuzzlePirates.Games.GameResources
{
    public class Rock
    {
        #region PropertiesRegion
        public Point location;

        public int size;

        public static int speed;
        #endregion


        public Rock(Point location, int size)
        {
            this.location = location;
            this.size = size;
        }

        public Rock(int x, int y, int size)
        {
            location = new Point(x, y);
            this.size = size;
        }


        public void UpdateRock()
        {
            location.X -= speed;
        }
    }
}
