using UnityEngine;

[AddComponentMenu("NGUI/CMune Extensions/Texture Remote")]
[ExecuteInEditMode]
public class UIRemoteTexture : MonoBehaviour {
	private DownloadState _state;

	[SerializeField]
	private UISprite defaultImage;

	[SerializeField]
	private UISprite loadingSpinning;

	private float rotationSpeed = 300f;

	[SerializeField]
	private UITexture uiTexture;

	[SerializeField]
	private string url;

	public string Url {
		get { return url; }
		set {
			if (url == value) {
				return;
			}

			url = value;

			if (!string.IsNullOrEmpty(url)) {
				State = DownloadState.Downloading;
			}
		}
	}

	private DownloadState State {
		get { return _state; }
		set {
			_state = value;
			this.TryEnableAndSetScale(uiTexture, value == DownloadState.Downloaded);
			this.TryEnableAndSetScale(loadingSpinning, value == DownloadState.None || value == DownloadState.Downloading);
			this.TryEnableAndSetScale(defaultImage, value == DownloadState.None || value == DownloadState.Downloading || value == DownloadState.Error);
		}
	}

	public void ShowDefault() {
		State = DownloadState.Error;
	}

	private void Start() {
		if (uiTexture == null) {
			uiTexture = new GameObject("LoadedTexture") {
				layer = gameObject.layer,
				transform = {
					parent = transform,
					localPosition = Vector3.zero,
					localScale = Vector3.zero,
					localRotation = Quaternion.identity
				}
			}.AddComponent<UITexture>();

			uiTexture.depth = 0;
			uiTexture.enabled = false;
		}

		if (loadingSpinning == null) {
			loadingSpinning = new GameObject("LoadingSpinning") {
				layer = gameObject.layer,
				transform = {
					parent = transform,
					localPosition = Vector3.zero,
					localScale = Vector3.zero,
					localRotation = Quaternion.identity
				}
			}.AddComponent<UISprite>();

			loadingSpinning.depth = 2;
		}

		if (defaultImage == null) {
			defaultImage = new GameObject("DefaultImage") {
				layer = gameObject.layer,
				transform = {
					parent = transform,
					localPosition = Vector3.zero,
					localScale = Vector3.zero,
					localRotation = Quaternion.identity
				}
			}.AddComponent<UISprite>();

			defaultImage.depth = 1;
		}

		if (!string.IsNullOrEmpty(url) && uiTexture.mainTexture == null) {
			State = ((!Application.isPlaying) ? DownloadState.None : DownloadState.Downloading);
		}
	}

	private void TryEnableAndSetScale(UITexture texture, bool enabled) {
		if (texture == null) {
			return;
		}

		texture.enabled = enabled;

		if (enabled && texture.transform.localScale == Vector3.zero && texture.mainTexture != null) {
			texture.transform.localScale = new Vector3((float)texture.mainTexture.width, (float)texture.mainTexture.height, 1f);
		}
	}

	private void TryEnableAndSetScale(UISprite sprite, bool enabled) {
		if (sprite == null) {
			return;
		}

		sprite.enabled = enabled;

		if (enabled) {
			UIAtlas.Sprite atlasSprite = sprite.GetAtlasSprite();

			if (sprite.transform.localScale == Vector3.zero && atlasSprite != null) {
				sprite.transform.localScale = new Vector3(atlasSprite.outer.width, atlasSprite.outer.height, 1f);
			}
		}
	}

	private void Update() {
		if (!Application.isPlaying) {
			return;
		}

		if (State == DownloadState.Downloading) {
			if (loadingSpinning != null && loadingSpinning.enabled) {
				loadingSpinning.transform.localRotation = Quaternion.Euler(0f, 0f, -Time.time * rotationSpeed + Time.time * rotationSpeed % 30f);
			}

			var holder = AutoMonoBehaviour<TextureLoader>.Instance.Load(url);

			if (holder.State != TextureLoader.State.Downloading) {
				if (holder.State == TextureLoader.State.Ok) {
					uiTexture.mainTexture = holder.Texture;
					State = DownloadState.Downloaded;
				} else {
					State = DownloadState.Error;
				}
			}
		}
	}

	private enum DownloadState {
		None,
		Downloading,
		Downloaded,
		Error
	}
}
