using System;
using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UnityEngine;

public class ItemToolTip : AutoMonoBehaviour<ItemToolTip> {
	private const int TextWidth = 80;
	private readonly Vector2 Size = new Vector2(260f, 240f);
	private FloatPropertyBar _accuracy = new FloatPropertyBar(LocalizedStrings.Accuracy);
	private float _alpha;
	private FloatPropertyBar _ammo = new FloatPropertyBar(LocalizedStrings.Ammo);
	private FloatPropertyBar _armorCarried = new FloatPropertyBar(LocalizedStrings.ArmorCarried);
	private Rect _cacheRect;
	private int _criticalHit;
	private FloatPropertyBar _damage = new FloatPropertyBar(LocalizedStrings.Damage);
	private FloatPropertyBar _damageRadius = new FloatPropertyBar(LocalizedStrings.Radius);
	private int _daysLeft;
	private string _description;
	private BuyingDurationType _duration;
	private Rect _finalRect = new Rect(0f, 0f, 260f, 230f);
	private FloatPropertyBar _fireRate = new FloatPropertyBar(LocalizedStrings.RateOfFire);
	private IUnityItem _item;
	private int _level;
	private Rect _rect = new Rect(0f, 0f, 260f, 230f);
	private FloatPropertyBar _velocity = new FloatPropertyBar(LocalizedStrings.Velocity);
	private Action OnDrawItemDetails;
	private Action OnDrawTip;
	public bool IsEnabled { get; private set; }

	private float Alpha {
		get { return Mathf.Clamp01(_alpha - Time.time); }
	}

	private void OnGUI() {
		_rect = _rect.Lerp(_finalRect, Time.deltaTime * 5f);

		if (IsEnabled && !PanelManager.IsAnyPanelOpen) {
			GUI.color = new Color(1f, 1f, 1f, Alpha);
			GUI.BeginGroup(_rect, BlueStonez.box_grey_outlined);
			_item.DrawIcon(new Rect(20f, 10f, 48f, 48f));
			GUI.Label(new Rect(75f, 15f, 200f, 30f), _item.View.Name, BlueStonez.label_interparkbold_13pt_left);
			GUI.Label(new Rect(20f, 70f, 220f, 50f), _description, BlueStonez.label_interparkmed_11pt_left);

			if (_duration != BuyingDurationType.None) {
				var guicontent = new GUIContent(ShopUtils.PrintDuration(_duration), ShopIcons.ItemexpirationIcon);
				GUI.Label(new Rect(75f, 40f, 200f, 20f), guicontent, BlueStonez.label_interparkbold_11pt_left);
			} else if (_daysLeft == 0) {
				var guicontent2 = new GUIContent(ShopUtils.PrintDuration(BuyingDurationType.Permanent), ShopIcons.ItemexpirationIcon);
				GUI.Label(new Rect(75f, 40f, 200f, 20f), guicontent2, BlueStonez.label_interparkmed_11pt_left);
			} else if (_daysLeft > 0) {
				var guicontent3 = new GUIContent(string.Format(LocalizedStrings.NDaysLeft, _daysLeft), ShopIcons.ItemexpirationIcon);
				GUI.Label(new Rect(75f, 40f, 200f, 20f), guicontent3, BlueStonez.label_interparkbold_11pt_left);
			}

			if (OnDrawItemDetails != null) {
				OnDrawItemDetails();
			}

			GUI.Label(new Rect(20f, 200f, 210f, 20f), LocalizedStrings.LevelRequired + _level, BlueStonez.label_interparkbold_11pt_left);

			if (_criticalHit > 0) {
				GUI.Label(new Rect(20f, 218f, 210f, 20f), LocalizedStrings.CriticalHitBonus + _criticalHit + "%", BlueStonez.label_interparkmed_11pt_left);
			}

			GUI.EndGroup();
			OnDrawTip();
			GUI.color = Color.white;

			if (_alpha - Time.time < 0f) {
				IsEnabled = false;
			}
		}
	}

