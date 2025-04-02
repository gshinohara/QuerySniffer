using Grasshopper.Kernel;
using System.Collections.Generic;

namespace QuerySniffer.Analysis.DAG
{
    public class ComponentNode : Node<GH_Component>
    {
        public ComponentNode(GH_Component value) : base(value)
        {
        }

        public override IEnumerable<INode> GetLeftNodes()
        {
            var left = new List<INode>();

            foreach (var inputParam in Value.Params.Input)
            {
                foreach (var source in inputParam.Sources)
                    left.Add(source.GetTopLevelNode());
            }

            return left;
        }

        public override IEnumerable<INode> GetRightNodes()
        {
            var right = new List<INode>();

            foreach (var outputParam in Value.Params.Output)
            {
                foreach (var recipient in outputParam.Recipients)
                    right.Add(recipient.GetTopLevelNode());
            }

            return right;
        }
    }
}
