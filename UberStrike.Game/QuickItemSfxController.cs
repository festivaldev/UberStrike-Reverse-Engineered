using System.Collections.Generic;
using UberStrike.Core.Types;
using UnityEngine;

public class QuickItemSfxController : Singleton<QuickItemSfxController> {
	private Dictionary<int, QuickItemSfx> _curShownEffects;
	private Dictionary<QuickItemLogic, QuickItemSfx> _effects;
	private int _sfxId;

	private int NextSfxId {
		get { return ++_sfxId; }
	}

	private QuickItemSfxController() {
		_effects = new Dictionary<QuickItemLogic, QuickItemSfx>();
		_curShownEffects = new Dictionary<int, QuickItemSfx>();
	}

	public void RegisterQuickItemEffect(QuickItemLogic behaviour, QuickItemSfx effect) {
		if (effect) {
			_effects[behaviour] = effect;
		} else {
			Debug.LogError("QuickItemSfx is null: " + behaviour);
		}
	}

	public void ShowThirdPersonEffect(CharacterConfig player, QuickItemLogic effectType, int robotLifeTime, int scrapsLifeTime, bool isInstant = false) {
		robotLifeTime = ((robotLifeTime <= 0) ? 5000 : robotLifeTime);
		scrapsLifeTime = ((scrapsLifeTime <= 0) ? 3000 : scrapsLifeTime);
		QuickItemSfx quickItemSfx;

		if (_effects.TryGetValue(effectType, out quickItemSfx)) {
			var quickItemSfx2 = UnityEngine.Object.Instantiate(quickItemSfx) as QuickItemSfx;
			quickItemSfx2.ID = NextSfxId;

			if (quickItemSfx2) {
				_curShownEffects.Add(quickItemSfx2.ID, quickItemSfx2);
				quickItemSfx2.transform.parent = player.transform;
				quickItemSfx2.transform.localRotation = Quaternion.AngleAxis(-45f, Vector3.up);
				quickItemSfx2.transform.localPosition = new Vector3(0f, 0.2f, 0f);
				quickItemSfx2.Play(robotLifeTime, scrapsLifeTime, isInstant);
				LayerUtil.SetLayerRecursively(quickItemSfx2.transform, UberstrikeLayer.IgnoreRaycast);
			}
		} else {
			Debug.LogError("Failed to get effect: " + effectType);
		}
	}

	public void RemoveEffect(int id) {
		QuickItemSfx quickItemSfx;

		if (_curShownEffects.TryGetValue(id, out quickItemSfx)) {
			_curShownEffects.Remove(id);
		}
	}

	public void DestroytSfxFromPlayer(byte playerNumber) {
		foreach (var keyValuePair in _curShownEffects) {
			if ((keyValuePair.Key & 255) == playerNumber) {
				keyValuePair.Value.Destroy();
				_curShownEffects.Remove(keyValuePair.Key);

				break;
			}
		}
	}

	private static int CreateGlobalSfxID(byte playerNumber, int sfxId) {
		return (sfxId << 8) + playerNumber;
	}
}
