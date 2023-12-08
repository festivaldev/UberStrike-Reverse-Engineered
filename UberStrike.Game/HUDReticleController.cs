using System.Collections.Generic;
using UberStrike.Core.Models;
using UberStrike.Core.Types;
using UnityEngine;

public class HUDReticleController : MonoBehaviour {
	public enum State {
		None,
		Enemy,
		Friend
	}

	private UberstrikeItemClass activeReticleId;

	[SerializeField]
	private ReticleView cannon;

	private float enemyReticleElapsedTime;
	private bool isDisplayingEnemyReticle;

	[SerializeField]
	private ReticleView launcher;

	[SerializeField]
	private ReticleView machinegun;

	[SerializeField]
	private ReticleView melee;

	private Dictionary<UberstrikeItemClass, ReticleView> reticles = new Dictionary<UberstrikeItemClass, ReticleView>();

	[SerializeField]
	private ReticleView shotgun;

	[SerializeField]
	private ReticleView sniper;

	[SerializeField]
	private ReticleView splattergun;

	[SerializeField]
	private ZoomInView zoomInView;

	private bool IsSecondaryReticle {
		get { return GameState.Current.PlayerData.ActiveWeapon.Value.View.SecondaryActionReticle != 1; }
	}

	public ReticleView ActiveReticle {
		get { return (!reticles.ContainsKey(activeReticleId)) ? null : reticles[activeReticleId]; }
	}

	private void OnEnable() {
		GameState.Current.PlayerData.ActiveWeapon.Fire();
	}

	private void OnDisable() {
		zoomInView.Show(false);
	}

	private void Start() {
		reticles.Add(UberstrikeItemClass.WeaponMelee, melee);
		reticles.Add(UberstrikeItemClass.WeaponMachinegun, machinegun);
		reticles.Add(UberstrikeItemClass.WeaponShotgun, shotgun);
		reticles.Add(UberstrikeItemClass.WeaponSplattergun, splattergun);
		reticles.Add(UberstrikeItemClass.WeaponCannon, cannon);
		reticles.Add(UberstrikeItemClass.WeaponLauncher, launcher);
		reticles.Add(UberstrikeItemClass.WeaponSniperRifle, sniper);

		GameState.Current.PlayerData.ActiveWeapon.AddEventAndFire(delegate(WeaponSlot el) {
			if (el == null) {
				return;
			}

			EnableReticle(false);
			activeReticleId = el.View.ItemClass;
			EnableReticle(activeReticleId != UberstrikeItemClass.WeaponSniperRifle || el.View.CustomProperties.ContainsKey("ShowReticleForSniper"));
			zoomInView.Show(false);
		}, this);

		GameState.Current.PlayerData.WeaponFired.AddEvent(delegate(WeaponSlot el) {
			if (ActiveReticle != null) {
				ActiveReticle.Shoot();
			}
		}, this);

		GameState.Current.PlayerData.FocusedPlayerTeam.AddEvent(delegate(TeamID el) {
			if (GameState.Current.IsTeamGame && el == GameState.Current.PlayerData.Player.TeamID) {
				UpdateColorForState(State.Friend);

				return;
			}

			UpdateColorForState((!isDisplayingEnemyReticle) ? State.None : State.Enemy);
		}, this);

		GameState.Current.PlayerData.AppliedDamage.AddEvent(delegate(DamageInfo el) {
			isDisplayingEnemyReticle = true;
			enemyReticleElapsedTime = Time.time + 1f;
		}, this);

		GameState.Current.PlayerData.IsIronSighted.AddEvent(delegate(bool el) {
			if (IsSecondaryReticle) {
				EnableReticle(!el);
			}
		}, this);

		GameState.Current.PlayerData.IsZoomedIn.AddEvent(delegate(bool el) { zoomInView.Show(el && IsSecondaryReticle); }, this);
	}

	public void EnableReticle(bool isEnabled) {
		if (ActiveReticle != null) {
			ActiveReticle.gameObject.SetActive(isEnabled);
		}
	}

	private void UpdateColorForState(State newState) {
		Color color;

		if (newState != State.Enemy) {
			if (newState != State.Friend) {
				color = Color.white;
			} else {
				color = Color.green;
			}
		} else {
			color = Color.red;
		}

		if (ActiveReticle != null) {
			ActiveReticle.SetColor(color);
		}
	}

	private void Update() {
		if (isDisplayingEnemyReticle && Time.time > enemyReticleElapsedTime) {
			isDisplayingEnemyReticle = false;
		}
	}
}
