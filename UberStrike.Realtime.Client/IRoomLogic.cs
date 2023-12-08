namespace UberStrike.Realtime.Client {
	public interface IRoomLogic : IEventDispatcher {
		IOperationSender Operations { get; }
	}
}
