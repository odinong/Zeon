using System;
using DiscordRPC.RPC.Payload;

namespace DiscordRPC.Message
{
	// Token: 0x0200002D RID: 45
	public class UnsubscribeMessage : IMessage
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000173 RID: 371 RVA: 0x000068C2 File Offset: 0x00004AC2
		public override MessageType Type
		{
			get
			{
				return MessageType.Unsubscribe;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000174 RID: 372 RVA: 0x000068C5 File Offset: 0x00004AC5
		// (set) Token: 0x06000175 RID: 373 RVA: 0x000068CD File Offset: 0x00004ACD
		public EventType Event { get; internal set; }

		// Token: 0x06000176 RID: 374 RVA: 0x000068D6 File Offset: 0x00004AD6
		internal UnsubscribeMessage(ServerEvent evt)
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
