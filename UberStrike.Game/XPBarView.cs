using System.Collections;
using UnityEngine;

public class XPBarView : MonoBehaviour {
	[SerializeField]
	private float animageSpeed = 2f;

	[SerializeField]
	private UISprite bar;

	[SerializeField]
	private UISprite bgr;

	private float cachedXP = -1f;

	[SerializeField]
	private UILabel currentLevel;

	[SerializeField]
	private UILabel nextLevel;

	private IEnumerator Animate(float percentage01) {
		percentage01 = Mathf.Clamp01(percentage01);
		Transform tr = bar.transform;
		float fullWidth = bgr.transform.localScale.x;

		while (Mathf.Abs(tr.localScale.x / fullWidth - percentage01) > 0.01f) {
			var scale = tr.localScale;
			scale.x = Mathf.MoveTowards(scale.x, fullWidth * percentage01, Time.deltaTime * animageSpeed * fullWidth);
			tr.localScale = scale;

			yield return 0;
		}
	}

	private void Update() {
		var playerExperience = PlayerDataManager.PlayerExperience;

		if (playerExperience != cachedXP) {
			cachedXP = playerExperience;
			var levelForXp = XpPointsUtil.GetLevelForXp(playerExperience);
			currentLevel.text = "Lvl " + levelForXp;
			nextLevel.text = "Lvl " + Mathf.Clamp(levelForXp + 1, 1, XpPointsUtil.MaxPlayerLevel);
			int num;
			int num2;
			XpPointsUtil.GetXpRangeForLevel(levelForXp, out num, out num2);
			StopAllCoroutines();
			StartCoroutine(Animate((playerExperience - num) / (float)(num2 - num)));
		}
	}
}
