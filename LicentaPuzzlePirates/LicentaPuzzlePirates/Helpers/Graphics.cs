using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace LicentaPuzzlePirates.Helpers
{
public class Graphics
{
    private DrawingCanvas drawingCanvas;
    private DrawingVisual drawingVisual;


    public Graphics(DrawingCanvas drawingCanvas)
    {
        this.drawingCanvas = drawingCanvas;
    }


    #region FillRegion
    public void FillRectangle(double xCoord, double yCoord, double width, double height, Brush brushColor)
    {
        drawingVisual = new DrawingVisual();

        using (DrawingContext drawingContext = drawingVisual.RenderOpen())
        {
            drawingContext.DrawRectangle(brushColor, new Pen(brushColor, 1), new Rect(xCoord, yCoord, width, height));
        }
        drawingCanvas.AddVisual(drawingVisual);
    }

    public void FillEllipse(double xCoord, double yCoord, double width, double height, Brush brushColor)
    {
        drawingVisual = new DrawingVisual();

        using (DrawingContext drawingContext = drawingVisual.RenderOpen())
        {
            drawingContext.DrawEllipse(brushColor, new Pen(brushColor, 1), new Point(xCoord, yCoord), width, height);
        }
        drawingCanvas.AddVisual(drawingVisual);
    }
    #endregion

    #region DrawRegion
    public void DrawRectangle(double xCoord, double yCoord, double width, double height, Brush brushColor, int penWidth = 1)
    {
        drawingVisual = new DrawingVisual();

        using (DrawingContext drawingContext = drawingVisual.RenderOpen())
        {
            drawingContext.DrawRectangle(null, new Pen(brushColor, penWidth), new Rect(xCoord, yCoord, width, height));
        }
        drawingCanvas.AddVisual(drawingVisual);
    }

    public void DrawEllipse(double xCoord, double yCoord, double width, double height, Brush brushColor, int penWidth = 1)
    {
        drawingVisual = new DrawingVisual();

        using (DrawingContext drawingContext = drawingVisual.RenderOpen())
        {
            drawingContext.DrawEllipse(brushColor, new Pen(brushColor, penWidth), new Point(xCoord, yCoord), width, height);
        }
        drawingCanvas.AddVisual(drawingVisual);
    }

    public void DrawLine(double xCoordStart, double yCoordStart, double xCoordDest, double yCoordDest, Brush brushColor, int penWidth = 1)
    {
        drawingVisual = new DrawingVisual();

        using (DrawingContext drawingContext = drawingVisual.RenderOpen())
        {
            drawingContext.DrawLine(new Pen(brushColor, penWidth), new Point(xCoordStart, yCoordStart), new Point(xCoordDest, yCoordDest));
        }
        drawingCanvas.AddVisual(drawingVisual);
    }

    public DrawingVisual DrawText(string text, double xCoord, double yCoord, Brush textColor)
    {
        DrawingVisual drawingVisual = new DrawingVisual();

        using (DrawingContext drawingContext = drawingVisual.RenderOpen())
        {
            Typeface typeFace = new Typeface(new FontFamily("TimesNewRoman"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);
            drawingContext.DrawText(new FormattedText(text, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, typeFace, 16, textColor), new Point(xCoord, yCoord));
        }
        drawingCanvas.AddVisual(drawingVisual);

        return drawingVisual;
    }
    #endregion
}
}
