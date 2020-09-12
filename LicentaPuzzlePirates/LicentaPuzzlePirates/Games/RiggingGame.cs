using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using LicentaPuzzlePirates.Games.GameResources;

namespace LicentaPuzzlePirates.Games
{
    public class RiggingGame : TileGame
    {
        #region PorpertiesRegion
        //ColorsUsed and the number of colors defiones the number of different Tile kinds
        private static Brush[] colors = new Brush[] { Brushes.Red, Brushes.Blue, Brushes.Orange, Brushes.Green };
        private static Brush backgroundColor = Brushes.SandyBrown;


        //Event&GameplayStats
        bool clicked = false;

        bool movingLeft = false;
        bool movingUp = false;

        int clickedX;
        int clickedY;

        int clickedTileI;
        int clickedTileJ;

        int mouseXOffset;
        int mouseYOffset;

        int moveMinValue;
        #endregion


        public RiggingGame(int tileWidthCount, int tileHeightCount) : base(tileWidthCount, tileHeightCount)
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
            tileSize = (int)(Math.Min(drawingCanvas.Width - 2 * margin, drawingCanvas.Height - 2 * margin) / Math.Max(tiles.GetLength(1), tiles.GetLength(0)));
            moveMinValue = (int)(tileSize / 4);
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
            drawingCanvas.MouseLeftButtonDown += CanvasMouseDown;
            gameWindow.MouseLeftButtonUp += CanvasMouseUp;
            drawingCanvas.MouseMove += MouseMove;
        }
        #endregion


        #region EventsRegion
        private void CanvasMouseUp(object sender, EventArgs e)
        {
            clicked = false;

            SnapTiles();

            drawingCanvas.RemoveVisuals(timerVisual);
            DrawAll();

            movingLeft = false;
            movingUp = false;
        }

        private void CanvasMouseDown(object sender, EventArgs e)
        {
            clickedX = (int)Mouse.GetPosition(drawingCanvas).X;
            clickedY = (int)Mouse.GetPosition(drawingCanvas).Y;

            clickedTileJ = clickedX - tileOffsetX - margin;
            clickedTileI = clickedY - tileOffsetY - margin;

            if (clickedTileJ >= 0 && clickedTileI >= 0 && clickedTileJ < tiles.GetLength(1) * tileSize && clickedTileI < tiles.GetLength(0) * tileSize)
            {
                clickedTileI = (int)(clickedTileI / tileSize);
                clickedTileJ = (int)(clickedTileJ / tileSize);
                clicked = true;
            }

        }