	public void SetItem(IUnityItem item, Rect bounds, PopupViewSide side, int daysLeft = -1, BuyingDurationType duration = BuyingDurationType.None) {
		if (Event.current.type != EventType.Repaint || item == null || Singleton<ItemManager>.Instance.IsDefaultGearItem(item.View.PrefabName) || (item.View.LevelLock > PlayerDataManager.PlayerLevel && !Singleton<InventoryManager>.Instance.Contains(item.View.ID))) {
			return;
		}

		var flag = _alpha < Time.time + 0.1f;
		_alpha = Mathf.Lerp(_alpha, Time.time + 1.1f, Time.deltaTime * 12f);

		if (_item != item || _cacheRect != bounds || !IsEnabled) {
			_cacheRect = bounds;
			bounds = GUITools.ToGlobal(bounds);
			IsEnabled = true;
			_item = item;
			_level = ((item.View == null) ? 0 : item.View.LevelLock);
			_description = ((item.View == null) ? string.Empty : item.View.Description);
			_daysLeft = daysLeft;
			_criticalHit = 0;
			_duration = duration;

			switch (side) {
				case PopupViewSide.Left: {
					var tipPosition2 = bounds.y - 10f + bounds.height * 0.5f;
					var rect = new Rect(bounds.x - Size.x - 9f, bounds.y - Size.y * 0.5f, Size.x, Size.y);
					var rect2 = new Rect(rect.xMax - 1f, tipPosition2, 12f, 21f);

					if (rect.y <= GlobalUIRibbon.Instance.Height()) {
						rect.y += GlobalUIRibbon.Instance.Height() - rect.y;
					}

					if (rect.yMax >= Screen.height) {
						rect.y -= rect.yMax - Screen.height;
					}

					if (rect2.y < _finalRect.y || rect2.yMax > _finalRect.yMax || _finalRect.x != rect.x) {
						_finalRect = rect;

						if (flag) {
							_rect = rect;
						}
					}

					OnDrawTip = delegate { GUI.DrawTexture(new Rect(_rect.xMax - 1f, tipPosition2, 12f, 21f), ConsumableHudTextures.TooltipRight); };

					break;
				}
				case PopupViewSide.Top: {
					var tipPosition = bounds.x - 10f + bounds.width * 0.5f;
					var rect3 = new Rect(bounds.x + (bounds.width - Size.x) * 0.5f, bounds.y - Size.y - 9f, Size.x, Size.y);
					var rect4 = new Rect(tipPosition, rect3.yMax - 1f, 21f, 12f);

					if (rect3.xMin <= 10f) {
						rect3.x = 10f;
					}

					if (rect3.xMax >= Screen.width - 10) {
						rect3.x -= rect3.xMax - Screen.width + 10f;
					}

					if (rect4.x < _finalRect.x || rect4.xMax > _finalRect.xMax || _finalRect.y != rect3.y) {
						_finalRect = rect3;

						if (flag) {
							_rect = rect3;
						}
					}

					OnDrawTip = delegate { GUI.DrawTexture(new Rect(tipPosition, _rect.yMax - 1f, 21f, 12f), ConsumableHudTextures.TooltipDown); };

					break;
				}
			}

			switch (item.View.ItemClass) {
				case UberstrikeItemClass.WeaponMelee: {
					OnDrawItemDetails = DrawMeleeWeapon;
					var uberStrikeItemWeaponView = item.View as UberStrikeItemWeaponView;

					if (uberStrikeItemWeaponView != null) {
						_damage.Value = WeaponConfigurationHelper.GetDamage(uberStrikeItemWeaponView);
						_damage.Max = WeaponConfigurationHelper.MaxDamage;
						_fireRate.Value = WeaponConfigurationHelper.GetRateOfFire(uberStrikeItemWeaponView);
						_fireRate.Max = WeaponConfigurationHelper.MaxRateOfFire;
					}

					return;
				}
				case UberstrikeItemClass.WeaponMachinegun:
				case UberstrikeItemClass.WeaponShotgun:
				case UberstrikeItemClass.WeaponSniperRifle: {
					OnDrawItemDetails = DrawInstantHitWeapon;
					var uberStrikeItemWeaponView2 = item.View as UberStrikeItemWeaponView;

					if (uberStrikeItemWeaponView2 != null) {
						_ammo.Value = WeaponConfigurationHelper.GetAmmo(uberStrikeItemWeaponView2);
						_ammo.Max = WeaponConfigurationHelper.MaxAmmo;
						_damage.Value = WeaponConfigurationHelper.GetDamage(uberStrikeItemWeaponView2);
						_damage.Max = WeaponConfigurationHelper.MaxDamage;
						_fireRate.Value = WeaponConfigurationHelper.GetRateOfFire(uberStrikeItemWeaponView2);
						_fireRate.Max = WeaponConfigurationHelper.MaxRateOfFire;
						_accuracy.Value = WeaponConfigurationHelper.MaxAccuracySpread - WeaponConfigurationHelper.GetAccuracySpread(uberStrikeItemWeaponView2);
						_accuracy.Max = WeaponConfigurationHelper.MaxAccuracySpread;

						if (item.View.ItemProperties.ContainsKey(ItemPropertyType.CritDamageBonus)) {
							_criticalHit = item.View.ItemProperties[ItemPropertyType.CritDamageBonus];
						} else {
							_criticalHit = 0;
						}
					}

					return;
				}
				case UberstrikeItemClass.WeaponCannon:
				case UberstrikeItemClass.WeaponSplattergun:
				case UberstrikeItemClass.WeaponLauncher: {
					OnDrawItemDetails = DrawProjectileWeapon;
					var uberStrikeItemWeaponView3 = item.View as UberStrikeItemWeaponView;

					if (uberStrikeItemWeaponView3 != null) {
						_ammo.Value = WeaponConfigurationHelper.GetAmmo(uberStrikeItemWeaponView3);
						_ammo.Max = WeaponConfigurationHelper.MaxAmmo;
						_damage.Value = WeaponConfigurationHelper.GetDamage(uberStrikeItemWeaponView3);
						_damage.Max = WeaponConfigurationHelper.MaxDamage;
						_fireRate.Value = WeaponConfigurationHelper.GetRateOfFire(uberStrikeItemWeaponView3);
						_fireRate.Max = WeaponConfigurationHelper.MaxRateOfFire;
						_velocity.Value = WeaponConfigurationHelper.GetProjectileSpeed(uberStrikeItemWeaponView3);
						_velocity.Max = WeaponConfigurationHelper.MaxProjectileSpeed;
						_damageRadius.Value = WeaponConfigurationHelper.GetSplashRadius(uberStrikeItemWeaponView3);
						_damageRadius.Max = WeaponConfigurationHelper.MaxSplashRadius;
					}

					return;
				}
				case UberstrikeItemClass.GearBoots:
				case UberstrikeItemClass.GearHead:
				case UberstrikeItemClass.GearFace:
				case UberstrikeItemClass.GearUpperBody:
				case UberstrikeItemClass.GearLowerBody:
				case UberstrikeItemClass.GearGloves:
				case UberstrikeItemClass.GearHolo:
					OnDrawItemDetails = DrawGear;
					_armorCarried.Value = ((UberStrikeItemGearView)item.View).ArmorPoints;
					_armorCarried.Max = 200f;

					return;
				case UberstrikeItemClass.QuickUseGeneral:
				case UberstrikeItemClass.QuickUseGrenade:
				case UberstrikeItemClass.QuickUseMine:
					OnDrawItemDetails = DrawQuickItem;

					return;
			}

			OnDrawItemDetails = null;
		}
	}

