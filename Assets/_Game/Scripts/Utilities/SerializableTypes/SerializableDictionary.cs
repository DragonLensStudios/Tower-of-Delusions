using System.Collections.Generic;
using UnityEngine;

namespace DLS.Game.Utilities
{
    [System.Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {

        [SerializeField] private List<TKey> keys = new List<TKey>();
        [SerializeField] private List<TValue> values = new List<TValue>();

        // save the dictionary to lists
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach (KeyValuePair<TKey, TValue> pair in this) 
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        // load the dictionary from lists
        public void OnAfterDeserialize()
        {
            SyncDictionaryFromLists();
        }

        public void SyncDictionaryFromLists()
        {
            Clear();

            if (keys.Count != values.Count) 
            {
                Debug.LogError("Tried to deserialize a SerializableDictionary, but the amount of keys ("
                               + keys.Count + ") does not match the number of values (" + values.Count 
                               + ") which indicates that something went wrong");
                return;
            }

            for (int i = 0; i < keys.Count; i++) 
            {
                if(!TryAdd(keys[i], values[i]))
                {
                    Debug.LogWarning($"Unable to add {keys[i].ToString()} with the value of {values[i].ToString()}");
                }
            }
        }
    }
}