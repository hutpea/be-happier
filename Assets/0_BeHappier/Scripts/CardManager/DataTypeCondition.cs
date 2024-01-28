[System.Serializable]
public class DataTypeCondition
{
    public string dataName;
    public CompareType compareType;
}

public enum CompareType
{
    EQUAL,
    NOT_EQUAL,
    LESS_THAN,
    LESS_OR_EQUAL_THAN,
    MORE_THAN,
    MORE_OR_EQUAL_THAN
}
