using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveDataManager : MonoBehaviour
{
    public GameAllCardsData gameAllCardsData;
    
    public CardObjectData GetCardObjectDataOf(CardObjectName name)
    {
        return gameAllCardsData.gameCardData[name];
    }
}
