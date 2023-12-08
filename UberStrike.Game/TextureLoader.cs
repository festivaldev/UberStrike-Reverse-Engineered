using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureLoader : AutoMonoBehaviour<TextureLoader> {
	public enum State {
		Downloading,
		Ok,
		Error,
		Timeout
	}

	private readonly float TIMEOUT = 30f;
	public Dictionary<string, Holder> cache = new Dictionary<string, Holder>();

	private Holder nullHolder = new Holder {
		State = State.Ok
	};

	public List<Holder> pending = new List<Holder>();

	protected override void Start() {
		base.Start();
		nullHolder.Texture = new Texture2D(1, 1, TextureFormat.RGB24, false);

		for (var i = 0; i < 5; i++) {
			StartCoroutine(WorkerCrt());
		}
	}

	private IEnumerator WorkerCrt() {
		for (;;) {
			while (pending.Count == 0) {
				yield return 0;
			}

			var item = pending[0];
			pending.RemoveAt(0);
			var www = new WWW(item.Url);
			var start = Time.time;

			while (!www.isDone && Time.time - start <= TIMEOUT) {
				yield return 0;
			}

			if (www.isDone) {
				if (string.IsNullOrEmpty(www.error)) {
					www.LoadImageIntoTexture(item.Texture);
					item.State = State.Ok;
				} else {
					item.State = State.Error;
					Debug.Log("Failed to download texture " + item.Url + ". " + www.error);
				}
			} else {
				item.State = State.Timeout;
				Debug.Log("Failed to download texture " + item.Url + ". Timeout.");
				www.Dispose();
			}
		}

		yield break;
	}

	public Texture2D LoadImage(string url, Texture2D placeholder = null) {
		return Load(url, placeholder).Texture;
	}

	public Holder Load(string url, Texture2D placeholder = null) {
		if (string.IsNullOrEmpty(url)) {
			return nullHolder;
		}

		Holder holder = null;

		if (cache.TryGetValue(url, out holder)) {
			return holder;
		}

		holder = new Holder {
			Url = url,
			Texture = ((!(placeholder == null)) ? (Instantiate(placeholder) as Texture2D) : new Texture2D(1, 1, TextureFormat.RGB24, false))
		};

		cache[url] = holder;
		pending.Add(holder);

		return holder;
	}

	public State GetState(string url) {
		return cache[url].State;
	}

	public State GetState(Texture2D tex) {
		return new List<Holder>(cache.Values).Find(el => el.Texture == tex).State;
	}

	public class Holder {
		public State State;
		public Texture2D Texture;
		public string Url;
	}
}
