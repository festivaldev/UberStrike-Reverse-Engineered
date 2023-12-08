using System.Collections.Generic;
using UnityEngine;

public class ReticleView : MonoBehaviour {
	private List<UISprite> sprites = new List<UISprite>();
	private List<UITweener> tweens = new List<UITweener>();

	private void Awake() {
		sprites = new List<UISprite>(gameObject.GetComponentsInChildren<UISprite>());
		tweens = new List<UITweener>(GetComponentsInChildren<UITweener>());
	}

	public void Shoot() =>
		tweens.ForEach(el => {
			if (el.direction == AnimationOrTween.Direction.Reverse)
				el.Toggle();
			else
				el.Play(true);
		});

	public void SetColor(Color color) {
		sprites.ForEach(delegate(UISprite el) {
			if (el) {
				el.color = color;
			}
		});
	}
}
