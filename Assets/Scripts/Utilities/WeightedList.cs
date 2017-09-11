using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeightedList<T> : ISerializationCallbackReceiver {

    [SerializeField]
    public List<T> items = new List<T>();
    [SerializeField]
    public List<int> weights = new List<int>();

    public Dictionary<T, int> dictionary = new Dictionary<T, int>();

    public T GetRandomItem() {
        // https://stackoverflow.com/questions/1761626/weighted-random-numbers

        int totalWeight = dictionary.Sum(e => e.Value);
        int random = Random.Range(0, totalWeight);

        foreach (var kvp in dictionary) {
            if (random < kvp.Value) {
                return kvp.Key;
            }
            random -= kvp.Value;
        }

        Debug.LogError("Oops!");
        return dictionary.FirstOrDefault().Key;
    }

    public void OnBeforeSerialize() {
        items.Clear();
        weights.Clear();

        items.AddRange(dictionary.Keys);
        weights.AddRange(dictionary.Values);
    }

    public void OnAfterDeserialize() {
        dictionary.Clear();

        for (int i = 0; i < Mathf.Min(items.Count, weights.Count); i++) {
            dictionary.Add(items[i], weights[i]);
        }
    }
}

[System.Serializable]
public class WeightedLaneType : WeightedList<LanesManager.LaneTile.Type> { };
[System.Serializable]
public class WeightedInt : WeightedList<int> { };
