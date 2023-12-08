using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EventCondition {
	public List<EventConditionEntry> conditions = new List<EventConditionEntry>();

	public bool Test(Animator animator) {
		if (conditions.Count == 0) {
			return true;
		}

		foreach (var eventConditionEntry in conditions) {
			if (!string.IsNullOrEmpty(eventConditionEntry.conditionParam)) {
				switch (eventConditionEntry.conditionParamType) {
					case EventConditionParamTypes.Int: {
						var integer = animator.GetInteger(eventConditionEntry.conditionParam);

						switch (eventConditionEntry.conditionMode) {
							case EventConditionModes.Equal:
								if (integer != eventConditionEntry.intValue) {
									return false;
								}

								break;
							case EventConditionModes.NotEqual:
								if (integer == eventConditionEntry.intValue) {
									return false;
								}

								break;
							case EventConditionModes.GreaterThan:
								if (integer <= eventConditionEntry.intValue) {
									return false;
								}

								break;
							case EventConditionModes.LessThan:
								if (integer >= eventConditionEntry.intValue) {
									return false;
								}

								break;
							case EventConditionModes.GreaterEqualThan:
								if (integer < eventConditionEntry.intValue) {
									return false;
								}

								break;
							case EventConditionModes.LessEqualThan:
								if (integer > eventConditionEntry.intValue) {
									return false;
								}

								break;
						}

						break;
					}
					case EventConditionParamTypes.Float: {
						var @float = animator.GetFloat(eventConditionEntry.conditionParam);
						var conditionMode = eventConditionEntry.conditionMode;

						if (conditionMode != EventConditionModes.GreaterThan) {
							if (conditionMode == EventConditionModes.LessThan) {
								if (@float >= eventConditionEntry.floatValue) {
									return false;
								}
							}
						} else if (@float <= eventConditionEntry.floatValue) {
							return false;
						}

						break;
					}
					case EventConditionParamTypes.Boolean: {
						var @bool = animator.GetBool(eventConditionEntry.conditionParam);

						if (@bool != eventConditionEntry.boolValue) {
							return false;
						}

						break;
					}
				}
			}
		}

		return true;
	}
}
