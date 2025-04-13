using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuerySniffer.Analysis.DAG
{
    public class Graph
    {
        private enum Direction
        {
            Left,
            Right,
        }

        private readonly List<INode> m_Nodes = new List<INode>();
        private readonly List<Edge> m_Edges = new List<Edge>();


        public IEnumerable<INode> Nodes => m_Nodes.AsEnumerable();

        public IEnumerable<Edge> Edges => m_Edges.AsEnumerable();

        private Graph()
        {
        }

        public static Graph Create(IGH_DocumentObject start)
        {
            INode startNode = start.GetTopLevelNode();

            Graph left = new Graph();
            left.SearchNodes(start.GetTopLevelNode(), Direction.Left);

            Graph right = new Graph();
            right.SearchNodes(start.GetTopLevelNode(), Direction.Right);

            Graph graph = new Graph();
            graph.m_Nodes.AddRange(left.Nodes);
            graph.m_Edges.AddRange(left.Edges);
            graph.m_Nodes.AddRange(right.Nodes);
            graph.m_Edges.AddRange(right.Edges);
            graph.m_Nodes.Add(startNode);

            startNode.Depth = 0;

            graph.m_Nodes.Sort((a, b) => (int)(a.Depth - b.Depth));

            return graph;
        }

        private void SearchNodes(INode start, Direction direction = Direction.Left)
        {
            start.Depth = 0;

            var visited = new HashSet<INode>();
            var stack = new Stack<INode>();

            stack.Push(start);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                IEnumerable<INode> neighbors;
                switch (direction)
                {
                    case Direction.Left:
                        neighbors = current.GetLeftNodes();
                        break;
                    case Direction.Right:
                        neighbors = current.GetRightNodes();
                        break;
                    default:
                        throw new Exception("Direction is not defined");
                }

                foreach (INode neighbor in neighbors)
                {
                    INode currentNeighbor;
                    if (visited.Add(neighbor))
                    {
                        stack.Push(neighbor);
                        currentNeighbor = neighbor;
                    }
                    else
                    {
                        currentNeighbor = visited.FirstOrDefault(n => n.Equals(neighbor));
                    }

                    m_Edges.Add(new Edge(current, currentNeighbor));

                    var neighborSiblings = m_Edges
                        .Where(e => e.EndNode.Equals(currentNeighbor))
                        .Select(e => e.StartNode);

                    int? maxDepth = neighborSiblings.Max(n => n.Depth);
                    int neighborDepth = (currentNeighbor.Depth == null) ? (int)maxDepth + 1 : Math.Max((int)currentNeighbor.Depth, (int)maxDepth + 1);
                    currentNeighbor.Depth = neighborDepth;
                }
            }

            m_Nodes.AddRange(visited);

            if (direction == Direction.Left)
            {
                foreach (INode node in m_Nodes)
                    node.Depth *= -1;
                foreach (Edge edge in m_Edges)
                    edge.Reverse();
            }
        }
    }
}
