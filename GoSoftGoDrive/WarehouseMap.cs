using GosoftGoDrive;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoSoftGoDrive
{
    public class WarehouseMap
    {
        public int[,] Grid { get; private set; }
        public List<Product> Products { get; private set; }
        public Node StartNode { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public void InitializeDefault()
        {
            string[] mapa = {
                ". . . . . . . . . . . . .",
                ". . X . X . X . X . X . .",
                ". X # . # . # . # . # X .",
                ". X # . # . # . # . # X .",
                ". X # . # . # . # . # X .",
                ". X # . # . # . # . # X .",
                ". X # . # . # . # . # X .",
                ". X # . # . # . # . # X .",
                ". . X . X . X . X . X . .",
                ". . . . . . . . . . . . .",
                ". . . . . . . . . . . . .",
                ". X # # # # # # # # # X .",
                ". X # # # # # # # # # X .",
                ". . . . . . . . . . . . .",
                ". X # # # # # # # # # X .",
                ". X # # # # # # # # # X .",
                ". . . . . . . . . . . . .",
                ". . . . . . . . . . . . .",
                ". X # # # # # # # # # X .",
                ". X # # # # # # # # # X .",
                ". . . . . . . . . . . . .",
                ". . . . . . . . . . . . ."
            };

            Height = mapa.Length;
            Width = mapa[0].Split(' ').Length;
            Grid = new int[Width, Height];

            for (int y = 0; y < Height; y++)
            {
                var vrstica = mapa[y].Split(' ');
                for (int x = 0; x < Width; x++)
                {
                    Grid[x, y] = vrstica[x] switch
                    {
                        "#" => 5,
                        "X" => 9,
                        _ => 1
                    };
                }
            }

            Products = new List<Product>
            {
                new Product(9, 19, "Laptop"),
                new Product(10, 4, "Bluetooth Speaker"),
                new Product(6, 3, "Wireless Mouse"),
                new Product(2, 6, "HDMI Cable"),
                new Product(10, 11, "Barcode Scanner"),
                new Product(4, 18, "Winter Jacket"),
                new Product(5, 11, "Notebook Set"),
                new Product(3, 12, "Smartphone Case"),
                new Product(7, 11, "Steel Water Bottle"),
                new Product(8, 3, "LED Desk Lamp"),
                new Product(6, 7, "Office Chair"),
                new Product(9, 12, "Cardboard Box Set"),
                new Product(2, 5, "Noise Cancelling Headphones"),
                new Product(6, 19, "Laptop Stand"),
                new Product(2, 3, "Backpack"),
                new Product(9, 18, "Shoe Box - Size 42"),
                new Product(6, 15, "Wireless Charger"),
                new Product(4, 5, "USB Power Bank"),
                new Product(3, 14, "Yoga Mat"),
                new Product(10, 5, "Helmet"),
                new Product(8, 11, "First Aid Kit"),
                new Product(8, 5, "Portable Heater")
            };

            StartNode = new Node(0, 0);
        }

        public List<Node> GetGoalNodes() => Products.Select(p => new Node(p.X, p.Y)).ToList();

        public Dictionary<(int, int), string> GetGoalNames() =>
            Products.ToDictionary(p => (p.X, p.Y), p => p.Name);

        public void PrintMapInfo()
        {
            Console.WriteLine($"velikost: {Width} x {Height}");
            Console.WriteLine($"izdelki: {Products.Count}");
            Console.WriteLine($"start: ({StartNode.X}, {StartNode.Y})\n");
        }
    }
}