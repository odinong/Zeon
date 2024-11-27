using System;
using DiscordRPC.Helper;
using Newtonsoft.Json;

namespace DiscordRPC
{
	// Token: 0x02000009 RID: 9
	[Serializable]
	public class Party
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x000041B7 File Offset: 0x000023B7
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x000041BF File Offset: 0x000023BF
		[JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
		public string ID
		{
			get
			{
				return this._partyid;
			}
			set
			{
				this._partyid = value.GetNullOrString();
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x000041CD File Offset: 0x000023CD
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x000041D5 File Offset: 0x000023D5
		[JsonIgnore]
		public int Size { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x000041DE File Offset: 0x000023DE
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x000041E6 File Offset: 0x000023E6
		[JsonIgnore]
		public int Max { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x000041EF File Offset: 0x000023EF
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x000041F7 File Offset: 0x000023F7
		[JsonProperty("privacy", NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Include)]
		public Party.PrivacySetting Privacy { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00004200 File Offset: 0x00002400
		// (set) Token: 0x060000AA RID: 170 RVA: 0x00004233 File Offset: 0x00002433
		[JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
		private int[] _size
		{
			get
			{
				int num = Math.Max(1, this.Size);
				return new int[]
				{
					num,
					Math.Max(num, this.Max)
				};
			}
			set
			{
				if (value.Length != 2)
				{
					this.Size = 0;
					this.Max = 0;
					return;
				}
				this.Size = value[0];
				this.Max = value[1];
			}
		}

		// Token: 0x0400003B RID: 59
		private string _partyid;

		// Token: 0x0200004D RID: 77
		public enum PrivacySetting
		{
			// Token: 0x040000EB RID: 235
			Private,
			// Token: 0x040000EC RID: 236
			Public
		}
	}
}
