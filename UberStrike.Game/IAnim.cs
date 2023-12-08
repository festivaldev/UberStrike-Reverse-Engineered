public interface IAnim : IUpdatable {
	bool IsAnimating { get; set; }
	float Duration { get; set; }
	float StartTime { get; set; }
	void Start();
	void Stop();
}
