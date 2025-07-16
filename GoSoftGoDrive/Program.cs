using GosoftGoDrive;
using System;
using System.Collections.Generic;

namespace GoSoftGoDrive
{
    class Program
    {
        static void Main(string[] args)
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
    }
}
