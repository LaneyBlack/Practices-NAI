using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Perceptron
{
    internal class Program
    {
        private static int Dimensions { get; set; }

        public static void Main()
        {
            Console.Write("Please input file with train-set (csv format): ");
            var trainFilePath = Console.ReadLine();
            // Parsing training file
            var trainPoints = new List<IrisPoint>();
            foreach (var line in File.ReadLines(trainFilePath!))
            {
                var values = line.Split(',');
                var coords = new List<double>();
                for (var i = 0; i < values.Length - 1; i++)
                    coords.Add(double.Parse(values[i]));
                trainPoints.Add(new IrisPoint(coords.ToArray(), values[values.Length - 1] == "Iris-versicolor" ? 1 : 0));
            }
            //Shuffle a list
            Random rand = new Random();
            trainPoints = trainPoints.OrderBy(_ => rand.Next()).ToList();
            Dimensions = trainPoints.First().Coords.Length;
            // Ask for Learn Rate
            Console.Write("Please type in LearnRate: ");
            var perceptron = new Perceptron(double.Parse(Console.ReadLine()?.Trim()!), Dimensions);
            // Go through train set
            foreach (var trainPoint in trainPoints)
            {
                // Classification
                var answer = perceptron.Classification(trainPoint);
                if (answer == trainPoint.Class) continue; // if answer is true, then skip
                // TrainPoint class is the correct answer in this example
                for (var i = 0; i < Dimensions; i++)
                    perceptron.Weights[i] +=
                        perceptron.LearnRate * (trainPoint.Class - answer) * trainPoint.Coords[i]; // w' = w + a*(d-y)*x
                perceptron.Threshold += (trainPoint.Class - answer) * perceptron.LearnRate * (-1); // O' = O + a*(d-y)
                Console.WriteLine(perceptron);
            }

            double badResults = 0, allResults = 0;
            while (true)
            {
                Console.WriteLine("Please select one option: \t1)test-set;\n" +
                                  "\t2)my point;\n" +
                                  "\t3)leave.");
                var choice = int.Parse(Console.ReadLine()!.Trim());
                IEnumerable<string> lines = null;
                switch (choice)
                {
                    case 1:
                    {
                        Console.Write("Please type test-set file path: ");
                        lines = File.ReadLines(Console.ReadLine()!.Trim());
                        break;
                    }
                    case 2:
                    {
                        Console.Write("Dimension number is " + perceptron.Weights.Length + ": ");
                        lines = new[] { Console.ReadLine() };
                        break;
                    }
                    case 3:
                    {
                        Environment.Exit(1);
                        break;
                    }
                    default: continue;
                }

                foreach (var line in lines)
                {
                    //Creating point
                    var values = line.Split(',');
                    var coords = new List<double>();
                    for (var i = 0; i < values.Length - 1; i++)
                        coords.Add(double.Parse(values[i]));
                    var testPoint = new IrisPoint(coords.ToArray(),
                        values[values.Length - 1] == "Iris-versicolor" ? 1 : 0);
                    if (perceptron.Classification(testPoint) != testPoint.Class)
                    {
                        Console.WriteLine("Bad point " + testPoint);
                        badResults++;
                    }
                    allResults++;
                }
                Console.WriteLine(badResults / allResults * 100 + "% of good predicitions");
            }
        }
    }

    public class IrisPoint
    {
        public double[] Coords { get; set; }
        public int Class { get; set; }

        public IrisPoint(double[] coords, int @class)
        {
            Coords = coords;
            Class = @class;
        }

        public override string ToString()
        {
            return "Point[" + Coords[0] + ".." + Coords[Coords.Length - 1] + "," + Class + "]";
        }
    }
}