	public void ComparisonOverlay(Rect position, float value, float otherValue) {
		var num = position.width - 80f - 50f;
		var num2 = (num - 4f) * Mathf.Clamp01(value);
		var num3 = (num - 4f) * Mathf.Clamp01(Mathf.Abs(value - otherValue));
		GUI.BeginGroup(position);

		if (value < otherValue) {
			GUI.color = Color.green.SetAlpha(Alpha * 0.9f);
			GUI.Label(new Rect(82f + num2, 3f, num3, 8f), string.Empty, BlueStonez.progressbar_thumb);
		} else {
			GUI.color = Color.red.SetAlpha(Alpha * 0.9f);
			GUI.Label(new Rect(82f + num2 - num3, 3f, num3, 8f), string.Empty, BlueStonez.progressbar_thumb);
		}

		GUI.color = new Color(1f, 1f, 1f, Alpha);
		GUI.EndGroup();
	}

	public void ProgressBar(Rect position, string text, float percentage, Color barColor, string value) {
		var num = position.width - 80f - 50f;
		GUI.BeginGroup(position);
		GUI.Label(new Rect(0f, 0f, 80f, 14f), text, BlueStonez.label_interparkbold_11pt_left);
		GUI.Label(new Rect(80f, 1f, num, 12f), GUIContent.none, BlueStonez.progressbar_background);
		GUI.color = barColor.SetAlpha(Alpha);
		GUI.Label(new Rect(82f, 3f, (num - 4f) * Mathf.Clamp01(percentage), 8f), string.Empty, BlueStonez.progressbar_thumb);
		GUI.color = new Color(1f, 1f, 1f, Alpha);

		if (!string.IsNullOrEmpty(value)) {
			GUI.Label(new Rect(80f + num + 10f, 0f, 40f, 14f), value, BlueStonez.label_interparkmed_10pt_left);
		}

		GUI.EndGroup();
	}

