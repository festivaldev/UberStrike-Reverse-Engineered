using UnityEngine;

public class PrefabManager : MonoBehaviour {
	[SerializeField]
	private AvatarDecorator _defaultAvatar;

	[SerializeField]
	private AvatarDecoratorConfig _defaultRagdoll;

	[SerializeField]
	private CharacterConfig _localCharacter;

	[SerializeField]
	private LocalPlayer _player;

	[SerializeField]
	private CharacterConfig _remoteCharacter;

	public static PrefabManager Instance { get; private set; }

	public static bool Exists {
		get { return Instance != null; }
	}

	public AvatarDecorator DefaultAvatar {
		get { return _defaultAvatar; }
	}

	public AvatarDecoratorConfig DefaultRagdoll {
		get { return _defaultRagdoll; }
	}

	public CharacterConfig InstantiateLocalCharacter() {
		return Instantiate(_localCharacter) as CharacterConfig;
	}

	public CharacterConfig InstantiateRemoteCharacter() {
		return Instantiate(_remoteCharacter) as CharacterConfig;
	}

	public LocalPlayer InstantiateLocalPlayer() {
		var localPlayer = Instantiate(_player) as LocalPlayer;
		DontDestroyOnLoad(localPlayer);
		localPlayer.SetEnabled(false);

		return localPlayer;
	}

	private void Awake() {
		Instance = this;
	}
}
