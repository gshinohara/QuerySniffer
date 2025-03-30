namespace QuerySniffer.Analysis.DAG
{
    public class Edge
    {
        public INode StartNode { get; }

        public INode EndNode { get; }

        public Edge(INode start, INode end)
        {
            StartNode = start;
            EndNode = end;
        }
    }
}
