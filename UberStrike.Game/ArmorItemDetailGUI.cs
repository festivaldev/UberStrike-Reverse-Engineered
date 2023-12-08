using UberStrike.Core.Models.Views;
using UnityEngine;

public class ArmorItemDetailGUI : IBaseItemDetailGUI {
	private Texture2D _armorPointsIcon;
	private UberStrikeItemGearView _item;

	public ArmorItemDetailGUI(UberStrikeItemGearView item, Texture2D armorPointsIcon) {
		_item = item;
		_armorPointsIcon = armorPointsIcon;
	}

	public void Draw() {
		GUI.DrawTexture(new Rect(48f, 89f, 32f, 32f), _armorPointsIcon);
		GUI.contentColor = Color.black;
		GUI.Label(new Rect(48f, 89f, 32f, 32f), _item.ArmorPoints.ToString(), BlueStonez.label_interparkbold_16pt);
		GUI.contentColor = Color.white;
		GUI.Label(new Rect(80f, 89f, 32f, 32f), "AP", BlueStonez.label_interparkbold_18pt_left);
	}
}
