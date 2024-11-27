using System;

namespace DiscordRPC.Message
{
	// Token: 0x02000021 RID: 33
	public class ConnectionEstablishedMessage : IMessage
	{
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000143 RID: 323 RVA: 0x000066E4 File Offset: 0x000048E4
		public override MessageType Type
		{
			get
			{
				return MessageType.ConnectionEstablished;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000144 RID: 324 RVA: 0x000066E8 File Offset: 0x000048E8
		// (set) Token: 0x06000145 RID: 325 RVA: 0x000066F0 File Offset: 0x000048F0
		public int ConnectedPipe { get; internal set; }
	}
}