	private void DrawGear() {
		ProgressBar(new Rect(20f, 120f, 200f, 12f), _armorCarried.Title, _armorCarried.Percent, ColorScheme.ProgressBar, string.Empty);
	}

	private void DrawProjectileWeapon() {
		var flag = Singleton<DragAndDrop>.Instance.IsDragging && ShopUtils.IsProjectileWeapon(Singleton<DragAndDrop>.Instance.DraggedItem.Item) && Singleton<DragAndDrop>.Instance.DraggedItem.Item.View.ItemClass == _item.View.ItemClass;
		ProgressBar(new Rect(20f, 120f, 200f, 12f), _damage.Title, _damage.Percent, ColorScheme.ProgressBar, string.Empty);
		ProgressBar(new Rect(20f, 135f, 200f, 12f), _fireRate.Title, 1f - _fireRate.Percent, ColorScheme.ProgressBar, string.Empty);
		ProgressBar(new Rect(20f, 150f, 200f, 12f), _velocity.Title, _velocity.Percent, ColorScheme.ProgressBar, string.Empty);
		ProgressBar(new Rect(20f, 165f, 200f, 12f), _damageRadius.Title, _damageRadius.Percent, ColorScheme.ProgressBar, string.Empty);
		ProgressBar(new Rect(20f, 180f, 200f, 12f), _ammo.Title, _ammo.Percent, ColorScheme.ProgressBar, string.Empty);

		if (flag) {
			var uberStrikeItemWeaponView = Singleton<DragAndDrop>.Instance.DraggedItem.Item.View as UberStrikeItemWeaponView;
			ComparisonOverlay(new Rect(20f, 120f, 200f, 12f), _damage.Percent, WeaponConfigurationHelper.GetDamageNormalized(uberStrikeItemWeaponView));
			ComparisonOverlay(new Rect(20f, 135f, 200f, 12f), 1f - _fireRate.Percent, 1f - WeaponConfigurationHelper.GetRateOfFireNormalized(uberStrikeItemWeaponView));
			ComparisonOverlay(new Rect(20f, 150f, 200f, 12f), _velocity.Percent, WeaponConfigurationHelper.GetProjectileSpeedNormalized(uberStrikeItemWeaponView));
			ComparisonOverlay(new Rect(20f, 165f, 200f, 12f), _damageRadius.Percent, WeaponConfigurationHelper.GetSplashRadiusNormalized(uberStrikeItemWeaponView));
		}
	}

