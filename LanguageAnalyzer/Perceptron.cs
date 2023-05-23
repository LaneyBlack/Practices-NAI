using System;
using System.Linq;
using LanguageAnalyzer;

namespace LanguageAnalyzer;

public class Perceptron
{
    public double[] Weights { get; set; }
    public double Threshold { get; set; }
    public double LearnRate { get; set; }
    public string Language { get; set; }

    public Perceptron(double learnRate, string language, int dimensions)
    {
        LearnRate = learnRate;
        Language = language;
        var random = new Random();
        Weights = new double[dimensions];
        for (var i = 0; i < dimensions; i++)
            // Weights[i] = random.NextDouble() * 10 - 5;
            Weights[i] = 0;
        // Threshold = random.NextDouble() * 5;
        // --------------------------- U can always modify Weights and Threshold ---------------------------
        Threshold = 1;
    }

    public int Classification(LanguageText lang)
    {
        if (lang.Letters.Length != Weights.Length)
            throw new IndexOutOfRangeException("Perceptron and point are declared in different dimensions!");
        double net = 0;
        for (var i = 0; i < Weights.Length; i++)
            net += lang.Letters[i] * Weights[i];
        return net >= Threshold ? 1 : 0;
    }

    public double Activation(LanguageText lang)
    {
        if (lang.Letters.Length != Weights.Length)
            throw new IndexOutOfRangeException("Perceptron and point are declared in different dimensions!");
        double net = 0;
        for (var i = 0; i < Weights.Length; i++)
            net += lang.Letters[i] * Weights[i];
        return net - Threshold;
    }

    public void Train(LanguageText trainText)
    {
        // Classification
        var answer = Classification(trainText);
        var actualClass = Language == trainText.Language ? 1 : 0;
        // if answer is true, then skip
        if (answer == actualClass)
            return;
        // TrainPoint class is the correct answer in this example
        for (var i = 0; i < Weights.Length; i++)
            Weights[i] += LearnRate * (actualClass - answer) * trainText.Letters[i]; // w' = w + a*(d-y)*x
        Threshold -= (actualClass - answer) * LearnRate; // O' = O - a*(d-y)
    }

    public void Normalise()
    {
        var norm = Weights.Sum(weight => Math.Pow(weight, 2));
        norm = Math.Sqrt(norm);
        for (var i = 0; i < Weights.Length; i++)
            Weights[i] /= norm;
        Threshold /= norm;
    }

    public override string ToString()
    {
        return "Perceptron" + Language + "  [" + Math.Round(Weights[0],3) + "..." + Math.Round(Weights[Weights.Length - 1]) + "] - " + Threshold;
    }
}