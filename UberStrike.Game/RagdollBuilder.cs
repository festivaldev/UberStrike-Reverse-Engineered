using System.Collections.Generic;
using UnityEngine;

public class RagdollBuilder : Singleton<RagdollBuilder> {
	private RagdollBuilder() { }

	public AvatarDecoratorConfig CreateRagdoll(Loadout gearLoadout) {
		IUnityItem unityItem;

		if (gearLoadout.TryGetItem(LoadoutSlotType.GearHolo, out unityItem)) {
			return CreateHolo(unityItem);
		}

		return CreateLutzRavinoff(gearLoadout);
	}

	private AvatarDecoratorConfig CreateLutzRavinoff(Loadout gearLoadout) {
		var defaultRagdoll = PrefabManager.Instance.DefaultRagdoll;
		var avatarDecoratorConfig = UnityEngine.Object.Instantiate(defaultRagdoll) as AvatarDecoratorConfig;
		var list = new List<GameObject>();
		SkinnedMeshCombiner.Combine(avatarDecoratorConfig.gameObject, list);

		foreach (var gameObject in list) {
			UnityEngine.Object.Destroy(gameObject);
		}

		return avatarDecoratorConfig;
	}

	private AvatarDecoratorConfig CreateHolo(IUnityItem holo) {
		AvatarDecoratorConfig avatarDecoratorConfig = null;
		var gameObject = holo.Create(Vector3.zero, Quaternion.identity);
		var component = gameObject.GetComponent<HoloGearItem>();

		if (component && component.Configuration.Ragdoll) {
			avatarDecoratorConfig = UnityEngine.Object.Instantiate(component.Configuration.Ragdoll) as AvatarDecoratorConfig;
			LayerUtil.SetLayerRecursively(avatarDecoratorConfig.transform, UberstrikeLayer.Ragdoll);
			SkinnedMeshCombiner.Combine(avatarDecoratorConfig.gameObject, new List<GameObject>());
		}

		UnityEngine.Object.Destroy(gameObject);

		return avatarDecoratorConfig;
	}
}
