using Grasshopper.Kernel;
using System.Collections.Generic;

namespace QuerySniffer.Analysis.DAG
{
    public abstract class Node<T> : INode where T : IGH_DocumentObject
    {
        public T Value { get; }

        public int? Depth { get; set; } = null;

        public Node(T value)
        {
            Value = value;
        }

        public abstract IEnumerable<INode> GetRightNodes();

        public abstract IEnumerable<INode> GetLeftNodes();

        public override bool Equals(object obj)
        {
            return obj is INode other && GetHashCode() == other.GetHashCode();
        }

        public override int GetHashCode()
        {
            return Value.InstanceGuid.GetHashCode();

        }
    }

    public interface INode
    {
        int? Depth { get; set; }

        IEnumerable<INode> GetRightNodes();

        IEnumerable<INode> GetLeftNodes();
    }
}
