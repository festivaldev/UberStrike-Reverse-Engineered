using System.Collections.Generic;
using UnityEngine;

public static class MaterialUtil {
	private static Dictionary<Material, MaterialCache> _cache = new Dictionary<Material, MaterialCache>();

	public static void SetFloat(Material m, string propertyName, float value) {
		if (m && m.HasProperty(propertyName)) {
			MaterialCache materialCache;

			if (!_cache.TryGetValue(m, out materialCache)) {
				materialCache = new MaterialCache();
				_cache[m] = materialCache;
			}

			if (!materialCache.Floats.ContainsKey(propertyName)) {
				materialCache.Floats[propertyName] = m.GetFloat(propertyName);
			}

			m.SetFloat(propertyName, value);
		} else {
			Debug.LogError(string.Format("Property<float> '{0}' not found in Material {1}", propertyName, (!m) ? "NULL" : m.name));
		}
	}

	public static void SetColor(Material m, string propertyName, Color value) {
		if (m && m.HasProperty(propertyName)) {
			MaterialCache materialCache;

			if (!_cache.TryGetValue(m, out materialCache)) {
				materialCache = new MaterialCache();
				_cache[m] = materialCache;
			}

			if (!materialCache.Colors.ContainsKey(propertyName)) {
				materialCache.Colors[propertyName] = m.GetColor(propertyName);
			}

			m.SetColor(propertyName, value);
		} else {
			Debug.LogError(string.Format("Property<Color> '{0}' not found in Material {1}", propertyName, (!m) ? "NULL" : m.name));
		}
	}

	public static void SetTextureOffset(Material m, string propertyName, Vector2 value) {
		if (m && m.HasProperty(propertyName)) {
			MaterialCache materialCache;

			if (!_cache.TryGetValue(m, out materialCache)) {
				materialCache = new MaterialCache();
				_cache[m] = materialCache;
			}

			if (!materialCache.TextureOffset.ContainsKey(propertyName)) {
				materialCache.TextureOffset[propertyName] = m.GetTextureOffset(propertyName);
			}

			m.SetTextureOffset(propertyName, value);
		} else {
			Debug.LogError(string.Format("Property<Vector2> '{0}' not found in Material {1}", propertyName, (!m) ? "NULL" : m.name));
		}
	}

	public static void SetTexture(Material m, string propertyName, Texture value) {
		if (m && m.HasProperty(propertyName)) {
			MaterialCache materialCache;

			if (!_cache.TryGetValue(m, out materialCache)) {
				materialCache = new MaterialCache();
				_cache[m] = materialCache;
			}

			if (!materialCache.Texture.ContainsKey(propertyName)) {
				materialCache.Texture[propertyName] = m.GetTexture(propertyName);
			}

			m.SetTexture(propertyName, value);
		} else {
			Debug.LogError(string.Format("Property<Texture> '{0}' not found in Material {1}", propertyName, (!m) ? "NULL" : m.name));
		}
	}

	public static void Reset(Material m) {
		MaterialCache materialCache;

		if (_cache.TryGetValue(m, out materialCache)) {
			foreach (var keyValuePair in materialCache.Colors) {
				m.SetColor(keyValuePair.Key, keyValuePair.Value);
			}

			foreach (var keyValuePair2 in materialCache.Floats) {
				m.SetFloat(keyValuePair2.Key, keyValuePair2.Value);
			}

			foreach (var keyValuePair3 in materialCache.TextureOffset) {
				m.SetTextureOffset(keyValuePair3.Key, keyValuePair3.Value);
			}
		}
	}

	private class MaterialCache {
		public Dictionary<string, Color> Colors = new Dictionary<string, Color>();
		public Dictionary<string, float> Floats = new Dictionary<string, float>();
		public Dictionary<string, Texture> Texture = new Dictionary<string, Texture>();
		public Dictionary<string, Vector2> TextureOffset = new Dictionary<string, Vector2>();
	}
}
