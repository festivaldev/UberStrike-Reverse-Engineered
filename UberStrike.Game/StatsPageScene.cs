public class StatsPageScene : PageScene {
	public override PageType PageType {
		get { return PageType.Stats; }
	}

	protected override void OnLoad() {
		if (_avatarAnchor) {
			GameState.Current.Avatar.Decorator.SetPosition(_avatarAnchor.position, _avatarAnchor.rotation);
		}
	}
}
