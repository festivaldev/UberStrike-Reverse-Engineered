using UnityEngine;

public class FloatProperty : Property<float> {
	public float Min { get; private set; }
	public float Max { get; private set; }

	public override float Value {
		get { return base.Value; }
		set { base.Value = Mathf.Clamp(value, Min, Max); }
	}

	public FloatProperty(float value = 0f, float min = -3.4028235E+38f, float max = 3.4028235E+38f) {
		Min = min;
		Max = max;
		Value = value;
	}
}
