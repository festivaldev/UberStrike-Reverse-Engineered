namespace UberStrike.DataCenter.Common.Entities {
	public class MapInfoView {
		public int MapId { get; set; }
		public string DisplayName { get; set; }
		public string SceneName { get; set; }
		public string Description { get; set; }
		public bool InUse { get; set; }
		public int PlayerMax { get; set; }
		public int SupportedGameModes { get; set; }
		public int SupportedItemClass { get; set; }

		public MapInfoView(int mapId, string displayName, string sceneName, string description) {
			MapId = mapId;
			DisplayName = displayName;
			SceneName = sceneName;
			Description = description;
		}
	}
}
