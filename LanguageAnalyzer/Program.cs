using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LanguageAnalyzer
{
    internal class Program
    {
        private const int EpoksNumber = 100000;

        public static void Main()
        {
            Console.Write("Please input file with train-set (csv format): ");
            var trainFilePath = Console.ReadLine();
            // Parsing training file
            var trainTexts = new List<LanguageText>();
            var languageOptions = new Dictionary<string, int>();
            // Run through 
            foreach (var line in File.ReadLines(trainFilePath!))
            {
                var values = line.Split(new[] { "," }, 2, StringSplitOptions.None);
                if (!languageOptions.ContainsKey(values[0]))
                    languageOptions.Add(values[0], languageOptions.Count + 1);
                trainTexts.Add(new LanguageText(values[1], values[0]));
            }

            // Ask for Learn Rate
            Console.Write("Please type in LearnRate: ");
            var learnRate = double.Parse(Console.ReadLine()?.Trim()!);
            var perceptronList = new List<Perceptron>();
            foreach (var languageOption in languageOptions)
                perceptronList.Add(new Perceptron(learnRate, languageOption.Key, 26));
            // Go through train set
            for (var repeat = 0; repeat < EpoksNumber; repeat++)
                foreach (var trainText in trainTexts)
                {
                    foreach (var perceptron in perceptronList)
                    {
                        // Classification
                        var classification = perceptron.Classification(trainText);
                        var @class = perceptron.Language == trainText.Language ? 1 : 0;
                        // if answer is true, then skip
                        if ((perceptron.Language != trainText.Language && classification == 0) ||
                            (perceptron.Language == trainText.Language && classification == 1))
                            continue;
                        // TrainPoint class is the correct answer in this example
                        for (var i = 0; i < perceptron.Weights.Length; i++)
                            perceptron.Weights[i] += perceptron.LearnRate * (@class - classification) * trainText.Letters[i]; // w' = w + a*(d-y)*x
                        perceptron.Threshold -= (@class - classification) * perceptron.LearnRate; // O' = O - a*(d-y)
                    }
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
                        Console.Write("Please input file with test-set: ");
                        lines = File.ReadLines(Console.ReadLine()!.Trim());
                        break;
                    }
                    case 2:
                    {
                        Console.Write("Dimension number is " + perceptronList.First().Weights.Length + ": ");
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
                    bool isRight = true;
                    //Creating point
                    var values = line.Split(',');
                    var testText = new LanguageText(values[1], values[0]);
                    foreach (var perceptron in perceptronList)
                    {
                        var answer = perceptron.Classification(testText);
                        if ((perceptron.Language == testText.Language && answer == 0) ||
                            (perceptron.Language != testText.Language && answer == 1))
                        {
                            Console.WriteLine("Bad classification for text " + testText);
                            isRight = false;
                        }
                    }

                    if (isRight==false)
                        badResults++;
                    allResults++;
                }

                Console.WriteLine((1 - badResults / allResults) * 100 + "% of good predictions");
                // Console.WriteLine(badResults / allResults * 100 + "% of bad predictions");
            }
        }
    }
}