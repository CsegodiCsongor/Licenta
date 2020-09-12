using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using LicentaPuzzlePirates.Games.GameResources;

namespace LicentaPuzzlePirates.Games
{
    class BilgePumpingGame : TileGame
    {
        #region PropertiesRegion
        //ColorsUsed and the number of colors defiones the number of different Tile kinds
        private static Brush[] colors = new Brush[] { Brushes.Blue, Brushes.DeepSkyBlue, Brushes.CadetBlue, Brushes.DarkBlue };
        private static Brush backgroundColor = Brushes.DodgerBlue;


        //GridStats
        private static Brush selectedBorderColor = Brushes.Gold;


        //GameplayStats
        int selectedTileI;
        int selectedTileJ;
        #endregion


        public BilgePumpingGame(int tileWidthCount, int tileHeightCount) : base(tileWidthCount, tileHeightCount)
        {
            DrawTiles();
            DrawGrid();
            timer.Start();

            gameWindow.Background = backgroundColor;
            gameWindow.ShowDialog();
        }


        #region InitRegion
        protected override void InitGameStats()
        {
            selectedTileI = 0;
            selectedTileJ = 0;

            tileSize = (int)(Math.Min(drawingCanvas.Width - 2 * margin, drawingCanvas.Height - 2 * margin) / Math.Max(tiles.GetLength(1), tiles.GetLength(0)));
            maxValX = (int)(tiles.GetLength(1) * tileSize);
            maxValY = (int)(tiles.GetLength(0) * tileSize);

            tileOffsetX = (int)((drawingCanvas.Width - 2 * margin - tiles.GetLength(1) * tileSize) / 2);
            tileOffsetY = (int)((drawingCanvas.Height - 2 * margin - tiles.GetLength(0) * tileSize) / 2);

            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    tiles[i, j] = new Tile();
                    tiles[i, j].value = GamesEngine.random.Next(colors.Length);
                }
            }
        }

        protected override void CreateControlls()
        {
            drawingCanvas.MouseMove += MouseMove;
            drawingCanvas.MouseUp += MouseClick;
        }
        #endregion


        #region EventsRegion
        private void MouseMove(object sender, EventArgs e)
        {
            int xInCanvas = (int)Mouse.GetPosition(drawingCanvas).X - tileOffsetX - margin;
            int yInCanvas = (int)Mouse.GetPosition(drawingCanvas).Y - tileOffsetY - margin;

            if (xInCanvas >= 0 && yInCanvas >= 0 && xInCanvas < tiles.GetLength(1) * tileSize && yInCanvas < tiles.GetLength(0) * tileSize)
            {
                int currentTileI = (int)(yInCanvas / tileSize);
                int currentTileJ = (int)(xInCanvas / tileSize);
                if (currentTileJ >= tiles.GetLength(1) - 1)
                {
                    currentTileJ = tiles.GetLength(1) - 2;
                }
                if (currentTileI != selectedTileI || currentTileJ != selectedTileJ)
                {
                    DrawGridTile(selectedTileI, selectedTileJ);
                    DrawGridTile(selectedTileI, selectedTileJ + 1);
                    selectedTileI = currentTileI;
                    selectedTileJ = currentTileJ;
                    DrawSelectedTile();
                }
            }
        }

        private void MouseClick(object sender, EventArgs e)
        {
            int auxValue = tiles[selectedTileI, selectedTileJ].value;
            tiles[selectedTileI, selectedTileJ].value = tiles[selectedTileI, selectedTileJ + 1].value;
            tiles[selectedTileI, selectedTileJ + 1].value = auxValue;
            if (CheckBreakTiles())
            {
                drawingCanvas.RemoveVisuals(timerVisual);
                DrawAll();
            }
            else
            {
                DrawTile(selectedTileI, selectedTileJ);
                DrawTile(selectedTileI, selectedTileJ + 1);
                DrawSelectedTile();
            }
        }
        #endregion


        #region DrawRegion

        #region DrawGridRegion
        public void DrawGrid()
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    DrawGridTile(i, j);
                }
            }
            DrawSelectedTile();
        }

        public void DrawSelectedTile()
        {
            graphics.DrawRectangle(tileOffsetX + margin + selectedTileJ * tileSize, tileOffsetY + margin + selectedTileI * tileSize, tileSize, tileSize, selectedBorderColor, borderWidth);
            graphics.DrawRectangle(tileOffsetX + margin + (selectedTileJ + 1) * tileSize, tileOffsetY + margin + selectedTileI * tileSize, tileSize, tileSize, selectedBorderColor, borderWidth);
        }

        public void DrawGridTile(int i, int j)
        {
            graphics.DrawRectangle(tileOffsetX + margin + j * tileSize, tileOffsetY + margin + i * tileSize, tileSize, tileSize, borderColor, borderWidth);
        }
        #endregion

        #region DrawTilesRegion
        public void DrawTiles()
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    graphics.FillRectangle(tileOffsetX + margin + j * tileSize, tileOffsetY + margin + i * tileSize, tileSize, tileSize, colors[tiles[i, j].value]);
                    DrawTile(i, j);
                }
            }
        }

        public void DrawTile(int i, int j)
        {
            graphics.FillRectangle(tileOffsetX + margin + j * tileSize, tileOffsetY + margin + i * tileSize, tileSize, tileSize, colors[tiles[i, j].value]);
        }
        #endregion

        private void DrawAll()
        {
            DrawTiles();
            DrawGrid();
        }

        #endregion


        #region GameplayRegion

        public void RefillTiles()
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                bool foundEmpty = false;
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    if (tiles[i, j] == null)
                    {
                        foundEmpty = true;
                        tiles[i, j] = new Tile() { value = GamesEngine.random.Next(colors.Length) };
                    }
                }
                if (!foundEmpty)
                {
                    break;
                }
            }
        }

        public void DropTiles()
        {
            for (int i = tiles.GetLength(0) - 2; i >= 0; i--)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    if (tiles[i, j] != null)
                    {
                        int height = i;
                        while (height < tiles.GetLength(0) - 1 && tiles[height + 1, j] == null)
                        {
                            height += 1;
                        }
                        if (height != i)
                        {
                            tiles[height, j] = new Tile() { value = tiles[i, j].value };
                            tiles[i, j] = null;
                        }
                    }
                }
            }
        }

        public bool CheckBreakTiles()
        {
            bool broke = BreakTiles(GetTilesAround(selectedTileI, selectedTileJ));
            if (tiles[selectedTileI, selectedTileJ + 1] != null)
            {
                if (!broke)
                {
                    broke = BreakTiles(GetTilesAround(selectedTileI, selectedTileJ + 1));
                }
                else
                {
                    BreakTiles(GetTilesAround(selectedTileI, selectedTileJ + 1));
                }
            }
            if (broke)
            {
                DropTiles();
                RefillTiles();
            }
            return broke;
        }

        public bool BreakTiles(List<Point> tilesToBreak)
        {
            bool broke = false;
            if (tilesToBreak.Count >= 3)
            {
                broke = true;
                for (int i = 0; i < tilesToBreak.Count; i++)
                {
                    tiles[(int)tilesToBreak[i].Y, (int)tilesToBreak[i].X] = null;
                }
            }

            ApplyRepairs(tilesToBreak.Count);

            return broke;
        }

        public void ApplyRepairs(int brokenTiles)
        {
            gamesEngine.boat.FloodPercent -= brokenTiles / 5;
        }


        #region LeeStyleRegion
        private List<Point> GetTilesAround(int i, int j)
        {
            bool[,] visited = new bool[tiles.GetLength(0), tiles.GetLength(1)];
            List<Point> points = new List<Point>();
            points.Add(new Point(j, i));

            List<Point> pointsInUse = new List<Point>();
            pointsInUse.Add(new Point(j, i));
            visited[i, j] = true;

            while (pointsInUse.Count != 0)
            {
                Point currentPoint = pointsInUse[0];
                pointsInUse.RemoveAt(0);

                if (Ok(currentPoint, -1, 0, visited))
                {
                    points.Add(new Point(currentPoint.X - 1, currentPoint.Y));
                    pointsInUse.Add(new Point(currentPoint.X - 1, currentPoint.Y));
                    visited[(int)currentPoint.Y, (int)currentPoint.X - 1] = true;
                }
                if (Ok(currentPoint, 1, 0, visited))
                {
                    points.Add(new Point(currentPoint.X + 1, currentPoint.Y));
                    pointsInUse.Add(new Point(currentPoint.X + 1, currentPoint.Y));
                    visited[(int)currentPoint.Y, (int)currentPoint.X + 1] = true;
                }
                if (Ok(currentPoint, 0, -1, visited))
                {
                    points.Add(new Point(currentPoint.X, currentPoint.Y - 1));
                    pointsInUse.Add(new Point(currentPoint.X, currentPoint.Y - 1));
                    visited[(int)currentPoint.Y - 1, (int)currentPoint.X] = true;
                }
                if (Ok(currentPoint, 0, 1, visited))
                {
                    points.Add(new Point(currentPoint.X, currentPoint.Y + 1));
                    pointsInUse.Add(new Point(currentPoint.X, currentPoint.Y + 1));
                    visited[(int)currentPoint.Y + 1, (int)currentPoint.X] = true;
                }
            }

            return points;
        }

        private bool Ok(Point point, int xOffset, int yOffset, bool[,] visited)
        {
            if (CouldGo(point, xOffset, yOffset) && ShouldGo(point, xOffset, yOffset, visited))
            {
                return true;
            }
            return false;
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

        private bool ShouldGo(Point point, int xOffset, int yOffset, bool[,] visited)
        {
            if (!visited[(int)point.Y + yOffset, (int)point.X + xOffset] &&
                tiles[(int)point.Y + yOffset, (int)point.X + xOffset] != null &&
                tiles[(int)point.Y, (int)point.X].value == tiles[(int)point.Y + yOffset, (int)point.X + xOffset].value)
            {
                return true;
            }
            return false;
        }
        #endregion
        #endregion
    }
}
