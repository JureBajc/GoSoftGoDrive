using GoSoftGoDrive;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GosoftGoDrive
{
    public class MapRenderer
    {
        public void PrintPathSummary(Node start, List<Node> path, WarehouseMap warehouse)
        {
            var goals = warehouse.GetGoalNames();
            var pozicijeCiljev = new HashSet<(int, int)>(warehouse.GetGoalNodes().Select(t => (t.X, t.Y)));
            if (path.Count == 0)
            {
                Console.WriteLine("Ni poti do vseh izdelkov.");
                return;
            }
            var obiski = new Dictionary<(int, int), int>();
            int ix = 1;
            foreach (var n in path)
                if (pozicijeCiljev.Contains((n.X, n.Y)) && !obiski.ContainsKey((n.X, n.Y)))
                    obiski[(n.X, n.Y)] = ix++;

            Node zadnji = start;
            int idx = 1;
            Console.WriteLine($"Start: ({start.X}, {start.Y})\n");

            foreach (var n in path)
            {
                if (goals.ContainsKey((n.X, n.Y)))
                {
                    Console.WriteLine($"{idx++}. {goals[(n.X, n.Y)]} ({n.X}, {n.Y})");
                    Console.WriteLine($"   +{Math.Abs(n.X - zadnji.X) + Math.Abs(n.Y - zadnji.Y)} korakov");
                    zadnji = n;
                }
            }
        }

        public void DrawMap(WarehouseMap warehouse, List<Node> path)
        {
            int width = warehouse.Width;
            int height = warehouse.Height;
            int[,] grid = warehouse.Grid;
            var start = warehouse.StartNode;
            var goals = warehouse.GetGoalNames();
            var pozicijeCiljev = new HashSet<(int, int)>(warehouse.GetGoalNodes().Select(t => (t.X, t.Y)));

            bool[,] potArr = new bool[width, height];
            var obiski = new Dictionary<(int, int), int>();
            int ix = 1;
            foreach (var n in path)
                if (pozicijeCiljev.Contains((n.X, n.Y)) && !obiski.ContainsKey((n.X, n.Y)))
                    obiski[(n.X, n.Y)] = ix++;
            foreach (var n in path)
                if (n.X >= 0 && n.X < width && n.Y >= 0 && n.Y < height)
                    potArr[n.X, n.Y] = true;

            Console.WriteLine("\n=== Skladišče ===");
            Console.Write("     ");
            for (int x = 0; x < width; x++) Console.Write($"{x % 10} ".PadLeft(2));
            Console.WriteLine();

            for (int y = 0; y < height; y++)
            {
                Console.Write($"{y:D2}: ");
                for (int x = 0; x < width; x++)
                {
                    string simbol;
                    var barva = Console.ForegroundColor;

                    if (x == start.X && y == start.Y)
                    {
                        simbol = " S";
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else if (potArr[x, y] && obiski.ContainsKey((x, y)))
                    {
                        simbol = obiski[(x, y)].ToString("D2");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    else if (potArr[x, y])
                    {
                        simbol = " *";
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    else if (grid[x, y] == 1)
                    {
                        simbol = " .";
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (grid[x, y] == 5)
                    {
                        simbol = " #";
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }
                    else if (grid[x, y] == 9)
                    {
                        simbol = " X";
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        simbol = " ?";
                        Console.ForegroundColor = ConsoleColor.Magenta;
                    }
                    Console.Write($"{simbol} ");
                    Console.ForegroundColor = barva;
                }
                Console.WriteLine();
            }
        }

        public void PrintLegend(int pathCount, int goalCount, double ms)
        {
            double potVMetr = pathCount * 3;
            double trajanjeVMin = potVMetr / 1.4 / 60;
            Console.WriteLine("\nLEGENDA:");
            Console.WriteLine("S = začetek");
            Console.WriteLine("1,2,3,... = vrstni red izdelkov");
            Console.WriteLine("* = pot");
            Console.WriteLine(". = cesta");
            Console.WriteLine("# = produkt");
            Console.WriteLine("X = blokada");
            Console.WriteLine($"\nšt. izdelkov: {goalCount}");
            Console.WriteLine($"skupaj poti: {pathCount} korakov");
            Console.WriteLine($"čas računanja: {ms:F2}ms");
            Console.WriteLine($"Pribl. čas poti: {trajanjeVMin:F1} min");
        }
    }
}
