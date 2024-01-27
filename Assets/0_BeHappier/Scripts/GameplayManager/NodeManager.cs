using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class NodeManager : SerializedMonoBehaviour
{
    public GameNode currentNode;

    public void ExecuteNode(GameNode targetNode)
    {
        StartCoroutine(ExecuteNodeCoroutine(targetNode));
    }

    private IEnumerator ExecuteNodeCoroutine(GameNode targetNode)
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
                break;
            }
        }
        //Phase 3: Before end action
        
        //Phase 4: Check conditions
        Debug.Log("End node " + targetNode.nodeID);
    }

    public void ExecuteCurrentNode()
    {
        ExecuteNode(currentNode);
    }

    public void ExecuteGameAction(GameAction gameAction)
    {
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
        }
    }
}
