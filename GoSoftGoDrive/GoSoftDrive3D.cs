/*using System;
using System.Collections.Generic;
using System.Linq;
namespace GoSoftGoDrive
{
    public class GoSoftDrive3D
    {
	    public GoSoftDrive3D()
	    {
                Console.WriteLine("=== GoDrive Sistem Poti 3D ===\n");

                // ASCII mapa skladišča za vsak nivo Z (tukaj podvajamo iz 2D primera)
                string[] baseMap = {
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
                // Primer: 2 nivoja
                var asciiMaps = new List<string[]> { baseMap, baseMap };

                int sizeZ = asciiMaps.Count;
                int sizeY = baseMap.Length;
                int sizeX = baseMap[0].Split(' ').Length;

                // Inicializacija 3D mreže
                int[,,] grid = new int[sizeX, sizeY, sizeZ];
                for (int z = 0; z < sizeZ; z++)
                {
                    for (int y = 0; y < sizeY; y++)
                    {
                        var row = asciiMaps[z][y].Split(' ');
                        for (int x = 0; x < sizeX; x++)
                        {
                            grid[x, y, z] = row[x] switch
                            {
                                "#" => 5,
                                "X" => 9,
                                _ => 1
                            };
                        }
                    }
                }

                // Seznam produktov z X, Y, Z in imenom
                var izdelki = new List<Product>
                {
                    new Product(9, 19, 0, "Laptop"),
                    new Product(9, 19, 1, "Laptop Backup"), // isti X,Y, različen nivo
                    new Product(10, 4, 0, "Bluetooth Speaker"),
                    new Product(6, 3, 0, "Wireless Mouse"),
                    // ... dodaj poljubno število produktov na različnih nivojih ...
                };

                var start = new Node(0, 0, 0);
                var targeti = izdelki.Select(p => new Node(p.X, p.Y, p.Z)).ToList();

                // Skupine imen produktov po lokaciji (X,Y,Z)
                var imenaCiljev = izdelki
                    .GroupBy(p => (p.X, p.Y, p.Z))
                    .ToDictionary(g => g.Key, g => g.Select(p => p.Name).ToList());

                var pathfinder = new AStar3D(grid);

                // Najdemo pot do vseh produktov zaporedoma (preprosta heurstika: najbližji cilj)
                var pot = pathfinder.OptSequenceBacktrack(start, targeti);
                if (pot.Count == 0)
                {
                    Console.WriteLine("Ni poti do vseh izdelkov.");
                    return;
                }

                // Izpis rezultatov
                Console.WriteLine($"Start: ({start.X},{start.Y},{start.Z})\n");
                var visited = new HashSet<(int, int, int)>();
                int order = 1;
                foreach (var node in pot)
                {
                    var key = (node.X, node.Y, node.Z);
                    if (imenaCiljev.ContainsKey(key) && !visited.Contains(key))
                    {
                        Console.WriteLine($"{order++}. Lokacija ({node.X},{node.Y},{node.Z}):");
                        foreach (var name in imenaCiljev[key])
                            Console.WriteLine($"   - {name}");
                        visited.Add(key);
                    }
                }

                // Prikaz poti po nivojih
                for (int z = 0; z < sizeZ; z++)
                {
                    Console.WriteLine($"\n=== Nivo Z={z} ===");
                    Console.Write("     ");
                    for (int x = 0; x < sizeX; x++) Console.Write($"{x % 10} ".PadLeft(2));
                    Console.WriteLine();
                    for (int y = 0; y < sizeY; y++)
                    {
                        Console.Write($"{y:D2}: ");
                        for (int x = 0; x < sizeX; x++)
                        {
                            char sym = grid[x, y, z] switch
                            {
                                1 => '.',
                                5 => '#',
                                9 => 'X',
                                _ => '?'
                            };
                            Console.Write($" {sym} ");
                        }
                        Console.WriteLine();
                    }
                }
            }
        }

        public class Product
        {
            public int X, Y, Z;
            public string Name;
            public Product(int x, int y, int z, string name)
            {
                X = x; Y = y; Z = z; Name = name;
            }
        }

        public class Node
        {
            public int X, Y, Z;
            public double G, H;
            public Node Parent;
            public double F => G + H;
            public Node(int x, int y, int z) { X = x; Y = y; Z = z; }
            public override bool Equals(object obj) =>
                obj is Node n && n.X == X && n.Y == Y && n.Z == Z;
            public override int GetHashCode() => HashCode.Combine(X, Y, Z);
        }

        public class AStar3D
        {
            private int[,,] grid;
            private int maxX, maxY, maxZ;
            private (int dx, int dy, int dz)[] dirs = new[]
            {
                (1,0,0),(-1,0,0),(0,1,0),(0,-1,0),(0,0,1),(0,0,-1)
            };

            public AStar3D(int[,,] grid)
            {
                this.grid = grid;
                maxX = grid.GetLength(0);
                maxY = grid.GetLength(1);
                maxZ = grid.GetLength(2);
            }

            private double Heuristic(Node a, Node b) =>
                Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z);

            private double GetMovementCost(int cell) => cell switch
            {
                1 => 1.0,
                5 => 2.0,
                _ => double.MaxValue
            };

            public List<Node> FindPath(Node start, Node goal)
            {
                var open = new List<Node>();
                var closed = new HashSet<(int, int, int)>();
                var all = new Dictionary<(int, int, int), Node>();
                Node Get(int x, int y, int z)
                {
                    var key = (x, y, z);
                    if (!all.TryGetValue(key, out var n))
                    {
                        n = new Node(x, y, z) { G = double.MaxValue };
                        all[key] = n;
                    }
                    return n;
                }

                var s = Get(start.X, start.Y, start.Z);
                var g = Get(goal.X, goal.Y, goal.Z);
                s.G = 0;
                s.H = Heuristic(s, g);
                open.Add(s);

                while (open.Any())
                {
                    var curr = open.OrderBy(n => n.F).ThenBy(n => n.H).First();
                    open.Remove(curr);
                    if (curr.Equals(g)) return Reconstruct(curr);

                    closed.Add((curr.X, curr.Y, curr.Z));
                    foreach (var (dx, dy, dz) in dirs)
                    {
                        int nx = curr.X + dx, ny = curr.Y + dy, nz = curr.Z + dz;
                        if (nx < 0 || ny < 0 || nz < 0 || nx >= maxX || ny >= maxY || nz >= maxZ) continue;
                        if (closed.Contains((nx, ny, nz))) continue;
                        int cell = grid[nx, ny, nz];
                        if (cell == 9) continue;

                        var nei = Get(nx, ny, nz);
                        double cost = curr.G + GetMovementCost(cell);
                        if (cost < nei.G)
                        {
                            nei.G = cost;
                            nei.H = Heuristic(nei, g);
                            nei.Parent = curr;
                            if (!open.Contains(nei)) open.Add(nei);
                        }
                    }
                }
                return new List<Node>();
            }

            public List<Node> OptSequenceBacktrack(Node start, List<Node> goals)
            {
                var current = start;
                var result = new List<Node> { start };
                var remaining = new List<Node>(goals);

                while (remaining.Any())
                {
                    Node best = null;
                    List<Node> bestPath = null;
                    int bestDist = int.MaxValue;
                    foreach (var g in remaining)
                    {
                        var path = FindPath(current, g);
                        int dist = path.Count > 0 ? path.Count : int.MaxValue;
                        if (dist < bestDist)
                        {
                            bestDist = dist;
                            best = g;
                            bestPath = path;
                        }
                    }
                    if (bestPath == null || bestPath.Count == 0) break;
                    // dodamo pot (razen prve točke)
                    result.AddRange(bestPath.Skip(1));
                    current = best;
                    remaining.Remove(best);
                }
                return result;
            }

            private List<Node> Reconstruct(Node end)
            {
                var path = new List<Node>();
                for (var n = end; n != null; n = n.Parent)
                    path.Add(n);
                path.Reverse();
                return path;
            }
        }
    }
*/