using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;

namespace QuerySniffer.Types
{
    public class GrasshopperObject : GH_Goo<IGH_DocumentObject>
    {
        public GrasshopperObject()
        {
        }

        public GrasshopperObject(IGH_DocumentObject value)
        {
            Value = value;
        }

        public override string TypeName => "Grasshopper Object";

        public override string TypeDescription => "Grasshopper Object";

        public override bool IsValid => Value != null;

        public override string ToString()
        {
            if (Activator.CreateInstance(Value.GetType()) is IGH_DocumentObject obj)
                return $"{obj.Name} ({obj.Category})";
            else
                return "Invalid Object";
        }

        public override IGH_Goo Duplicate()
        {
            return new GrasshopperObject(Value);
        }

        public override bool CastTo<Q>(ref Q target)
        {
            if(Value is Q)
            {
                target = (Q)Value;
                return true;
            }

            return false;
        }
    }
}
