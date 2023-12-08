public class HomePageScene : PageScene {
	public override PageType PageType {
		get { return PageType.Home; }
	}

	protected override void OnLoad() {
		if (_avatarAnchor && GameState.Current.Avatar.Decorator) {
			GameState.Current.Avatar.Decorator.SetPosition(_avatarAnchor.position, _avatarAnchor.rotation);
			GameState.Current.Avatar.HideWeapons();
			GameState.Current.Avatar.Decorator.HudInformation.SetAvatarLabel(PlayerDataManager.NameAndTag);
		}

		Singleton<EventPopupManager>.Instance.ShowNextPopup(1);
	}
}
