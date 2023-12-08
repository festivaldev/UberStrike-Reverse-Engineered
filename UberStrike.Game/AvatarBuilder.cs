using UnityEngine;

public static class AvatarBuilder {
	public static void Destroy(GameObject obj) {
		var componentsInChildren = obj.GetComponentsInChildren<Renderer>();

		foreach (var renderer in componentsInChildren) {
			foreach (var material in renderer.materials) {
				UnityEngine.Object.Destroy(material);
			}
		}

		var componentInChildren = obj.GetComponentInChildren<SkinnedMeshRenderer>();

		if (componentInChildren) {
			UnityEngine.Object.Destroy(componentInChildren.sharedMesh);
		}

		UnityEngine.Object.Destroy(obj);
	}

	public static AvatarDecorator CreateLocalAvatar() {
		var avatarDecorator = CreateAvatarMesh(Singleton<LoadoutManager>.Instance.Loadout.GetAvatarGear());
		UnityEngine.Object.DontDestroyOnLoad(avatarDecorator.gameObject);
		SetupLocalAvatar(avatarDecorator);

		return avatarDecorator;
	}

	public static void UpdateLocalAvatar(AvatarGearParts gear) {
		if (GameState.Current.Avatar.Decorator.name != gear.Base.name) {
			var decorator = GameState.Current.Avatar.Decorator;
			var avatarDecorator = CreateAvatarMesh(gear);
			UnityEngine.Object.DontDestroyOnLoad(avatarDecorator.gameObject);
			avatarDecorator.transform.ShareParent(decorator.transform);
			avatarDecorator.gameObject.SetActive(decorator.gameObject.activeSelf);
			ReparentChildren(decorator.WeaponAttachPoint, avatarDecorator.WeaponAttachPoint);
			UnityEngine.Object.Destroy(decorator.gameObject);
			GameState.Current.Avatar.SetDecorator(avatarDecorator);
			SetupLocalAvatar(GameState.Current.Avatar.Decorator);
		} else if (GameState.Current.Avatar.Decorator) {
			UpdateAvatarMesh(GameState.Current.Avatar.Decorator, gear);
			SetupLocalAvatar(GameState.Current.Avatar.Decorator);
		} else {
			Debug.LogError("No local Player created yet! Call 'CreateLocalPlayerAvatar' first!");
		}
	}

	private static void ReparentChildren(Transform oldParent, Transform newParent) {
		while (oldParent.childCount > 0) {
			var child = oldParent.GetChild(0);
			child.Reparent(newParent);
		}
	}

	public static AvatarDecorator CreateRemoteAvatar(AvatarGearParts gear, Color skinColor) {
		var avatarDecorator = CreateAvatarMesh(gear);
		SetupRemoteAvatar(avatarDecorator, skinColor);

		return avatarDecorator;
	}

	public static AvatarDecorator UpdateRemoteAvatar(AvatarDecorator avatar, AvatarGearParts gear, Color skinColor) {
		if (avatar.name != gear.Base.name) {
			var avatarDecorator = avatar;
			avatar = CreateAvatarMesh(gear);
			avatar.transform.ShareParent(avatarDecorator.transform);
			avatar.gameObject.SetActive(avatarDecorator.gameObject.activeSelf);
			ReparentChildren(avatarDecorator.WeaponAttachPoint, avatar.WeaponAttachPoint);
			UnityEngine.Object.Destroy(avatarDecorator.gameObject);
			SetupRemoteAvatar(avatar, skinColor);
		} else {
			UpdateAvatarMesh(avatar, gear);
			SetupRemoteAvatar(avatar, skinColor);
		}

		return avatar;
	}

	public static AvatarDecoratorConfig InstantiateRagdoll(AvatarGearParts gear, Color skinColor) {
		SkinnedMeshCombiner.Combine(gear.Base, gear.Parts);
		LayerUtil.SetLayerRecursively(gear.Base.transform, UberstrikeLayer.Ragdoll);
		gear.Base.GetComponent<SkinnedMeshRenderer>().updateWhenOffscreen = true;
		var component = gear.Base.GetComponent<AvatarDecoratorConfig>();
		component.SetSkinColor(skinColor);

		return component;
	}

	private static void UpdateAvatarMesh(AvatarDecorator avatar, AvatarGearParts gear) {
		if (!avatar) {
			Debug.LogError("AvatarDecorator is null!");

			return;
		}

		gear.Parts.Add(gear.Base);

		foreach (var particleSystem in avatar.GetComponentsInChildren<ParticleSystem>(true)) {
			UnityEngine.Object.Destroy(particleSystem.gameObject);
		}

		SkinnedMeshCombiner.Update(avatar.gameObject, gear.Parts);
		avatar.renderer.receiveShadows = false;
		gear.Parts.ForEach(delegate(GameObject obj) { UnityEngine.Object.Destroy(obj); });
	}

	private static AvatarDecorator CreateAvatarMesh(AvatarGearParts gear) {
		SkinnedMeshCombiner.Combine(gear.Base, gear.Parts);
		gear.Parts.ForEach(delegate(GameObject obj) { UnityEngine.Object.Destroy(obj); });

		return gear.Base.GetComponent<AvatarDecorator>();
	}

	private static void SetupLocalAvatar(AvatarDecorator avatar) {
		if (avatar) {
			avatar.SetLayers(UberstrikeLayer.RemotePlayer);
			avatar.Configuration.SetSkinColor(PlayerDataManager.SkinColor);
			avatar.HudInformation.SetAvatarLabel(PlayerDataManager.NameAndTag);
		} else {
			Debug.LogError("No AvatarDecorator to setup!");
		}
	}

	private static void SetupRemoteAvatar(AvatarDecorator avatar, Color skinColor) {
		if (avatar) {
			avatar.SetLayers(UberstrikeLayer.RemotePlayer);
			avatar.Configuration.SetSkinColor(skinColor);
		} else {
			Debug.LogError("No AvatarDecorator to setup!");
		}
	}
}
