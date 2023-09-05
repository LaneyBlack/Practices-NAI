using System;
using System.Collections.Generic;

namespace Knapsack
{
    public class Knapsack
    {
        public int[] ObjectWeights { get; set; }
        public int[] ObjectValues { get; set; }

        public int ObjectsNumber { get; set; }

        public int MaxCapacity { get; set; }

        // public List<int> PossibleSolutions { get; set; }
        public int BestSolutionValue { get; set; }
        public int BestSolutionWeight { get; set; }

        public Knapsack(int[] objectWeights, int[] objectValues, int objectsNumber, int maxCapacity)
        {
            ObjectWeights = objectWeights;
            ObjectValues = objectValues;
            ObjectsNumber = objectsNumber;
            MaxCapacity = maxCapacity;
            // PossibleSequences = new int[(int)Math.Pow(2,objectsNumber)];
        }

        public void SolveBruteForce()
        {
            BestSolutionWeight = 0;
            BestSolutionValue = 0;
            int[] currentCharacteristics = new int[2];
            for (long i = 1; i < (long)(1 << ObjectsNumber); i++) // i < equals Math.Pow(2,ObjectsNumber)
            {
                long possibleSolution = 0;
                var currentWeight = 0;
                for (var j = 0; j < ObjectsNumber; j++)
                {
                    // i goes through every possible option
                    // 1 10 11 100 101 111
                    // so if on place j there is a bit, then it's an added element to solution 
                    if ((i & (1 << j)) == 0) continue;

                    // Console.Write(ObjectWeights[j] + ",");
                    possibleSolution = possibleSolution | (1 << j);
                    currentWeight += ObjectWeights[j];
                    if (currentWeight > MaxCapacity)
                        break;
                }

                // Count value and weight of this example
                currentCharacteristics = GetSolutionValueAndWeight(possibleSolution);
                if (currentCharacteristics[1] <= MaxCapacity && currentCharacteristics[0] > BestSolutionValue)
                {
                    BestSolutionValue = currentCharacteristics[0];
                    BestSolutionWeight = currentCharacteristics[1];
                }
            }
        }

        public void SolveHillClimb()
        {
            // Generate Random suitable solution
            long solution = GenerateRandomSolution();
            // Do Hillclimbing
            List<long> neighbours;
            while (true)
            {
                // Set best with current solution
                var bestNeighbour = solution;
                var tmp = GetSolutionValueAndWeight(solution);
                BestSolutionValue = tmp[0];
                BestSolutionWeight = tmp[1];
                // Generate Neighbours
                neighbours = GenerateNeighbours(solution);
                // Go through every neighbour and check if it's better
                foreach (var neighbour in neighbours)
                {
                    tmp = GetSolutionValueAndWeight(neighbour);
                    if (tmp[1] > MaxCapacity || tmp[0] <= BestSolutionValue) continue; // if this solution is not better
                    bestNeighbour = neighbour;
                    BestSolutionValue = tmp[0];
                    BestSolutionWeight = tmp[1];
                }

                if (bestNeighbour == solution)
                    break;
                solution = bestNeighbour;
            }
        }

        public int[] GetSolutionValueAndWeight(long solution)
        {
            var result = new int[2];
            result[0] = 0; // value
            result[1] = 0; // weight
            for (int i = 0; i < 32; i++) // every bit
            {
                if (((solution & (1 << i)) >> i) == 1)
                {
                    result[0] += ObjectValues[i];
                    result[1] += ObjectWeights[i];
                }
            }

            return result;
        }


        public List<long> GenerateNeighbours(long currentSolution)
        {
            List<long> neighbours = new List<long>();
            for (int i = 0; i < ObjectsNumber; i++)
            {
                long neighbour = currentSolution ^ (1L << i); // Revert the i-th bit
                neighbours.Add(neighbour);
            }
            return neighbours;
        }

        public long GenerateRandomSolution()
        {
            long result = 0;
            int resultWeight = 0;
            var random = new Random();
            for (int i = 0; i < ObjectsNumber; i++)
            {
                if (random.Next(2) == 0)
                {
                    if (resultWeight + ObjectWeights[i] <= MaxCapacity)
                    {
                        result |= (1L << i); // Set the i-th bit to 1
                        resultWeight += ObjectWeights[i];
                    }
                }
            }

            return result;
        }
    }
}