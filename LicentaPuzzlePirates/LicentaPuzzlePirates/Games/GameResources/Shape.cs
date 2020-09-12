using System.Collections.Generic;
using System.Windows;

namespace LicentaPuzzlePirates.Games.GameResources
{
    public class Shape
    {
        #region StaticRegion
        public static int PercentOfExpansion = 33;


        public static int[,] CreateSomeShape(int shapeSize)
        {
            int[,] shape = new int[shapeSize, shapeSize];

            shape[shapeSize / 2, shapeSize / 2] = 1;
            List<Point> points = new List<Point>();
            points.Add(new Point(shapeSize / 2, shapeSize / 2));

            while (points.Count > 0)
            {
                Point currentPoint = points[0];
                points.RemoveAt(0);

                if (Go(currentPoint, -1, 0, shape))
                {
                    points.Add(new Point(currentPoint.X - 1, currentPoint.Y));
                    shape[(int)currentPoint.Y, (int)currentPoint.X - 1] = 1;
                }
                if (Go(currentPoint, 1, 0, shape))
                {
                    points.Add(new Point(currentPoint.X + 1, currentPoint.Y));
                    shape[(int)currentPoint.Y, (int)currentPoint.X + 1] = 1;
                }
                if (Go(currentPoint, 0, -1, shape))
                {
                    points.Add(new Point(currentPoint.X, currentPoint.Y - 1));
                    shape[(int)currentPoint.Y - 1, (int)currentPoint.X] = 1;
                }
                if (Go(currentPoint, 0, 1, shape))
                {
                    points.Add(new Point(currentPoint.X, currentPoint.Y + 1));
                    shape[(int)currentPoint.Y + 1, (int)currentPoint.X] = 1;
                }

            }

            return shape;
        }

        private static bool CouldGo(Point currentPoint, int xOffset, int yOffset, int[,] shape)
        {
            if (currentPoint.X + xOffset >= 0 && currentPoint.X + xOffset < shape.GetLength(1)
                && currentPoint.Y + yOffset >= 0 && currentPoint.Y + yOffset < shape.GetLength(0))
            {
                return true;
            }
            return false;
        }

        private static bool ShouldGo(Point currentPoint, int xOffset, int yOffset, int[,] shape)
        {
            if (shape[(int)currentPoint.Y + yOffset, (int)currentPoint.X + xOffset] == 0)
            {
                return true;
            }
            return false;
        }

        private static bool Go(Point currentPoint, int xOffset, int yOffset, int[,] shape)
        {
            if (GamesEngine.random.Next(100) < PercentOfExpansion && CouldGo(currentPoint, xOffset, yOffset, shape) && ShouldGo(currentPoint, xOffset, yOffset, shape))
            {
                return true;
            }
            return false;
        }
        #endregion


        private int[,] shapeMatrix;


        public Shape(Shape shape)
        {
            shapeMatrix = shape.shapeMatrix;
        }

        public Shape(int[,] shape)
        {
            shapeMatrix = shape;
        }


        public void TurnShape()
        {
            int[,] turnedShape = new int[shapeMatrix.GetLength(0), shapeMatrix.GetLength(1)];

            for (int i = 0; i < shapeMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < shapeMatrix.GetLength(1); j++)
                {
                    turnedShape[i, j] = shapeMatrix[shapeMatrix.GetLength(1) - j - 1, i];
                }
            }

            shapeMatrix = turnedShape;
        }


        public int this[int i, int j]
        {
            get { return shapeMatrix[i, j]; }
            set { shapeMatrix[i, j] = value; }
        }
    }
}
