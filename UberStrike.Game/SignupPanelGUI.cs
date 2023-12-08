using System.Collections.Generic;
using Cmune.DataCenter.Common.Entities;
using UberStrike.WebService.Unity;
using UnityEngine;

public class SignupPanelGUI : PanelGuiBase {
	private const float NORMAL_HEIGHT = 300f;
	private const float EXTENDED_HEIGHT = 340f;
	private string _emailAddress = string.Empty;
	private bool _enableGUI = true;
	private string _errorMessage = string.Empty;
	private Color _errorMessageColor = Color.red;
	private Dictionary<MemberRegistrationResult, string> _errorMessages;
	private float _height = 300f;
	private float _keyboardOffset;
	private string _password1 = string.Empty;
	private string _password2 = string.Empty;
	private float _targetHeight = 300f;
	private float _targetKeyboardOffset;

	private void Awake() {
		_errorMessages = new Dictionary<MemberRegistrationResult, string>();
	}

	private void Start() {
		_errorMessages.Add(MemberRegistrationResult.DuplicateEmail, LocalizedStrings.EmailAddressInUseMsg);
		_errorMessages.Add(MemberRegistrationResult.DuplicateEmailName, LocalizedStrings.EmailAddressAndNameInUseMsg);
		_errorMessages.Add(MemberRegistrationResult.DuplicateHandle, LocalizedStrings.NameInUseMsg);
		_errorMessages.Add(MemberRegistrationResult.DuplicateName, LocalizedStrings.NameInUseMsg);
		_errorMessages.Add(MemberRegistrationResult.InvalidData, LocalizedStrings.InvalidData);
		_errorMessages.Add(MemberRegistrationResult.InvalidEmail, LocalizedStrings.EmailAddressIsInvalid);
		_errorMessages.Add(MemberRegistrationResult.InvalidEsns, LocalizedStrings.InvalidData + " (Esns)");
		_errorMessages.Add(MemberRegistrationResult.InvalidHandle, LocalizedStrings.InvalidData + " (Handle)");
		_errorMessages.Add(MemberRegistrationResult.InvalidName, LocalizedStrings.NameInvalidCharsMsg);
		_errorMessages.Add(MemberRegistrationResult.InvalidPassword, LocalizedStrings.PasswordIsInvalid);
		_errorMessages.Add(MemberRegistrationResult.IsIpBanned, "IP is banned");
		_errorMessages.Add(MemberRegistrationResult.MemberNotFound, "I can't find that member. Maybe he's hiding. In any case, you'll have to try again.");
		_errorMessages.Add(MemberRegistrationResult.OffensiveName, LocalizedStrings.OffensiveNameMsg);
	}

	private void HideKeyboard() { }

	private void Update() {
		if (_height != _targetHeight) {
			_height = Mathf.Lerp(_height, _targetHeight, 10f * Time.deltaTime);

			if (Mathf.Approximately(_height, _targetHeight)) {
				_height = _targetHeight;
			}
		}
	}

	private void SetTargetKeyboardOffset() {
		_targetKeyboardOffset = (Screen.height - _height) * 0.5f - (Screen.height * 0.5f - _height) * 0.5f;
	}

