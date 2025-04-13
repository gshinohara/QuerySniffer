namespace QuerySniffer.Analysis.DAG
{
    public class Edge
    {
        public INode StartNode { get; private set; }

        public INode EndNode { get; private set; }

        public Edge(INode start, INode end)
        {
            StartNode = start;
            EndNode = end;
        }

        public void Reverse()
        {
            INode tempNode = StartNode;
            StartNode = EndNode;
            EndNode = tempNode;
        }
    }
}
