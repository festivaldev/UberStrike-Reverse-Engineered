using System;
using System.Collections.Generic;
using UnityEngine;

public class Avatar {
	private bool _isLocal;
	private Dictionary<LoadoutSlotType, BaseWeaponDecorator> _weapons;
	public Loadout Loadout { get; private set; }
	public AvatarDecorator Decorator { get; private set; }
	public AvatarDecoratorConfig Ragdoll { get; private set; }
	public LoadoutSlotType CurrentWeaponSlot { get; private set; }

	public Avatar(Loadout loadout, bool local) {
		_isLocal = local;
		_weapons = new Dictionary<LoadoutSlotType, BaseWeaponDecorator>();
		SetLoadout(loadout);
	}

	public event Action OnDecoratorChanged = delegate { };

	public void SetDecorator(AvatarDecorator decorator) {
		Decorator = decorator;
		OnDecoratorChanged();
	}

	public void SetLoadout(Loadout loadout) {
		if (Loadout != null) {
			Loadout.ClearAllSlots();
			Loadout.OnGearChanged -= RebuildDecorator;
			Loadout.OnWeaponChanged -= UpdateWeapon;
		}

		Loadout = loadout;
		Loadout.OnGearChanged += RebuildDecorator;
		Loadout.OnWeaponChanged += UpdateWeapon;
		RebuildDecorator();
	}

	public void RebuildDecorator() {
		if (!Decorator) {
			return;
		}

		var avatarGear = Loadout.GetAvatarGear();

		if (_isLocal) {
			AvatarBuilder.UpdateLocalAvatar(avatarGear);
		} else {
			SetDecorator(AvatarBuilder.UpdateRemoteAvatar(Decorator, avatarGear, Color.white));
		}
	}

	public void CleanupRagdoll() {
		if (Ragdoll) {
			AvatarBuilder.Destroy(Ragdoll.gameObject);
			Ragdoll = null;

			if (Decorator && Decorator.gameObject) {
				Decorator.gameObject.SetActive(true);
			}
		}
	}

	public void SpawnRagdoll(DamageInfo damageInfo) {
		var ragdollGear = Loadout.GetRagdollGear();
		Ragdoll = AvatarBuilder.InstantiateRagdoll(ragdollGear, Decorator.Configuration.GetSkinColor());

		try {
			ragdollGear.DestroyGearParts();
			Ragdoll.transform.position = Decorator.transform.position;
			Ragdoll.transform.rotation = Decorator.transform.rotation;
			AvatarDecoratorConfig.CopyBones(Decorator.Configuration, Ragdoll);

			foreach (var arrowProjectile in Decorator.GetComponentsInChildren<ArrowProjectile>(true)) {
				var localPosition = arrowProjectile.transform.localPosition;
				var localRotation = arrowProjectile.transform.localRotation;
				arrowProjectile.transform.parent = Ragdoll.GetBone(BoneIndex.Hips);
				arrowProjectile.transform.localPosition = localPosition;
				arrowProjectile.transform.localRotation = localRotation;
			}

			foreach (var rigidbody in Ragdoll.GetComponentsInChildren<Rigidbody>(true)) {
				if (rigidbody.gameObject.GetComponent<RagdollBodyPart>() == null) {
					rigidbody.gameObject.AddComponent<RagdollBodyPart>();
				}
			}

			Ragdoll.ApplyDamageToRagdoll(damageInfo);
			Decorator.gameObject.SetActive(false);
		} catch (Exception ex) {
			Debug.LogWarning(ex);
		}
	}

	public void UpdateAllWeapons() {
		UpdateWeapon(LoadoutSlotType.WeaponMelee);
		UpdateWeapon(LoadoutSlotType.WeaponPrimary);
		UpdateWeapon(LoadoutSlotType.WeaponSecondary);
		UpdateWeapon(LoadoutSlotType.WeaponTertiary);
	}

	private void UpdateWeapon(LoadoutSlotType slot) {
		IUnityItem unityItem;

		if (Loadout.TryGetItem(slot, out unityItem) && Decorator && Decorator.WeaponAttachPoint) {
			var gameObject = unityItem.Create(Decorator.WeaponAttachPoint.position, Decorator.WeaponAttachPoint.rotation);

			if (gameObject) {
				AssignWeapon(slot, gameObject.GetComponent<BaseWeaponDecorator>(), unityItem);
			}
		}
	}

	public void AssignWeapon(LoadoutSlotType slot, BaseWeaponDecorator weapon, IUnityItem item) {
		if (weapon) {
			BaseWeaponDecorator baseWeaponDecorator;

			if (_weapons.TryGetValue(slot, out baseWeaponDecorator) && baseWeaponDecorator) {
				UnityEngine.Object.Destroy(baseWeaponDecorator.gameObject);
			}

			_weapons[slot] = weapon;
			weapon.transform.parent = Decorator.WeaponAttachPoint;

			foreach (var transform in weapon.gameObject.transform.GetComponentsInChildren<Transform>(true)) {
				if (transform.gameObject.name == "Head") {
					transform.gameObject.name = "WeaponHead";
				}
			}

			LayerUtil.SetLayerRecursively(weapon.gameObject.transform, Decorator.gameObject.layer);
			weapon.transform.localPosition = Vector3.zero;
			weapon.transform.localRotation = Quaternion.identity;
			weapon.IsEnabled = slot == CurrentWeaponSlot;
			weapon.WeaponClass = item.View.ItemClass;
		} else {
			UnassignWeapon(slot);
		}
	}

	public void UnassignWeapon(LoadoutSlotType slot) {
		CurrentWeaponSlot = slot;
		BaseWeaponDecorator baseWeaponDecorator;

		if (_weapons.TryGetValue(slot, out baseWeaponDecorator) && baseWeaponDecorator) {
			UnityEngine.Object.Destroy(baseWeaponDecorator.gameObject);
		}

		_weapons.Remove(slot);
	}

	public void ShowWeapon(LoadoutSlotType slot) {
		CurrentWeaponSlot = slot;

		foreach (var keyValuePair in _weapons) {
			if (keyValuePair.Value) {
				keyValuePair.Value.IsEnabled = slot == keyValuePair.Key;

				if (slot == keyValuePair.Key) {
					Decorator.AnimationController.ChangeWeaponType(keyValuePair.Value.WeaponClass);
				}
			}
		}
	}

	public void ShowFirstWeapon() {
		foreach (var keyValuePair in _weapons) {
			if (keyValuePair.Value) {
				ShowWeapon(keyValuePair.Key);

				break;
			}
		}
	}

	public void HideWeapons() {
		foreach (var baseWeaponDecorator in _weapons.Values) {
			if (baseWeaponDecorator) {
				baseWeaponDecorator.IsEnabled = false;
			}
		}

		Decorator.AnimationController.ChangeWeaponType(0);
	}
}
