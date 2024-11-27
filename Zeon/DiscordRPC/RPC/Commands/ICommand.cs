using System;
using DiscordRPC.RPC.Payload;

namespace DiscordRPC.RPC.Commands
{
	// Token: 0x02000017 RID: 23
	internal interface ICommand
	{
		// Token: 0x06000115 RID: 277
		IPayload PreparePayload(long nonce);
	}
}
