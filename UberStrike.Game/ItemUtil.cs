using UberStrike.Core.Types;

public class ItemUtil {
	public static UberstrikeItemClass ItemClassFromSlot(LoadoutSlotType slot) {
		UberstrikeItemClass uberstrikeItemClass = 0;

		switch (slot) {
			case LoadoutSlotType.GearHead:
				uberstrikeItemClass = UberstrikeItemClass.GearHead;

				break;
			case LoadoutSlotType.GearFace:
				uberstrikeItemClass = UberstrikeItemClass.GearFace;

				break;
			case LoadoutSlotType.GearGloves:
				uberstrikeItemClass = UberstrikeItemClass.GearGloves;

				break;
			case LoadoutSlotType.GearUpperBody:
				uberstrikeItemClass = UberstrikeItemClass.GearUpperBody;

				break;
			case LoadoutSlotType.GearLowerBody:
				uberstrikeItemClass = UberstrikeItemClass.GearLowerBody;

				break;
			case LoadoutSlotType.GearBoots:
				uberstrikeItemClass = UberstrikeItemClass.GearBoots;

				break;
			case LoadoutSlotType.GearHolo:
				uberstrikeItemClass = UberstrikeItemClass.GearHolo;

				break;
		}

		return uberstrikeItemClass;
	}

	public static LoadoutSlotType SlotFromItemClass(UberstrikeItemClass itemClass) {
		var loadoutSlotType = LoadoutSlotType.None;

		switch (itemClass) {
			case UberstrikeItemClass.GearBoots:
				loadoutSlotType = LoadoutSlotType.GearBoots;

				break;
			case UberstrikeItemClass.GearHead:
				loadoutSlotType = LoadoutSlotType.GearHead;

				break;
			case UberstrikeItemClass.GearFace:
				loadoutSlotType = LoadoutSlotType.GearFace;

				break;
			case UberstrikeItemClass.GearUpperBody:
				loadoutSlotType = LoadoutSlotType.GearUpperBody;

				break;
			case UberstrikeItemClass.GearLowerBody:
				loadoutSlotType = LoadoutSlotType.GearLowerBody;

				break;
			case UberstrikeItemClass.GearGloves:
				loadoutSlotType = LoadoutSlotType.GearGloves;

				break;
			case UberstrikeItemClass.GearHolo:
				loadoutSlotType = LoadoutSlotType.GearHolo;

				break;
		}

		return loadoutSlotType;
	}
}
