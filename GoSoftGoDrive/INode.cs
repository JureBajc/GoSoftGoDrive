using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoSoftGoDrive;


namespace GoSoftGoDrive
{
    public interface INode<T> where T : INode<T>
    {
        double G { get; set; }
        double H { get; set; }
        T Parent { get; set; }
        double F { get; }
    }
    public interface IGraph<TNode>
    {
        void AddNode(TNode node);
        void AddEdge(TNode from, TNode to, double weight = 1.0, bool bidirectional = true);
        IEnumerable<(TNode neighbor, double weight)> GetNeighbors(TNode node);
    }
    public interface IPathfinder<TNode>
    {
        List<TNode> FindPath(TNode start, TNode goal);
    }
}