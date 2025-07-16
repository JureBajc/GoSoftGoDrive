using System;
using System.Collections.Generic;
using System.Linq;
using GoSoftGoDrive;

namespace GosoftGoDrive
{
    public class AStar2D
    {
        private int[,] grid;
        private int maxX, maxY;
        public AStar2D(int[,] g) { grid = g; maxX = g.GetLength(0); maxY = g.GetLength(1); }
        private double GetMovementCost(int cellValue)
        {
            switch (cellValue)
            {
                case 1: return 1.0;
                case 5: return 2.0;
                case 9: return double.MaxValue;
                default: return 1.0;
            }
        }
        public List<Node> FindPath(Node start, Node goal)
        {
            var open = new List<Node>();
            var closed = new HashSet<(int, int)>();
            var allNodes = new Dictionary<(int, int), Node>();

            Node get(int x, int y)
            {
                var k = (x, y);
                if (!allNodes.TryGetValue(k, out var n))
                {
                    n = new Node(x, y);
                    n.G = double.MaxValue;
                    n.H = 0;
                    n.Parent = null;
                    allNodes[k] = n;
                }
                return n;
            }

            var s = get(start.X, start.Y);
            var kGoal = get(goal.X, goal.Y);

            s.G = 0;
            s.H = Math.Abs(s.X - kGoal.X) + Math.Abs(s.Y - kGoal.Y);
            s.Parent = null;
            open.Add(s);

            int iter = 0;
            while (open.Any() && iter < 10000)
            {
                iter++;
                var curr = open.OrderBy(n => n.F).ThenBy(n => n.H).First();
                open.Remove(curr);

                if (curr.X == kGoal.X && curr.Y == kGoal.Y)
                    return ReconstructPath(curr);

                closed.Add((curr.X, curr.Y));

                foreach (var (dx, dy) in new[] { (1, 0), (-1, 0), (0, 1), (0, -1) })
                {
                    int nx = curr.X + dx, ny = curr.Y + dy;
                    if (nx < 0 || ny < 0 || nx >= maxX || ny >= maxY)
                        continue;
                    if (closed.Contains((nx, ny)))
                        continue;
                    int cell = grid[nx, ny];
                    if (cell == 9)
                        continue;
                    if (cell == 5 && !(nx == kGoal.X && ny == kGoal.Y))
                        continue;
                    var nei = get(nx, ny);

                    double movementCost = GetMovementCost(cell);
                    double tG = curr.G + movementCost;

                    if (tG < nei.G)
                    {
                        nei.Parent = curr;
                        nei.G = tG;
                        nei.H = Math.Abs(nei.X - kGoal.X) + Math.Abs(nei.Y - kGoal.Y);

                        if (!open.Contains(nei))
                            open.Add(nei);
                    }
                }
            }
            return new List<Node>();
        }

        public List<Node> OptSekvencaBacktrack(Node start, List<Node> goals)
        {
            var current = start;
            var rezultat = new List<Node>();
            var preostali = new List<Node>(goals);

            while (preostali.Any())
            {
                var best = preostali.OrderBy(g =>
                {
                    var pot = FindPath(current, g);
                    return pot != null && pot.Count > 0 ? pot.Count : int.MaxValue;
                }).First();

                var potDo = FindPath(current, best);
                if (potDo != null && potDo.Count > 1)
                {
                    rezultat.AddRange(potDo.Skip(1));
                    if (potDo.Count > 1)
                    {
                        var backtrackPosition = potDo[potDo.Count - 2];
                        rezultat.Add(backtrackPosition);
                        current = backtrackPosition;
                    }
                    else
                    {
                        current = best;
                    }
                }
                else
                {
                    current = best;
                }

                preostali.Remove(best);
            }
            return rezultat;
        }

