using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class NodeManager : SerializedMonoBehaviour
{
    public GameNode initializeNode;
    
    public GameNode currentNode;

    public List<SwitchNodeCondition> currentSwitchConditionList = new List<SwitchNodeCondition>();
    [SerializeField] private bool conditionEnd = false;
    [SerializeField] private float timeSinceNodeStartExecuted = 0F;
    [SerializeField] private bool enableTimerRun = false;
    [SerializeField] private float checkElapsedTime = 0.5F;

    [HideInInspector]
    public UnityEvent endExecuteNoteStartEvent;

    public Coroutine CheckConditionCoroutine;

    private void Awake()
    {
        endExecuteNoteStartEvent.AddListener(OnEndExecuteNoteStart);
    }

    public void ExecuteNode(GameNode targetNode)
    {
        currentNode = targetNode;
        currentNode.isEndExecuteNoteStart = false;
        StartCoroutine(ExecuteNodeStartCoroutine(targetNode));
    }

    private IEnumerator ExecuteNodeStartCoroutine(GameNode targetNode)
    {
        Debug.Log("Execute node " + targetNode.nodeID);
        //Phase 1: After load actions
        foreach (var gameAction in targetNode.nodeStartActionList)
        {
            ExecuteGameAction(gameAction);
            yield return new WaitForSeconds(gameAction.actionTime);
        }
        //Phase 2: Node action
        switch (targetNode)
        {
            case AddCardNode node:
            {
                GameManager.Instance.boardManager.AddNewCardToSlot(node.cardObjectName, node.cardInitState, node.initCardContent, node.slotID);
                break;
            }
            case ChoiceNode node:
            {
                GameManager.Instance.gameUIManager.SetupChoice(node.choiceItemDataList);
                break;
            }
            case DeliverContentNode node:
            {
                GameManager.Instance.boardManager.DeliverContentToCardObject(node.cardObjectName, node.dialogueItemList);
                break;
            }
            case DeliverNarativeNode node:
            {
                GameManager.Instance.gameUIManager.SetNarrativeContentList(node.contentList);
                break;
            }
        }
    }

    private void OnEndExecuteNoteStart()
    {
        Debug.Log(currentNode.nodeID + " OnEndExecuteNoteStart()");
        if (currentNode == null)
        {
            Debug.LogError("There is no currentNode available to handle!");
            return;
        }

        if (!currentNode.isEndExecuteNoteStart)
        {
            currentNode.isEndExecuteNoteStart = true;
            CheckConditionCoroutine = StartCoroutine(CheckConditionIEnumrator());
        }
    }

    private IEnumerator CheckConditionIEnumrator()
    {
        Debug.Log($"Start Check node {currentNode.nodeID} conditions");
        currentSwitchConditionList.Clear();
        foreach (var switchCondition in currentNode.switchNodeConditions)
        {
            currentSwitchConditionList.Add(switchCondition);
        }
        conditionEnd = false;
        timeSinceNodeStartExecuted = 0F;
        enableTimerRun = true;

        SwitchNodeCondition selectedCondition = null;

        float checkTime = checkElapsedTime;
        
        while (conditionEnd == false)
        {
            timeSinceNodeStartExecuted += Time.deltaTime;

            //Debug.Log("start while check " + currentNode.nodeID);
            GameManager.Instance.gameUIManager.checkConditionNodeDebugTxt.SetText("Check condition node " + currentNode.nodeID);
            foreach (SwitchNodeCondition condition in currentSwitchConditionList)
            {
                if (!conditionEnd)
                {
                    bool check = CheckCondition(condition);
                    //Debug.Log("check condition " + condition.conditionType + " check=" + check);
                    if (check)
                    {
                        //Debug.Log(condition.conditionType + " pass checking, will switch to node " + condition.switchToNode.nodeID);
                        conditionEnd = true;
                        selectedCondition = condition;
                    }
                }
            }
            yield return null;
            //Debug.Log("after wait " + currentNode.nodeID);
        }
        FinishCheckCondition();
        StartCoroutine(ExecuteNodeEndCoroutine(currentNode, selectedCondition.switchToNode));
    }

    private void FinishCheckCondition()
    {
        enableTimerRun = false;
        timeSinceNodeStartExecuted = 0F;
        Debug.Log($"Finish Check node {currentNode.nodeID} conditions");
        conditionEnd = false;
    }

    private bool CheckCondition(SwitchNodeCondition switchNodeCondition)
    {
        bool result = false;
        switch (switchNodeCondition.conditionType)
        {
            case ConditionType.IF:
            {
                result = CheckDataTypeCondition(switchNodeCondition);
                break;
            }
            case ConditionType.IF_NOT:
            {
                result = !CheckDataTypeCondition(switchNodeCondition);
                break;
            }
            case ConditionType.TIME_OUT:
            {
                result = (timeSinceNodeStartExecuted > switchNodeCondition.elapsedDurationForTimeout);
                break;
            }
        }
        return result;
    }

    private bool CheckDataTypeCondition(SwitchNodeCondition switchNodeCondition)
    {
        if (switchNodeCondition.conditionType != ConditionType.IF &&
            switchNodeCondition.conditionType != ConditionType.IF_NOT)
        {
            return false;
        }

        bool result = false;

        switch (switchNodeCondition.dataTypeCondition)
        {
            case BooleanTypeCondition dataTypeCondition:
            {
                bool booleanData = GameData.GetBoolData(dataTypeCondition.dataName);
                result = CompareBooleanData(booleanData, dataTypeCondition.value, dataTypeCondition.compareType);
                break;
            }
            case IntegerTypeCondition dataTypeCondition:
            {
                int integerData = GameData.GetIntData(dataTypeCondition.dataName);
                result = CompareIntData(integerData, dataTypeCondition.value, dataTypeCondition.compareType);
                break;
            }
            case FloatTypeCondition dataTypeCondition:
            {
                float floatData = GameData.GetFloatData(dataTypeCondition.dataName);
                result = CompareFloatData(floatData, dataTypeCondition.value, dataTypeCondition.compareType);
                break;
            }
            case StringTypeCondition dataTypeCondition:
            {
                string stringData = GameData.GetStringData(dataTypeCondition.dataName);
                result = CompareStringData(stringData, dataTypeCondition.value, dataTypeCondition.compareType);
                break;
            }
        }

        return result;
    }

    private bool CompareBooleanData(bool dataValue, bool targetValue, CompareType compareType)
    {
        switch (compareType)
        {
            case CompareType.EQUAL:
            {
                return dataValue == targetValue;
            }
            case CompareType.NOT_EQUAL:
            {
                return dataValue != targetValue;
            }
            default:
            {
                return false;
            }
        }
    }
    
    private bool CompareIntData(int dataValue, int targetValue, CompareType compareType)
    {
        switch (compareType)
        {
            case CompareType.EQUAL:
            {
                return dataValue == targetValue;
            }
            case CompareType.NOT_EQUAL:
            {
                return dataValue != targetValue;
            }
            case CompareType.LESS_THAN:
            {
                return dataValue < targetValue;
            }
            case CompareType.LESS_OR_EQUAL_THAN:
            {
                return dataValue <= targetValue;
            }
            case CompareType.MORE_THAN:
            {
                return dataValue > targetValue;
            }
            case CompareType.MORE_OR_EQUAL_THAN:
            {
                return dataValue >= targetValue;
            }
            default:
            {
                return false;
            }
        }
    }
    
    private bool CompareFloatData(float dataValue, float targetValue, CompareType compareType)
    {
        switch (compareType)
        {
            case CompareType.EQUAL:
            {
                return dataValue == targetValue;
            }
            case CompareType.NOT_EQUAL:
            {
                return dataValue != targetValue;
            }
            case CompareType.LESS_THAN:
            {
                return dataValue < targetValue;
            }
            case CompareType.LESS_OR_EQUAL_THAN:
            {
                return dataValue <= targetValue;
            }
            case CompareType.MORE_THAN:
            {
                return dataValue > targetValue;
            }
            case CompareType.MORE_OR_EQUAL_THAN:
            {
                return dataValue >= targetValue;
            }
            default:
            {
                return false;
            }
        }
    }

    private bool CompareStringData(string dataValue, string targetValue, CompareType compareType)
    {
        switch (compareType)
        {
            case CompareType.EQUAL:
            {
                return String.Compare(dataValue, targetValue, StringComparison.Ordinal) == 0;
            }
            case CompareType.NOT_EQUAL:
            {
                return String.Compare(dataValue, targetValue, StringComparison.Ordinal) != 0;;
            }
            default:
            {
                return false;
            }
        }
    }
    
    private IEnumerator ExecuteNodeEndCoroutine(GameNode targetNode, GameNode switchToNode)
    {
        //Phase 3: Before end action
        foreach (var gameAction in targetNode.nodeEndActionList)
        {
            ExecuteGameAction(gameAction);
            yield return new WaitForSeconds(gameAction.actionTime);
        }
        //Phase 4: Switch node
        ExecuteNode(switchToNode);
    }

    public void ExecuteInitializeNode()
    {
        ExecuteNode(initializeNode);
    }

    public void ExecuteGameAction(GameAction gameAction)
    {
        Debug.Log("Execute action " + gameAction.actionType);
        switch (gameAction.actionType)
        {
            case ActionType.SetBoolData:
            {
                GameData.SetBoolData(gameAction.boolDataString, gameAction.boolDataValue);
                break;
            }
            case ActionType.SetIntData:
            {
                GameData.SetIntData(gameAction.intDataString, gameAction.intDataValue);
                break;
            }
            case ActionType.SetFloatData:
            {
                GameData.SetFloatData(gameAction.floatDataString, gameAction.floatDataValue);
                break;
            }
            case ActionType.SetStringData:
            {
                GameData.SetStringData(gameAction.stringDataString, gameAction.stringDataValue);
                break;
            }
            case ActionType.SetVolumeNormal:
            {
                GameManager.Instance.gameVolumeManager.ResetToNormalMode();
                break;
            }
            case ActionType.SetVolumePast:
            {
                GameManager.Instance.gameVolumeManager.ResetToBlackWhiteMode();
                break;
            }
            case ActionType.PlaySound:
            {
                SoundManager.Instance.Play(gameAction.audioName);
                break;
            }
            case ActionType.MoveCard:
            {
                GameManager.Instance.boardManager.MoveCard(gameAction.targetCardObjectName, gameAction.targetSlotID);
                break;
            }
            case ActionType.RemoveCard:
            {
                GameManager.Instance.boardManager.RemoveCard(gameAction.targetCardObjectName);
                break;
            }
            case ActionType.SetDarkBackground:
            {
                GameManager.Instance.gameUIManager.SetDarkBackground(gameAction.darkGroundDuration);
                break;
            }
        }
    }
}
