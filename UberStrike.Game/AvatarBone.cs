using System;
using UnityEngine;

[Serializable]
public class AvatarBone {
	public BoneIndex Bone;
	public Transform Transform;
	public Vector3 OriginalPosition { get; set; }
	public Quaternion OriginalRotation { get; set; }
	public Collider Collider { get; set; }
	public Rigidbody Rigidbody { get; set; }
}
