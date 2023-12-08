public class InventoryItemFilter : IShopItemFilter {
	public bool CanPass(IUnityItem item) {
		return !Singleton<LoadoutManager>.Instance.IsItemEquipped(item.View.ID) && item.View.PrefabName != "LutzDefaultGearHead" && item.View.PrefabName != "LutzDefaultGearGloves" && item.View.PrefabName != "LutzDefaultGearUpperBody" && item.View.PrefabName != "LutzDefaultGearLowerBody" && item.View.PrefabName != "LutzDefaultGearBoots" && item.Name != "Privateer License";
	}
}
