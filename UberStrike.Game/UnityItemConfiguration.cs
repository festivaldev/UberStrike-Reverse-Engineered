using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UberStrike.Core.Types;
using UnityEngine;

public class UnityItemConfiguration : MonoBehaviour {
	public List<Texture2D> DefaultWeaponIcons;
	private Dictionary<string, string> m_AvailablePrefabs;

	[SerializeField]
	private TextAsset m_ItemPrefabXml;

	public List<GearItem> UnityItemsDefaultGears;
	public List<WeaponItem> UnityItemsDefaultWeapons;
	public List<FunctionalItemHolder> UnityItemsFunctional;
	public static UnityItemConfiguration Instance { get; private set; }

	public static bool Exists {
		get { return Instance != null; }
	}

	private void Awake() {
		Instance = this;
		var xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(m_ItemPrefabXml.text);
		var xmlNodeList = xmlDocument.DocumentElement.SelectNodes("/ItemAssetBundle/Item");
		m_AvailablePrefabs = new Dictionary<string, string>();

		foreach (var obj in xmlNodeList) {
			var xmlNode = (XmlNode)obj;
			var text = xmlNode.Attributes.GetNamedItem("Prefab").Value;
			text = text.Replace(".prefab", string.Empty).Replace("Assets/", string.Empty);
			var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
			m_AvailablePrefabs[fileNameWithoutExtension] = text;
		}
	}

	public GameObject GetDefaultItem(UberstrikeItemClass itemClass) {
		if (UnityItemsDefaultGears.Exists(i => i.TestItemClass == itemClass)) {
			return UnityItemsDefaultGears.Find(i => i.TestItemClass == itemClass).gameObject;
		}

		if (UnityItemsDefaultWeapons.Exists(i => i.TestItemClass == itemClass)) {
			return UnityItemsDefaultWeapons.Find(i => i.TestItemClass == itemClass).gameObject;
		}

		Debug.LogError("Couldn't find default item with class: " + itemClass);

		return null;
	}

	public string GetPrefabPath(string prefabName) {
		var empty = string.Empty;

		if (m_AvailablePrefabs.TryGetValue(prefabName, out empty)) {
			return empty;
		}

		return string.Empty;
	}

	public bool IsPrefabAvailable(string prefabName) {
		return m_AvailablePrefabs.ContainsKey(prefabName);
	}

	public bool Contains(string prefabName) {
		return UnityItemsDefaultGears.Find(item => item.name.Equals(prefabName)) || UnityItemsDefaultWeapons.Find(item => item.name.Equals(prefabName));
	}

	public Texture2D GetDefaultIcon(UberstrikeItemClass itemClass) {
		switch (itemClass) {
			case UberstrikeItemClass.WeaponMelee:
				return DefaultWeaponIcons.Find(icon => icon.name.Contains("Melee"));
			case UberstrikeItemClass.WeaponMachinegun:
				return DefaultWeaponIcons.Find(icon => icon.name.Contains("Machine"));
			case UberstrikeItemClass.WeaponShotgun:
				return DefaultWeaponIcons.Find(icon => icon.name.Contains("Shot"));
			case UberstrikeItemClass.WeaponSniperRifle:
				return DefaultWeaponIcons.Find(icon => icon.name.Contains("Sniper"));
			case UberstrikeItemClass.WeaponCannon:
				return DefaultWeaponIcons.Find(icon => icon.name.Contains("Cannon"));
			case UberstrikeItemClass.WeaponSplattergun:
				return DefaultWeaponIcons.Find(icon => icon.name.Contains("Splatter"));
			case UberstrikeItemClass.WeaponLauncher:
				return DefaultWeaponIcons.Find(icon => icon.name.Contains("Launcher"));
		}

		return null;
	}

	public Texture2D GetFunctionalItemIcon(int itemId) {
		var functionalItemHolder = UnityItemsFunctional.Find(holder => holder.ItemId == itemId);

		if (functionalItemHolder == null) {
			Debug.LogWarning("Failed to find icon for functional item with id: " + itemId);

			return null;
		}

		return functionalItemHolder.Icon;
	}

	[Serializable]
	public class FunctionalItemHolder {
		public Texture2D Icon;
		public int ItemId;
		public string Name;
	}
}
