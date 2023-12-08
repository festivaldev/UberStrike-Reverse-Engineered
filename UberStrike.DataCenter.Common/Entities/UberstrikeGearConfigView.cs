using System.Text;

namespace UberStrike.DataCenter.Common.Entities {
	public class UberstrikeGearConfigView {
		public int ArmorPoints { get; set; }
		public int ArmorWeight { get; set; }
		public int LevelRequired { get; set; }

		public override string ToString() {
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("[UberstrikeGearConfigView: [ArmorPoints: ");
			stringBuilder.Append(ArmorPoints);
			stringBuilder.Append("][ArmorWeight: ");
			stringBuilder.Append(ArmorWeight);
			stringBuilder.Append("]]");

			return stringBuilder.ToString();
		}
	}
}
