public class LoginPageScene : PageScene {
	public override PageType PageType {
		get { return PageType.Login; }
	}

	protected override void OnLoad() {
		PanelManager.Instance.OpenPanel(PanelType.Login);
	}

	protected override void OnUnload() {
		PanelManager.Instance.ClosePanel(PanelType.Login);
	}
}
