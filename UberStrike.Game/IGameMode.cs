using System;

public interface IGameMode : IDisposable {
	GameMode Type { get; }
}
