using System;
using DiscordRPC.RPC.Payload;

namespace DiscordRPC.RPC.Commands
{
	// Token: 0x0200001A RID: 26
	internal class SubscribeCommand : ICommand
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00006034 File Offset: 0x00004234
		// (set) Token: 0x06000123 RID: 291 RVA: 0x0000603C File Offset: 0x0000423C
		public ServerEvent Event { get; set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00006045 File Offset: 0x00004245
		// (set) Token: 0x06000125 RID: 293 RVA: 0x0000604D File Offset: 0x0000424D
		public bool IsUnsubscribe { get; set; }

		// Token: 0x06000126 RID: 294 RVA: 0x00006056 File Offset: 0x00004256
		public IPayload PreparePayload(long nonce)
		{
			return new EventPayload(nonce)
			{
				Command = (this.IsUnsubscribe ? Command.Unsubscribe : Command.Subscribe),
				Event = new ServerEvent?(this.Event)
			};
		}
	}
}
