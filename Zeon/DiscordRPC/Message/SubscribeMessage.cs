using System;
using DiscordRPC.RPC.Payload;

namespace DiscordRPC.Message
{
	// Token: 0x0200002C RID: 44
	public class SubscribeMessage : IMessage
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600016F RID: 367 RVA: 0x0000687B File Offset: 0x00004A7B
		public override MessageType Type
		{
			get
			{
				return MessageType.Subscribe;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000170 RID: 368 RVA: 0x0000687E File Offset: 0x00004A7E
		// (set) Token: 0x06000171 RID: 369 RVA: 0x00006886 File Offset: 0x00004A86
		public EventType Event { get; internal set; }

		// Token: 0x06000172 RID: 370 RVA: 0x0000688F File Offset: 0x00004A8F
		internal SubscribeMessage(ServerEvent evt)
		{
			switch (evt)
			{
			default:
				this.Event = EventType.Join;
				return;
			case ServerEvent.ActivitySpectate:
				this.Event = EventType.Spectate;
				return;
			case ServerEvent.ActivityJoinRequest:
				this.Event = EventType.JoinRequest;
				return;
			}
		}
	}
}
