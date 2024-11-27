using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DiscordRPC.RPC.Payload
{
	// Token: 0x02000013 RID: 19
	internal class ArgumentPayload : IPayload
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00005E4D File Offset: 0x0000404D
		// (set) Token: 0x06000102 RID: 258 RVA: 0x00005E55 File Offset: 0x00004055
		[JsonProperty("args", NullValueHandling = NullValueHandling.Ignore)]
		public JObject Arguments { get; set; }

		// Token: 0x06000103 RID: 259 RVA: 0x00005E5E File Offset: 0x0000405E
		public ArgumentPayload()
		{
			this.Arguments = null;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00005E6D File Offset: 0x0000406D
		public ArgumentPayload(long nonce) : base(nonce)
		{
			this.Arguments = null;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00005E7D File Offset: 0x0000407D
		public ArgumentPayload(object args, long nonce) : base(nonce)
		{
			this.SetObject(args);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00005E8D File Offset: 0x0000408D
		public void SetObject(object obj)
		{
			this.Arguments = JObject.FromObject(obj);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00005E9B File Offset: 0x0000409B
		public T GetObject<T>()
		{
			return this.Arguments.ToObject<T>();
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00005EA8 File Offset: 0x000040A8
		public override string ToString()
		{
			return "Argument " + base.ToString();
		}
	}
}
