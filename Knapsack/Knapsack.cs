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

        public void SolveBf()
        {
            // PossibleSolutions = new List<int>();
            // PossibleSolutions.Add(0);
            BestSolutionWeight = 0;
            BestSolutionValue = 0;
            for (long i = 1; i < ((long)1 << ObjectsNumber); i++) // i < equals Math.Pow(2,ObjectsNumber)
            {
                var possibleSolution = 0;
                for (var j = 0; j < ObjectsNumber; j++)
                {
                    // i goes through every possible option
                    // 1 10 11 100 101 111
                    // so if on place j there is a bit, then it's an added element to solution 
                    if ((i & (1 << j)) == 0) continue;

                    // Console.Write(ObjectWeights[j] + ",");
                    possibleSolution = possibleSolution | (1 << j);
                }

                // Count value and weight of this example
                var currentCharacteristics = GetSolutionWeightAndValue(possibleSolution);
                if (currentCharacteristics[1] <= MaxCapacity && currentCharacteristics[0] > BestSolutionValue)
                {
                    BestSolutionValue = currentCharacteristics[0];
                    BestSolutionWeight = currentCharacteristics[1];
                }
                // Console.WriteLine();
            }
        }

        public int[] GetSolutionWeightAndValue(long solution)
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
    }
}