using System;

[Serializable]
public class EventConditionEntry {
	public bool boolValue;
	public EventConditionModes conditionMode;
	public string conditionParam;
	public EventConditionParamTypes conditionParamType;
	public float floatValue;
	public int intValue;
}
