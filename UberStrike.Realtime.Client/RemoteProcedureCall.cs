using System.Collections.Generic;

namespace UberStrike.Realtime.Client {
	public delegate bool RemoteProcedureCall(byte customOpCode, Dictionary<byte, object> customOpParameters, bool sendReliable = true, byte channelId = 0, bool encryption = false);
}
