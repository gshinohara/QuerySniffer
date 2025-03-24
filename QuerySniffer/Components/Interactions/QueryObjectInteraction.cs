﻿using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.GUI.Canvas.Interaction;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using QuerySniffer.Components.Attributes;
using QuerySniffer.Types;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuerySniffer;

namespace QuerySniffer.Components.Interactions
{
    internal class QueryObjectInteraction : GH_AbstractInteraction
    {
        private enum WireState
        {
            New,
            Add,
            Remove,
        }

        private QueryObject m_QueryObject;
        private PointF m_CanvasMouseLocation;
        private WireState m_WireState;
        private IGH_DocumentObject m_DocumentObject;

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

            PointF startPt = (m_QueryObject.Attributes as QueryObjectAttributes).CustomInputGrip;
            PointF endPt = m_CanvasMouseLocation;
            sender.Graphics.DrawQueryWire(startPt, endPt, QuerySnifferUtility.QueryWireColor);
        }

        public override GH_ObjectResponse RespondToMouseMove(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            m_CanvasMouseLocation = e.CanvasLocation;

            if (sender.Document is GH_Document document)
                m_DocumentObject = document.Objects.FirstOrDefault(obj => GH_GraphicsUtil.IsPointInEllipse(obj.Attributes.Bounds, e.CanvasLocation));

            sender.Invalidate();
            return base.RespondToMouseMove(sender, e);
        }

        public override GH_ObjectResponse RespondToKeyDown(GH_Canvas sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.ShiftKey:
                    m_WireState = WireState.Add;
                    break;
                case Keys.ControlKey:
                    m_WireState = WireState.Remove;
                    break;
            }
            return base.RespondToKeyDown(sender, e);
        }

        public override GH_ObjectResponse RespondToKeyUp(GH_Canvas sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.ShiftKey:
                case Keys.ControlKey:
                    m_WireState = WireState.New;
                    break;
            }
            return base.RespondToKeyUp(sender, e);
        }

        public override GH_ObjectResponse RespondToMouseUp(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (m_DocumentObject == null)
            {
                Canvas.ActiveInteraction = null;
                return base.RespondToMouseUp(sender, e);
            }

            switch (m_WireState)
            {
                case WireState.New:
                    m_QueryObject.ConnectedObjects.Clear();
                    m_QueryObject.ConnectedObjects.Add(new GrasshopperObject(m_DocumentObject));
                    break;
                case WireState.Add:
                    m_QueryObject.ConnectedObjects.Add(new GrasshopperObject(m_DocumentObject));
                    break;
                case WireState.Remove:
                    var item = m_QueryObject.VolatileData.AllData(true).FirstOrDefault(obj => (obj as GrasshopperObject).Value == m_DocumentObject);
                    m_QueryObject.ConnectedObjects.Remove(item as GrasshopperObject);
                    break;
            }

            m_QueryObject.ExpireSolution(true);

            Canvas.ActiveInteraction = null;
            return base.RespondToMouseUp(sender, e);
        }
    }
}
