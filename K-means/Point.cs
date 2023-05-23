using System;
using System.Collections.Generic;
using Enumerable = System.Linq.Enumerable;

namespace K_means;

public class Point
{
    public double[] Coordinates { get; set; }
    public string Group { get; set; }

    public Point(string[] values)
    {
        Coordinates = new double[values.Length - 1];
        for (int i = 0; i < values.Length - 1; i++)
            Coordinates[i] = double.Parse(values[i]);
        Group = values[values.Length - 1];
    }

    public Point(double[] coordinates, string group)
    {
        Coordinates = coordinates;
        Group = group;
    }

    public double GetDistance(Point point)
    {
        if (Coordinates.Length != point.Coordinates.Length)
            throw new ArgumentException("Point dimensions are different");
        var result = 0.0;
        for (var i = 0; i < Coordinates.Length; i++)
            result += Math.Pow(Coordinates[i] - point.Coordinates[i], 2);
        result = Math.Sqrt(result);
        return result;
    }

    public override string ToString()
    {
        if (Coordinates.Length != 0)
            return "Point{[" + Coordinates[0] + ".." + Coordinates[Coordinates.Length - 1] + "]" + Group + "}";
        return "Point{[]" + Group + "}";
    }
}