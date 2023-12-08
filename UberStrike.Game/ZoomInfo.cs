public class ZoomInfo {
	private float _defaultMultiplier = 1f;
	private float _maxMultiplier = 1f;
	private float _minMultiplier = 1f;
	public float CurrentMultiplier;

	public float MinMultiplier {
		get { return _minMultiplier; }
		private set { _minMultiplier = value; }
	}

	public float MaxMultiplier {
		get { return _maxMultiplier; }
		private set { _maxMultiplier = value; }
	}

	public float DefaultMultiplier {
		get { return _defaultMultiplier; }
		private set { _defaultMultiplier = value; }
	}

	public ZoomInfo(float defaultMultiplier, float minMultiplier, float maxMultiplier) {
		if (minMultiplier > 0f) {
			MinMultiplier = minMultiplier;
		}

		if (maxMultiplier > 0f) {
			MaxMultiplier = maxMultiplier;
		}

		if (defaultMultiplier > 0f) {
			DefaultMultiplier = defaultMultiplier;
			CurrentMultiplier = DefaultMultiplier;
		}
	}
}
