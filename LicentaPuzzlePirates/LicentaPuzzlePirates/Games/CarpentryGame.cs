using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using LicentaPuzzlePirates.Games.GameResources;

namespace LicentaPuzzlePirates.Games
{
    public class CarpentryGame : TileGame
    {
        #region PropertiesRegion
        //GridStats
        private static Brush shapeColor = Brushes.Chocolate;
        private static Brush tileColor = Brushes.Brown;
        private static Brush workplacColor = Brushes.Red;
        private static Brush boxColor = Brushes.Blue;
        private static Brush backgroundColor = Brushes.SaddleBrown;
        private static Brush emptyTileColor = Brushes.DarkGray;

        //PositionStats
        private int borderTileCount = 2;
        private int spaceBetweenShapes = 1;

        private Point boxStartingPoint;
        private int boxHeight;
        private int boxWidth;

        private Point workplaceStartingPoint;
        private int workplaceHeight = 6;
        private int workplaceWidth;


        //TilesForGameplay
        private Shape[,] shapes;


        //Event&GameplayStats
        private static int HealthForCompleted = 40;

        private int shapeSize = 3;
        private int shapeCollumns;
        private int shapeRows;

        private int clickedTileI;
        private int clickedTileJ;

        private int clickedShapeI;
        private int clickedShapeJ;

        bool mouseDown = false;
        #endregion


        public CarpentryGame(int shapeRows, int shapeCollumns) : base()
        {
            timeToPlaySeconds = 60;
            this.shapeCollumns = shapeCollumns;
            this.shapeRows = shapeRows;
            Init();
            DrawAll();
            timer.Start();

            gameWindow.Background = backgroundColor;
            gameWindow.ShowDialog();
        }


        #region InitRegion
        protected override void InitGameStats()
        {
            int tilesWidth = 2 * borderTileCount + (shapeCollumns - 1) * spaceBetweenShapes + shapeCollumns * shapeSize;
            int tilesHeight = 3 * borderTileCount + (shapeRows - 1) * spaceBetweenShapes + shapeRows * shapeSize + workplaceHeight;
            tiles = new Tile[tilesHeight, tilesWidth];
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    tiles[i, j] = new Tile();
                }
            }
            shapes = new Shape[shapeRows, shapeCollumns];
            for (int i = 0; i < shapes.GetLength(0); i++)
            {
                for (int j = 0; j < shapes.GetLength(1); j++)
                {
                    shapes[i, j] = new Shape(Shape.CreateSomeShape(shapeSize));
                }
            }

            maxValX = (int)(tiles.GetLength(1) * tileSize);
            maxValY = (int)(tiles.GetLength(0) * tileSize);

            boxStartingPoint = new Point(borderTileCount, borderTileCount);
            boxWidth = shapeSize * shapeCollumns + (shapeCollumns - 1) * spaceBetweenShapes;
            boxHeight = shapeSize * shapeRows + (shapeRows - 1) * spaceBetweenShapes;

            workplaceStartingPoint = new Point(borderTileCount, 2 * borderTileCount + (shapeRows - 1) * spaceBetweenShapes + shapeRows * shapeSize);
            workplaceWidth = shapeSize * shapeCollumns + (shapeCollumns - 1) * spaceBetweenShapes;

            tileSize = (int)(Math.Min(drawingCanvas.Width - 2 * margin, drawingCanvas.Height - 2 * margin) / Math.Max(tiles.GetLength(1), tiles.GetLength(0)));

