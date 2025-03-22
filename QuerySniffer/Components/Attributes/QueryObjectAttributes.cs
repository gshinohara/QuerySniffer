using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel.Attributes;
using System.Drawing;

namespace QuerySniffer.Components.Attributes
{
    internal class QueryObjectAttributes : GH_FloatingParamAttributes
    {
        public QueryObjectAttributes(QueryObject owner) : base(owner)
        {
        }

        public override bool HasInputGrip => false;

        public PointF CustomInputGrip => new PointF(Bounds.Left + 10, Bounds.Bottom);

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            switch (channel)
            {
                case GH_CanvasChannel.Objects:
                    GH_Capsule capsule = IsIconMode(Owner.IconDisplayMode) ?
                        GH_Capsule.CreateCapsule(Bounds, GH_CapsuleRenderEngine.GetImpliedPalette(Owner)) :
                        GH_Capsule.CreateTextCapsule(Bounds, Bounds, GH_CapsuleRenderEngine.GetImpliedPalette(Owner), Owner.NickName);

                    capsule.AddInputGrip(CustomInputGrip);
                    capsule.AddOutputGrip(OutputGrip.Y);

                    if (IsIconMode(Owner.IconDisplayMode))
                        capsule.Render(graphics, icon: Owner.Locked ? Owner.Icon_24x24_Locked : Owner.Icon_24x24, selected: Selected, locked: Owner.Locked, hidden: false);
                    else
                        capsule.Render(graphics, selected: Selected, locked: Owner.Locked, hidden: false);

                    capsule.Dispose();

                    break;
            }
        }
    }
}
