using System;
using Newtonsoft.Json;

namespace DiscordRPC.Message
{
	// Token: 0x0200002A RID: 42
	public class ReadyMessage : IMessage
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00006832 File Offset: 0x00004A32
		public override MessageType Type
		{
			get
			{
				return MessageType.Ready;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00006835 File Offset: 0x00004A35
		// (set) Token: 0x06000167 RID: 359 RVA: 0x0000683D File Offset: 0x00004A3D
		[JsonProperty("config")]
		public Configuration Configuration { get; set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00006846 File Offset: 0x00004A46
		// (set) Token: 0x06000169 RID: 361 RVA: 0x0000684E File Offset: 0x00004A4E
		[JsonProperty("user")]
		public User User { get; set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00006857 File Offset: 0x00004A57
		// (set) Token: 0x0600016B RID: 363 RVA: 0x0000685F File Offset: 0x00004A5F
		[JsonProperty("v")]
		public int Version { get; set; }
	}
}
