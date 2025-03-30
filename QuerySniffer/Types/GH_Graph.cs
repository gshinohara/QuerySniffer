using Grasshopper.Kernel.Types;
using QuerySniffer.Analysis.DAG;

namespace QuerySniffer.Types
{
    public class GH_Graph : GH_Goo<Graph>
    {
        public GH_Graph()
        {
        }

        public GH_Graph(Graph graph)
        {
            Value = graph;
        }

        public override bool CastFrom(object source)
        {
            switch (source)
            {
                case Graph graph:
                    Value = graph;
                    return true;
            }
            return false;
        }

        public override bool CastTo<Q>(ref Q target)
        {
            if (typeof(Graph).IsAssignableFrom(typeof(Q)))
            {
                target = (Q)(object)Value;
                return true;
            }
            return false;
        }

        public override IGH_Goo Duplicate()
        {
            return new GH_Graph(Value);
        }

        public override bool IsValid => true;

        public override string TypeDescription => "Directed Acyclic Graph in Grasshopper Algorythm";

        public override string TypeName => "Graph";

        public override string ToString()
        {
            return "Graph";
        }
    }
}
