using System;
using Newtonsoft.Json;

namespace DiscordRPC.Message
{
	// Token: 0x02000023 RID: 35
	public class ErrorMessage : IMessage
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600014B RID: 331 RVA: 0x0000671E File Offset: 0x0000491E
		public override MessageType Type
		{
			get
			{
				return MessageType.Error;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00006721 File Offset: 0x00004921
		// (set) Token: 0x0600014D RID: 333 RVA: 0x00006729 File Offset: 0x00004929
		[JsonProperty("code")]
		public ErrorCode Code { get; internal set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00006732 File Offset: 0x00004932
		// (set) Token: 0x0600014F RID: 335 RVA: 0x0000673A File Offset: 0x0000493A
		[JsonProperty("message")]
		public string Message { get; internal set; }
	}
}
