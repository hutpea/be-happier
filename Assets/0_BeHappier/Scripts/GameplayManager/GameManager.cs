using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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

    [Title("Act nodes")]
    public GameNode act0PrologueNode;
    public GameNode act1FirstNode;
    public GameNode mematQuakhuNode;
    public GameNode act1PastNode;
    public GameNode act1SecondNode;
    public GameNode act3FirstNode;
    public GameNode act4;
    public GameNode endNode;
    
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
        cameraManager.SwitchToCameraMain();
        yield return new WaitForSeconds(1.75F);
        nodeManager.ExecuteInitializeNode();
    }

    [Button]
    public void SetUpAct1First()
    {
        nodeManager.initializeNode = act1FirstNode;
    }

    [Button]
    public void SetUpMeMatQuaKhu()
    {
        nodeManager.initializeNode = mematQuakhuNode;
    }
    
    [Button]
    public void SetUpAct1Second()
    {
        nodeManager.initializeNode = act1SecondNode;
    }
    
    [Button]
    public void SetUpAct3FirstNode()
    {
        nodeManager.initializeNode = act3FirstNode;
    }
    
    [Button]
    public void SetUpAct4Node()
    {
        nodeManager.initializeNode = act4;
    }
    
    [Button]
    public void SetUpEndActNode()
    {
        nodeManager.initializeNode = endNode;
    }
}
