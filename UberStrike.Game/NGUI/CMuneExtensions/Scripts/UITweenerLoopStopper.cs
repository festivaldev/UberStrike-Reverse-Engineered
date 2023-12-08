using System;
using UnityEngine;

[AddComponentMenu("NGUI/CMune Extensions/Tweener Loop Stopper")]
public class UITweenerLoopStopper : MonoBehaviour {
	private int currentCycles;

	[SerializeField]
	private bool fireOnce;

	[SerializeField]
	private int numberOfCycles = 1;

	[SerializeField]
	private UITweener tweener;

	private void Awake() {
		if (tweener == null && gameObject.GetComponent<UITweener>()) {
			tweener = gameObject.GetComponent<UITweener>();
		}

		if (tweener != null) {
			UITweener uitweener = tweener;
			uitweener.onCycleFinished = (UITweener.OnFinished)Delegate.Combine(uitweener.onCycleFinished, new UITweener.OnFinished(this.HandleOnCycleFinished));
		} else {
			Debug.LogError("No tween was assigned to UITweenerLoopStopper in " + gameObject.name);
		}
	}

	private void HandleOnCycleFinished(UITweener tween) {
		if (fireOnce) {
			tweener.Reset();
			tweener.enabled = false;

			return;
		}

		if (currentCycles >= numberOfCycles) {
			tweener.Reset();
			tweener.enabled = false;
			currentCycles = 0;

			return;
		}

		currentCycles++;
	}
}
