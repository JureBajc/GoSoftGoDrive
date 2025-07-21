using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoSoftGoDrive
{
    internal class Product3D
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public string Name { get; set; }

        public Product3D(int x, int y, int z, string name)
        {
            X = x;
            Y = y;
            Z = z;
            Name = name;
        }
    }
}