        private void MouseMove(object sender, EventArgs e)
        {
            if (clicked)
            {
                mouseXOffset = (int)Mouse.GetPosition(drawingCanvas).X - clickedX;
                mouseYOffset = (int)Mouse.GetPosition(drawingCanvas).Y - clickedY;
                if (Math.Abs(mouseXOffset) >= moveMinValue || Math.Abs(mouseYOffset) >= moveMinValue)
                {

                    if (Math.Abs(mouseXOffset) > Math.Abs(mouseYOffset))
                    {
                        if (!movingUp)
                        {
                            movingLeft = true;

                            drawingCanvas.RemoveVisuals(timerVisual);
                            DrawAll();
                            DrawTiles(clickedTileI, clickedTileJ, mouseXOffset, 0);
                        }
                    }
                    else
                    {
                        if (!movingLeft)
                        {
                            movingUp = true;

                            drawingCanvas.RemoveVisuals(timerVisual);
                            DrawAll();
                            DrawTiles(clickedTileI, clickedTileJ, 0, mouseYOffset);
                        }
                    }
                }
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
                    DrawTile(i, j, 0, 0);
                }
            }
        }

        public void DrawTiles(int i, int j, int xOffset, int yOffset)
        {
            if (xOffset != 0)
            {
                for (int q = 0; q < tiles.GetLength(1); q++)
                {
                    DrawTile(i, q, xOffset, 0);
                }
                for (int q = 0; q < tiles.GetLength(1); q++)
                {
                    DrawGridTile(i, q);
                }
            }
            else if (yOffset != 0)
            {
                for (int q = 0; q < tiles.GetLength(0); q++)
                {
                    DrawTile(q, j, 0, yOffset);
                }
                for (int q = 0; q < tiles.GetLength(0); q++)
                {
                    DrawGridTile(q, j);
                }
            }
        }

        public void DrawTile(int i, int j, int xOffset, int yOffset)
        {
            if (yOffset == 0 && xOffset == 0)
            {
                graphics.FillRectangle(tileOffsetX + margin + j * tileSize, tileOffsetY + margin + i * tileSize, tileSize, tileSize, colors[tiles[i, j].value]);
            }
            else
            {
                int maxValX = (int)(tiles.GetLength(1) * tileSize);
                int maxValY = (int)(tiles.GetLength(0) * tileSize);

                if (yOffset < 0)
                {
                    yOffset = (int)(maxValY + yOffset);
                }
                if (xOffset < 0)
                {
                    xOffset = (int)(maxValX + xOffset);
                }

                if ((i * tileSize + yOffset) % maxValY + tileSize > maxValY)
                {
                    int leftOut = (int)(((i * tileSize + yOffset) % maxValY + tileSize) % maxValY);
                    graphics.FillRectangle(tileOffsetX + margin + (j * tileSize + xOffset) % maxValX, tileOffsetY + margin + (i * tileSize + yOffset) % maxValY, tileSize, tileSize - leftOut, colors[tiles[i, j].value]);
                    graphics.FillRectangle(tileOffsetX + margin + (j * tileSize + xOffset) % maxValX, tileOffsetY + margin, tileSize, leftOut, colors[tiles[i, j].value]);
                }
                else if ((j * tileSize + xOffset) % maxValX + tileSize > maxValX)
                {
                    int leftOut = (int)(((j * tileSize + xOffset) % maxValX + tileSize) % maxValX);
                    graphics.FillRectangle(tileOffsetX + margin + (j * tileSize + xOffset) % maxValX, tileOffsetY + margin + (i * tileSize + yOffset) % maxValY, tileSize - leftOut, tileSize, colors[tiles[i, j].value]);
                    graphics.FillRectangle(tileOffsetX + margin, tileOffsetY + margin + (i * tileSize + yOffset) % maxValY, leftOut, tileSize, colors[tiles[i, j].value]);
                }
                else
                {
                    graphics.FillRectangle(tileOffsetX + margin + (j * tileSize + xOffset) % maxValX, tileOffsetY + margin + (i * tileSize + yOffset) % maxValY, tileSize, tileSize, colors[tiles[i, j].value]);
                }
            }
        }
        #endregion


        private void DrawAll()
        {
            DrawTiles();
            DrawGrid();
        }
        #endregion


        #region GameplayRegion
        public void SnapTiles()
        {
            bool broke = false;
            Tile[,] auxTiles = new Tile[tiles.GetLength(0), tiles.GetLength(1)];
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    auxTiles[i, j] = new Tile() { value = tiles[i, j].value };
                }
            }

            if (movingLeft && Math.Abs(mouseXOffset) > tileSize / 2)
            {
                if (mouseXOffset < 0)
                {
                    mouseXOffset = (int)(maxValX + mouseXOffset);
                }
                int toMove = ((int)((mouseXOffset - tileSize / 2) / tileSize)) % tiles.GetLength(1);

                int[] values = new int[tiles.GetLength(1)];
                for (int i = 0; i < tiles.GetLength(1); i++)
                {
                    values[(i + 1 + toMove) % tiles.GetLength(1)] = tiles[clickedTileI, i].value;
                }
                for (int i = 0; i < tiles.GetLength(1); i++)
                {
                    tiles[clickedTileI, i].value = values[i];
                }

                broke = CheckBreakTiles();
            }
            else if (movingUp && Math.Abs(mouseYOffset) > tileSize / 2)
            {
                if (mouseYOffset < 0)
                {
                    mouseYOffset = (int)(maxValY + mouseYOffset);
                }
                int toMove = ((int)((mouseYOffset - tileSize / 2) / tileSize)) % tiles.GetLength(0);

                int[] values = new int[tiles.GetLength(0)];
                for (int i = 0; i < tiles.GetLength(0); i++)
                {
                    values[(i + 1 + toMove) % tiles.GetLength(0)] = tiles[i, clickedTileJ].value;
                }
                for (int i = 0; i < tiles.GetLength(0); i++)
                {
                    tiles[i, clickedTileJ].value = values[i];
                }

                broke = CheckBreakTiles();
            }
            if (!broke)
            {
                tiles = auxTiles;
            }
            else
            {
                DropTiles();
                RefillTiles();
            }
        }

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
            bool broke = false;
            if (movingLeft)
            {
                Console.Write("Moving Horizontal  ");
                for (int i = 0; i < tiles.GetLength(1); i++)
                {
                    if (tiles[clickedTileI, i] != null)
                    {
                        if (BreakTiles(GetTilesAround(clickedTileI, i)))
                        {
                            broke = true;
                        }

                    }
                }
            }
            else
            {
                Console.Write("Moving Vertical  ");
                for (int i = 0; i < tiles.GetLength(0); i++)
                {
                    if (tiles[i, clickedTileJ] != null)
                    {
                        if (BreakTiles(GetTilesAround(i, clickedTileJ)))
                        {
                            broke = true;
                        }
                    }
                }
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
                    Console.Write(tilesToBreak[i].ToString());
                    tiles[(int)tilesToBreak[i].Y, (int)tilesToBreak[i].X] = null;
                }
            }

            ApplyRepairs(tilesToBreak.Count);

            return broke;
        }

        public void ApplyRepairs(int brokenTiles)
        {
            gamesEngine.boat.RigginPercent += (double)brokenTiles / 10d;
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
