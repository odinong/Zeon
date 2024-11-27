using System;
using Newtonsoft.Json;

namespace DiscordRPC
{
	// Token: 0x02000008 RID: 8
	[Serializable]
	public class Timestamps
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00003F7A File Offset: 0x0000217A
		public static Timestamps Now
		{
			get
			{
				return new Timestamps(DateTime.UtcNow);
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003F86 File Offset: 0x00002186
		public static Timestamps FromTimeSpan(double seconds)
		{
			return Timestamps.FromTimeSpan(TimeSpan.FromSeconds(seconds));
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003F93 File Offset: 0x00002193
		public static Timestamps FromTimeSpan(TimeSpan timespan)
		{
			return new Timestamps
			{
				Start = new DateTime?(DateTime.UtcNow),
				End = new DateTime?(DateTime.UtcNow + timespan)
			};
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00003FC0 File Offset: 0x000021C0
		// (set) Token: 0x06000095 RID: 149 RVA: 0x00003FC8 File Offset: 0x000021C8
		[JsonIgnore]
		public DateTime? Start { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00003FD1 File Offset: 0x000021D1
		// (set) Token: 0x06000097 RID: 151 RVA: 0x00003FD9 File Offset: 0x000021D9
		[JsonIgnore]
		public DateTime? End { get; set; }

		// Token: 0x06000098 RID: 152 RVA: 0x00003FE4 File Offset: 0x000021E4
		public Timestamps()
		{
			this.Start = null;
			this.End = null;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004018 File Offset: 0x00002218
		public Timestamps(DateTime start)
		{
			this.Start = new DateTime?(start);
			this.End = null;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00004046 File Offset: 0x00002246
		public Timestamps(DateTime start, DateTime end)
		{
			this.Start = new DateTime?(start);
			this.End = new DateTime?(end);
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00004068 File Offset: 0x00002268
		// (set) Token: 0x0600009C RID: 156 RVA: 0x000040A8 File Offset: 0x000022A8
		[JsonProperty("start", NullValueHandling = NullValueHandling.Ignore)]
		public ulong? StartUnixMilliseconds
		{
			get
			{
				if (this.Start == null)
				{
					return null;
				}
				return new ulong?(Timestamps.ToUnixMilliseconds(this.Start.Value));
			}
			set
			{
				this.Start = ((value != null) ? new DateTime?(Timestamps.FromUnixMilliseconds(value.Value)) : null);
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600009D RID: 157 RVA: 0x000040E0 File Offset: 0x000022E0
		// (set) Token: 0x0600009E RID: 158 RVA: 0x00004120 File Offset: 0x00002320
		[JsonProperty("end", NullValueHandling = NullValueHandling.Ignore)]
		public ulong? EndUnixMilliseconds
		{
			get
			{
				if (this.End == null)
				{
					return null;
				}
				return new ulong?(Timestamps.ToUnixMilliseconds(this.End.Value));
			}
			set
			{
				this.End = ((value != null) ? new DateTime?(Timestamps.FromUnixMilliseconds(value.Value)) : null);
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004158 File Offset: 0x00002358
		public static DateTime FromUnixMilliseconds(ulong unixTime)
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return dateTime.AddMilliseconds(Convert.ToDouble(unixTime));
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004184 File Offset: 0x00002384
		public static ulong ToUnixMilliseconds(DateTime date)
		{
			DateTime d = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return Convert.ToUInt64((date - d).TotalMilliseconds);
		}
	}
}
