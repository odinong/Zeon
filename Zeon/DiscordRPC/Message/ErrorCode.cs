using System;

namespace DiscordRPC.Message
{
	// Token: 0x02000024 RID: 36
	public enum ErrorCode
	{
		// Token: 0x040000A2 RID: 162
		Success,
		// Token: 0x040000A3 RID: 163
		PipeException,
		// Token: 0x040000A4 RID: 164
		ReadCorrupt,
		// Token: 0x040000A5 RID: 165
		NotImplemented = 10,
		// Token: 0x040000A6 RID: 166
		UnkownError = 1000,
		// Token: 0x040000A7 RID: 167
		InvalidPayload = 4000,
		// Token: 0x040000A8 RID: 168
		InvalidCommand = 4002,
		// Token: 0x040000A9 RID: 169
		InvalidEvent = 4004
	}
}
