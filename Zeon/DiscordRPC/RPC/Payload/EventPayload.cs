using System;
using DiscordRPC.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DiscordRPC.RPC.Payload
{
	// Token: 0x02000014 RID: 20
	internal class EventPayload : IPayload
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00005EBA File Offset: 0x000040BA
		// (set) Token: 0x0600010A RID: 266 RVA: 0x00005EC2 File Offset: 0x000040C2
		[JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
		public JObject Data { get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00005ECB File Offset: 0x000040CB
		// (set) Token: 0x0600010C RID: 268 RVA: 0x00005ED3 File Offset: 0x000040D3
		[JsonProperty("evt")]
		[JsonConverter(typeof(EnumSnakeCaseConverter))]
		public ServerEvent? Event { get; set; }

		// Token: 0x0600010D RID: 269 RVA: 0x00005EDC File Offset: 0x000040DC
		public EventPayload()
		{
			this.Data = null;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00005EEB File Offset: 0x000040EB
		public EventPayload(long nonce) : base(nonce)
		{
			this.Data = null;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005EFC File Offset: 0x000040FC
		public T GetObject<T>()
		{
			if (this.Data == null)
			{
				return default(T);
			}
			return this.Data.ToObject<T>();
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00005F28 File Offset: 0x00004128
		public override string ToString()
		{
			return "Event " + base.ToString() + ", Event: " + ((this.Event != null) ? this.Event.ToString() : "N/A");
		}
	}
}
