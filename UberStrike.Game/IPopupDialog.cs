public interface IPopupDialog {
	string Text { get; set; }
	string Title { get; set; }
	GuiDepth Depth { get; }
	void OnGUI();
	void OnHide();
}
