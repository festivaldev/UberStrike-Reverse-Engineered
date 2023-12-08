using System;
using System.Text;
using Steamworks;
using UnityEngine;

internal class SteamManager : MonoBehaviour {
	private static SteamManager m_instance;
	private bool m_bInitialized;
	private SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;

	public static bool Initialized {
		get { return m_instance.m_bInitialized; }
	}

	private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText) {
		Debug.LogWarning(pchDebugText);
	}

	private void Awake() { }

	private void Start() {
		Debug.Log("INITIALIZING STEAMWORKS SDK");

		if (m_instance != null) {
			Destroy(gameObject);

			return;
		}

		m_instance = this;

		try {
			if (SteamAPI.RestartAppIfNecessary((AppId_t)291210U)) {
				Application.Quit();

				return;
			}
		} catch (DllNotFoundException ex) {
			Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + ex, this);
			Application.Quit();

			return;
		}

		if (!SteamAPI.Init()) {
			Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
			Application.Quit();

			return;
		}

		m_bInitialized = true;
		Debug.Log("SteamAPI was successfully initialized!");

		if (!SteamUser.BLoggedOn()) {
			Debug.LogError("[Steamworks.NET] Steam user must be logged in to play this game (SteamUser()->BLoggedOn() returned false).", this);
			Application.Quit();
		}
	}

	private void OnEnable() {
		if (!m_bInitialized) {
			return;
		}

		if (m_SteamAPIWarningMessageHook == null) {
			m_SteamAPIWarningMessageHook = SteamAPIDebugTextHook;
			SteamClient.SetWarningMessageHook(m_SteamAPIWarningMessageHook);
		}
	}

	private void OnApplicationQuit() {
		if (!m_bInitialized) {
			return;
		}

		SteamAPI.Shutdown();
	}

	private void Update() {
		if (!m_bInitialized) {
			return;
		}

		SteamAPI.RunCallbacks();
	}
}
