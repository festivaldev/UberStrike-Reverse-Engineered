using System.Collections;
using UberStrike.Core.Models;
using UnityEngine;

public class PlayerDropCoinPickupItem : PickupItem {
	private float _timeout;
	public float Timeout = 10f;

	private IEnumerator Start() {
		_timeout = Time.time + Timeout;
		var oldpos = transform.position;
		var newpos = oldpos;
		RaycastHit hit;

		if (Physics.Raycast(oldpos + Vector3.up, Vector3.down, out hit, 100f, UberstrikeLayerMasks.ProtectionMask) && oldpos.y > hit.point.y + 1f) {
			newpos = hit.point + Vector3.up;
		}

		_timeout = Time.time + Timeout;
		var time = 0f;

		while (_timeout > Time.time) {
			yield return new WaitForEndOfFrame();

			time += Time.deltaTime;
			transform.position = Vector3.Lerp(oldpos, newpos, time);
		}

		SetItemAvailable(false);
		enabled = false;

		yield return new WaitForSeconds(2f);

		Destroy(gameObject);
	}

	private void Update() {
		if (_pickupItem) {
			_pickupItem.Rotate(Vector3.up, 150f * Time.deltaTime, Space.Self);
		}
	}

	protected override bool OnPlayerPickup() {
		GameState.Current.Actions.PickupPowerup(PickupID, PickupItemType.Coin, 1);
		GameData.Instance.OnItemPickup.Fire("Point", PickUpMessageType.Coin);
		PlayLocalPickupSound(GameAudio.GetPoints);
		StartCoroutine(StartHidingPickupForSeconds(0));

		return true;
	}

	protected override void OnRemotePickup() {
		PlayRemotePickupSound(GameAudio.GetPoints, transform.position);
	}
}
