using System;
using Newtonsoft.Json;

namespace DiscordRPC.Message
{
	// Token: 0x02000027 RID: 39
	public class JoinRequestMessage : IMessage
	{
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00006782 File Offset: 0x00004982
		public override MessageType Type
		{
			get
			{
				return MessageType.JoinRequest;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00006785 File Offset: 0x00004985
		// (set) Token: 0x0600015A RID: 346 RVA: 0x0000678D File Offset: 0x0000498D
		[JsonProperty("user")]
		public User User { get; internal set; }
	}
}
