using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameControlManager gameControlManager;
    public NodeManager nodeManager;
    public BoardManager boardManager;
    public CameraManager cameraManager;
    public GameVolumeManager gameVolumeManager;
    public PrefabManager prefabManager;
    public GameUIManager gameUIManager;
    public LiveDataManager liveDataManager;
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

    private void Start()
    {
        StartCoroutine(StartGameCoroutine());
    }

    IEnumerator StartGameCoroutine()
    {
        SoundManager.Instance.Play(GameAudioName.SadMusic1);
        cameraManager.SwitchToCameraMain();
        yield return new WaitForSeconds(2.5F);
        nodeManager.ExecuteInitializeNode();
    }
}
