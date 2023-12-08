using System.Text;
using Cmune.DataCenter.Common.Entities;

namespace UberStrike.DataCenter.Common.Entities {
	public class UberstrikeItemWeaponModView : UberstrikeItemView {
		public UberstrikeWeaponModConfigView Config { get; set; }
		public UberstrikeItemWeaponModView() { }

		public UberstrikeItemWeaponModView(ItemView item, int level, UberstrikeWeaponModConfigView config) : base(item, level) {
			Config = config;
		}

		public override string ToString() {
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("[UberstrikeWeaponModView: ");
			stringBuilder.Append(base.ToString());
			stringBuilder.Append(Config);
			stringBuilder.Append("]]");

			return stringBuilder.ToString();
		}
	}
}
