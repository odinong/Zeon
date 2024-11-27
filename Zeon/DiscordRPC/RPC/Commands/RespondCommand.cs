using System;
using DiscordRPC.RPC.Payload;
using Newtonsoft.Json;

namespace DiscordRPC.RPC.Commands
{
	// Token: 0x02000019 RID: 25
	internal class RespondCommand : ICommand
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00005FEF File Offset: 0x000041EF
		// (set) Token: 0x0600011D RID: 285 RVA: 0x00005FF7 File Offset: 0x000041F7
		[JsonProperty("user_id")]
		public string UserID { get; set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00006000 File Offset: 0x00004200
		// (set) Token: 0x0600011F RID: 287 RVA: 0x00006008 File Offset: 0x00004208
		[JsonIgnore]
		public bool Accept { get; set; }

		// Token: 0x06000120 RID: 288 RVA: 0x00006011 File Offset: 0x00004211
		public IPayload PreparePayload(long nonce)
		{
			return new ArgumentPayload(this, nonce)
			{
				Command = (this.Accept ? Command.SendActivityJoinInvite : Command.CloseActivityJoinRequest)
			};
		}
	}
}
