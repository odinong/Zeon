using System;

namespace DiscordRPC.Message
{
	// Token: 0x0200002B RID: 43
	public class SpectateMessage : JoinMessage
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600016D RID: 365 RVA: 0x00006870 File Offset: 0x00004A70
		public override MessageType Type
		{
			get
			{
				return MessageType.Spectate;
			}
		}
	}
}