            tileOffsetX = (int)((drawingCanvas.Width - 2 * margin - tiles.GetLength(1) * tileSize) / 2);
            tileOffsetY = (int)((drawingCanvas.Height - 2 * margin - tiles.GetLength(0) * tileSize) / 2);
        }

        protected override void CreateControlls()
        {
            stackPanel.MouseDown += MouseDownEvent;
            stackPanel.MouseUp += MouseUpEvent;
            stackPanel.MouseMove += MouseMoveEvent;
            gameWindow.KeyDown += KeyPressEvent;
        }
        #endregion


        #region EventsRegion
        public void MouseDownEvent(object sender, EventArgs e)
        {
            clickedTileI = (int)Mouse.GetPosition(drawingCanvas).Y;
            clickedTileJ = (int)Mouse.GetPosition(drawingCanvas).X;

            clickedTileI -= margin + tileOffsetY;
            clickedTileJ -= margin + tileOffsetX;

            clickedTileI /= (int)tileSize;
            clickedTileJ /= (int)tileSize;

            if (ItsShape())
            {
                mouseDown = true;
                DrawTiles();
                DrawGrid();
                DrawShapes(clickedShapeI, clickedShapeJ);
                DrawShape(clickedShapeI, clickedShapeJ, (int)Mouse.GetPosition(drawingCanvas).X, (int)Mouse.GetPosition(drawingCanvas).Y);
            }
        }

        public void MouseUpEvent(object sender, EventArgs e)
        {
            if (mouseDown)
            {
                mouseDown = false;
                int mouseTileJ = ((int)Mouse.GetPosition(drawingCanvas).X - margin - tileOffsetX) / (int)tileSize;
                int mouseTileI = ((int)Mouse.GetPosition(drawingCanvas).Y - margin - tileOffsetY) / (int)tileSize;

                if (mouseTileI >= workplaceStartingPoint.Y && mouseTileI < workplaceStartingPoint.Y + workplaceHeight
                    && mouseTileJ >= workplaceStartingPoint.X && mouseTileJ < workplaceStartingPoint.X + workplaceWidth)
                {
                    for (int i = 0; i < shapeSize; i++)
                    {
                        for (int j = 0; j < shapeSize; j++)
                        {
                            tiles[mouseTileI + (i - shapeSize / 2), mouseTileJ + (j - shapeSize / 2)].value += shapes[clickedShapeI, clickedShapeJ][i, j];
                        }
                    }

                    shapes[clickedShapeI, clickedShapeJ] = new Shape(Shape.CreateSomeShape(shapeSize));

                    if (IsCompleted())
                    {
                        CountScore();
                        ResetWorkplace();
                    }
                }
                DrawAll();
            }
        }

        public void MouseMoveEvent(object sender, EventArgs e)
        {
            if (mouseDown)
            {
                drawingCanvas.RemoveVisuals(timerVisual);
                DrawTiles();
                DrawGrid();
                DrawShapes(clickedShapeI, clickedShapeJ);
                DrawShape(clickedShapeI, clickedShapeJ, (int)Mouse.GetPosition(drawingCanvas).X, (int)Mouse.GetPosition(drawingCanvas).Y);
            }
        }

        public void KeyPressEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.R)
            {
                if (mouseDown)
                {
                    shapes[clickedShapeI, clickedShapeJ].TurnShape();
                    DrawTiles();
                    DrawGrid();
                    DrawShapes(clickedShapeI, clickedShapeJ);
                    DrawShape(clickedShapeI, clickedShapeJ, (int)Mouse.GetPosition(drawingCanvas).X, (int)Mouse.GetPosition(drawingCanvas).Y);
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
            DrawBoxCountour();
            DrawWorkBenchContour();
        }

        public void DrawGridTile(int i, int j)
        {
            graphics.DrawRectangle(tileOffsetX + margin + j * tileSize, tileOffsetY + margin + i * tileSize, tileSize, tileSize, borderColor, borderWidth);
        }

        public void DrawWorkBenchContour()
        {
            graphics.DrawRectangle(tileOffsetX + margin + workplaceStartingPoint.X * tileSize, tileOffsetY + margin + workplaceStartingPoint.Y * tileSize, workplaceWidth * tileSize, workplaceHeight * tileSize, workplacColor, borderWidth);
        }

        public void DrawBoxCountour()
        {
            graphics.DrawRectangle(tileOffsetX + margin + boxStartingPoint.X * tileSize, tileOffsetY + margin + boxStartingPoint.Y * tileSize, boxWidth * tileSize, boxHeight * tileSize, boxColor, borderWidth);
        }
        #endregion

        #region DrawTilesRegion
        public void DrawTiles()
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    DrawTile(i, j);
                }
            }
        }

        public void DrawTile(int i, int j)
        {
            if (tiles[i, j].value == 0)
            {
                if (i >= workplaceStartingPoint.Y && i < workplaceStartingPoint.Y + workplaceHeight &&
                    j >= workplaceStartingPoint.X && j < workplaceStartingPoint.X + workplaceWidth)
                {
                    graphics.FillRectangle(tileOffsetX + margin + j * tileSize, tileOffsetY + margin + i * tileSize, tileSize, tileSize, emptyTileColor);
                }
                else
                {
                    graphics.FillRectangle(tileOffsetX + margin + j * tileSize, tileOffsetY + margin + i * tileSize, tileSize, tileSize, tileColor);
                }
            }
            else
            {
                graphics.FillRectangle(tileOffsetX + margin + j * tileSize, tileOffsetY + margin + i * tileSize, tileSize, tileSize, shapeColor);
            }
        }
        #endregion

        #region DrawShapesRegion
        public void DrawShapes()
        {
            for (int i = 0; i < shapes.GetLength(0); i++)
            {
                for (int j = 0; j < shapes.GetLength(1); j++)
                {
                    DrawShape(i, j);
                }
            }
        }

        public void DrawShapes(int q, int w)
        {
            for (int i = 0; i < shapes.GetLength(0); i++)
            {
                for (int j = 0; j < shapes.GetLength(1); j++)
                {
                    if (i != q || j != w)
                    {
                        DrawShape(i, j);
                    }
                }
            }
        }

        public void DrawShape(int i, int j)
        {
            int startI = (int)boxStartingPoint.Y + i * spaceBetweenShapes + i * shapeSize;
            int startJ = (int)boxStartingPoint.X + j * spaceBetweenShapes + j * shapeSize;

            for (int q = 0; q < shapeSize; q++)
            {
                for (int w = 0; w < shapeSize; w++)
                {
                    if (shapes[i, j][q, w] == 1)
                    {
                        graphics.FillRectangle(tileOffsetX + margin + (startJ + w) * tileSize, tileOffsetY + margin + (startI + q) * tileSize, tileSize, tileSize, shapeColor);
                    }
                }
            }
        }

        public void DrawShape(int i, int j, int mouseX, int mouseY)
        {
            for (int q = 0; q < shapeSize; q++)
            {
                for (int w = 0; w < shapeSize; w++)
                {
                    if (shapes[i, j][q, w] == 1)
                    {
                        graphics.FillRectangle(mouseX + (w - shapeSize / 2) * tileSize - tileSize / 2, mouseY + (q - shapeSize / 2) * tileSize - tileSize / 2, tileSize, tileSize, shapeColor);
                    }
                }
            }
        }
        #endregion

        private void DrawAll()
        {
            DrawTiles();
            DrawGrid();
            DrawShapes();
        }
        #endregion


        #region GameplayRegion
        private bool ItsShape()
        {
            if (boxStartingPoint.X <= clickedTileJ && boxStartingPoint.Y <= clickedTileI && boxStartingPoint.X + boxWidth > clickedTileJ && boxStartingPoint.Y + boxHeight > clickedTileI)
            {

                int clickedWorkTileI = clickedTileI - (int)boxStartingPoint.Y;
                int clickedWorkTileJ = clickedTileJ - (int)boxStartingPoint.X;

                if ((clickedWorkTileI != 0 && clickedWorkTileI != boxHeight - 1 && (clickedWorkTileI + 1) % (shapeSize + spaceBetweenShapes) < spaceBetweenShapes) || (clickedWorkTileJ != 0 && clickedWorkTileJ != boxWidth - 1 && (clickedWorkTileJ + 1) % (shapeSize + spaceBetweenShapes) < spaceBetweenShapes))
                {
                }
                else
                {
                    int clickedShapeCol = clickedWorkTileJ % (shapeSize + spaceBetweenShapes);
                    int clickedShapeRow = clickedWorkTileI % (shapeSize + spaceBetweenShapes);

                    int clickedShapeILoc = clickedWorkTileI / (shapeSize + spaceBetweenShapes);
                    int clickedShapeJLoc = clickedWorkTileJ / (shapeSize + spaceBetweenShapes);

                    if (shapes[clickedShapeILoc, clickedShapeJLoc][clickedShapeRow, clickedShapeCol] == 1)
                    {
                        clickedShapeI = clickedShapeILoc;
                        clickedShapeJ = clickedShapeJLoc;
                        return true;
                    }
                }
            }
            return false;
        }


        private bool IsCompleted()
        {
            for (int i = (int)workplaceStartingPoint.Y; i < (int)workplaceStartingPoint.Y + workplaceHeight; i++)
            {
                for (int j = (int)workplaceStartingPoint.X; j < (int)workplaceStartingPoint.X + workplaceWidth; j++)
                {
                    if (tiles[i, j].value == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void ResetWorkplace()
        {
            for (int i = (int)workplaceStartingPoint.Y - shapeSize / 2; i < (int)workplaceStartingPoint.Y + workplaceHeight + shapeSize / 2; i++)
            {
                for (int j = (int)workplaceStartingPoint.X - shapeSize / 2; j < (int)workplaceStartingPoint.X + workplaceWidth + shapeSize / 2; j++)
                {
                    tiles[i, j].value = 0;
                }
            }
        }

        private void CountScore()
        {
            int score = 0;
            for (int i = (int)workplaceStartingPoint.Y - shapeSize / 2; i < (int)workplaceStartingPoint.Y + workplaceHeight + shapeSize / 2; i++)
            {
                for (int j = (int)workplaceStartingPoint.X - shapeSize / 2; j < (int)workplaceStartingPoint.X + workplaceWidth + shapeSize / 2; j++)
                {
                    if (i >= workplaceStartingPoint.Y && i < workplaceStartingPoint.Y + workplaceHeight
                        && j >= workplaceStartingPoint.X && j < workplaceStartingPoint.X + workplaceWidth)
                    {
                        score += 1 - tiles[i, j].value;
                    }
                    else
                    {
                        score--;
                    }
                }
            }

            ApplyRepairs(score);
        }


        private void ApplyRepairs(int score)
        {
            gamesEngine.boat.FixHull(score + HealthForCompleted);
        }
        #endregion
    }
}
