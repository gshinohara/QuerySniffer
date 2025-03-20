using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using QuerySniffer.Components.Attributes;
using System;

namespace QuerySniffer.Components
{
    public class QueryObject : GH_Param<IGH_Goo>
    {
        public QueryObject() : base("Query Object", "QO", "Query Object", "Params", "Util", GH_ParamAccess.item)
        {
        }

        public override Guid ComponentGuid => new Guid("ECE7231E-02C9-4352-AA18-86B193F92361");

        public override void CreateAttributes()
        {
            Attributes = new QueryObjectAttributes(this);
        }
    }
}