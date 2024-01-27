using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    public CardObjectName currentCardObjectName;
    public string currentState;
    public CardObjectData currentCardObjectData;
    
    public Image cardImage;
    public TextEffectManager textEffectManager;

    private RectTransform cardRect;
    private bool canBeSelect = true;
    
    private void Awake()
    {
        cardRect = GetComponent<RectTransform>();
    }

    public void SetupCard(CardObjectName cardObjectName, string startState, string initContent = null)
    {
        this.currentCardObjectName = cardObjectName;
        this.currentState = startState;
        this.currentCardObjectData = GameManager.Instance.liveDataManager.GetCardObjectDataOf(cardObjectName);
        SetCardState();
        if (initContent != null)
        {
            textEffectManager.Display(initContent);
        }
        else
        {
            textEffectManager.Display("");
        }
    }

    public void SetCardState()
    {
        Sprite stateSprite = currentCardObjectData.GetSpriteOfState(this.currentState);
        cardImage.sprite = stateSprite;
    }

    public void OnMouseEnterEvent()
    {
        if (!canBeSelect) return;
        SoundManager.Instance.Play(GameAudioName.FingerTap1);
        cardRect.DOMoveZ(0.33F, 0.15F).SetEase(Ease.Linear).OnStart(() =>
        {
            canBeSelect = false;
        }).OnComplete(() =>
        {
            canBeSelect = true;
        });
    }
    
    public void OnMouseExitEvent()
    {
        if (!canBeSelect) return;
        cardRect.DOMoveZ(0F, 0.15F).SetEase(Ease.Linear).OnStart(() =>
        {
            canBeSelect = false;
        }).OnComplete(() =>
        {
            canBeSelect = true;
        });
    }
}
