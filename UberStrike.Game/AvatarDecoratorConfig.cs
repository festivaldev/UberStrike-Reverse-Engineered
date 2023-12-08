using System.Collections;
using System.Collections.Generic;
using UberStrike.Core.Models;
using UnityEngine;

public class AvatarDecoratorConfig : MonoBehaviour {
	[SerializeField]
	private AvatarBone[] _avatarBones;

	private float _hangTime = 0.5f;
	private float _hangTimeDownwardForce = 4f;
	private Color _skinColor;

	private void Awake() {
		_skinColor = Color.magenta;

		foreach (var avatarBone in _avatarBones) {
			avatarBone.Collider = avatarBone.Transform.GetComponent<Collider>();
			avatarBone.Rigidbody = avatarBone.Transform.GetComponent<Rigidbody>();
			avatarBone.OriginalPosition = avatarBone.Transform.localPosition;
			avatarBone.OriginalRotation = avatarBone.Transform.localRotation;
		}
	}

	private void SetGravity(bool enabled) {
		foreach (var avatarBone in _avatarBones) {
			if (avatarBone != null && avatarBone.Rigidbody) {
				avatarBone.Rigidbody.useGravity = enabled;
			}
		}
	}

	public void ApplyDamageToRagdoll(DamageInfo damageInfo) {
		GameObject gameObject = null;

		switch (damageInfo.BodyPart) {
			case BodyPart.Body:
				gameObject = GetBone(BoneIndex.Spine).gameObject;

				break;
			case BodyPart.Head:
				gameObject = GetBone(BoneIndex.Head).gameObject;

				break;
			case BodyPart.Nuts:
				gameObject = GetBone(BoneIndex.Hips).gameObject;

				break;
		}

		if (gameObject != null) {
			var component = gameObject.GetComponent<RagdollBodyPart>();

			if (component != null) {
				StartCoroutine(Die(component, damageInfo));
			} else {
				Debug.LogError(gameObject.name + " doesn't contain a RagdollBodyPart component.");
			}
		} else {
			Debug.LogError("Bone GameObject " + damageInfo.BodyPart + " was not found.");
		}
	}

	private IEnumerator Die(RagdollBodyPart ragdollBodyPart, DamageInfo damageInfo) {
		SetGravity(false);

		if (damageInfo.IsExplosion) {
			damageInfo.Force *= damageInfo.Damage;
			damageInfo.UpwardsForceMultiplier = 10f;
		}

		ragdollBodyPart.ApplyDamage(damageInfo);
		var bTime = 0f;

		while (bTime < _hangTime) {
			bTime += Time.deltaTime;
			ragdollBodyPart.rigidbody.AddForce(Vector3.down * _hangTimeDownwardForce);

			yield return new WaitForEndOfFrame();
		}

		SetGravity(true);
	}

	public Color GetSkinColor() {
		return _skinColor;
	}

	public void SetSkinColor(Color skinColor) {
		var componentInChildren = GetComponentInChildren<SkinnedMeshRenderer>();

		if (componentInChildren != null) {
			_skinColor = skinColor;

			foreach (var material in componentInChildren.materials) {
				if (material.name.Contains("Skin")) {
					material.color = skinColor;
				}
			}
		}
	}

	public Transform GetBone(BoneIndex bone) {
		foreach (var avatarBone in _avatarBones) {
			if (avatarBone.Bone == bone) {
				return avatarBone.Transform;
			}
		}

		return transform;
	}

	public void SetBones(List<AvatarBone> bones) {
		_avatarBones = bones.ToArray();
	}

	public static void CopyBones(AvatarDecoratorConfig srcAvatar, AvatarDecoratorConfig dstAvatar) {
		foreach (var avatarBone in srcAvatar._avatarBones) {
			var bone = dstAvatar.GetBone(avatarBone.Bone);

			if (bone) {
				bone.position = avatarBone.Transform.position;
				bone.rotation = avatarBone.Transform.rotation;
			}
		}
	}
}
