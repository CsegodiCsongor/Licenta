using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using LicentaPuzzlePirates.Games.GameResources;

namespace LicentaPuzzlePirates.Games
{
    public class PatchingGame : TileGame
    {
        #region PropertiesRegion
        //GridStats
        private static Brush innerColor = Brushes.Yellow;
        private static Brush sewColor = Brushes.Brown;
        private static Brush startingPointColor = Brushes.Green;
        private static Brush destinationPointColor = Brushes.Red;
        private static Brush sewedTileSewColor = Brushes.Red;
        private static Brush backgroundColor = Brushes.PapayaWhip;
        private static int sewWidth = 20;


        //TilesForGameplay
        private PatchTile[,] patchTiles;

        private Point startingPoint;
        private Point destinationPoint;
        #endregion


        public PatchingGame(int tileWidthCount, int tileHeightCount) : base()
        {
            patchTiles = new PatchTile[tileHeightCount, tileWidthCount];
            Init();
            DrawAll();
            timer.Start();

            gameWindow.Background = backgroundColor;
            gameWindow.ShowDialog();
        }


        #region InitRegion
        protected override void InitGameStats()
        {
            maxValX = (int)(patchTiles.GetLength(1) * tileSize);
            maxValY = (int)(patchTiles.GetLength(0) * tileSize);

            tileSize = (int)(Math.Min(drawingCanvas.Width - 2 * margin, drawingCanvas.Height - 2 * margin) / Math.Max(patchTiles.GetLength(1), patchTiles.GetLength(0)));

            tileOffsetX = (int)((drawingCanvas.Width - 2 * margin - patchTiles.GetLength(1) * tileSize) / 2);
            tileOffsetY = (int)((drawingCanvas.Height - 2 * margin - patchTiles.GetLength(0) * tileSize) / 2);

            tiles = new Tile[patchTiles.GetLength(0), patchTiles.GetLength(1)];

            CreateNewGame();
        }

        protected override void CreateControlls()
        {
            drawingCanvas.MouseUp += MouseUpEvent;
        }
        #endregion


        #region EventsRegion
        public void MouseUpEvent(object sender, EventArgs e)
        {
            int mouseTileJ = ((int)Mouse.GetPosition(drawingCanvas).X - margin - tileOffsetX) / (int)tileSize;
            int mouseTileI = ((int)Mouse.GetPosition(drawingCanvas).Y - margin - tileOffsetY) / (int)tileSize;

            if (mouseTileI == destinationPoint.Y && mouseTileJ == destinationPoint.X)
            {
                if (tiles[(int)destinationPoint.Y, (int)destinationPoint.X].value != 0)
                {
                    CountSewed();
                }
            }
            else
            {
                patchTiles[mouseTileI, mouseTileJ].UpdateDirections();
                CreateSewPath();

                drawingCanvas.RemoveVisuals(timerVisual);
                DrawAll();
            }
        }
        #endregion


        #region DrawRegion

        #region DrawGridRegion
        public void DrawGrid()
        {
            for (int i = 0; i < patchTiles.GetLength(0); i++)
            {
                for (int j = 0; j < patchTiles.GetLength(1); j++)
                {
                    DrawGridTile(i, j);
                }
            }
        }

        public void DrawGridTile(int i, int j)
        {
            graphics.DrawRectangle(tileOffsetX + margin + j * tileSize, tileOffsetY + margin + i * tileSize, tileSize, tileSize, borderColor, borderWidth);
        }
        #endregion

        #region DrawTilesRegion
        public void DrawTiles()
        {
            for (int i = 0; i < patchTiles.GetLength(0); i++)
            {
                for (int j = 0; j < patchTiles.GetLength(1); j++)
                {
                    DrawTile(i, j);
                }
            }
        }

        public void DrawTile(int i, int j)
        {
            if (i == destinationPoint.Y && j == destinationPoint.X)
            {
                graphics.FillRectangle(tileOffsetX + margin + j * tileSize, tileOffsetY + margin + i * tileSize, tileSize, tileSize, destinationPointColor);
            }
            else if (i == startingPoint.Y && j == startingPoint.X)
            {
                graphics.FillRectangle(tileOffsetX + margin + j * tileSize, tileOffsetY + margin + i * tileSize, tileSize, tileSize, startingPointColor);
            }
            else
            {
                graphics.FillRectangle(tileOffsetX + margin + j * tileSize, tileOffsetY + margin + i * tileSize, tileSize, tileSize, innerColor);
            }
        }
        #endregion

