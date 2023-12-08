using System.Text;

namespace UberStrike.DataCenter.Common.Entities {
	public class UberstrikeWeaponModConfigView {
		public int LevelRequired { get; set; }

		public override string ToString() {
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("[UberstrikeWeaponModConfigView: ");
			stringBuilder.Append("]");

			return stringBuilder.ToString();
		}
	}
}
