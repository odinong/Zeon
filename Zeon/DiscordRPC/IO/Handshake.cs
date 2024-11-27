using System;
using Newtonsoft.Json;

namespace DiscordRPC.IO
{
	// Token: 0x02000033 RID: 51
	internal class Handshake
	{
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600019B RID: 411 RVA: 0x00006C99 File Offset: 0x00004E99
		// (set) Token: 0x0600019C RID: 412 RVA: 0x00006CA1 File Offset: 0x00004EA1
		[JsonProperty("v")]
		public int Version { get; set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600019D RID: 413 RVA: 0x00006CAA File Offset: 0x00004EAA
		// (set) Token: 0x0600019E RID: 414 RVA: 0x00006CB2 File Offset: 0x00004EB2
		[JsonProperty("client_id")]
		public string ClientID { get; set; }
	}
}
