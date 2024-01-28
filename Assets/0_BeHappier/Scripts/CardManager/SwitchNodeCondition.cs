using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class SwitchNodeCondition
{
    public ConditionType conditionType;
    [ShowIf("@this.conditionType == ConditionType.IF || this.conditionType == ConditionType.IF_NOT")]
    [SerializeReference] public DataTypeCondition dataTypeCondition;
    [ShowIf("conditionType", ConditionType.TIME_OUT)]
    public float elapsedDurationForTimeout;
    public GameNode switchToNode;
}