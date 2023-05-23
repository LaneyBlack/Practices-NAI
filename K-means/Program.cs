using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace K_means
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (args[0] == "-help")
            {
                Console.WriteLine("""
                    Execution definition:
                    program.exe filepath kNumber maxIterationNumber
                    """
                );
                return;
            }

            // Add Points
            var points = new List<Point>();
            var centroidGroups = new Dictionary<Point, List<Point>>();
            foreach (var line in File.ReadLines(args[0]))
            {
                var values = line.Split(',');
                points.Add(new Point(values));
            }

            // Init centroids
            Random random = new Random();
            for (int i = 0; i < int.Parse(args[1]); i++)
            {
                var index = random.Next(points.Count);
                var coords = new double[points[index].Coordinates.Length];
                for (int j = 0; j < points[index].Coordinates.Length; j++)
                    coords[j] = points[index].Coordinates[j];
                centroidGroups.Add(new Point(coords, "Centroid" + i), new List<Point>());
            }

            // Find Centroids
            bool stop = true;
            int iteration = 0, maxIteration = int.Parse(args[2]);
            while (stop && iteration < maxIteration)
            {
                Console.Write("Iteration " + iteration + ": ");
                stop = AssignToGroups(centroidGroups, points);
                MoveCentroids(centroidGroups);
                iteration++;
            }
            PrintGroups(centroidGroups);
        }

        private static void MoveCentroids(Dictionary<Point, List<Point>> centroidGroups)
        {
            foreach (var centroidGroup in centroidGroups)
            {
                centroidGroup.Key.Coordinates = new double[centroidGroup.Key.Coordinates.Length];
                foreach (var point in centroidGroup.Value)
                {
                    // Sum all points in group
                    for (int i = 0; i < point.Coordinates.Length; i++)
                    {
                        centroidGroup.Key.Coordinates[i] += point.Coordinates[i];
                    }
                }

                // Divide by number of point
                for (int i = 0; i < centroidGroup.Key.Coordinates.Length; i++)
                {
                    centroidGroup.Key.Coordinates[i] /= centroidGroup.Value.Count;
                }
            }
        }

        public static bool AssignToGroups(Dictionary<Point, List<Point>> centroidGroups, List<Point> data)
        {
            bool isChanged = false;
            double allDistance = 0;
            //For every point
            foreach (var point in data)
            {
                //Count distances
                var pointDistances = new double[centroidGroups.Keys.Count];
                int index = 0;
                foreach (var centroid in centroidGroups.Keys)
                {
                    pointDistances[index] = point.GetDistance(centroid);
                    index++;
                }

                //Get min distance
                var minIndex = 0;
                for (int i = 1; i < pointDistances.Length; i++)
                    if (pointDistances[minIndex] > pointDistances[i])
                        minIndex = i;
                allDistance += pointDistances[minIndex];
                // Add to Group
                index = 0;
                foreach (var centroidGroup in centroidGroups)
                {
                    // Remove if was in another group 
                    if (centroidGroup.Value.Contains(point) && index != minIndex)
                    {
                        centroidGroup.Value.Remove(point);
                        isChanged = true;
                    }

                    if (index == minIndex && !centroidGroup.Value.Contains(point))
                    {
                        centroidGroup.Value.Add(point);
                        isChanged = true;
                    }
                    
                    index++;
                }
            }
            Console.WriteLine(allDistance);
            return isChanged;
        }

        private static void PrintGroups(Dictionary<Point, List<Point>> centroidGroups)
        {
            foreach (var centroidGroup in centroidGroups)
            {
                Console.WriteLine($"----------------------{centroidGroup.Key}----------------------");
                foreach (var p in centroidGroup.Value)
                {
                    Console.WriteLine(p);
                }
            }
        }
    }
}