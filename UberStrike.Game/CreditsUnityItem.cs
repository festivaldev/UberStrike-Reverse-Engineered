using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UnityEngine;

public class CreditsUnityItem : IUnityItem {
	public CreditsUnityItem(int credits) {
		Name = credits.ToString("N0") + " Credits";

		View = new DummyItemView {
			Description = string.Format("An extra {0:N0} Credits to fatten up your UberWallet!", credits)
		};
	}

	public bool Equippable {
		get { return false; }
	}

	public string Name { get; private set; }
	public BaseUberStrikeItemView View { get; private set; }

	public bool IsLoaded {
		get { return true; }
	}

	public GameObject Prefab {
		get { return null; }
	}

	public void Unload() { }

	public GameObject Create(Vector3 position, Quaternion rotation) {
		return null;
	}

	public void DrawIcon(Rect position) {
		GUI.DrawTexture(position, ShopIcons.CreditsIcon48x48);
	}

	private class DummyItemView : BaseUberStrikeItemView {
		public override UberstrikeItemType ItemType {
			get { return UberstrikeItemType.Special; }
		}
	}
}
