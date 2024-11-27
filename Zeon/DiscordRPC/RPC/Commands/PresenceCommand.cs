using System;
using DiscordRPC.RPC.Payload;
using Newtonsoft.Json;

namespace DiscordRPC.RPC.Commands
{
	// Token: 0x02000018 RID: 24
	internal class PresenceCommand : ICommand
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00005FB5 File Offset: 0x000041B5
		// (set) Token: 0x06000117 RID: 279 RVA: 0x00005FBD File Offset: 0x000041BD
		[JsonProperty("pid")]
		public int PID { get; set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00005FC6 File Offset: 0x000041C6
		// (set) Token: 0x06000119 RID: 281 RVA: 0x00005FCE File Offset: 0x000041CE
		[JsonProperty("activity")]
		public RichPresence Presence { get; set; }

		// Token: 0x0600011A RID: 282 RVA: 0x00005FD7 File Offset: 0x000041D7
		public IPayload PreparePayload(long nonce)
		{
			return new ArgumentPayload(this, nonce)
			{
				Command = Command.SetActivity
			};
		}
	}
}
