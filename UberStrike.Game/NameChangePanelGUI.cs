using Cmune.DataCenter.Common.Entities;
using UberStrike.Realtime.UnitySdk;
using UberStrike.WebService.Unity;
using UnityEngine;

public class NameChangePanelGUI : PanelGuiBase {
	private const int MAX_CHARACTER_NAME_LENGTH = 18;
	private Rect _groupRect = new Rect(1f, 1f, 1f, 1f);
	private float _keyboardOffset;
	private float _targetKeyboardOffset;
	private bool isChangingName;
	private IUnityItem nameChangeItem;
	private string newName = string.Empty;
	private string oldName = string.Empty;
	private void Update() { }
	private void HideKeyboard() { }

	private void OnGUI() {
		if (Mathf.Abs(_keyboardOffset - _targetKeyboardOffset) > 2f) {
			_keyboardOffset = Mathf.Lerp(_keyboardOffset, _targetKeyboardOffset, Time.deltaTime * 4f);
		} else {
			_keyboardOffset = _targetKeyboardOffset;
		}

		_groupRect = new Rect((Screen.width - 340) * 0.5f, (Screen.height - 200) * 0.5f - _keyboardOffset, 340f, 200f);
		GUI.depth = 3;
		GUI.skin = BlueStonez.Skin;
		var groupRect = _groupRect;
		GUI.BeginGroup(groupRect, string.Empty, BlueStonez.window_standard_grey38);

		if (nameChangeItem != null) {
			nameChangeItem.DrawIcon(new Rect(8f, 8f, 48f, 48f));

			if (BlueStonez.label_interparkbold_32pt_left.CalcSize(new GUIContent(nameChangeItem.View.Name)).x > groupRect.width - 72f) {
				GUI.Label(new Rect(64f, 8f, groupRect.width - 72f, 30f), nameChangeItem.View.Name, BlueStonez.label_interparkbold_18pt_left);
			} else {
				GUI.Label(new Rect(64f, 8f, groupRect.width - 72f, 30f), nameChangeItem.View.Name, BlueStonez.label_interparkbold_32pt_left);
			}
		}

		GUI.Label(new Rect(64f, 30f, groupRect.width - 72f, 30f), LocalizedStrings.FunctionalItem, BlueStonez.label_interparkbold_16pt_left);
		var rect = new Rect(8f, 116f, _groupRect.width - 16f, _groupRect.height - 120f - 46f);
		GUI.BeginGroup(new Rect(rect.xMin, 74f, rect.width, rect.height + 42f), string.Empty, BlueStonez.group_grey81);
		GUI.EndGroup();
		GUI.Label(new Rect(56f, 72f, 227f, 20f), LocalizedStrings.ChooseCharacterName, BlueStonez.label_interparkbold_11pt);
		GUI.SetNextControlName("@ChooseName");
		var rect2 = new Rect(56f, 102f, 227f, 24f);
		GUI.changed = false;
		newName = GUI.TextField(rect2, newName, 18, BlueStonez.textField);
		newName = TextUtilities.Trim(newName);

		if (string.IsNullOrEmpty(newName) && GUI.GetNameOfFocusedControl() != "@ChooseName") {
			GUI.color = new Color(1f, 1f, 1f, 0.3f);
			GUI.Label(rect2, LocalizedStrings.EnterYourName, BlueStonez.label_interparkmed_11pt);
			GUI.color = Color.white;
		}

		if (GUITools.Button(new Rect(groupRect.width - 118f, 160f, 110f, 32f), new GUIContent(LocalizedStrings.CancelCaps), BlueStonez.button)) {
			HideKeyboard();
			Hide();
		}

		GUI.enabled = !isChangingName;

		if (GUITools.Button(new Rect(groupRect.width - 230f, 160f, 110f, 32f), new GUIContent(LocalizedStrings.OkCaps), BlueStonez.button_green)) {
			HideKeyboard();
			ChangeName();
		}

		GUI.EndGroup();
		GUI.enabled = true;

		if (isChangingName) {
			WaitingTexture.Draw(new Vector2(groupRect.x + 305f, groupRect.y + 114f));
		}

		GuiManager.DrawTooltip();
	}

	private void ChangeName() {
		if (!newName.Equals(oldName) && !string.IsNullOrEmpty(newName)) {
			isChangingName = true;

			UserWebServiceClient.ChangeMemberName(PlayerDataManager.AuthToken, newName, ApplicationDataManager.CurrentLocale.ToString(), SystemInfo.deviceUniqueIdentifier, delegate(MemberOperationResult t) {
				switch (t) {
					case MemberOperationResult.Ok:
						PlayerDataManager.Name = newName;
						AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Operations.SendAuthenticationRequest(PlayerDataManager.AuthToken, PlayerDataManager.MagicHash);
						StartCoroutine(Singleton<ItemManager>.Instance.StartGetInventory(false));
						PopupSystem.ShowMessage("Congratulations", "You successfully changed your name to:\n" + newName, PopupSystem.AlertType.OK, "YEAH", delegate { });
						Hide();

						break;
					default:
						switch (t) {
							case MemberOperationResult.InvalidName:
								PopupSystem.ShowMessage(LocalizedStrings.Error, LocalizedStrings.NameInvalidCharsMsg);

								goto IL_123;
							case MemberOperationResult.OffensiveName:
								PopupSystem.ShowMessage(LocalizedStrings.Error, LocalizedStrings.OffensiveNameMsg);

								goto IL_123;
						}

						Debug.LogError("Failed to change name: " + t);
						PopupSystem.ShowMessage(LocalizedStrings.Error, LocalizedStrings.Unknown);

						break;
					case MemberOperationResult.DuplicateName:
						PopupSystem.ShowMessage(LocalizedStrings.Error, LocalizedStrings.NameInUseMsg);

						break;
				}

				IL_123:
				isChangingName = false;
			}, delegate {
				isChangingName = false;
				Hide();
			});
		}
	}

	public override void Show() {
		base.Show();
		nameChangeItem = Singleton<ItemManager>.Instance.GetItemInShop(1294);
		oldName = PlayerDataManager.Name;
		newName = oldName;
		_targetKeyboardOffset = 0f;
		_keyboardOffset = 0f;
	}
}