        #region DrawPathsRegion
        public void DrawPaths()
        {
            for (int i = 0; i < patchTiles.GetLength(0); i++)
            {
                for (int j = 0; j < patchTiles.GetLength(1); j++)
                {
                    if (patchTiles[i, j] != null)
                    {
                        DrawPath(i, j);
                    }
                }
            }
        }

        public void DrawPath(int i, int j)
        {
            for (int q = 0; q < patchTiles[i, j].directions.Length; q++)
            {
                Brush color = sewColor;
                if (tiles[i, j].value != 0)
                {
                    color = sewedTileSewColor;
                }

                if (patchTiles[i, j].directions[q] == DirectionEnum.Left)
                {
                    DrawPathLeft(i, j, color);
                }
                else if (patchTiles[i, j].directions[q] == DirectionEnum.Right)
                {
                    DrawPathRight(i, j, color);
                }
                else if (patchTiles[i, j].directions[q] == DirectionEnum.Up)
                {
                    DrawPathUp(i, j, color);
                }
                else if (patchTiles[i, j].directions[q] == DirectionEnum.Down)
                {
                    DrawPathDown(i, j, color);
                }
            }
        }

        private void DrawPathLeft(int i, int j, Brush color)
        {
            graphics.DrawLine(tileOffsetX + margin + j * tileSize + tileSize / 2, tileOffsetY + margin + i * tileSize + tileSize / 2, tileOffsetX + margin + j * tileSize, tileOffsetY + margin + i * tileSize + tileSize / 2, color, sewWidth);
        }

        private void DrawPathRight(int i, int j, Brush color)
        {
            graphics.DrawLine(tileOffsetX + margin + j * tileSize + tileSize / 2, tileOffsetY + margin + i * tileSize + tileSize / 2, tileOffsetX + margin + (j + 1) * tileSize, tileOffsetY + margin + i * tileSize + tileSize / 2, color, sewWidth);
        }

        private void DrawPathUp(int i, int j, Brush color)
        {
            graphics.DrawLine(tileOffsetX + margin + j * tileSize + tileSize / 2, tileOffsetY + margin + i * tileSize + tileSize / 2, tileOffsetX + margin + j * tileSize + tileSize / 2, tileOffsetY + margin + i * tileSize, color, sewWidth);
        }

        private void DrawPathDown(int i, int j, Brush color)
        {
            graphics.DrawLine(tileOffsetX + margin + j * tileSize + tileSize / 2, tileOffsetY + margin + i * tileSize + tileSize / 2, tileOffsetX + margin + j * tileSize + tileSize / 2, tileOffsetY + margin + (i + 1) * tileSize, color, sewWidth);
        }
        #endregion

        private void DrawAll()
        {
            DrawTiles();
            DrawPaths();
            DrawGrid();
        }
        #endregion


        #region GameplayRegion
        private void CreateNewGame()
        {
            startingPoint = new Point(GamesEngine.random.Next(patchTiles.GetLength(1)), GamesEngine.random.Next(patchTiles.GetLength(0)));
            destinationPoint = new Point(GamesEngine.random.Next(patchTiles.GetLength(1)), GamesEngine.random.Next(patchTiles.GetLength(0)));
            while (startingPoint == destinationPoint)
            {
                destinationPoint = new Point(GamesEngine.random.Next(patchTiles.GetLength(1)), GamesEngine.random.Next(patchTiles.GetLength(0)));
            }

            for (int i = 0; i < patchTiles.GetLength(0); i++)
            {
                for (int j = 0; j < patchTiles.GetLength(1); j++)
                {
                    tiles[i, j] = new Tile();

                    if ((i != startingPoint.Y || j != startingPoint.X) && (i != destinationPoint.Y || j != destinationPoint.X))
                    {
                        patchTiles[i, j] = PatchTile.CreateRandomSewShape();
                    }
                    else
                    {
                        List<DirectionEnum> directions = new List<DirectionEnum>();
                        for (int q = 0; q < Enum.GetValues(typeof(DirectionEnum)).Length; q++)
                        {
                            directions.Add((DirectionEnum)Enum.GetValues(typeof(DirectionEnum)).GetValue(q));
                        }
                        patchTiles[i, j] = new PatchTile(directions.ToArray());
                    }
                }
            }

            CreateSewPath();
        }

        private void CountSewed()
        {
            int sewedTiles = 0;

            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    if (tiles[i, j].value != 0)
                    {
                        sewedTiles++;
                    }
                }
            }

