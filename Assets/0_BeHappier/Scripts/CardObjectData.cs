using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "CardObjectData_", menuName = "Data/CardObjectData")]
public class CardObjectData : SerializedScriptableObject
{
    public Dictionary<string, Sprite> stateDataDictionary;

    public Sprite GetSpriteOfState(string state)
    {
        return stateDataDictionary[state];
    }
}