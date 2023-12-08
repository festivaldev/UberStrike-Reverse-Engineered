using System;
using System.Text;
using UberStrike.Core.Types;

namespace UberStrike.DataCenter.Common.Entities {
	[Serializable]
	public class LoadoutView {
		public int LoadoutId { get; set; }
		public int Backpack { get; set; }
		public int Boots { get; set; }
		public int Cmid { get; set; }
		public int Face { get; set; }
		public int FunctionalItem1 { get; set; }
		public int FunctionalItem2 { get; set; }
		public int FunctionalItem3 { get; set; }
		public int Gloves { get; set; }
		public int Head { get; set; }
		public int LowerBody { get; set; }
		public int MeleeWeapon { get; set; }
		public int QuickItem1 { get; set; }
		public int QuickItem2 { get; set; }
		public int QuickItem3 { get; set; }
		public AvatarType Type { get; set; }
		public int UpperBody { get; set; }
		public int Weapon1 { get; set; }
		public int Weapon1Mod1 { get; set; }
		public int Weapon1Mod2 { get; set; }
		public int Weapon1Mod3 { get; set; }
		public int Weapon2 { get; set; }
		public int Weapon2Mod1 { get; set; }
		public int Weapon2Mod2 { get; set; }
		public int Weapon2Mod3 { get; set; }
		public int Weapon3 { get; set; }
		public int Weapon3Mod1 { get; set; }
		public int Weapon3Mod2 { get; set; }
		public int Weapon3Mod3 { get; set; }
		public int Webbing { get; set; }
		public string SkinColor { get; set; }

		public LoadoutView() {
			Type = AvatarType.LutzRavinoff;
			SkinColor = string.Empty;
		}

		public LoadoutView(int loadoutId, int backpack, int boots, int cmid, int face, int functionalItem1, int functionalItem2, int functionalItem3, int gloves, int head, int lowerBody, int meleeWeapon, int quickItem1, int quickItem2, int quickItem3, AvatarType type, int upperBody, int weapon1, int weapon1Mod1, int weapon1Mod2, int weapon1Mod3, int weapon2, int weapon2Mod1, int weapon2Mod2, int weapon2Mod3, int weapon3, int weapon3Mod1, int weapon3Mod2, int weapon3Mod3, int webbing, string skinColor) {
			Backpack = backpack;
			Boots = boots;
			Cmid = cmid;
			Face = face;
			FunctionalItem1 = functionalItem1;
			FunctionalItem2 = functionalItem2;
			FunctionalItem3 = functionalItem3;
			Gloves = gloves;
			Head = head;
			LoadoutId = loadoutId;
			LowerBody = lowerBody;
			MeleeWeapon = meleeWeapon;
			QuickItem1 = quickItem1;
			QuickItem2 = quickItem2;
			QuickItem3 = quickItem3;
			Type = type;
			UpperBody = upperBody;
			Weapon1 = weapon1;
			Weapon1Mod1 = weapon1Mod1;
			Weapon1Mod2 = weapon1Mod2;
			Weapon1Mod3 = weapon1Mod3;
			Weapon2 = weapon2;
			Weapon2Mod1 = weapon2Mod1;
			Weapon2Mod2 = weapon2Mod2;
			Weapon2Mod3 = weapon2Mod3;
			Weapon3 = weapon3;
			Weapon3Mod1 = weapon3Mod1;
			Weapon3Mod2 = weapon3Mod2;
			Weapon3Mod3 = weapon3Mod3;
			Webbing = webbing;
			SkinColor = skinColor;
		}

		public override string ToString() {
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("[LoadoutView: [Backpack: ");
			stringBuilder.Append(Backpack);
			stringBuilder.Append("][Boots: ");
			stringBuilder.Append(Boots);
			stringBuilder.Append("][Cmid: ");
			stringBuilder.Append(Cmid);
			stringBuilder.Append("][Face: ");
			stringBuilder.Append(Face);
			stringBuilder.Append("][FunctionalItem1: ");
			stringBuilder.Append(FunctionalItem1);
			stringBuilder.Append("][FunctionalItem2: ");
			stringBuilder.Append(FunctionalItem2);
			stringBuilder.Append("][FunctionalItem3: ");
			stringBuilder.Append(FunctionalItem3);
			stringBuilder.Append("][Gloves: ");
			stringBuilder.Append(Gloves);
			stringBuilder.Append("][Head: ");
			stringBuilder.Append(Head);
			stringBuilder.Append("][LoadoutId: ");
			stringBuilder.Append(LoadoutId);
			stringBuilder.Append("][LowerBody: ");
			stringBuilder.Append(LowerBody);
			stringBuilder.Append("][MeleeWeapon: ");
			stringBuilder.Append(MeleeWeapon);
			stringBuilder.Append("][QuickItem1: ");
			stringBuilder.Append(QuickItem1);
			stringBuilder.Append("][QuickItem2: ");
			stringBuilder.Append(QuickItem2);
			stringBuilder.Append("][QuickItem3: ");
			stringBuilder.Append(QuickItem3);
			stringBuilder.Append("][Type: ");
			stringBuilder.Append(Type);
			stringBuilder.Append("][UpperBody: ");
			stringBuilder.Append(UpperBody);
			stringBuilder.Append("][Weapon1: ");
			stringBuilder.Append(Weapon1);
			stringBuilder.Append("][Weapon1Mod1: ");
			stringBuilder.Append(Weapon1Mod1);
			stringBuilder.Append("][Weapon1Mod2: ");
			stringBuilder.Append(Weapon1Mod2);
			stringBuilder.Append("][Weapon1Mod3: ");
			stringBuilder.Append(Weapon1Mod3);
			stringBuilder.Append("][Weapon2: ");
			stringBuilder.Append(Weapon2);
			stringBuilder.Append("][Weapon2Mod1: ");
			stringBuilder.Append(Weapon2Mod1);
			stringBuilder.Append("][Weapon2Mod2: ");
			stringBuilder.Append(Weapon2Mod2);
			stringBuilder.Append("][Weapon2Mod3: ");
			stringBuilder.Append(Weapon2Mod3);
			stringBuilder.Append("][Weapon3: ");
			stringBuilder.Append(Weapon3);
			stringBuilder.Append("][Weapon3Mod1: ");
			stringBuilder.Append(Weapon3Mod1);
			stringBuilder.Append("][Weapon3Mod2: ");
			stringBuilder.Append(Weapon3Mod2);
			stringBuilder.Append("][Weapon3Mod3: ");
			stringBuilder.Append(Weapon3Mod3);
			stringBuilder.Append("][Webbing: ");
			stringBuilder.Append(Webbing);
			stringBuilder.Append("][SkinColor: ");
			stringBuilder.Append(SkinColor);
			stringBuilder.Append("]]");

			return stringBuilder.ToString();
		}
	}
}
