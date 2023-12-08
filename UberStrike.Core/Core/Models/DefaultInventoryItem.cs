using UberStrike.Core.Types;

namespace UberStrike.Core.Models {
	public class DefaultInventoryItem {
		public int ItemId { get; set; }
		public int Duration { get; set; }
		public bool DisplayToPlayer { get; set; }
		public bool EquipOnAccountCreation { get; set; }
		public LoadoutSlotType LoadoutSlot { get; set; }
	}
}
