using System;
using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class TextEffectManager : MonoBehaviour
{
    public TextMeshProUGUI targetText;
    public Coroutine textEffectCoroutine;

    public float typingSpeed = 5;

    private void Awake()
    {
    }

    public void StopMyCoroutine()
    {
        StopCoroutine(textEffectCoroutine);
    }

    public void Display(string content)
    {
        targetText.SetText(content);
    }
    
    [Button]
    public void Typing(string content)
    {
        textEffectCoroutine = StartCoroutine(TypingCoroutine(content));
    }

    private IEnumerator TypingCoroutine(string content)
    {
        GameManager.Instance.boardManager.ToggleInteractionAllCardViews(false);
        targetText.SetText("");
        int contentCount = content.Length;
        int i = 0;
        string tempString = "";
        SoundManager.Instance.Play(GameAudioName.Keyboard1);
        while (i < contentCount)
        {
            char charAtI = content[i];
            if (charAtI.CompareTo('*') == 0)
            {
                i++;
                yield return new WaitForSeconds(0.75F);
                continue;
            }
            tempString += charAtI;
            targetText.SetText(tempString);
            i++;
            yield return new WaitForSeconds(1F / typingSpeed);
        }
        GameManager.Instance.boardManager.ToggleInteractionAllCardViews(true);
    }
}
