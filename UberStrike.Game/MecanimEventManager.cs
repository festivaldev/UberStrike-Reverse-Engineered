using System.Collections.Generic;
using UnityEngine;

public static class MecanimEventManager {
	private static MecanimEventData[] eventDataSources;
	private static Dictionary<int, Dictionary<int, Dictionary<int, List<MecanimEvent>>>> loadedData;
	private static Dictionary<int, Dictionary<int, AnimatorStateInfo>> lastStates = new Dictionary<int, Dictionary<int, AnimatorStateInfo>>();

	public static void SetEventDataSource(MecanimEventData dataSource) {
		if (dataSource != null) {
			eventDataSources = new MecanimEventData[1];
			eventDataSources[0] = dataSource;
			LoadDataInGame();
		}
	}

	public static void SetEventDataSource(MecanimEventData[] dataSources) {
		if (dataSources != null) {
			eventDataSources = dataSources;
			LoadDataInGame();
		}
	}

	public static void OnLevelLoaded() {
		lastStates.Clear();
	}

	public static ICollection<MecanimEvent> GetEvents(int animatorControllerId, Animator animator) {
		var list = new List<MecanimEvent>();
		var hashCode = animator.GetHashCode();

		if (!lastStates.ContainsKey(hashCode)) {
			lastStates[hashCode] = new Dictionary<int, AnimatorStateInfo>();
		}

		var layerCount = animator.layerCount;
		var dictionary = lastStates[hashCode];

		for (var i = 0; i < layerCount; i++) {
			if (!dictionary.ContainsKey(i)) {
				dictionary[i] = default(AnimatorStateInfo);
			}

			var animatorStateInfo = dictionary[i];
			var currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(i);
			var num = (int)animatorStateInfo.normalizedTime;
			var num2 = (int)currentAnimatorStateInfo.normalizedTime;
			var num3 = animatorStateInfo.normalizedTime - num;
			var num4 = currentAnimatorStateInfo.normalizedTime - num2;

			if (animatorStateInfo.nameHash == currentAnimatorStateInfo.nameHash) {
				if (currentAnimatorStateInfo.loop) {
					if (num == num2) {
						list.AddRange(CollectEvents(animator, animatorControllerId, i, currentAnimatorStateInfo.nameHash, currentAnimatorStateInfo.tagHash, num3, num4));
					} else {
						list.AddRange(CollectEvents(animator, animatorControllerId, i, currentAnimatorStateInfo.nameHash, currentAnimatorStateInfo.tagHash, num3, 1.00001f));
						list.AddRange(CollectEvents(animator, animatorControllerId, i, currentAnimatorStateInfo.nameHash, currentAnimatorStateInfo.tagHash, 0f, num4));
					}
				} else {
					var num5 = Mathf.Clamp01(animatorStateInfo.normalizedTime);
					var num6 = Mathf.Clamp01(currentAnimatorStateInfo.normalizedTime);

					if (num == 0 && num2 == 0) {
						if (num5 != num6) {
							list.AddRange(CollectEvents(animator, animatorControllerId, i, currentAnimatorStateInfo.nameHash, currentAnimatorStateInfo.tagHash, num5, num6));
						}
					} else if (num == 0 && num2 > 0) {
						list.AddRange(CollectEvents(animator, animatorControllerId, i, animatorStateInfo.nameHash, animatorStateInfo.tagHash, num5, 1.00001f));
					}
				}
			} else {
				list.AddRange(CollectEvents(animator, animatorControllerId, i, currentAnimatorStateInfo.nameHash, currentAnimatorStateInfo.tagHash, 0f, num4));

				if (!animatorStateInfo.loop) {
					list.AddRange(CollectEvents(animator, animatorControllerId, i, animatorStateInfo.nameHash, animatorStateInfo.tagHash, num3, 1.00001f, true));
				}
			}

			dictionary[i] = currentAnimatorStateInfo;
		}

		return list;
	}

	private static ICollection<MecanimEvent> CollectEvents(Animator animator, int animatorControllerId, int layer, int nameHash, int tagHash, float normalizedTimeStart, float normalizedTimeEnd, bool onlyCritical = false) {
		if (loadedData.ContainsKey(animatorControllerId) && loadedData[animatorControllerId].ContainsKey(layer) && loadedData[animatorControllerId][layer].ContainsKey(nameHash)) {
			var list = loadedData[animatorControllerId][layer][nameHash];
			var list2 = new List<MecanimEvent>();

			foreach (var mecanimEvent in list) {
				if (mecanimEvent.normalizedTime >= normalizedTimeStart && mecanimEvent.normalizedTime < normalizedTimeEnd && mecanimEvent.condition.Test(animator)) {
					if (!onlyCritical || mecanimEvent.critical) {
						var mecanimEvent2 = new MecanimEvent(mecanimEvent);

						mecanimEvent2.SetContext(new EventContext {
							controllerId = animatorControllerId,
							layer = layer,
							stateHash = nameHash,
							tagHash = tagHash
						});

						list2.Add(mecanimEvent2);
					}
				}
			}

			return list2;
		}

		return new MecanimEvent[0];
	}

	private static void LoadDataInGame() {
		if (eventDataSources == null) {
			return;
		}

		loadedData = new Dictionary<int, Dictionary<int, Dictionary<int, List<MecanimEvent>>>>();

		foreach (var mecanimEventData in eventDataSources) {
			if (!(mecanimEventData == null)) {
				var data = mecanimEventData.data;

				foreach (var mecanimEventDataEntry in data) {
					var instanceID = mecanimEventDataEntry.animatorController.GetInstanceID();

					if (!loadedData.ContainsKey(instanceID)) {
						loadedData[instanceID] = new Dictionary<int, Dictionary<int, List<MecanimEvent>>>();
					}

					if (!loadedData[instanceID].ContainsKey(mecanimEventDataEntry.layer)) {
						loadedData[instanceID][mecanimEventDataEntry.layer] = new Dictionary<int, List<MecanimEvent>>();
					}

					loadedData[instanceID][mecanimEventDataEntry.layer][mecanimEventDataEntry.stateNameHash] = new List<MecanimEvent>(mecanimEventDataEntry.events);
				}
			}
		}
	}
}
