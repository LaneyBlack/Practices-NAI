using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Knn
{
    internal class Program
    {
        private static double _wrongTests = 0;

        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Correct input example:\n" +
                                  "project.exe train-set");
                throw new ArgumentException("Incorrect argument input!");
            }
            Console.Write("Please type in the kNN variable: ");
            var kNN = int.Parse(Console.ReadLine()?.Trim() ?? throw new InvalidOperationException());
            var fLines = File.ReadLines(args[0]); // File lines
            var trainValues = new HashSet<Point>(); // Creating HashSet of Training points. They must be unique
            foreach (var line in fLines) // Parsing the training file
            {
                var values = line.Split(',');
                var coords = new List<double>();
                for (var i = 0; i < values.Length - 1; i++)
                    coords.Add(double.Parse(values[i]));
                trainValues.Add(new Point(coords, values[values.Length - 1]));
            }
            
            var allTests = 0;
            var working = true;
            while (working) // Interaction with user
            {
                Console.WriteLine("Press what do you want to do (type a number):\n" +
                                  "\t(1)test test-set file\n" +
                                  "\t(2)test your point\n" +
                                  "\t(3)exit");
                var answer = int.Parse(Console.ReadLine()?.Trim() ?? throw new InvalidOperationException());
                switch (answer)
                {
                    case 1:
                    {
                        // C:\\PJATK\\4th\\NAI\\NAI-P1\\NAI-P1\\Data\\iris.test.data
                        Console.Write("Please put path to file: ");
                        fLines = File.ReadLines(Console.ReadLine()?.Trim() ?? throw new InvalidOperationException());
                        break;
                    }
                    case 2:
                    {
                        Console.Write("Please put your Point using this example:\n" +
                                          "\t[7.3,...,1.8,GroupName]\n" +
                                          "\tDimensions are " + trainValues.First().Coordinates.Count + ": ");
                        fLines = new[] { Console.ReadLine() };
                        break;
                    }
                    case 3:
                    {
                        working = false;
                        break;
                    }
                }

                if (!working) continue; // skip if i am not working
                foreach (var line in fLines) // going through lines in test file/console input
                {
                    var values = line.Split(',');
                    var coords = new List<double>(); // list of counted coordinates
                    for (var i = 0; i < values.Length - 1; i++)
                        coords.Add(double.Parse(values[i]));
                    var currentPoint = new Point(coords, values[values.Length - 1]); // build current point
                    CountAnswer(currentPoint, trainValues, kNN); // count the answer
                    allTests++;
                }

                Console.WriteLine("Percent of good tests is:\t" +
                                  Math.Round((1 - _wrongTests / allTests) * 10000) / 100 + "%");
                Console.WriteLine("-----------------------------------------------------------------------");
            }
        }

        private static double GetDistance(Point p1, Point p2)
        {
            double result = 0;
            if (p1.Coordinates.Count != p2.Coordinates.Count)
                throw new ArgumentException("Points with different dimensions!");

            for (var i = 0; i < p1.Coordinates.Count; i++)
                result += Math.Pow(p1.Coordinates.ElementAt(i) - p2.Coordinates.ElementAt(i), 2);

            result = Math.Sqrt(result);
            return result;
        }

        private static void CountAnswer(Point currentPoint, HashSet<Point> trainValues, int kNN)
        {
            var distList = new List<Tuple<double, Point>>(); //list of distances
            foreach (var trainPoint in trainValues) // count distances to point
            {
                var distance = GetDistance(currentPoint, trainPoint); //distance to the current point
                distList.Add(new Tuple<double, Point>(distance, trainPoint));
            }

            distList = distList.OrderBy(x => x.Item1).ToList(); //sorting distances by lenght short..long
            var counter = new Dictionary<string, int>(); //dictionary of Groups
            for (var i = 0; i < kNN; i++) // count groups of knn nearest points
            {
                var minDistance = distList.First().Item2;
                if (counter.ContainsKey(minDistance.Group))
                    counter[minDistance.Group]++;
                else
                    counter.Add(minDistance.Group, 1);
                distList.Remove(distList.First());
            }

            counter = counter.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            if (currentPoint.Group != counter.Last().Key) // get the biggest Group
            {
                Console.WriteLine(currentPoint + " must be " + counter.Last().Key);
                currentPoint.Group = counter.Last().Key;
                _wrongTests++;
            }

            // if (kNN > 3 && counter.Last().Value / kNN > 0.75) // assurance > 75% => add point into the training collection
                // trainValues.Add(currentPoint);
        }
    }

    internal class Point
    {
        public List<double> Coordinates { get; set; }
        public string Group { get; set; }

        public Point(List<double> coordinates, string group)
        {
            Coordinates = coordinates;
            Group = group;
        }

        public override string ToString()
        {
            if (Coordinates.Count != 0)
                return "Point{[" + Coordinates[0] + ".." + Coordinates[Coordinates.Count - 1] + "]" + Group + "}";
            return "Point{[]" + Group +"}";
    }
    }
}