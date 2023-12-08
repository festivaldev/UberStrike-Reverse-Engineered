using System.Text;
using Cmune.DataCenter.Common.Entities;

namespace UberStrike.DataCenter.Common.Entities {
	public class UberstrikeItemGearView : UberstrikeItemView {
		public UberstrikeGearConfigView Config { get; set; }
		public UberstrikeItemGearView() { }

		public UberstrikeItemGearView(ItemView item, int levelRequired, UberstrikeGearConfigView config) : base(item, levelRequired) {
			Config = config;
		}

		public override string ToString() {
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("[UberstrikeGearView: ");
			stringBuilder.Append(base.ToString());
			stringBuilder.Append(Config);
			stringBuilder.Append("]]");

			return stringBuilder.ToString();
		}
	}
}
