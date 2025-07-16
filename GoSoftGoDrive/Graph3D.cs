using System.Collections.Generic;
using System.Linq;
using GoSoftGoDrive;

namespace GoSoftGoDrive
{
    public class Graph3D : IGraph<Node3D>
    {
        private readonly Dictionary<Node3D, List<(Node3D neighbor, double weight)>> _adj =
            new Dictionary<Node3D, List<(Node3D, double)>>();

        public void AddNode(Node3D node)
        {
            if (!_adj.ContainsKey(node))
                _adj[node] = new List<(Node3D, double)>();
        }

        public void AddEdge(Node3D from, Node3D to, double weight = 1.0, bool bidirectional = true)
        {
            AddNode(from);
            AddNode(to);
            _adj[from].Add((to, weight));
            if (bidirectional)
                _adj[to].Add((from, weight));
        }

        public IEnumerable<(Node3D neighbor, double weight)> GetNeighbors(Node3D node)
        {
            return _adj.TryGetValue(node, out var list)
                ? list
                : Enumerable.Empty<(Node3D, double)>();
        }
    }
}
