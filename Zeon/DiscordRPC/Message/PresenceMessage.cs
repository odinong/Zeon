using System;

namespace DiscordRPC.Message
{
	// Token: 0x02000029 RID: 41
	public class PresenceMessage : IMessage
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600015C RID: 348 RVA: 0x0000679E File Offset: 0x0000499E
		public override MessageType Type
		{
			get
			{
				return MessageType.PresenceUpdate;
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x000067A1 File Offset: 0x000049A1
		internal PresenceMessage() : this(null)
		{
		}

		// Token: 0x0600015E RID: 350 RVA: 0x000067AC File Offset: 0x000049AC
		internal PresenceMessage(RichPresenceResponse rpr)
		{
			if (rpr == null)
			{
				this.Presence = null;
				this.Name = "No Rich Presence";
				this.ApplicationID = "";
				return;
			}
			this.Presence = rpr;
			this.Name = rpr.Name;
			this.ApplicationID = rpr.ClientID;
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600015F RID: 351 RVA: 0x000067FF File Offset: 0x000049FF
		// (set) Token: 0x06000160 RID: 352 RVA: 0x00006807 File Offset: 0x00004A07
		public BaseRichPresence Presence { get; internal set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00006810 File Offset: 0x00004A10
		// (set) Token: 0x06000162 RID: 354 RVA: 0x00006818 File Offset: 0x00004A18
		public string Name { get; internal set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00006821 File Offset: 0x00004A21
		// (set) Token: 0x06000164 RID: 356 RVA: 0x00006829 File Offset: 0x00004A29
		public string ApplicationID { get; internal set; }
	}
}
