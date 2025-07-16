using GoSoftGoDrive;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoSoftGoDrive3D.Algorithms
{
    public class AStar3D : IPathfinder<Node3D>
    {
        private readonly IGraph<Node3D> _graph;
        private const int MaxIterations = 100_000;

        public AStar3D(IGraph<Node3D> graph)
        {
            _graph = graph;
        }

        private double Heuristic(Node3D a, Node3D b)
            => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z);

        public List<Node3D> FindPath(Node3D start, Node3D goal)
        {
            // Unique instance map
            var all = new Dictionary<Node3D, Node3D>();
            Node3D Get(Node3D n)
            {
                if (!all.TryGetValue(n, out var ex))
                {
                    ex = new Node3D(n.X, n.Y, n.Z);
                    all[n] = ex;
                }
                return ex;
            }

            var s = Get(start);
            var g = Get(goal);
            s.G = 0;
            s.H = Heuristic(s, g);
            s.Parent = null;

            var open = new List<Node3D> { s };
            var closed = new HashSet<Node3D>();
            int it = 0;

            while (open.Count > 0 && it++ < MaxIterations)
            {
                var current = open.OrderBy(n => n.F).ThenBy(n => n.H).First();
                open.Remove(current);

                if (current.Equals(g))
                    return Reconstruct(current);

                closed.Add(current);

                foreach (var (nbr, w) in _graph.GetNeighbors(current))
                {
                    var node = Get(nbr);
                    if (closed.Contains(node)) continue;

                    var tg = current.G + w;
                    if (tg < node.G)
                    {
                        node.Parent = current;
                        node.G = tg;
                        node.H = Heuristic(node, g);
                        if (!open.Contains(node)) open.Add(node);
                    }
                }
            }

            return new List<Node3D>();
        }

        private List<Node3D> Reconstruct(Node3D end)
        {
            var path = new List<Node3D>();
            for (var n = end; n != null; n = n.Parent)
                path.Add(n);
            path.Reverse();
            return path;
        }
    }
}