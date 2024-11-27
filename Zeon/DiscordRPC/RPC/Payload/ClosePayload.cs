using System;
using Newtonsoft.Json;

namespace DiscordRPC.RPC.Payload
{
	// Token: 0x02000010 RID: 16
	internal class ClosePayload : IPayload
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00005DB5 File Offset: 0x00003FB5
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x00005DBD File Offset: 0x00003FBD
		[JsonProperty("code")]
		public int Code { get; set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00005DC6 File Offset: 0x00003FC6
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x00005DCE File Offset: 0x00003FCE
		[JsonProperty("message")]
		public string Reason { get; set; }

		// Token: 0x060000F9 RID: 249 RVA: 0x00005DD7 File Offset: 0x00003FD7
		[JsonConstructor]
		public ClosePayload()
		{
			this.Code = -1;
			this.Reason = "";
		}
	}
}
