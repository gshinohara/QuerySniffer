using QuerySniffer.Components.Attributes;
using System.Drawing;

namespace QuerySniffer
{
    internal static class QuerySnifferUtility
    {
        public static Color QueryWireColor => Color.Orange;

        public static void DrawQueryWire(this Graphics graphics, PointF startPt, PointF endPt, Color color)
        {
            using (Pen pen = new Pen(color, 5))
            {
                graphics.DrawBezier(pen, startPt, startPt + new SizeF(0, 200), endPt + new SizeF(0, -200), endPt);
            }
        }
    }
}
