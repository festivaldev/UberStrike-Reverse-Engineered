using System.Text;
using Cmune.DataCenter.Common.Entities;

namespace UberStrike.DataCenter.Common.Entities {
	public class UberstrikeItemWeaponView : UberstrikeItemView {
		public UberstrikeWeaponConfigView Config { get; set; }
		public UberstrikeItemWeaponView() { }

		public UberstrikeItemWeaponView(ItemView item, int levelRequired, UberstrikeWeaponConfigView config) : base(item, levelRequired) {
			Config = config;
		}

		public override string ToString() {
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("[UberstrikeWeaponView: ");
			stringBuilder.Append(base.ToString());
			stringBuilder.Append(Config);
			stringBuilder.Append("]]");

			return stringBuilder.ToString();
		}
	}
}