	private void DrawInstantHitWeapon() {
		var flag = Singleton<DragAndDrop>.Instance.IsDragging && ShopUtils.IsInstantHitWeapon(Singleton<DragAndDrop>.Instance.DraggedItem.Item) && Singleton<DragAndDrop>.Instance.DraggedItem.Item.View.ItemClass == _item.View.ItemClass;
		ProgressBar(new Rect(20f, 120f, 200f, 12f), _damage.Title, _damage.Percent, ColorScheme.ProgressBar, string.Empty);
		ProgressBar(new Rect(20f, 135f, 200f, 12f), _fireRate.Title, 1f - _fireRate.Percent, ColorScheme.ProgressBar, string.Empty);
		ProgressBar(new Rect(20f, 150f, 200f, 12f), _accuracy.Title, _accuracy.Percent, ColorScheme.ProgressBar, string.Empty);
		ProgressBar(new Rect(20f, 165f, 200f, 12f), _ammo.Title, _ammo.Percent, ColorScheme.ProgressBar, string.Empty);

		if (flag) {
			var uberStrikeItemWeaponView = Singleton<DragAndDrop>.Instance.DraggedItem.Item.View as UberStrikeItemWeaponView;
			ComparisonOverlay(new Rect(20f, 120f, 200f, 12f), _damage.Percent, WeaponConfigurationHelper.GetDamageNormalized(uberStrikeItemWeaponView));
			ComparisonOverlay(new Rect(20f, 135f, 200f, 12f), 1f - _fireRate.Percent, 1f - WeaponConfigurationHelper.GetRateOfFireNormalized(uberStrikeItemWeaponView));
			ComparisonOverlay(new Rect(20f, 150f, 200f, 12f), _accuracy.Percent, 1f - WeaponConfigurationHelper.GetAccuracySpreadNormalized(uberStrikeItemWeaponView));
		}
	}

	private void DrawMeleeWeapon() {
		ProgressBar(new Rect(20f, 120f, 200f, 12f), _damage.Title, _damage.Percent, ColorScheme.ProgressBar, string.Empty);
		ProgressBar(new Rect(20f, 135f, 200f, 12f), _fireRate.Title, 1f - _fireRate.Percent, ColorScheme.ProgressBar, string.Empty);

		if (Singleton<DragAndDrop>.Instance.IsDragging && ShopUtils.IsMeleeWeapon(Singleton<DragAndDrop>.Instance.DraggedItem.Item)) {
			var uberStrikeItemWeaponView = Singleton<DragAndDrop>.Instance.DraggedItem.Item.View as UberStrikeItemWeaponView;
			ComparisonOverlay(new Rect(20f, 120f, 200f, 12f), _damage.Percent, WeaponConfigurationHelper.GetDamageNormalized(uberStrikeItemWeaponView));
			ComparisonOverlay(new Rect(20f, 135f, 200f, 12f), 1f - _fireRate.Percent, 1f - WeaponConfigurationHelper.GetRateOfFireNormalized(uberStrikeItemWeaponView));
		}
	}

