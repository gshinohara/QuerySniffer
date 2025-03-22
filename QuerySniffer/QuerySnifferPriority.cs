using Grasshopper;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using QuerySniffer.Components;
using QuerySniffer.Components.Attributes;
using QuerySniffer.Components.Interactions;
using System.Drawing;
using System.Windows.Forms;

namespace QuerySniffer
{
    public class QuerySnifferPriority : GH_AssemblyPriority
    {
        public override GH_LoadingInstruction PriorityLoad()
        {
            Instances.CanvasCreated += Instances_CanvasCreated;
            return GH_LoadingInstruction.Proceed;
        }

        private static void Instances_CanvasCreated(GH_Canvas canvas)
        {
            canvas.MouseDown += Canvas_MouseDown;
        }

        private static void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is GH_Canvas canvas && canvas.Document is GH_Document document)
            {
                // Checks if the mouse is clicking near a QueryObject's custom grip point.
                // If so, it starts an interaction with that object on the Grasshopper canvas.
                foreach (var obj in document.Objects)
                {
                    if (obj is QueryObject queryObject)
                    {
                        RectangleF mouseArea = new RectangleF { Location = canvas.Viewport.UnprojectPoint(e.Location), Width = 20, Height = 20 };
                        mouseArea.Offset(-mouseArea.Width / 2, -mouseArea.Height / 2);

                        PointF grip = (queryObject.Attributes as QueryObjectAttributes).CustomInputGrip;

                        if (GH_GraphicsUtil.IsPointInEllipse(mouseArea, grip))
                        {
                            canvas.ActiveInteraction = new QueryObjectInteraction(canvas, new GH_CanvasMouseEvent(canvas.Viewport, e), queryObject);
                            break;
                        }

                    }
                }
            }
        }
    }
}
