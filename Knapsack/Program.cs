using System;

namespace Knapsack
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            //Data
            int[] weights =
                { 3, 1, 6, 10, 1, 4, 9, 1, 7, 2, 6, 1, 6, 2, 2, 4, 8, 1, 7, 3, 6, 2, 9, 5, 3, 3, 4, 7, 3, 5, 30, 50 };
            int[] values =
            {
                7, 4, 9, 18, 9, 15, 4, 2, 6, 13, 18, 12, 12, 16, 19, 19, 10, 16, 14, 3, 14, 4, 15, 7, 5, 10, 10, 13, 19,
                9, 8, 5
            };
            var size = 32;
            var capacity = 75;
            // int[] weights = { 3, 1, 6 };
            // int[] values = { 7, 4, 9 };
            // int size = 3;
            // int capacity = 9;

            var knapsack = new Knapsack(weights, values, size, capacity);
            if (int.Parse(args[0]) == 1)
                knapsack.SolveBruteForce();
            else
                knapsack.SolveHillClimb(); // Best was value-251
            Console.WriteLine($"Best solution has value={knapsack.BestSolutionValue}, weight={knapsack.BestSolutionWeight}");
        }
    }
}