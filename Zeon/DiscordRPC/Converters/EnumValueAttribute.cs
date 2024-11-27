using System;

namespace DiscordRPC.Converters
{
	// Token: 0x0200004C RID: 76
	internal class EnumValueAttribute : Attribute
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000221 RID: 545 RVA: 0x00007E04 File Offset: 0x00006004
		// (set) Token: 0x06000222 RID: 546 RVA: 0x00007E0C File Offset: 0x0000600C
		public string Value { get; set; }

		// Token: 0x06000223 RID: 547 RVA: 0x00007E15 File Offset: 0x00006015
		public EnumValueAttribute(string value)
		{
			this.Value = value;
		}
	}
}
