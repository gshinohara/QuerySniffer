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

        public static Graph CreateInLeft(IGH_DocumentObject start)
        {
            Graph graph = new Graph();
            graph.SearchNodes(start.GetTopLevelNode(), Direction.Left);
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
                    if (visited.Add(neighbor))
                        stack.Push(neighbor);
                    m_Edges.Add(new Edge(current, neighbor));

                    var connectedSources = m_Edges
                        .Where(e => e.EndNode == neighbor)
                        .Select(e => e.StartNode.Depth);

                    int? maxDepth = connectedSources.Any() ? connectedSources.Max() : current.Depth;
                    int neighborDepth = (neighbor.Depth == null) ? (int)maxDepth + 1 : Math.Max((int)neighbor.Depth, (int)maxDepth + 1);
                    neighbor.Depth = neighborDepth;
                }
            }

            m_Nodes.AddRange(visited);
        }
    }
}
