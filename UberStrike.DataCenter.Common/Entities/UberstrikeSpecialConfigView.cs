using System.Text;

namespace UberStrike.DataCenter.Common.Entities {
	public class UberstrikeSpecialConfigView {
		public int LevelRequired { get; set; }

		public override string ToString() {
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("[UberstrikeSpecialConfigView: ");
			stringBuilder.Append("]");

			return stringBuilder.ToString();
		}
	}
}