	private void DrawQuickItem() {
		if (_item != null) {
			var quickItemConfiguration = _item.View as QuickItemConfiguration;

			if (_item.View is HealthBuffConfiguration) {
				var healthBuffConfiguration = _item.View as HealthBuffConfiguration;
				GUI.Label(new Rect(20f, 102f, 200f, 20f), LocalizedStrings.HealthColon + healthBuffConfiguration.GetHealthBonusDescription(), BlueStonez.label_interparkbold_11pt_left);
				GUI.Label(new Rect(20f, 117f, 200f, 20f), LocalizedStrings.TimeColon + ((healthBuffConfiguration.IncreaseTimes <= 0) ? LocalizedStrings.Instant : ((healthBuffConfiguration.IncreaseFrequency * healthBuffConfiguration.IncreaseTimes / 1000f).ToString("f1") + "s")), BlueStonez.label_interparkbold_11pt_left);
			} else if (_item.View is AmmoBuffConfiguration) {
				var ammoBuffConfiguration = _item.View as AmmoBuffConfiguration;
				GUI.Label(new Rect(20f, 102f, 200f, 20f), LocalizedStrings.AmmoColon + ammoBuffConfiguration.GetAmmoBonusDescription(), BlueStonez.label_interparkbold_11pt_left);
				GUI.Label(new Rect(20f, 117f, 200f, 20f), LocalizedStrings.TimeColon + ((ammoBuffConfiguration.IncreaseTimes <= 0) ? LocalizedStrings.Instant : ((ammoBuffConfiguration.IncreaseFrequency * ammoBuffConfiguration.IncreaseTimes / 1000f).ToString("f1") + "s")), BlueStonez.label_interparkbold_11pt_left);
			} else if (_item.View is ArmorBuffConfiguration) {
				var armorBuffConfiguration = _item.View as ArmorBuffConfiguration;
				GUI.Label(new Rect(20f, 102f, 200f, 20f), LocalizedStrings.ArmorColon + armorBuffConfiguration.GetArmorBonusDescription(), BlueStonez.label_interparkbold_11pt_left);
				GUI.Label(new Rect(20f, 117f, 200f, 20f), LocalizedStrings.TimeColon + ((armorBuffConfiguration.IncreaseTimes <= 0) ? LocalizedStrings.Instant : ((armorBuffConfiguration.IncreaseFrequency * armorBuffConfiguration.IncreaseTimes / 1000f).ToString("f1") + "s")), BlueStonez.label_interparkbold_11pt_left);
			} else if (_item.View is ExplosiveGrenadeConfiguration) {
				var explosiveGrenadeConfiguration = _item.View as ExplosiveGrenadeConfiguration;
				GUI.Label(new Rect(20f, 102f, 200f, 20f), LocalizedStrings.DamageColon + explosiveGrenadeConfiguration.Damage + "HP", BlueStonez.label_interparkbold_11pt_left);
				GUI.Label(new Rect(20f, 117f, 200f, 20f), LocalizedStrings.RadiusColon + explosiveGrenadeConfiguration.SplashRadius + "m", BlueStonez.label_interparkbold_11pt_left);
			} else if (_item.View is SpringGrenadeConfiguration) {
				var springGrenadeConfiguration = _item.View as SpringGrenadeConfiguration;
				GUI.Label(new Rect(20f, 102f, 200f, 20f), LocalizedStrings.ForceColon + springGrenadeConfiguration.Force, BlueStonez.label_interparkbold_11pt_left);
				GUI.Label(new Rect(20f, 117f, 200f, 20f), LocalizedStrings.LifetimeColon + springGrenadeConfiguration.LifeTime + "s", BlueStonez.label_interparkbold_11pt_left);
			}

			if (quickItemConfiguration != null) {
				GUI.Label(new Rect(20f, 132f, 200f, 20f), LocalizedStrings.WarmupColon + ((quickItemConfiguration.WarmUpTime <= 0) ? LocalizedStrings.Instant : ((quickItemConfiguration.WarmUpTime / 1000f).ToString("f1") + "s")), BlueStonez.label_interparkbold_11pt_left);
				GUI.Label(new Rect(20f, 147f, 200f, 20f), LocalizedStrings.CooldownColon + (quickItemConfiguration.CoolDownTime / 1000f).ToString("f1") + "s", BlueStonez.label_interparkbold_11pt_left);
				GUI.Label(new Rect(20f, 162f, 200f, 20f), LocalizedStrings.UsesPerLifeColon + ((quickItemConfiguration.UsesPerLife <= 0) ? LocalizedStrings.Unlimited : quickItemConfiguration.UsesPerLife.ToString()), BlueStonez.label_interparkbold_11pt_left);
				GUI.Label(new Rect(20f, 177f, 200f, 20f), LocalizedStrings.UsesPerGameColon + ((quickItemConfiguration.UsesPerGame <= 0) ? LocalizedStrings.Unlimited : quickItemConfiguration.UsesPerGame.ToString()), BlueStonez.label_interparkbold_11pt_left);
			}
		}
	}

	private class FloatPropertyBar {
		private float _lastValue;
		private float _max = 1f;
		private float _time;
		private float _value;
		public string Title { get; private set; }

		public float SmoothValue {
			get { return Mathf.Lerp(_lastValue, Value, (Time.time - _time) * 5f); }
		}

		public float Value {
			get { return _value; }
			set {
				_lastValue = _value;
				_time = Time.time;
				_value = value;
			}
		}

		public float Percent {
			get { return SmoothValue / Max; }
		}

		public float Max {
			get { return _max; }
			set { _max = Mathf.Max(value, 1f); }
		}

		public FloatPropertyBar(string title) {
			Title = title;
		}
	}
}
