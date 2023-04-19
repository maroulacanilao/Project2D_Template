using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[System.Serializable]
public class WeightedDictionary<T>
{
    [SerializeField] private SerializedDictionary<T, float> itemDictionary;
    private Dictionary<T, float> fixedChances;

    private System.Random rand;

    private bool hasInitialized = false;
    private float totalWeight;

    private T itemWithLargestWeight;

    public void Initialize()
    {
        if (hasInitialized) return;
        
        ForceInitialize();
    }

    public void ForceInitialize()
    {
        rand = new System.Random();
        fixedChances = new Dictionary<T, float>();
        RecalculateChances();

        hasInitialized = true;
    }
    
    public void RecalculateChances()
    {
        if (fixedChances == null) fixedChances = new Dictionary<T, float>();
        else fixedChances.Clear();

        float _largestWeight = 0;
        foreach (var w in itemDictionary)
        {
            if (w.Value > _largestWeight)
            {
                _largestWeight = w.Value;
                itemWithLargestWeight = w.Key;
            }

            totalWeight += w.Value;
            fixedChances.Add(w.Key, totalWeight);
        }
    }

    public T GetWeightedRandom()
    {
        // init class if it hasn't already
        Initialize();

        // check if the dictionary size the item list count is the same. recalculate chances if it is not 
        if (fixedChances.Count != itemDictionary.Count) RecalculateChances();

        // early returns
        if (fixedChances.Count == 0) return default;

        // random number
        float rngVal = ((float)rand.NextDouble() * totalWeight);

        foreach (var w in fixedChances)
        {
            if (w.Value > rngVal)
            {
                return w.Key;
            }
        }

        // fallback option, returns item with largest weight. this should not happen tho
        return itemWithLargestWeight;
    }

    public void AddItem(T itemToAdd, float weight)
    {
        itemDictionary.Add(itemToAdd, weight);
        
        totalWeight += weight;
        
        fixedChances.Add(itemToAdd, totalWeight);
    }

    public void RemoveItem(T itemToRemove)
    {
        if(!itemDictionary.Remove(itemToRemove)) return;
        if(!fixedChances.Remove(itemToRemove)) return;
        
        RecalculateChances();
    }

    public float GetChanceOfItem(T itemToEvaluate)
    {
        return itemDictionary[itemToEvaluate];
    }

    public void ChangeWeightOfItem(T itemToChange, float newWeight)
    {
        itemDictionary[itemToChange] = newWeight;
        RecalculateChances();
    }
}
