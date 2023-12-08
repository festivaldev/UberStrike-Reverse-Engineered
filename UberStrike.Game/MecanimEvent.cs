using System;
using System.Collections.Generic;

[Serializable]
public class MecanimEvent {
	public bool boolParam;
	public EventCondition condition;
	private EventContext context;
	public bool critical;
	public float floatParam;
	public string functionName;
	public int intParam;
	public float normalizedTime;
	public MecanimEventParamTypes paramType;
	public string stringParam;
	public static EventContext Context { get; protected set; }

	public object parameter {
		get {
			switch (paramType) {
				case MecanimEventParamTypes.Int32:
					return intParam;
				case MecanimEventParamTypes.Float:
					return floatParam;
				case MecanimEventParamTypes.String:
					return stringParam;
				case MecanimEventParamTypes.Boolean:
					return boolParam;
				default:
					return null;
			}
		}
	}

	public MecanimEvent() {
		condition = new EventCondition();
	}

	public MecanimEvent(MecanimEvent other) {
		normalizedTime = other.normalizedTime;
		functionName = other.functionName;
		paramType = other.paramType;

		switch (paramType) {
			case MecanimEventParamTypes.Int32:
				intParam = other.intParam;

				break;
			case MecanimEventParamTypes.Float:
				floatParam = other.floatParam;

				break;
			case MecanimEventParamTypes.String:
				stringParam = other.stringParam;

				break;
			case MecanimEventParamTypes.Boolean:
				boolParam = other.boolParam;

				break;
		}

		condition = new EventCondition();
		condition.conditions = new List<EventConditionEntry>(other.condition.conditions);
		critical = other.critical;
	}

	public void SetContext(EventContext context) {
		this.context = context;
		this.context.current = this;
	}

	public static void SetCurrentContext(MecanimEvent e) {
		Context = e.context;
	}
}
