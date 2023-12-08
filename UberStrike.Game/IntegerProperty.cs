using UnityEngine;

public class IntegerProperty : Property<int> {
	public int Min { get; private set; }
	public int Max { get; private set; }

	public override int Value {
		get { return base.Value; }
		set { base.Value = Mathf.Clamp(value, Min, Max); }
	}

	public IntegerProperty(int value = 0, int min = -2147483648, int max = 2147483647) {
		Min = min;
		Max = max;
		Value = value;
	}
}
