using System.Collections.Generic;

namespace QuerySniffer.Analysis.DAG
{
    public static class NodeExplorer
    {
        public static HashSet<INode> GetAllLeftNodes(INode start)
        {
            var visited = new HashSet<INode>();
            var stack = new Stack<INode>();

            stack.Push(start);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                foreach (var left in current.GetLeftNodes())
                {
                    if (visited.Add(left))
                        stack.Push(left);
                }
            }

            return visited;
        }

        public static HashSet<INode> GetAllRightNodes(INode start)
        {
            var visited = new HashSet<INode>();
            var stack = new Stack<INode>();
            stack.Push(start);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                foreach (var right in current.GetRightNodes())
                {
                    if (visited.Add(right))
                    {
                        stack.Push(right);
                    }
                }
            }

            return visited;
        }
    }
}
