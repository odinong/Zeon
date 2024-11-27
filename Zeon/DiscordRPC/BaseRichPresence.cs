using System;
using System.Text;
using DiscordRPC.Exceptions;
using DiscordRPC.Helper;
using Newtonsoft.Json;

namespace DiscordRPC
{
	// Token: 0x02000005 RID: 5
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	[Serializable]
	public class BaseRichPresence
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000061 RID: 97 RVA: 0x0000378C File Offset: 0x0000198C
		// (set) Token: 0x06000062 RID: 98 RVA: 0x00003794 File Offset: 0x00001994
		[JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
		public string State
		{
			get
			{
				return this._state;
			}
			set
			{
				if (!BaseRichPresence.ValidateString(value, out this._state, 128, Encoding.UTF8))
				{
					throw new StringOutOfRangeException("State", 0, 128);
				}
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000063 RID: 99 RVA: 0x000037BF File Offset: 0x000019BF
		// (set) Token: 0x06000064 RID: 100 RVA: 0x000037C7 File Offset: 0x000019C7
		[JsonProperty("details", NullValueHandling = NullValueHandling.Ignore)]
		public string Details
		{
			get
			{
				return this._details;
			}
			set
			{
				if (!BaseRichPresence.ValidateString(value, out this._details, 128, Encoding.UTF8))
				{
					throw new StringOutOfRangeException(128);
				}
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000065 RID: 101 RVA: 0x000037EC File Offset: 0x000019EC
		// (set) Token: 0x06000066 RID: 102 RVA: 0x000037F4 File Offset: 0x000019F4
		[JsonProperty("timestamps", NullValueHandling = NullValueHandling.Ignore)]
		public Timestamps Timestamps { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000067 RID: 103 RVA: 0x000037FD File Offset: 0x000019FD
		// (set) Token: 0x06000068 RID: 104 RVA: 0x00003805 File Offset: 0x00001A05
		[JsonProperty("assets", NullValueHandling = NullValueHandling.Ignore)]
		public Assets Assets { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000069 RID: 105 RVA: 0x0000380E File Offset: 0x00001A0E
		// (set) Token: 0x0600006A RID: 106 RVA: 0x00003816 File Offset: 0x00001A16
		[JsonProperty("party", NullValueHandling = NullValueHandling.Ignore)]
		public Party Party { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600006B RID: 107 RVA: 0x0000381F File Offset: 0x00001A1F
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00003827 File Offset: 0x00001A27
		[JsonProperty("secrets", NullValueHandling = NullValueHandling.Ignore)]
		public Secrets Secrets { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00003830 File Offset: 0x00001A30
		// (set) Token: 0x0600006E RID: 110 RVA: 0x00003838 File Offset: 0x00001A38
		[JsonProperty("instance", NullValueHandling = NullValueHandling.Ignore)]
		[Obsolete("This was going to be used, but was replaced by JoinSecret instead")]
		private bool Instance { get; set; }

		// Token: 0x0600006F RID: 111 RVA: 0x00003844 File Offset: 0x00001A44
		public bool HasTimestamps()
		{
			return this.Timestamps != null && (this.Timestamps.Start != null || this.Timestamps.End != null);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003885 File Offset: 0x00001A85
		public bool HasAssets()
		{
			return this.Assets != null;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003890 File Offset: 0x00001A90
		public bool HasParty()
		{
			return this.Party != null && this.Party.ID != null;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000038AA File Offset: 0x00001AAA
		public bool HasSecrets()
		{
			return this.Secrets != null && (this.Secrets.JoinSecret != null || this.Secrets.SpectateSecret != null);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000038D4 File Offset: 0x00001AD4
		internal static bool ValidateString(string str, out string result, int bytes, Encoding encoding)
		{
			result = str;
			if (str == null)
			{
				return true;
			}
			string str2 = str.Trim();
			if (!str2.WithinLength(bytes, encoding))
			{
				return false;
			}
			result = str2.GetNullOrString();
			return true;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003905 File Offset: 0x00001B05
		public static implicit operator bool(BaseRichPresence presesnce)
		{
			return presesnce != null;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x0000390C File Offset: 0x00001B0C
		internal virtual bool Matches(RichPresence other)
		{
			if (other == null)
			{
				return false;
			}
			if (this.State != other.State || this.Details != other.Details)
			{
				return false;
			}
			if (this.Timestamps != null)
			{
				if (other.Timestamps != null)
				{
					ulong? num = other.Timestamps.StartUnixMilliseconds;
					ulong? num2 = this.Timestamps.StartUnixMilliseconds;
					if (num.GetValueOrDefault() == num2.GetValueOrDefault() & num != null == (num2 != null))
					{
						num2 = other.Timestamps.EndUnixMilliseconds;
						num = this.Timestamps.EndUnixMilliseconds;
						if (num2.GetValueOrDefault() == num.GetValueOrDefault() & num2 != null == (num != null))
						{
							goto IL_C2;
						}
					}
				}
				return false;
			}
			if (other.Timestamps != null)
			{
				return false;
			}
			IL_C2:
			if (this.Secrets != null)
			{
				if (other.Secrets == null || other.Secrets.JoinSecret != this.Secrets.JoinSecret || other.Secrets.MatchSecret != this.Secrets.MatchSecret || other.Secrets.SpectateSecret != this.Secrets.SpectateSecret)
				{
					return false;
				}
			}
			else if (other.Secrets != null)
			{
				return false;
			}
			if (this.Party != null)
			{
				if (other.Party == null || other.Party.ID != this.Party.ID || other.Party.Max != this.Party.Max || other.Party.Size != this.Party.Size || other.Party.Privacy != this.Party.Privacy)
				{
					return false;
				}
			}
			else if (other.Party != null)
			{
				return false;
			}
			if (this.Assets != null)
			{
				if (other.Assets == null || other.Assets.LargeImageKey != this.Assets.LargeImageKey || other.Assets.LargeImageText != this.Assets.LargeImageText || other.Assets.SmallImageKey != this.Assets.SmallImageKey || other.Assets.SmallImageText != this.Assets.SmallImageText)
				{
					return false;
				}
			}
			else if (other.Assets != null)
			{
				return false;
			}
			return this.Instance == other.Instance;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003B70 File Offset: 0x00001D70
		public RichPresence ToRichPresence()
		{
			RichPresence richPresence = new RichPresence();
			richPresence.State = this.State;
			richPresence.Details = this.Details;
			richPresence.Party = ((!this.HasParty()) ? this.Party : null);
			richPresence.Secrets = ((!this.HasSecrets()) ? this.Secrets : null);
			if (this.HasAssets())
			{
				richPresence.Assets = new Assets
				{
					SmallImageKey = this.Assets.SmallImageKey,
					SmallImageText = this.Assets.SmallImageText,
					LargeImageKey = this.Assets.LargeImageKey,
					LargeImageText = this.Assets.LargeImageText
				};
			}
			if (this.HasTimestamps())
			{
				richPresence.Timestamps = new Timestamps();
				if (this.Timestamps.Start != null)
				{
					richPresence.Timestamps.Start = this.Timestamps.Start;
				}
				if (this.Timestamps.End != null)
				{
					richPresence.Timestamps.End = this.Timestamps.End;
				}
			}
			return richPresence;
		}

		// Token: 0x04000027 RID: 39
		protected internal string _state;

		// Token: 0x04000028 RID: 40
		protected internal string _details;
	}
}
