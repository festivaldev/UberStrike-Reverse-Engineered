using UnityEngine;

public class DynamicTexture {
	private float alpha;
	private TextureLoader.Holder holder;
	private string url;

	public float Aspect {
		get { return (holder == null) ? 1f : (holder.Texture.height / (float)holder.Texture.width); }
	}

	public string Url {
		get { return url; }
	}

	public DynamicTexture(string url, bool loadNow = false) {
		this.url = url;

		if (loadNow) {
			holder = AutoMonoBehaviour<TextureLoader>.Instance.Load(url);
		}
	}

	public void Draw(Rect rect, bool forceAlpha = false) {
		if (holder == null) {
			holder = AutoMonoBehaviour<TextureLoader>.Instance.Load(url);
		}

		if (holder.State == TextureLoader.State.Ok) {
			if (forceAlpha) {
				GUI.DrawTexture(rect, holder.Texture);
			} else {
				var color = GUI.color;
				alpha = Mathf.Lerp(alpha, 1f, Time.deltaTime);
				GUI.color = new Color(1f, 1f, 1f, (!GUI.enabled) ? Mathf.Min(alpha, 0.5f) : alpha);
				GUI.DrawTexture(rect, holder.Texture);
				GUI.color = color;
			}
		} else if (holder.State == TextureLoader.State.Downloading) {
			WaitingTexture.Draw(rect.center);
		} else {
			GUI.Label(rect, "N/A", BlueStonez.label_interparkbold_13pt);
		}
	}
}
