using System;
using System.Collections;
using System.Collections.Generic;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class WeaponController : Singleton<WeaponController>, IWeaponController {
	private const float _upwardsForceMultiplier = 10f;
	private CircularInteger _currentSlotID;
	private float _holsterTime;
	private Dictionary<LoadoutSlotType, IUnityItem> _loadoutWeapons = new Dictionary<LoadoutSlotType, IUnityItem>();
	private int _projectileId;
	private WeaponSlot _weapon_current;
	private WeaponSlot[] _weapons;
	public Property<LoadoutSlotType> SelectedLoadout = new Property<LoadoutSlotType>();

	public Dictionary<LoadoutSlotType, IUnityItem> LoadoutWeapons {
		get { return _loadoutWeapons; }
	}

	private WeaponSlot _currentSlot {
		get { return _weapon_current; }
		set {
			_weapon_current = value;
			GameState.Current.PlayerData.ActiveWeapon.Value = _weapon_current;
		}
	}

	public bool HasAnyWeapon {
		get {
			foreach (var weaponSlot in _weapons) {
				if (weaponSlot != null) {
					return true;
				}
			}

			return false;
		}
	}

	public BaseWeaponDecorator CurrentWeapon {
		get {
			if (IsWeaponValid) {
				return _currentSlot.Decorator;
			}

			return null;
		}
	}

	public bool IsWeaponValid {
		get { return _currentSlot != null && _currentSlot.Logic != null && _currentSlot.Decorator != null; }
	}

	public bool IsWeaponReady {
		get { return IsWeaponValid && _currentSlot.InputHandler.FireHandler.CanShoot && _currentSlot.Logic.IsWeaponActive; }
	}

	public bool IsSecondaryAction {
		get { return _currentSlot != null && !_currentSlot.InputHandler.CanChangeWeapon(); }
	}

	public bool IsEnabled { get; set; }

	public LoadoutSlotType CurrentSlot {
		get { return (_currentSlot == null) ? LoadoutSlotType.None : _currentSlot.Slot; }
	}

	private WeaponController() {
		_weapons = new WeaponSlot[4];
		_currentSlotID = new CircularInteger(0, 3);
		IsEnabled = true;
		EventHandler.Global.AddListener(new Action<GlobalEvents.InputChanged>(OnInputChanged));
	}

	public byte PlayerNumber {
		get { return GameState.Current.PlayerData.Player.PlayerId; }
	}

	public int Cmid {
		get { return PlayerDataManager.Cmid; }
	}

	public bool IsLocal {
		get { return true; }
	}

	public void UpdateWeaponDecorator(IUnityItem item) { }

	public int NextProjectileId() {
		return ProjectileManager.CreateGlobalProjectileID(PlayerNumber, ++_projectileId);
	}

	public void LateUpdate() {
		if (_holsterTime > 0f) {
			_holsterTime = Mathf.Max(_holsterTime - Time.deltaTime, 0f);
		}

		if (_currentSlot != _weapons[_currentSlotID.Current] && _holsterTime == 0f) {
			_currentSlot = _weapons[_currentSlotID.Current];
			PutdownCurrentWeapon();
			GameState.Current.PlayerData.SwitchWeaponSlot(_currentSlotID.Current);

			if (_currentSlot.Logic != null && _currentSlot.Decorator != null) {
				WeaponFeedbackManager.Instance.PickUp(_currentSlot);
				_currentSlot.Decorator.PlayEquipSound();
			}
		}

		if (CheckPerformShotConditions() && _currentSlot != null && _currentSlot.HasWeapon) {
			_currentSlot.InputHandler.Update();
		}
	}

	private void SetSlotWeapon(LoadoutSlotType slot, IUnityItem weapon) {
		if (weapon != null) {
			_loadoutWeapons[slot] = weapon;
		} else if (_loadoutWeapons.ContainsKey(slot)) {
			_loadoutWeapons.Remove(slot);
		}
	}

	public void NextWeapon() {
		if (!HasAnyWeapon) {
			return;
		}

		var num = _currentSlotID.Current;
		var num2 = _currentSlotID.Next;

		while (_weapons[num2] == null) {
			num2 = _currentSlotID.Next;
		}

		if (num2 != num) {
			if (_currentSlot != null && _currentSlot.InputHandler != null) {
				_currentSlot.InputHandler.Stop();
				_currentSlot = null;
			}

			GameState.Current.PlayerData.NextActiveWeapon.Value = _weapons[num2];
		}
	}

	public void PrevWeapon() {
		if (!HasAnyWeapon) {
			return;
		}

		var num = _currentSlotID.Current;
		var num2 = _currentSlotID.Prev;

		while (_weapons[num2] == null) {
			num2 = _currentSlotID.Prev;
		}

		if (num2 != num) {
			if (_currentSlot != null && _currentSlot.InputHandler != null) {
				_currentSlot.InputHandler.Stop();
				_currentSlot = null;
			}

			GameState.Current.PlayerData.NextActiveWeapon.Value = _weapons[num2];
		}
	}

	public void ShowFirstWeapon() {
		_currentSlotID.Reset();

		if (!HasAnyWeapon) {
			return;
		}

		if (_currentSlot != null && _currentSlot.InputHandler != null) {
			_currentSlot.InputHandler.Stop();
			_currentSlot = null;
		}

		var num = _currentSlotID.Next;

		while (_weapons[num] == null) {
			num = _currentSlotID.Next;
		}
	}

	public bool CheckWeapons(List<int> weaponIds) {
		if (_weapons.Length != weaponIds.Count) {
			return false;
		}

		for (var i = 0; i < weaponIds.Count; i++) {
			if (_weapons[i] == null && weaponIds[i] != 0) {
				return false;
			}

			if (_weapons[i] != null && _weapons[i].View.ID != weaponIds[i]) {
				return false;
			}
		}

		return true;
	}

	public void PutdownCurrentWeapon() {
		WeaponFeedbackManager.Instance.PutDown();
	}

	public void PickupCurrentWeapon() {
		if (_currentSlot != null) {
			WeaponFeedbackManager.Instance.PickUp(_currentSlot);
		}
	}

	public bool CheckAmmoCount() {
		return AmmoDepot.HasAmmoOfClass(_currentSlot.View.ItemClass);
	}

	public bool Shoot() {
		var flag = false;

		if (IsWeaponReady) {
			if (CheckAmmoCount()) {
				_currentSlot.InputHandler.FireHandler.RegisterShot();
				_holsterTime = WeaponConfigurationHelper.GetRateOfFire(_currentSlot.View);
				var ray = new Ray(GameState.Current.PlayerData.ShootingPoint + GameState.Current.Player.EyePosition, GameState.Current.PlayerData.ShootingDirection);
				CmunePairList<BaseGameProp, ShotPoint> cmunePairList;
				_currentSlot.Logic.Shoot(ray, out cmunePairList);

				if (!_currentSlot.Decorator.HasShootAnimation) {
					WeaponFeedbackManager.Instance.Fire();
				}

				AmmoDepot.UseAmmoOfClass(_currentSlot.View.ItemClass, _currentSlot.Logic.AmmoCountPerShot);
				GameState.Current.PlayerData.WeaponFired.Value = _currentSlot;
				flag = true;
			} else {
				_currentSlot.Decorator.PlayOutOfAmmoSound();
				GameData.Instance.OnNotificationFull.Fire(string.Empty, "Out of ammo!", 1f);
			}
		}

		return flag;
	}

	public WeaponSlot GetPrimaryWeapon() {
		return _weapons[1];
	}

	public WeaponSlot GetCurrentWeapon() {
		return _currentSlot;
	}

	public void InitializeAllWeapons(Transform attachPoint) {
		for (var i = 0; i < _weapons.Length; i++) {
			if (_weapons[i] != null && _weapons[i].Decorator != null) {
				UnityEngine.Object.Destroy(_weapons[i].Decorator.gameObject);
			}

			_weapons[i] = null;
		}

		for (var j = 0; j < LoadoutManager.WeaponSlots.Length; j++) {
			var loadoutSlotType = LoadoutManager.WeaponSlots[j];
			InventoryItem inventoryItem;

			if (Singleton<LoadoutManager>.Instance.TryGetItemInSlot(loadoutSlotType, out inventoryItem)) {
				var weaponSlot = new WeaponSlot(loadoutSlotType, inventoryItem.Item, attachPoint, this);
				AddGameLogicToWeapon(weaponSlot);
				_weapons[j] = weaponSlot;
				AmmoDepot.SetMaxAmmoForType(inventoryItem.Item.View.ItemClass, ((UberStrikeItemWeaponView)inventoryItem.Item.View).MaxAmmo);
				AmmoDepot.SetStartAmmoForType(inventoryItem.Item.View.ItemClass, ((UberStrikeItemWeaponView)inventoryItem.Item.View).StartAmmo);
				SetSlotWeapon(loadoutSlotType, inventoryItem.Item);
			} else {
				SetSlotWeapon(loadoutSlotType, null);
			}
		}

		GameState.Current.PlayerData.LoadoutWeapons.Value = LoadoutWeapons;
		Singleton<QuickItemController>.Instance.Initialize();
		Reset();
	}

	public void Reset() {
		AmmoDepot.Reset();
		_currentSlotID.SetRange(0, 3);
		_currentSlot = null;
		ShowFirstWeapon();
	}

	public bool HasWeaponOfClass(UberstrikeItemClass itemClass) {
		for (var i = 0; i < 4; i++) {
			var weaponSlot = _weapons[i];

			if (weaponSlot != null && weaponSlot.HasWeapon && weaponSlot.View.ItemClass == itemClass) {
				return true;
			}
		}

		return false;
	}

	public void StopInputHandler() {
		if (_currentSlot != null) {
			_currentSlot.InputHandler.Stop();
		}
	}

	private int GetWeaponCount() {
		var num = 0;

		foreach (var weaponSlot in _weapons) {
			if (weaponSlot != null) {
				num++;
			}
		}

		return num;
	}

	private void OnInputChanged(GlobalEvents.InputChanged ev) {
		if (AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled && CheckPerformShotConditions()) {
			switch (ev.Key) {
				case GameInputKey.PrimaryFire:
					OnPrimaryFire(ev);

					break;
				case GameInputKey.SecondaryFire:
					OnSecondaryFire(ev);

					break;
				case GameInputKey.Weapon1:
					OnSelectWeapon(ev, LoadoutSlotType.WeaponPrimary);

					break;
				case GameInputKey.Weapon2:
					OnSelectWeapon(ev, LoadoutSlotType.WeaponSecondary);

					break;
				case GameInputKey.Weapon3:
					OnSelectWeapon(ev, LoadoutSlotType.WeaponTertiary);

					break;
				case GameInputKey.WeaponMelee:
					OnSelectWeapon(ev, LoadoutSlotType.WeaponMelee);

					break;
				case GameInputKey.NextWeapon:
					OnNextWeapon(ev);

					break;
				case GameInputKey.PrevWeapon:
					OnPrevWeapon(ev);

					break;
			}
		}
	}

	private void OnSelectWeapon(GlobalEvents.InputChanged ev, LoadoutSlotType slotType) {
		if (ev.IsDown && !LevelCamera.IsZoomedIn && slotType != _weapons[_currentSlotID.Current].Slot && GetWeaponCount() > 1) {
			for (var i = 0; i < _weapons.Length; i++) {
				if (_weapons[i] != null && _weapons[i].Slot == slotType && _weapons[i] != _currentSlot) {
					if (_currentSlot != null) {
						_currentSlot.InputHandler.Stop();
						_currentSlot = null;
					}

					_currentSlotID.Current = i;
					GameState.Current.PlayerData.NextActiveWeapon.Value = _weapons[i];
				}
			}
		}
	}

	private void OnPrevWeapon(GlobalEvents.InputChanged ev) {
		if ((_currentSlot == null || (ev.IsDown && _currentSlot.InputHandler.CanChangeWeapon())) && GUITools.SaveClickIn(0.2f)) {
			GUITools.Clicked();
			NextWeapon();
		} else if (_currentSlot != null && ev.IsDown) {
			_currentSlot.InputHandler.OnPrevWeapon();
		}
	}

	private void OnNextWeapon(GlobalEvents.InputChanged ev) {
		if ((_currentSlot == null || (ev.IsDown && _currentSlot.InputHandler.CanChangeWeapon())) && GUITools.SaveClickIn(0.2f)) {
			GUITools.Clicked();
			PrevWeapon();
		} else if (_currentSlot != null && ev.IsDown) {
			_currentSlot.InputHandler.OnNextWeapon();
		}
	}

	private void OnPrimaryFire(GlobalEvents.InputChanged ev) {
		if (ev.IsDown) {
			if (_currentSlot != null && _currentSlot.HasWeapon) {
				_currentSlot.InputHandler.OnPrimaryFire(true);
			}
		} else if (_currentSlot != null) {
			_currentSlot.InputHandler.OnPrimaryFire(false);
		}
	}

	private void OnSecondaryFire(GlobalEvents.InputChanged ev) {
		if (GameState.Current.PlayerData.IsAlive && IsEnabled && _currentSlot != null && _currentSlot.HasWeapon) {
			_currentSlot.InputHandler.OnSecondaryFire(ev.IsDown);
		}
	}

	private bool CheckPerformShotConditions() {
		return IsEnabled && GameState.Current.Player != null && GameState.Current.Player.EnableWeaponControl && !GameState.Current.IsPlayerPaused && !GameState.Current.IsPlayerDead;
	}

	private IEnumerator StartHidingWeapon(GameObject weapon, bool destroy) {
		for (var time = 0f; time < 2f; time += Time.deltaTime) {
			yield return new WaitForEndOfFrame();
		}

		if (destroy) {
			UnityEngine.Object.Destroy(weapon);
		}
	}

	private IEnumerator StartApplyDamage(WeaponSlot slot, float delay, CmunePairList<BaseGameProp, ShotPoint> hits) {
		yield return new WaitForSeconds(delay);

		ApplyDamage(slot, hits);
	}

	private void ApplyDamage(WeaponSlot slot, CmunePairList<BaseGameProp, ShotPoint> hits) {
		foreach (var keyValuePair in hits) {
			var damageInfo = new DamageInfo(Convert.ToInt16(slot.View.DamagePerProjectile * keyValuePair.Value.Count)) {
				Bullets = (byte)keyValuePair.Value.Count,
				Force = GameState.Current.Player.WeaponCamera.transform.forward * (slot.View.DamagePerProjectile * keyValuePair.Value.Count),
				UpwardsForceMultiplier = 10f,
				Hitpoint = keyValuePair.Value.MidPoint,
				ProjectileID = keyValuePair.Value.ProjectileId,
				SlotId = slot.SlotId,
				WeaponID = slot.View.ID,
				WeaponClass = slot.View.ItemClass,
				CriticalStrikeBonus = WeaponConfigurationHelper.GetCriticalStrikeBonus(slot.View)
			};

			if (keyValuePair.Key != null) {
				keyValuePair.Key.ApplyDamage(damageInfo);
			}
		}
	}

	private void AddGameLogicToWeapon(WeaponSlot weapon) {
		var movement = WeaponConfigurationHelper.GetRecoilMovement(weapon.View);
		var kickback = WeaponConfigurationHelper.GetRecoilKickback(weapon.View);
		var slot = weapon.Slot;

		if (weapon.Logic is ProjectileWeapon) {
			var w = weapon.Logic as ProjectileWeapon;

			w.OnProjectileShoot += delegate(ProjectileInfo p) {
				var projectileDetonator = new ProjectileDetonator(WeaponConfigurationHelper.GetSplashRadius(weapon.View), weapon.View.DamagePerProjectile, weapon.View.DamageKnockback, p.Direction, weapon.SlotId, p.Id, weapon.View.ID, weapon.View.ItemClass, w.Config.DamageEffectFlag, w.Config.DamageEffectValue);

				if (p.Projectile != null) {
					p.Projectile.Detonator = projectileDetonator;

					if (weapon.View.ItemClass != UberstrikeItemClass.WeaponSplattergun) {
						GameState.Current.Actions.EmitProjectile(p.Position, p.Direction, slot, p.Id, false);
					}
				} else {
					projectileDetonator.Explode(p.Position);

					if (weapon.View.ItemClass != UberstrikeItemClass.WeaponSplattergun) {
						GameState.Current.Actions.EmitProjectile(p.Position, p.Direction, slot, p.Id, true);
					}
				}

				if (weapon.View.ItemClass != UberstrikeItemClass.WeaponSplattergun) {
					if (w.HasProjectileLimit) {
						Singleton<ProjectileManager>.Instance.AddLimitedProjectile(p.Projectile, p.Id, w.MaxConcurrentProjectiles);
					} else {
						Singleton<ProjectileManager>.Instance.AddProjectile(p.Projectile, p.Id);
					}
				}

				LevelCamera.DoFeedback(LevelCamera.FeedbackType.ShootWeapon, Vector3.back, 0f, movement / 8f, 0.1f, 0.3f, kickback / 3f, Vector3.left);
			};
		} else if (weapon.Logic is MeleeWeapon) {
			var delay = weapon.Logic.HitDelay;

			weapon.Logic.OnTargetHit += delegate(CmunePairList<BaseGameProp, ShotPoint> h) {
				if (!weapon.View.HasAutomaticFire) {
					GameState.Current.Actions.SingleBulletFire();
				}

				if (h != null) {
					UnityRuntime.StartRoutine(StartApplyDamage(weapon, delay, h));
				}

				LevelCamera.DoFeedback(LevelCamera.FeedbackType.ShootWeapon, Vector3.back, 0f, movement / 8f, 0.1f, 0.3f, kickback / 3f, Vector3.left);
			};
		} else {
			weapon.Logic.OnTargetHit += delegate(CmunePairList<BaseGameProp, ShotPoint> h) {
				if (!weapon.View.HasAutomaticFire) {
					GameState.Current.Actions.SingleBulletFire();
				}

				if (h != null) {
					ApplyDamage(weapon, h);
				}

				LevelCamera.DoFeedback(LevelCamera.FeedbackType.ShootWeapon, Vector3.back, 0f, movement / 8f, 0.1f, 0.3f, kickback / 3f, Vector3.left);
			};
		}
	}
}
