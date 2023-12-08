using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;

public class UberstrikeMap {
	public bool IsVisible { get; set; }
	public MapView View { get; private set; }
	public DynamicTexture Icon { get; private set; }
	public bool IsBuiltIn { get; set; }

	public int Id {
		get { return View.MapId; }
	}

	public string Name {
		get { return View.DisplayName; }
	}

	public string Description {
		get { return View.Description; }
	}

	public string SceneName {
		get { return View.SceneName; }
	}

	public string MapIconUrl { get; private set; }

	public UberstrikeMap(MapView view) {
		View = view;
		IsVisible = true;
		MapIconUrl = ApplicationDataManager.ImagePath + "maps/" + View.SceneName + ".jpg";
		var flag = View.SceneName != "Menu";
		Icon = new DynamicTexture(MapIconUrl, flag);
	}

	public bool IsGameModeSupported(GameModeType mode) {
		return View.Settings != null && View.Settings.ContainsKey(mode);
	}
}
