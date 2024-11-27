using System;

namespace DiscordRPC.Message
{
	// Token: 0x02000020 RID: 32
	public class CloseMessage : IMessage
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600013C RID: 316 RVA: 0x000066A8 File Offset: 0x000048A8
		public override MessageType Type
		{
			get
			{
				return MessageType.Close;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600013D RID: 317 RVA: 0x000066AB File Offset: 0x000048AB
		// (set) Token: 0x0600013E RID: 318 RVA: 0x000066B3 File Offset: 0x000048B3
		public string Reason { get; internal set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600013F RID: 319 RVA: 0x000066BC File Offset: 0x000048BC
		// (set) Token: 0x06000140 RID: 320 RVA: 0x000066C4 File Offset: 0x000048C4
		public int Code { get; internal set; }

		// Token: 0x06000141 RID: 321 RVA: 0x000066CD File Offset: 0x000048CD
		internal CloseMessage()
		{
		}

		// Token: 0x06000142 RID: 322 RVA: 0x000066D5 File Offset: 0x000048D5
		internal CloseMessage(string reason)
		{
			this.Reason = reason;
		}
	}
}
