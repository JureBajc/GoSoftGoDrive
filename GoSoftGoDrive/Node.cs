using System;
using GoSoftGoDrive;


namespace GoSoftGoDrive
{
    public class Node
    {
        public int X, Y;
        public double G, H;
        public Node Parent;
        public double F => G + H;
        public Node(int x, int y) { X = x; Y = y; }
        public override bool Equals(object obj) =>
            obj is Node node && X == node.X && Y == node.Y;
        public override int GetHashCode() => HashCode.Combine(X, Y);
    }
}