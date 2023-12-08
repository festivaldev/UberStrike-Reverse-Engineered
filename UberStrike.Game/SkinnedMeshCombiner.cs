using System;
using System.Collections.Generic;
using UnityEngine;

public class SkinnedMeshCombiner {
	public static void Combine(GameObject target, List<GameObject> objects) {
		if (target && objects != null) {
			CopyComponents(target, objects);
			var list = new List<SkinnedMeshRenderer>();

			foreach (var gameObject in objects) {
				if (gameObject != null) {
					list.AddRange(gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true));
				}
			}

			SuperCombineCreate(target, list);
		}
	}

	public static void Update(GameObject target, List<GameObject> objects) {
		if (target && objects != null) {
			CopyComponents(target, objects);
			var list = new List<SkinnedMeshRenderer>();

			foreach (var gameObject in objects) {
				if (gameObject) {
					list.AddRange(gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true));
				}
			}

			SuperCombineUpdate(target, list);
		}
	}

	private static void CopyComponents(GameObject target, List<GameObject> objects) {
		var hashSet = new HashSet<string>(Enum.GetNames(typeof(BoneIndex)));

		foreach (var gameObject in objects) {
			var list = new List<Component>(gameObject.GetComponentsInChildren<ParticleSystem>(true));
			list.AddRange(gameObject.GetComponentsInChildren<AudioSource>(true));

			foreach (var component in list) {
				if (!(component.transform.parent == null)) {
					var name = component.transform.parent.name;

					if (hashSet.Contains(name)) {
						var transform = target.transform.FindChildWithName(name);

						if (transform != null) {
							component.transform.Reparent(transform);
						}
					}
				}
			}
		}
	}

	private static GameObject SuperCombineCreate(GameObject sourceGameObject, List<SkinnedMeshRenderer> otherGear) {
		foreach (var skinnedMeshRenderer in otherGear) {
			if (skinnedMeshRenderer.sharedMesh == null) {
				Debug.LogError(skinnedMeshRenderer.name + "'s sharedMesh is null!");
			}
		}

		var list = new List<CombineInstance>();
		var list2 = new List<Material>();
		var list3 = new List<Transform>();
		var dictionary = new Dictionary<string, Transform>();

		foreach (var transform in sourceGameObject.GetComponentsInChildren<Transform>(true)) {
			dictionary[transform.name] = transform.transform;
		}

		foreach (var skinnedMeshRenderer2 in sourceGameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true)) {
			list2.AddRange(skinnedMeshRenderer2.sharedMaterials);

			for (var k = 0; k < skinnedMeshRenderer2.sharedMesh.subMeshCount; k++) {
				list.Add(new CombineInstance {
					mesh = skinnedMeshRenderer2.sharedMesh,
					subMeshIndex = k
				});

				list3.AddRange(skinnedMeshRenderer2.bones);
			}

			UnityEngine.Object.Destroy(skinnedMeshRenderer2);
		}

		if (otherGear != null && otherGear.Count > 0) {
			foreach (var skinnedMeshRenderer3 in otherGear) {
				list2.AddRange(skinnedMeshRenderer3.sharedMaterials);

				if (!(skinnedMeshRenderer3.sharedMesh == null)) {
					for (var l = 0; l < skinnedMeshRenderer3.sharedMesh.subMeshCount; l++) {
						list.Add(new CombineInstance {
							mesh = skinnedMeshRenderer3.sharedMesh,
							subMeshIndex = l
						});

						foreach (var transform2 in skinnedMeshRenderer3.bones) {
							if (dictionary.ContainsKey(transform2.name)) {
								list3.Add(dictionary[transform2.name]);
							} else {
								Debug.LogError("Couldn't find a matching bone transform in the gameobject you're trying to add this skinned mesh to! " + transform2.name);
							}
						}
					}
				}
			}
		}

		var skinnedMeshRenderer4 = sourceGameObject.AddComponent<SkinnedMeshRenderer>();

		if (skinnedMeshRenderer4.sharedMesh == null) {
			skinnedMeshRenderer4.sharedMesh = new Mesh();
		}

		skinnedMeshRenderer4.sharedMesh.Clear();
		skinnedMeshRenderer4.sharedMesh.name = "CombinedMesh";
		skinnedMeshRenderer4.sharedMesh.CombineMeshes(list.ToArray(), false, false);
		skinnedMeshRenderer4.bones = list3.ToArray();

		foreach (var material in skinnedMeshRenderer4.materials) {
			UnityEngine.Object.Destroy(material);
		}

		skinnedMeshRenderer4.materials = list2.ToArray();
		var component = sourceGameObject.GetComponent<Animation>();

		if (component) {
			component.cullingType = AnimationCullingType.BasedOnClipBounds;
		}

		return sourceGameObject;
	}

	private static GameObject SuperCombineUpdate(GameObject sourceGameObject, List<SkinnedMeshRenderer> otherGear) {
		var list = new List<CombineInstance>();
		var list2 = new List<Material>();
		var list3 = new List<Transform>();
		var dictionary = new Dictionary<string, Transform>();

		foreach (var transform in sourceGameObject.GetComponentsInChildren<Transform>(true)) {
			dictionary[transform.name] = transform.transform;
		}

		if (otherGear != null && otherGear.Count > 0) {
			foreach (var skinnedMeshRenderer in otherGear) {
				list2.AddRange(skinnedMeshRenderer.sharedMaterials);

				if (skinnedMeshRenderer.sharedMesh == null) {
					Debug.Log("No shared mesh in " + skinnedMeshRenderer.name);
				} else {
					for (var j = 0; j < skinnedMeshRenderer.sharedMesh.subMeshCount; j++) {
						list.Add(new CombineInstance {
							mesh = skinnedMeshRenderer.sharedMesh,
							subMeshIndex = j
						});

						foreach (var transform2 in skinnedMeshRenderer.bones) {
							if (dictionary.ContainsKey(transform2.name)) {
								var transform3 = dictionary[transform2.name];
								transform3.localPosition = transform2.localPosition;
								list3.Add(transform3);
							} else {
								Debug.LogError("I couldn't find a matching bone transform in the gameobject you're trying to add this skinned mesh to! " + transform2.name);
							}
						}
					}
				}
			}
		} else {
			Debug.LogError("Gear array contains no Skinned Meshes! Trying to go naked?");
		}

		var component = sourceGameObject.GetComponent<SkinnedMeshRenderer>();

		if (component) {
			if (component.sharedMesh == null) {
				component.sharedMesh = new Mesh();
			}

			component.sharedMesh.Clear();
			component.sharedMesh.name = "CombinedMesh";
			component.sharedMesh.CombineMeshes(list.ToArray(), false, false);
			component.bones = list3.ToArray();

			foreach (var material in component.materials) {
				UnityEngine.Object.Destroy(material);
			}

			component.materials = list2.ToArray();
		} else {
			Debug.LogError("There is no SkinnedMeshRenderer on " + sourceGameObject.name);
		}

		var component2 = sourceGameObject.GetComponent<Animation>();

		if (component2) {
			component2.cullingType = AnimationCullingType.AlwaysAnimate;
		}

		return sourceGameObject;
	}
}
