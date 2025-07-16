using GosoftGoDrive;
using GoSoftGoDrive3D.Algorithms;
using System;
using System.Collections.Generic;

namespace GoSoftGoDrive
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test2D();
            Test3D(); 

        }
        static void Test2D()
        {
            var warehouse = new WarehouseMap();
            warehouse.InitializeDefault();

            Console.WriteLine("=== GoDrive Sistem Poti ===\n");
            warehouse.PrintMapInfo();

            var pathfinder = new AStar2D(warehouse.Grid);
            var renderer = new MapRenderer();

            string choice = UserInput.GetAlgorithmChoice();
            var start = warehouse.StartNode;
            var goals = warehouse.GetGoalNodes();

            List<Node> path;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            if (choice == "2")
                path = pathfinder.OptSekvencaKoraki(start, goals, warehouse.GetGoalNames());
            else if (choice == "1")
                path = pathfinder.OptSekvencaBacktrack(start, goals);
            else
                path = pathfinder.FindPath(start, goals[0]);

            timer.Stop();

            if (path.Count == 0)
            {
                Console.WriteLine("ni poti do vseh izdelkov.");
                return;
            }

            renderer.PrintPathSummary(start, path, warehouse);
            renderer.DrawMap(warehouse, path);
            renderer.PrintLegend(path.Count, goals.Count, timer.ElapsedMilliseconds);
        }
        static void Test3D()
        {
            var graph = new Graph3D();
            var start = new Node3D(0, 0, 0);
            var mid = new Node3D(2, 1, 3);
            var goal = new Node3D(5, 5, 5);

            graph.AddNode(start);
            graph.AddNode(mid);
            graph.AddNode(goal);

            graph.AddEdge(start, mid, weight: 4.2);
            graph.AddEdge(mid, goal, weight: 5.1);

            var astar = new AStar3D(graph);
            var path = astar.FindPath(start, goal);

            if (path.Count == 0)
            {
                Console.WriteLine("No path found.");
            }
            else
            {
                Console.WriteLine("Path found:");
                foreach (var node in path)
                {
                    Console.WriteLine($"  ({node.X}, {node.Y}, {node.Z})  [G={node.G:F2}  H={node.H:F2}  F={node.F:F2}]");
                }
            }
        }
    }
}