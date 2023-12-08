using System;

[AttributeUsage(AttributeTargets.Field)]
public class CustomPropertyAttribute : Attribute {
	public string Name { get; private set; }

	public CustomPropertyAttribute(string name) {
		Name = name;
	}
}
