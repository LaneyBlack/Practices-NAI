using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;

namespace LanguageAnalyzer
{
    internal class Program
    {
        private const int EpoksNumber = 1_000;

        public static void Main()
        {
            // --------------------------- Hand input ---------------------------
            // Console.Write("Please input file with train-set (csv format): ");
            // var trainFilePath = Console.ReadLine();
            var trainFilePath = "C:\\PJATK\\4th\\NAI\\CwiczeniaNAI\\LanguageAnalyzer\\Data\\lang.train.csv";
            // Parsing training file
            var trainTexts = new List<LanguageText>();
            var languageOptions = new HashSet<string>();
            // Run through 
            foreach (var line in File.ReadLines(trainFilePath!))
            {
                var values = line.Split(new[] { "," }, 2, StringSplitOptions.None);
                if (!languageOptions.Contains(values[0]))
                    languageOptions.Add(values[0]);
                trainTexts.Add(new LanguageText(values[1], values[0]));
            }

            // Ask for Learn Rate
            Console.Write("Please type in LearnRate: ");
            var learnRate = double.Parse(Console.ReadLine()?.Trim()!);
            var perceptronList = new List<Perceptron>();
            foreach (var languageOption in languageOptions)
                perceptronList.Add(new Perceptron(learnRate, languageOption, 26));
            // Go through train set
            for (var repeat = 0; repeat < EpoksNumber; repeat++)
            {
                foreach (var trainText in trainTexts)
                {
                    foreach (var perceptron in perceptronList)
                    {
                        perceptron.Train(trainText);
                    }
                }
            }

            //Normalise all perceptrons
            foreach (var perceptron in perceptronList)
            {
                Console.WriteLine(perceptron);
                perceptron.Normalise();
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
                        // --------------------------- Hand input ---------------------------
                        // Console.Write("Please input file with test-set: ");
                        // lines = File.ReadLines(Console.ReadLine()!.Trim());
                        lines = File.ReadLines("C:\\PJATK\\4th\\NAI\\CwiczeniaNAI\\LanguageAnalyzer\\Data\\lang.test.csv");
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
                    testText.Normalise();
                    var maxActivationPerceptron = new Tuple<double, Perceptron>(
                        perceptronList.First().Activation(testText),
                        perceptronList.First()
                        );
                    // Aby klasyfikować język tekstu, wybieramy perceptron z maksymalną aktywacją.
                    foreach (var perceptron in perceptronList)
                    {
                        var curActivation = perceptron.Activation(testText);
                        if (maxActivationPerceptron.Item1 < curActivation)
                        {
                            maxActivationPerceptron = new Tuple<double, Perceptron>(curActivation, perceptron);
                        }
                    }
                    
                    var answer = maxActivationPerceptron.Item2.Classification(testText);
                    var actualAnswer = maxActivationPerceptron.Item2.Language == testText.Language ? 1 : 0;
                    if (actualAnswer!=answer)
                    {
                        Console.WriteLine($"Bad classification for text {testText} using {maxActivationPerceptron.Item2}");
                        badResults++;
                    }
                    allResults++;
                }

                Console.WriteLine((1 - badResults / allResults) * 100 + "% of good predictions");
                // Console.WriteLine(badResults / allResults * 100 + "% of bad predictions");
            }
        }
    }
}