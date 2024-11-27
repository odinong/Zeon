using System;
using DiscordRPC.RPC.Payload;
using Newtonsoft.Json;

namespace DiscordRPC.RPC.Commands
{
	// Token: 0x02000016 RID: 22
	internal class CloseCommand : ICommand
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00005F75 File Offset: 0x00004175
		// (set) Token: 0x06000112 RID: 274 RVA: 0x00005F7D File Offset: 0x0000417D
		[JsonProperty("pid")]
		public int PID { get; set; }

		// Token: 0x06000113 RID: 275 RVA: 0x00005F86 File Offset: 0x00004186
		public IPayload PreparePayload(long nonce)
		{
			return new ArgumentPayload
			{
				Command = Command.Dispatch,
				Nonce = null,
				Arguments = null
			};
		}

		// Token: 0x0400008D RID: 141
		[JsonProperty("close_reason")]
		public string value = "Unity 5.5 doesn't handle thread aborts. Can you please close me discord?";
	}
}
