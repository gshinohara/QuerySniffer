using Grasshopper;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using QuerySniffer.Analysis.DAG;
using QuerySniffer.Components;
using QuerySniffer.Components.Attributes;
using QuerySniffer.Components.Interactions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace QuerySniffer
{
    public class QuerySnifferPriority : GH_AssemblyPriority
    {
        public static readonly List<Graph> VisGraphs = new List<Graph>();

        public override GH_LoadingInstruction PriorityLoad()
        {
            Instances.CanvasCreated += Instances_CanvasCreated;
            return GH_LoadingInstruction.Proceed;
        }

        private static void Instances_CanvasCreated(GH_Canvas canvas)
        {
            canvas.MouseDown += Canvas_MouseDown;
            canvas.CanvasPostPaintWires += Canvas_CanvasPostPaintWires;
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

        private static void Canvas_CanvasPostPaintWires(GH_Canvas sender)
        {
            foreach (Graph graph in VisGraphs)
            {
                Func<int?, Color> getColor = depth =>
                {
                    int? maxDepth = graph.Nodes.Max(n => n.Depth);
                    int? minDepth = graph.Nodes.Min(n => n.Depth);
                    double? normalizedDepth = ((double)(int)depth - minDepth) / (maxDepth - minDepth);

                    Color color1 = Color.Salmon;
                    Color color2 = Color.SkyBlue;

                    return Color.FromArgb(
                        (int)(color1.R + (color2.R - color1.R) * normalizedDepth),
                        (int)(color1.G + (color2.G - color1.G) * normalizedDepth),
                        (int)(color1.B + (color2.B - color1.B) * normalizedDepth)
                    );
                };

                foreach (Edge edge in graph.Edges)
                {
                    PointF startPt;
                    PointF endPt;

                    switch (edge.StartNode)
                    {
                        case ParameterNode startNode:
                            startPt = startNode.Value.Attributes.OutputGrip;
                            switch (edge.EndNode)
                            {
                                case ParameterNode endNode:
                                    endPt = endNode.Value.Attributes.InputGrip;
                                    break;
                                case ComponentNode endNode:
                                    IGH_Param param = endNode.Value.Params.FirstOrDefault(p => p.Sources.Contains(startNode.Value));
                                    endPt = param.Attributes.InputGrip;
                                    break;
                                default:
                                    throw new NotImplementedException();
                            }
                            break;
                        case ComponentNode startNode:
                            switch (edge.EndNode)
                            {
                                case ParameterNode endNode:
                                    IGH_Param param = startNode.Value.Params.FirstOrDefault(p => p.Recipients.Contains(endNode.Value));
                                    startPt = param.Attributes.OutputGrip;
                                    endPt = endNode.Value.Attributes.InputGrip;
                                    break;
                                case ComponentNode endNode:
                                    IGH_Param paramStart = startNode.Value.Params.FirstOrDefault(p => p.Recipients.Intersect(endNode.Value.Params).Any());
                                    IGH_Param paramEnd = endNode.Value.Params.FirstOrDefault(p => p.Sources.Contains(paramStart));
                                    startPt = paramStart.Attributes.OutputGrip;
                                    endPt = paramEnd.Attributes.InputGrip;
                                    break;
                                default:
                                    throw new NotImplementedException();
                            }
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    using (GraphicsPath path = GH_Painter.ConnectionPath(startPt, endPt, GH_WireDirection.right, GH_WireDirection.left))
                    //using (Pen pen = new Pen(Color.Red, 4f))
                    using (Pen pen = new Pen(new LinearGradientBrush(startPt, endPt, getColor(edge.StartNode.Depth), getColor(edge.EndNode.Depth)), 4f))
                    using (Pen penBold = new Pen(Color.Gray, 6f))
                    {
                        sender.Graphics.DrawPath(penBold, path);
                        sender.Graphics.DrawPath(pen, path);
                    }
                }
            }
        }
    }
}
