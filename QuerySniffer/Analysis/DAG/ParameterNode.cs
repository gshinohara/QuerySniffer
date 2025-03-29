using Grasshopper.Kernel;
using System.Collections.Generic;

namespace QuerySniffer.Analysis.DAG
{
    public class ParameterNode:Node<IGH_Param>
    {
        public ParameterNode(IGH_Param value) : base(value)
        {
        }

        public override IEnumerable<INode> GetLeftNodes()
        {
            var sources = new List<INode>();
            foreach (IGH_Param source in Value.Sources)
                sources.Add(source.GetTopLevelNode());
            return sources;
        }

        public override IEnumerable<INode> GetRightNodes()
        {
            var recipients = new List<INode>();
            foreach (IGH_Param recipient in Value.Recipients)
                recipients.Add(recipient.GetTopLevelNode());
            return recipients;
        }
    }
}
