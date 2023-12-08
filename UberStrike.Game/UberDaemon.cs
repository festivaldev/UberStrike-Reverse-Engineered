using System.Diagnostics;
using UnityEngine;

public class UberDaemon : MonoBehaviour {
	public static UberDaemon Instance;

	private void Awake() {
		Instance = this;
	}

	public string GetMagicHash(string authToken) {
		var processStartInfo = new ProcessStartInfo();
		var text = "bash";
		processStartInfo.Arguments = "uberdaemon.sh " + authToken;

		if (Application.platform == RuntimePlatform.WindowsPlayer) {
			text = "uberdaemon.exe";
			processStartInfo.Arguments = authToken;
		}

		processStartInfo.RedirectStandardError = true;
		processStartInfo.RedirectStandardOutput = true;
		processStartInfo.UseShellExecute = false;
		processStartInfo.FileName = text;
		processStartInfo.WindowStyle = ProcessWindowStyle.Minimized;
		processStartInfo.CreateNoWindow = true;
		var process = Process.Start(processStartInfo);

		return process.StandardOutput.ReadToEnd().Trim();
	}
}
