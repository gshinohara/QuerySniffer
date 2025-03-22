using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.GUI.Canvas.Interaction;
using QuerySniffer.Components.Attributes;
using System.Drawing;

namespace QuerySniffer.Components.Interactions
{
    internal class QueryObjectInteraction : GH_AbstractInteraction
    {
        private QueryObject m_QueryObject;
        private PointF m_CanvasMouseLocation;

        public QueryObjectInteraction(GH_Canvas canvas, GH_CanvasMouseEvent mouseEvent, QueryObject queryObject) : base(canvas, mouseEvent)
        {
            m_QueryObject = queryObject;

            canvas.CanvasPostPaintWires += Canvas_CanvasPostPaintWires;
        }

        public override void Destroy()
        {
            Canvas.CanvasPostPaintWires -= Canvas_CanvasPostPaintWires;
            base.Destroy();
        }

        private void Canvas_CanvasPostPaintWires(GH_Canvas sender)
        {
            if (m_CanvasMouseLocation.IsEmpty)
                return;
            using (Pen pen = new Pen(Color.Orange, 5))
            {
                PointF startPt = (m_QueryObject.Attributes as QueryObjectAttributes).CustomInputGrip;
                PointF endPt = m_CanvasMouseLocation;
                sender.Graphics.DrawLine(pen, startPt, endPt);
            }
        }

        public override GH_ObjectResponse RespondToMouseMove(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            m_CanvasMouseLocation = e.CanvasLocation;
            sender.Invalidate();
            return base.RespondToMouseMove(sender, e);
        }

        public override GH_ObjectResponse RespondToMouseUp(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            Canvas.ActiveInteraction = null;
            return base.RespondToMouseUp(sender, e);
        }
    }
}
