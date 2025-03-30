using Grasshopper.Kernel;
using QuerySniffer.Types;
using System;
using System.Collections.Generic;

namespace QuerySniffer.Parameters
{
    public class Param_Graph : GH_PersistentParam<GH_Graph>
    {
        public Param_Graph() : base(new GH_InstanceDescription("Graph", "G", "A directed acyclic graph in Grasshopper", string.Empty, string.Empty))
        {
        }

        protected override GH_GetterResult Prompt_Singular(ref GH_Graph value)
        {
            return GH_GetterResult.cancel;
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GH_Graph> values)
        {
            return GH_GetterResult.cancel;
        }

        public override Guid ComponentGuid => new Guid("E506F05D-0701-4E3C-A266-727D602ABF3C");
    }
}
