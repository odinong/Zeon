using System;
using DiscordRPC.Converters;
using Newtonsoft.Json;

namespace DiscordRPC.RPC.Payload
{
	// Token: 0x02000012 RID: 18
	internal abstract class IPayload
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00005DF1 File Offset: 0x00003FF1
		// (set) Token: 0x060000FB RID: 251 RVA: 0x00005DF9 File Offset: 0x00003FF9
		[JsonProperty("cmd")]
		[JsonConverter(typeof(EnumSnakeCaseConverter))]
		public Command Command { get; set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00005E02 File Offset: 0x00004002
		// (set) Token: 0x060000FD RID: 253 RVA: 0x00005E0A File Offset: 0x0000400A
		[JsonProperty("nonce")]
		public string Nonce { get; set; }

		// Token: 0x060000FE RID: 254 RVA: 0x00005E13 File Offset: 0x00004013
		protected IPayload()
		{
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00005E1B File Offset: 0x0000401B
		protected IPayload(long nonce)
		{
			this.Nonce = nonce.ToString();
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00005E30 File Offset: 0x00004030
		public override string ToString()
		{
			return string.Format("Payload || Command: {0}, Nonce: {1}", this.Command, this.Nonce);
		}
	}
}