            ApplyRepairs(sewedTiles);

            CreateNewGame();
            DrawAll();
        }

        private void ApplyRepairs(int sewedTiles)
        {
            gamesEngine.boat.SailHealth += sewedTiles;
        }


        private void CreateSewPath()
        {
            tiles = new Tile[patchTiles.GetLength(0), patchTiles.GetLength(1)];
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    tiles[i, j] = new Tile();
                }
            }

            List<Point> pointsInUse = new List<Point>();

            tiles[(int)startingPoint.Y, (int)startingPoint.X].value = 1;
            pointsInUse.Add(startingPoint);

            while (pointsInUse.Count > 0)
            {
                Point currentPoint = pointsInUse[0];
                pointsInUse.RemoveAt(0);

                for (int i = 0; i < patchTiles[(int)currentPoint.Y, (int)currentPoint.X].directions.Length; i++)
                {
                    if (patchTiles[(int)currentPoint.Y, (int)currentPoint.X].directions[i] == DirectionEnum.Up && Ok(currentPoint, 0, -1))
                    {
                        pointsInUse.Add(new Point(currentPoint.X, currentPoint.Y - 1));
                        tiles[(int)currentPoint.Y - 1, (int)currentPoint.X].value = tiles[(int)currentPoint.Y, (int)currentPoint.X].value + 1;
                    }
                    if (patchTiles[(int)currentPoint.Y, (int)currentPoint.X].directions[i] == DirectionEnum.Down && Ok(currentPoint, 0, 1))
                    {
                        pointsInUse.Add(new Point(currentPoint.X, currentPoint.Y + 1));
                        tiles[(int)currentPoint.Y + 1, (int)currentPoint.X].value = tiles[(int)currentPoint.Y, (int)currentPoint.X].value + 1;
                    }
                    if (patchTiles[(int)currentPoint.Y, (int)currentPoint.X].directions[i] == DirectionEnum.Right && Ok(currentPoint, 1, 0))
                    {
                        pointsInUse.Add(new Point(currentPoint.X + 1, currentPoint.Y));
                        tiles[(int)currentPoint.Y, (int)currentPoint.X + 1].value = tiles[(int)currentPoint.Y, (int)currentPoint.X].value + 1;
                    }
                    if (patchTiles[(int)currentPoint.Y, (int)currentPoint.X].directions[i] == DirectionEnum.Left && Ok(currentPoint, -1, 0))
                    {
                        pointsInUse.Add(new Point(currentPoint.X - 1, currentPoint.Y));
                        tiles[(int)currentPoint.Y, (int)currentPoint.X - 1].value = tiles[(int)currentPoint.Y, (int)currentPoint.X].value + 1;
                    }
                }
            }


        }

        private bool CouldGo(Point point, int xOffset, int yOffset)
        {
            if (point.X + xOffset < tiles.GetLength(1) && point.X + xOffset >= 0 &&
                point.Y + yOffset < tiles.GetLength(0) && point.Y + yOffset >= 0)
            {
                return true;
            }
            return false;
        }

        private bool ShouldGo(Point point, int xOffset, int yOffset)
        {
            if (xOffset > 0)
            {
                if (tiles[(int)point.Y, (int)point.X + xOffset].value == 0
                    && patchTiles[(int)point.Y, (int)point.X + xOffset].directions.Contains(DirectionEnum.Left))
                {
                    return true;
                }
            }
            else if (xOffset < 0)
            {
                if (tiles[(int)point.Y, (int)point.X + xOffset].value == 0
                    && patchTiles[(int)point.Y, (int)point.X + xOffset].directions.Contains(DirectionEnum.Right))
                {
                    return true;
                }
            }
            else if (yOffset > 0)
            {
                if (tiles[(int)point.Y + yOffset, (int)point.X].value == 0
                    && patchTiles[(int)point.Y + yOffset, (int)point.X].directions.Contains(DirectionEnum.Up))
                {
                    return true;
                }
            }
            else if (yOffset < 0)
            {
                if (tiles[(int)point.Y + yOffset, (int)point.X].value == 0
                     && patchTiles[(int)point.Y + yOffset, (int)point.X].directions.Contains(DirectionEnum.Down
))
                {
                    return true;
                }
            }
            return false;
        }

        private bool Ok(Point point, int xOffset, int yOffset)
        {
            if (CouldGo(point, xOffset, yOffset) && ShouldGo(point, xOffset, yOffset))
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
