using System;
using System.Collections;
using UnityEngine;

public static class AssetBundleLoader {
	public static IEnumerator LoadAssetBundleNoCache(string path, Action<float> progress = null, Action<AssetBundle> onLoaded = null, Action<string> onError = null) {
		Debug.Log("LOADING ASSETBUNDLE: " + path);
		var loader = new WWW(path);

		while (!loader.isDone) {
			yield return new WaitForEndOfFrame();

			if (progress != null) {
				progress(loader.progress);
			}
		}

		if (!string.IsNullOrEmpty(loader.error)) {
			Debug.LogError("Failed to locate Asset " + path + ". Error" + loader.error);

			if (onError != null) {
				onError("Failed to locate Asset " + path + ". Error" + loader.error);
			}
		} else if (onLoaded != null) {
			onLoaded(loader.assetBundle);
		}

		if (progress != null) {
			progress(1f);
		}

		loader.Dispose();
	}
}