	private void OnGUI() {
		if (Mathf.Abs(_keyboardOffset - _targetKeyboardOffset) > 2f) {
			_keyboardOffset = Mathf.Lerp(_keyboardOffset, _targetKeyboardOffset, Time.deltaTime * 4f);
		} else {
			_keyboardOffset = _targetKeyboardOffset;
		}

		var rect = new Rect((Screen.width - 500) * 0.5f, (Screen.height - _height) * 0.5f - _keyboardOffset, 500f, _height);
		GUI.BeginGroup(rect, GUIContent.none, BlueStonez.window);
		GUI.Label(new Rect(0f, 0f, rect.width, 56f), LocalizedStrings.Welcome, BlueStonez.tab_strip);
		var rect2 = new Rect(20f, 55f, rect.width - 40f, rect.height - 78f);
		GUI.Label(rect2, GUIContent.none, BlueStonez.window_standard_grey38);
		GUI.BeginGroup(rect2);
		GUI.Label(new Rect(0f, 0f, rect2.width, 60f), LocalizedStrings.PleaseProvideValidEmailPasswordMsg, BlueStonez.label_interparkbold_18pt);
		GUI.Label(new Rect(0f, 76f, 170f, 11f), LocalizedStrings.Email, BlueStonez.label_interparkbold_11pt_right);
		GUI.Label(new Rect(0f, 110f, 170f, 11f), LocalizedStrings.Password, BlueStonez.label_interparkbold_11pt_right);
		GUI.Label(new Rect(0f, 147f, 170f, 11f), LocalizedStrings.VerifyPassword, BlueStonez.label_interparkbold_11pt_right);
		GUI.enabled = _enableGUI;
		GUI.SetNextControlName("@Email");
		_emailAddress = GUI.TextField(new Rect(180f, 69f, 180f, 22f), _emailAddress, BlueStonez.textField);

		if (string.IsNullOrEmpty(_emailAddress) && GUI.GetNameOfFocusedControl() != "@Email") {
			GUI.color = new Color(1f, 1f, 1f, 0.3f);
			GUI.Label(new Rect(188f, 75f, 180f, 22f), LocalizedStrings.EnterYourEmailAddress, BlueStonez.label_interparkmed_11pt_left);
			GUI.color = Color.white;
		}

		GUI.SetNextControlName("@Password1");
		_password1 = GUI.PasswordField(new Rect(180f, 104f, 180f, 22f), _password1, '*', BlueStonez.textField);

		if (string.IsNullOrEmpty(_password1) && GUI.GetNameOfFocusedControl() != "@Password1") {
			GUI.color = new Color(1f, 1f, 1f, 0.3f);
			GUI.Label(new Rect(188f, 110f, 172f, 18f), LocalizedStrings.EnterYourPassword, BlueStonez.label_interparkmed_11pt_left);
			GUI.color = Color.white;
		}

		GUI.SetNextControlName("@Password2");
		_password2 = GUI.PasswordField(new Rect(180f, 140f, 180f, 22f), _password2, '*', BlueStonez.textField);

		if (string.IsNullOrEmpty(_password2) && GUI.GetNameOfFocusedControl() != "@Password2") {
			GUI.color = new Color(1f, 1f, 1f, 0.3f);
			GUI.Label(new Rect(188f, 146f, 180f, 22f), LocalizedStrings.RetypeYourPassword, BlueStonez.label_interparkmed_11pt_left);
			GUI.color = Color.white;
		}

		GUI.enabled = true;
		GUI.contentColor = _errorMessageColor;
		GUI.Label(new Rect(0f, 175f, rect2.width, 40f), _errorMessage, BlueStonez.label_interparkbold_11pt);
		GUI.contentColor = Color.white;
		GUI.EndGroup();
		GUI.Label(new Rect(100f, rect.height - 42f - 22f, 300f, 16f), "By clicking OK you agree to the", BlueStonez.label_interparkbold_11pt);

		if (GUI.Button(new Rect(185f, rect.height - 30f - 12f, 130f, 20f), "Terms of Service", BlueStonez.buttondark_small)) {
			ApplicationDataManager.OpenUrl("Terms Of Service", "http://www.cmune.com/index.php/terms-of-service/");
			HideKeyboard();
		}

		GUI.Label(new Rect(207f, rect.height - 15f - 22f, 90f, 20f), GUIContent.none, BlueStonez.horizontal_line_grey95);
		GUI.enabled = _enableGUI;

		if (GUITools.Button(new Rect(rect.width - 150f, rect.height - 42f - 22f, 120f, 32f), new GUIContent(LocalizedStrings.OkCaps), BlueStonez.button_green)) {
			HideKeyboard();

			if (!ValidationUtilities.IsValidEmailAddress(_emailAddress)) {
				_targetHeight = 340f;
				_errorMessageColor = Color.red;
				_errorMessage = LocalizedStrings.EmailAddressIsInvalid;
			} else if (_password1 != _password2) {
				_targetHeight = 340f;
				_errorMessageColor = Color.red;
				_errorMessage = LocalizedStrings.PasswordDoNotMatch;
			} else if (!ValidationUtilities.IsValidPassword(_password1)) {
				_targetHeight = 340f;
				_errorMessageColor = Color.red;
				_errorMessage = LocalizedStrings.PasswordInvalidCharsMsg;
			} else {
				_enableGUI = false;
				_targetHeight = 340f;
				_errorMessageColor = Color.grey;
				_errorMessage = LocalizedStrings.PleaseWait;

				AuthenticationWebServiceClient.CreateUser(_emailAddress, _password1, ApplicationDataManager.Channel, ApplicationDataManager.CurrentLocale.ToString(), SystemInfo.deviceUniqueIdentifier, delegate(MemberRegistrationResult result) {
					if (result == MemberRegistrationResult.Ok) {
						Hide();
						CmunePrefs.WriteKey(CmunePrefs.Key.Player_Email, _emailAddress);
						CmunePrefs.WriteKey(CmunePrefs.Key.Player_Password, _password1);
						UnityRuntime.StartRoutine(Singleton<AuthenticationManager>.Instance.StartLoginMemberEmail(_emailAddress, _password1));
						_targetHeight = 300f;
						_errorMessage = string.Empty;
						_emailAddress = string.Empty;
						_password1 = string.Empty;
						_password2 = string.Empty;
						_errorMessageColor = Color.red;
						_enableGUI = true;
					} else {
						_enableGUI = true;
						_targetHeight = 340f;
						_errorMessageColor = Color.red;
						_errorMessages.TryGetValue(result, out _errorMessage);
					}
				}, delegate {
					_enableGUI = true;
					_targetHeight = 300f;
					_errorMessage = string.Empty;
					ShowSignUpErrorPopup(LocalizedStrings.Error, "Sign Up was unsuccessful. There was an error communicating with the server.");
				});
			}
		}

		if (GUITools.Button(new Rect(30f, rect.height - 42f - 22f, 120f, 32f), new GUIContent(LocalizedStrings.BackCaps), BlueStonez.button)) {
			Hide();
			HideKeyboard();
			PanelManager.Instance.OpenPanel(PanelType.Login);
		}

		GUI.enabled = true;
		GUI.EndGroup();
	}

	private void ShowSignUpErrorPopup(string title, string message) {
		Hide();

		PopupSystem.ShowMessage(title, message, PopupSystem.AlertType.OK, delegate {
			LoginPanelGUI.ErrorMessage = string.Empty;
			Show();
		});
	}
}
