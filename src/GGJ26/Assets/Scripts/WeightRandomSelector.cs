using UnityEngine;

public static class WeightRandomSelector
{
    public static int ChooseRandomFromWeightedValues(int[] weights)
    {
        int totalWeights = 0;
        foreach (var weight in weights)
        {
            totalWeights += weight;
        }

        int randomValue = Random.Range(0, totalWeights);
        int cumulativeWeight = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            cumulativeWeight += weights[i];
            if (randomValue < cumulativeWeight)
            {
                return i;
            }
        }

        return weights.Length - 1;
    }
}
