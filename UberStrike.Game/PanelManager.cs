using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour {
	private static bool _wasAnyPanelOpen;
	private IDictionary<PanelType, IPanelGui> _allPanels;
	public static PanelManager Instance { get; private set; }

	public static bool Exists {
		get { return Instance != null; }
	}

	public LoginPanelGUI LoginPanel {
		get { return _allPanels[PanelType.Login] as LoginPanelGUI; }
	}

	public static bool IsAnyPanelOpen { get; private set; }

	private void Awake() {
		Instance = this;

		_allPanels = new Dictionary<PanelType, IPanelGui> {
			{
				PanelType.Login, GetComponent<LoginPanelGUI>()
			}, {
				PanelType.Signup, GetComponent<SignupPanelGUI>()
			}, {
				PanelType.CompleteAccount, GetComponent<CompleteAccountPanelGUI>()
			}, {
				PanelType.Options, GetComponent<OptionsPanelGUI>()
			}, {
				PanelType.Help, GetComponent<HelpPanelGUI>()
			}, {
				PanelType.CreateGame, GetComponent<CreateGamePanelGUI>()
			}, {
				PanelType.ReportPlayer, GetComponent<ReportPlayerPanelGUI>()
			}, {
				PanelType.Moderation, GetComponent<ModerationPanelGUI>()
			}, {
				PanelType.SendMessage, GetComponent<SendMessagePanelGUI>()
			}, {
				PanelType.FriendRequest, GetComponent<FriendRequestPanelGUI>()
			}, {
				PanelType.ClanRequest, GetComponent<InviteToClanPanelGUI>()
			}, {
				PanelType.BuyItem, GetComponent<BuyPanelGUI>()
			}, {
				PanelType.NameChange, GetComponent<NameChangePanelGUI>()
			}
		};

		foreach (var panelGui in _allPanels.Values) {
			var monoBehaviour = (MonoBehaviour)panelGui;

			if (monoBehaviour) {
				monoBehaviour.enabled = false;
			}
		}
	}

	private void OnGUI() {
		IsAnyPanelOpen = false;

		foreach (var panelGui in _allPanels.Values) {
			if (panelGui.IsEnabled) {
				IsAnyPanelOpen = true;

				break;
			}
		}

		if (Event.current.type == EventType.Layout) {
			if (IsAnyPanelOpen) {
				GuiLockController.EnableLock(GuiDepth.Panel);
			} else {
				GuiLockController.ReleaseLock(GuiDepth.Panel);
				enabled = false;
			}

			if (_wasAnyPanelOpen != IsAnyPanelOpen) {
				if (_wasAnyPanelOpen) {
					AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.ClosePanel);
				} else {
					AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.OpenPanel);
				}

				_wasAnyPanelOpen = !_wasAnyPanelOpen;
			}
		}
	}

	public bool IsPanelOpen(PanelType panel) {
		return _allPanels[panel].IsEnabled;
	}

	public void CloseAllPanels(PanelType except = PanelType.None) {
		foreach (var panelGui in _allPanels.Values) {
			if (panelGui.IsEnabled) {
				panelGui.Hide();
			}
		}
	}

	public IPanelGui OpenPanel(PanelType panel) {
		foreach (var keyValuePair in _allPanels) {
			if (panel == keyValuePair.Key) {
				if (!keyValuePair.Value.IsEnabled) {
					keyValuePair.Value.Show();
				}
			} else if (keyValuePair.Value.IsEnabled) {
				keyValuePair.Value.Hide();
			}
		}

		enabled = true;

		return _allPanels[panel];
	}

	public void ClosePanel(PanelType panel) {
		if (_allPanels.ContainsKey(panel) && _allPanels[panel].IsEnabled) {
			_allPanels[panel].Hide();
		}
	}
}
