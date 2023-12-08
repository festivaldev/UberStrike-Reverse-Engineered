using System.IO;

public interface IInputChannel {
	InputChannelType ChannelType { get; }
	string Name { get; }
	bool IsChanged { get; }
	float Value { get; }
	float RawValue();
	void Listen();
	void Reset();
	void Serialize(MemoryStream stream);
}
