using System;

namespace DiscordRPC.Message
{
	// Token: 0x02000022 RID: 34
	public class ConnectionFailedMessage : IMessage
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00006701 File Offset: 0x00004901
		public override MessageType Type
		{
			get
			{
				return MessageType.ConnectionFailed;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000148 RID: 328 RVA: 0x00006705 File Offset: 0x00004905
		// (set) Token: 0x06000149 RID: 329 RVA: 0x0000670D File Offset: 0x0000490D
		public int FailedPipe { get; internal set; }
	}
}
