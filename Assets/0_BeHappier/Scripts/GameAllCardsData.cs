using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "GameAllCardsData", menuName = "Data/GameAllCardsData")]
public class GameAllCardsData : SerializedScriptableObject
{
    public Dictionary<CardObjectName, CardObjectData> gameCardData;

}
