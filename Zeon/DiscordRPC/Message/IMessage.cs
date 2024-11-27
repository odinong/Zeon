using System;

namespace DiscordRPC.Message
{
	// Token: 0x02000025 RID: 37
	public abstract class IMessage
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000151 RID: 337
		public abstract MessageType Type { get; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000152 RID: 338 RVA: 0x0000674B File Offset: 0x0000494B
		public DateTime TimeCreated
		{
			get
			{
				return this._timecreated;
			}
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00006753 File Offset: 0x00004953
		public IMessage()
		{
			this._timecreated = DateTime.Now;
		}

		// Token: 0x040000AA RID: 170
		private DateTime _timecreated;
	}
}
