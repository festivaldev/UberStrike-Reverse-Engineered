public class ShopPageScene : PageScene {
	public override PageType PageType {
		get { return PageType.Shop; }
	}

	protected override void OnLoad() {
		if (!GameState.Current.HasJoinedGame) {
			if (_avatarAnchor) {
				GameState.Current.Avatar.Decorator.SetPosition(_avatarAnchor.position, _avatarAnchor.rotation);
			}

			if (GameState.Current.Avatar != null) {
				GameState.Current.Avatar.HideWeapons();
			}

			EventHandler.Global.Fire(new GameEvents.PlayerPause());
		}
	}

	protected override void OnUnload() {
		if (!GameState.Current.HasJoinedGame) {
			Singleton<TemporaryLoadoutManager>.Instance.ResetLoadout();
		}
	}
}
