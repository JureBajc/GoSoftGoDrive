using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoSoftGoDrive;


namespace GosoftGoDrive
{
    public class Product
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; }

        public Product(int x, int y, string name)
        {
            X = x;
            Y = y;
            Name = name;
        }
    }
}