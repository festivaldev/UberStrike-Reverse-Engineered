using System.Collections;
using UnityEngine;

public class LoginPanelGUI : PanelGuiBase {
	private string _emailAddress = string.Empty;
	private float _errorAlpha;
	private float _keyboardOffset;
	private float _panelAlpha;
	private string _password = string.Empty;
	private Rect _rect;
	private bool _rememberPassword;
	private float _targetKeyboardOffset;
	private float dialogTimer;
	public static string ErrorMessage { get; set; }
	public static bool IsBanned { get; set; }

	private void Start() {
		_rememberPassword = CmunePrefs.ReadKey<bool>(CmunePrefs.Key.Player_AutoLogin);

		if (_rememberPassword) {
			_password = CmunePrefs.ReadKey<string>(CmunePrefs.Key.Player_Password);
			_emailAddress = CmunePrefs.ReadKey<string>(CmunePrefs.Key.Player_Email);
		}
	}

	public override void Hide() {
		base.Hide();
		_errorAlpha = 0f;
		ErrorMessage = string.Empty;
	}

	public override void Show() {
		base.Show();

		if (IsBanned) {
			ErrorMessage = LocalizedStrings.YourAccountHasBeenBanned;
		}

		if (!string.IsNullOrEmpty(ErrorMessage)) {
			_errorAlpha = 1f;
		}

		_panelAlpha = 0f;
		_keyboardOffset = 0f;
		_targetKeyboardOffset = 0f;
	}

	private void HideKeyboard() { }

	private void Update() {
		if (!string.IsNullOrEmpty(_emailAddress)) {
			_emailAddress = _emailAddress.Replace("\n", string.Empty).Replace("\t", string.Empty);
		}

		if (!string.IsNullOrEmpty(_password)) {
			_password = _password.Replace("\n", string.Empty).Replace("\t", string.Empty);
		}

		if (_errorAlpha > 0f) {
			_errorAlpha -= Time.deltaTime * 0.1f;
		}
	}

	private void OnGUI() {
		_panelAlpha = Mathf.Lerp(_panelAlpha, 1f, Time.deltaTime / 2f);
		GUI.color = new Color(1f, 1f, 1f, _panelAlpha);

		if (Mathf.Abs(_keyboardOffset - _targetKeyboardOffset) > 2f) {
			_keyboardOffset = Mathf.Lerp(_keyboardOffset, _targetKeyboardOffset, Time.deltaTime * 4f);
		} else {
			_keyboardOffset = _targetKeyboardOffset;
		}

		_rect = new Rect((Screen.width - 400) / 2, (Screen.height - 290) / 2 - _keyboardOffset, 400f, 290f);
		DrawLoginPanel();

		if (!string.IsNullOrEmpty(GUI.tooltip)) {
			var matrix = GUI.matrix;
			GUI.matrix = Matrix4x4.identity;
			var vector = BlueStonez.tooltip.CalcSize(new GUIContent(GUI.tooltip));
			var rect = new Rect(Mathf.Clamp(Event.current.mousePosition.x, 14f, Screen.width - (vector.x + 14f)), Event.current.mousePosition.y + 24f, vector.x, vector.y + 16f);
			GUI.Label(rect, GUI.tooltip, BlueStonez.tooltip);
			GUI.matrix = matrix;
		}

		GUI.color = Color.white;
	}

	private void DrawLoginPanel() {
		GUI.BeginGroup(_rect, GUIContent.none, BlueStonez.window);
		GUI.depth = 3;
		GUI.Label(new Rect(0f, 0f, _rect.width, 23f), "Add an existing UberStrike Account to Steam", BlueStonez.tab_strip);
		GUI.Label(new Rect(0f, 48f, _rect.width - 10f, 48f), "Your UberStrike account will be permanently associated with your Steam account", BlueStonez.label_interparkbold_11pt);
		GUI.Label(new Rect(20f, 108f, 100f, 24f), "Email:");
		_emailAddress = GUI.TextField(new Rect(128f, 108f, _rect.width - 164f, 24f), _emailAddress, 100, BlueStonez.textField);

		if (string.IsNullOrEmpty(_emailAddress)) {
			GUI.color = Color.white.SetAlpha(0.3f);
			GUI.color = Color.white;
		}

		GUI.Label(new Rect(20f, 144f, 100f, 24f), "Password:");
		_password = GUI.PasswordField(new Rect(128f, 144f, _rect.width - 164f, 24f), _password, '*', 64, BlueStonez.textField);

		if (string.IsNullOrEmpty(_password)) {
			GUI.color = Color.white.SetAlpha(0.3f);
			GUI.color = Color.white;
		}

		if (GUITools.Button(new Rect(70f, 190f, 100f, 52f), new GUIContent("Cancel"), BlueStonez.buttondark_medium)) {
			Hide();
			Singleton<AuthenticationManager>.Instance.LoginByChannel();
		}

		if (GUITools.Button(new Rect(210f, 190f, 100f, 52f), new GUIContent("Add"), BlueStonez.button_green)) {
			HideKeyboard();
			Login(_emailAddress, _password);
		}

		GUI.Label(new Rect(8f, 256f, _rect.width - 16f, 8f), GUIContent.none, BlueStonez.horizontal_line_grey95);

		if (GUITools.Button(new Rect(20f, 264f, 100f, 40f), new GUIContent("Forgot password?"), BlueStonez.label_interparkbold_11pt_url)) {
			HideKeyboard();
			ApplicationDataManager.OpenUrl(string.Empty, "http://www.uberstrike.com/#forgot_password");
		}

		if (GUITools.Button(new Rect(_rect.width - 118f, 264f, 98f, 40f), new GUIContent("Facebook player?"), BlueStonez.label_interparkbold_11pt_url)) {
			HideKeyboard();
			ApplicationDataManager.OpenUrl(string.Empty, "http://www.uberstrike.com/steam");
		}

		GUI.enabled = true;
		GUI.EndGroup();
	}

	public IEnumerator StartCancelDialogTimer() {
		if (dialogTimer < 5f) {
			dialogTimer = 5f;
		}

		yield break;
	}

	private void Login(string emailAddress, string password) {
		CmunePrefs.WriteKey(CmunePrefs.Key.Player_AutoLogin, _rememberPassword);

		if (_rememberPassword) {
			CmunePrefs.WriteKey(CmunePrefs.Key.Player_Password, password);
			CmunePrefs.WriteKey(CmunePrefs.Key.Player_Email, emailAddress);
		}

		_errorAlpha = 1f;

		if (string.IsNullOrEmpty(emailAddress)) {
			ErrorMessage = LocalizedStrings.EnterYourEmailAddress;
		} else if (string.IsNullOrEmpty(password)) {
			ErrorMessage = LocalizedStrings.EnterYourPassword;
		} else if (!ValidationUtilities.IsValidEmailAddress(emailAddress)) {
			ErrorMessage = LocalizedStrings.EmailAddressIsInvalid;
		} else if (!ValidationUtilities.IsValidPassword(password)) {
			ErrorMessage = LocalizedStrings.PasswordIsInvalid;
		} else {
			Hide();
			UnityRuntime.StartRoutine(Singleton<AuthenticationManager>.Instance.StartLoginMemberEmail(emailAddress, password));
		}
	}
}
