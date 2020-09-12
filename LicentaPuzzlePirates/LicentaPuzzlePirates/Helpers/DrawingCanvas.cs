using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace LicentaPuzzlePirates.Helpers
{
    public class DrawingCanvas : Canvas
    {
        private List<Visual> visuals = new List<Visual>();


        protected override int VisualChildrenCount
        {
            get { return visuals.Count; }
        }

        protected override Visual GetVisualChild(int index)
        {
            return visuals[index];
        }


        public void AddVisual(Visual visual)
        {
            visuals.Add(visual);
            base.AddVisualChild(visual);
            base.AddLogicalChild(visual);
        }

        public void DeleteVisual(Visual visual)
        {
            visuals.Remove(visual);
            base.RemoveVisualChild(visual);
            base.RemoveLogicalChild(visual);
        }


        public void RemoveVisuals()
        {
            for (int i = 0; i < visuals.Count; i++)
            {
                DeleteVisual(visuals[i]);
                i--;
            }
        }

        public void RemoveVisuals(DrawingVisual visualToKeep)
        {
            for (int i = 0; i < visuals.Count; i++)
            {
                if (visuals[i] != visualToKeep)
                {
                    DeleteVisual(visuals[i]);
                    i--;
                }
            }
        }

        public void PrioratizeFirstElement()
        {
            visuals.Add(visuals[0]);
            visuals.RemoveAt(0);
        }
    }
}
