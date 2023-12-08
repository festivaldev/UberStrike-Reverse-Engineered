using Cmune.DataCenter.Common.Entities;
using UnityEngine;

public static class ColorScheme {
	public static readonly Color ProgressBar = new Color(0f, 0.607f, 0.662f);
	public static readonly Color HudTeamRed = ColorConverter.RgbToColor(206f, 87f, 87f);
	public static readonly Color HudTeamBlue = ColorConverter.RgbToColor(76f, 127f, 216f);
	public static readonly Color GuiTeamRed = new Color(0.929f, 0.27f, 0.27f);
	public static readonly Color GuiTeamBlue = new Color(0.176f, 0.643f, 0.792f);
	public static readonly Color ChatNameCurrentUser = ColorConverter.RgbToColor(0f, 217f, 255f);
	public static readonly Color ChatNameFriendsUser = ColorConverter.RgbToColor(0f, 204f, 0f);
	public static readonly Color ChatNameFacebookFriendUser = ColorConverter.RgbToColor(0f, 184f, 158f);
	public static readonly Color ChatNameAdminUser = ColorConverter.RgbToColor(204f, 0f, 0f);
	public static readonly Color ChatNameModeratorUser = ColorConverter.RgbToColor(242f, 101f, 34f);
	public static readonly Color ChatNameSeniorModeratorUser = ColorConverter.RgbToColor(255f, 255f, 0f);
	public static readonly Color ChatNameQAUser = ColorConverter.RgbToColor(79f, 48f, 235f);
	public static readonly Color ChatNameSeniorQAUser = ColorConverter.RgbToColor(223f, 0f, 255f);
	public static readonly Color ChatNameOtherUser = ColorConverter.RgbToColor(153f, 153f, 153f);
	public static readonly Color UberStrikeYellow = new Color(0.87f, 0.64f, 0.035f);
	public static readonly Color UberStrikeBlue = new Color(0.176f, 0.643f, 0.792f);
	public static readonly Color UberStrikeRed = new Color(0.929f, 0.27f, 0.27f);
	public static readonly Color UberStrikeGreen = new Color(0f, 0.62f, 0.07f);
	public static readonly Color CheatWarningRed = ColorConverter.RgbToColor(183f, 48f, 48f);
	public static readonly Color XPColor = ColorConverter.RgbToColor(255f, 127f, 0f);
	public static readonly Color TeamOutline = Color.white;

	public static Color GetNameColorByAccessLevel(MemberAccessLevel accessLevel) {
		switch (accessLevel) {
			case MemberAccessLevel.QA:
				return ChatNameQAUser;
			case MemberAccessLevel.Moderator:
				return ChatNameModeratorUser;
			case MemberAccessLevel.SeniorQA:
				return ChatNameSeniorQAUser;
			case MemberAccessLevel.SeniorModerator:
				return ChatNameSeniorModeratorUser;
			case MemberAccessLevel.Admin:
				return ChatNameAdminUser;
		}

		return ChatNameOtherUser;
	}
}
