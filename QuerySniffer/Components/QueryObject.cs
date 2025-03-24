﻿using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using QuerySniffer.Components.Attributes;
using QuerySniffer.Types;
using System;

namespace QuerySniffer.Components
{
    public class QueryObject : GH_Param<GrasshopperObject>
    {
        public QueryObject() : base("Query Object", "QO", "Query Object", "QuerySniffer", "Pick", GH_ParamAccess.item)
        {
            Optional = true;
        }

        public override Guid ComponentGuid => new Guid("ECE7231E-02C9-4352-AA18-86B193F92361");

        public override void CreateAttributes()
        {
            Attributes = new QueryObjectAttributes(this);
        }
    }
}