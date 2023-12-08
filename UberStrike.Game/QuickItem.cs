using UberStrike.Core.Types;
using UnityEngine;

public abstract class QuickItem : BaseUnityItem {
	[SerializeField]
	private QuickItemLogic _logic;

	[SerializeField]
	private QuickItemSfx _sfx;

	public QuickItemLogic Logic {
		get { return _logic; }
	}

	public QuickItemSfx Sfx {
		get { return _sfx; }
	}

	public abstract QuickItemConfiguration Configuration { get; set; }
	public QuickItemBehaviour Behaviour { get; set; }
	protected abstract void OnActivated();

	private void Awake() {
		Behaviour = new QuickItemBehaviour(this, OnActivated);
	}
}
