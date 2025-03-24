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

        public override bool IsValid => true;

        public override string ToString()
        {
            return (Activator.CreateInstance(Value.GetType()) as IGH_DocumentObject).Name;
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
