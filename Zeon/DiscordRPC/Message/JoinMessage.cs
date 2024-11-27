using System;
using Newtonsoft.Json;

namespace DiscordRPC.Message
{
	// Token: 0x02000026 RID: 38
	public class JoinMessage : IMessage
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00006766 File Offset: 0x00004966
		public override MessageType Type
		{
			get
			{
				return MessageType.Join;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000155 RID: 341 RVA: 0x00006769 File Offset: 0x00004969
		// (set) Token: 0x06000156 RID: 342 RVA: 0x00006771 File Offset: 0x00004971
		[JsonProperty("secret")]
		public string Secret { get; internal set; }
	}
}
