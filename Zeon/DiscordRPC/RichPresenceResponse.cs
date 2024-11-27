using System;
using Newtonsoft.Json;

namespace DiscordRPC
{
	// Token: 0x0200000C RID: 12
	internal sealed class RichPresenceResponse : BaseRichPresence
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000BF RID: 191 RVA: 0x000046AF File Offset: 0x000028AF
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x000046B7 File Offset: 0x000028B7
		[JsonProperty("application_id")]
		public string ClientID { get; private set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x000046C0 File Offset: 0x000028C0
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x000046C8 File Offset: 0x000028C8
		[JsonProperty("name")]
		public string Name { get; private set; }
	}
}
