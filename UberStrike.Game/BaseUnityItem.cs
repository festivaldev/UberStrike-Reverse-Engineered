using UberStrike.Core.Types;
using UnityEngine;

public abstract class BaseUnityItem : MonoBehaviour {
	[SerializeField]
	private UberstrikeItemClass _testItemClass;

	public UberstrikeItemClass TestItemClass {
		get { return _testItemClass; }
		set { _testItemClass = value; }
	}
}
