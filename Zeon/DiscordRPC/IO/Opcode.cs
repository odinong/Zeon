using System;

namespace DiscordRPC.IO
{
	// Token: 0x02000036 RID: 54
	public enum Opcode : uint
	{
		// Token: 0x040000DA RID: 218
		Handshake,
		// Token: 0x040000DB RID: 219
		Frame,
		// Token: 0x040000DC RID: 220
		Close,
		// Token: 0x040000DD RID: 221
		Ping,
		// Token: 0x040000DE RID: 222
		Pong
	}
}