        public List<Node> OptSekvencaKoraki(Node start, List<Node> goals, Dictionary<(int, int), string> imenaCiljev)
        {
            var current = start;
            var rezultat = new List<Node>();
            var preostali = new List<Node>(goals);
            int korak = 1;

            Console.WriteLine("\n=== PODROBNI PRIKAZ ALGORITMSKIH KORAKOV ===");
            Console.WriteLine($"Začetna pozicija: ({current.X}, {current.Y})");
            Console.WriteLine($"Skupaj ciljev: {preostali.Count}\n");

            while (preostali.Any())
            {
                Console.WriteLine($"--- KORAK {korak} ---");
                Console.WriteLine($"Trenutna pozicija: ({current.X}, {current.Y})");
                Console.WriteLine($"Preostali cilji: {preostali.Count}");

                var razdalje = new List<(Node cilj, int razdalja, List<Node> pot)>();
                foreach (var cilj in preostali)
                {
                    var pot = FindPath(current, cilj);
                    int razdalja = pot != null && pot.Count > 0 ? pot.Count - 1 : int.MaxValue;
                    razdalje.Add((cilj, razdalja, pot));
                }

                Console.WriteLine("Razdalje do preostalih ciljev:");
                foreach (var (cilj, razdalja, pot) in razdalje.OrderBy(x => x.razdalja))
                {
                    string naziv = imenaCiljev.ContainsKey((cilj.X, cilj.Y)) ? imenaCiljev[(cilj.X, cilj.Y)] : "Neznano";
                    if (razdalja == int.MaxValue)
                        Console.WriteLine($"  - {naziv} ({cilj.X}, {cilj.Y}): NEDOSTOPNO");
                    else
                        Console.WriteLine($"  - {naziv} ({cilj.X}, {cilj.Y}): {razdalja} korakov");
                }

                var best = razdalje.OrderBy(x => x.razdalja).First();
                Console.WriteLine($"Izbran cilj: {imenaCiljev[(best.cilj.X, best.cilj.Y)]} ({best.cilj.X}, {best.cilj.Y}) - {best.razdalja} korakov");

                if (best.pot != null && best.pot.Count > 1)
                {
                    Console.WriteLine("Pot do cilja:");
                    for (int i = 0; i < best.pot.Count; i++)
                    {
                        var node = best.pot[i];
                        if (i == 0)
                            Console.WriteLine($"  {i + 1}. ({node.X}, {node.Y}) - START");
                        else if (i == best.pot.Count - 1)
                            Console.WriteLine($"  {i + 1}. ({node.X}, {node.Y}) - CILJ (produkt)");
                        else
                            Console.WriteLine($"  {i + 1}. ({node.X}, {node.Y})");
                    }
                    rezultat.AddRange(best.pot.Skip(1));
                    Console.WriteLine($"POBIRANJE: Delavec pobere produkt na ({best.cilj.X}, {best.cilj.Y})");

                    if (best.pot.Count > 1)
                    {
                        var backtrackPosition = best.pot[best.pot.Count - 2];
                        Console.WriteLine($"VRNITEV: Delavec se vrne na zadnjo pozicijo v poti ({backtrackPosition.X}, {backtrackPosition.Y})");
                        rezultat.Add(backtrackPosition);
                        current = backtrackPosition;
                    }
                    else
                    {
                        Console.WriteLine("OPOZORILO: Pot je prekratka za vrnitev");
                        current = best.cilj;
                    }
                }
                else
                {
                    Console.WriteLine("OPOZORILO: Ni poti do cilja");
                    current = best.cilj;
                }

                preostali.Remove(best.cilj);

                Console.WriteLine($"Nova pozicija: ({current.X}, {current.Y})");
                Console.WriteLine($"Skupaj korakov do sedaj: {rezultat.Count}");
                Console.WriteLine();

                korak++;
            }

            Console.WriteLine("=== ALGORITEM KONČAN ===");
            Console.WriteLine($"Skupaj korakov: {rezultat.Count}");
            Console.WriteLine($"Obiščenih ciljev: {goals.Count}");
            Console.WriteLine();
            return rezultat;
        }

        private List<Node> ReconstructPath(Node koncni)
        {
            var path = new List<Node>();
            for (var curr = koncni; curr != null; curr = curr.Parent)
                path.Add(curr);
            path.Reverse();
            return path;
        }
    }
}