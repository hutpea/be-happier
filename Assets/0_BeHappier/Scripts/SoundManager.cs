using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class SoundManager : SerializedMonoBehaviour
{
    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public Dictionary<GameAudioName, GameAudioAsset> gameAudioAssets;

    public AudioSource backgroundMusicSource;
    public AudioSource sfxSource;
    
    public void Play(GameAudioName audioName)
    {
        GameAudioAsset playedGameAudioAsset = gameAudioAssets[audioName];
        if (playedGameAudioAsset != null)
        {
            switch (playedGameAudioAsset.gameAudioType)
            {
                case GameAudioType.BackgroundMusic:
                {
                    backgroundMusicSource.Stop();
                    backgroundMusicSource.clip = playedGameAudioAsset.audioClip;
                    backgroundMusicSource.Play();
                    break;
                }
                case GameAudioType.SFX:
                {
                    sfxSource.PlayOneShot(playedGameAudioAsset.audioClip);
                    break;
                }
            }
        }
    }
}

[System.Serializable]
public class GameAudioAsset
{
    public GameAudioType gameAudioType;
    public AudioClip audioClip;
}

public enum GameAudioName
{
    MenuMusic,
    SadMusic1,
    SadMusic2,
    CardClick1,
    Keyboard1,
    FingerTap1
}

public enum GameAudioType
{
    BackgroundMusic = 0,
    SFX = 1
}
