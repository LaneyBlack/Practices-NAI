using System;
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
            Weights[i] = random.NextDouble() * 10 - 5;
        Threshold = random.NextDouble() * 10 - 5;
        // Threshold = 0;
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

    public override string ToString()
    {
        return "Perceptron " + Weights[0] + "..." + Weights[Weights.Length - 1] + " - " + Threshold;
    }
}