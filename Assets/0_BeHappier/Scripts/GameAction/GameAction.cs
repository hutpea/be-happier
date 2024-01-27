using Sirenix.OdinInspector;

[System.Serializable]
public class GameAction
{
    public ActionType actionType;

    [ShowIf("actionType", ActionType.SetBoolData)]
    public string boolDataString;
    [ShowIf("actionType", ActionType.SetBoolData)]
    public bool boolDataValue;
    [ShowIf("actionType", ActionType.SetIntData)]
    public string intDataString;
    [ShowIf("actionType", ActionType.SetIntData)]
    public int intDataValue;
    [ShowIf("actionType", ActionType.SetFloatData)]
    public string floatDataString;
    [ShowIf("actionType", ActionType.SetFloatData)]
    public float floatDataValue;
    [ShowIf("actionType", ActionType.SetStringData)]
    public string stringDataString;
    [ShowIf("actionType", ActionType.SetStringData)]
    public string stringDataValue;

    public float actionTime;
}
