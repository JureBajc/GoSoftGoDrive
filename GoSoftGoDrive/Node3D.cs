using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoSoftGoDrive;
namespace GoSoftGoDrive
{
    public class Node3D:INode<Node3D>
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }
        public double G { get; set; }
        public double H { get; set; }
        public Node3D Parent { get; set; }
        public double F => G + H;

        public Node3D(int x, int y, int z)
        {
            X = x; Y = y; Z = z;
            G = double.MaxValue;
            H = 0;
            Parent = null;
        }

        public override bool Equals(object obj) =>
            obj is Node3D o && X == o.X && Y == o.Y && Z == o.Z;
        public override int GetHashCode() => HashCode.Combine(X, Y, Z);
    }
}