using System;
using System.Text;
using DiscordRPC.Exceptions;
using Newtonsoft.Json;

namespace DiscordRPC
{
	// Token: 0x02000007 RID: 7
	[Serializable]
	public class Assets
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00003DA4 File Offset: 0x00001FA4
		// (set) Token: 0x06000084 RID: 132 RVA: 0x00003DAC File Offset: 0x00001FAC
		[JsonProperty("large_image", NullValueHandling = NullValueHandling.Ignore)]
		public string LargeImageKey
		{
			get
			{
				return this._largeimagekey;
			}
			set
			{
				if (!BaseRichPresence.ValidateString(value, out this._largeimagekey, 256, Encoding.UTF8))
				{
					throw new StringOutOfRangeException(256);
				}
				string largeimagekey = this._largeimagekey;
				this._islargeimagekeyexternal = (largeimagekey != null && largeimagekey.StartsWith("mp:external/"));
				this._largeimageID = null;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00003E05 File Offset: 0x00002005
		[JsonIgnore]
		public bool IsLargeImageKeyExternal
		{
			get
			{
				return this._islargeimagekeyexternal;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00003E0D File Offset: 0x0000200D
		// (set) Token: 0x06000087 RID: 135 RVA: 0x00003E15 File Offset: 0x00002015
		[JsonProperty("large_text", NullValueHandling = NullValueHandling.Ignore)]
		public string LargeImageText
		{
			get
			{
				return this._largeimagetext;
			}
			set
			{
				if (!BaseRichPresence.ValidateString(value, out this._largeimagetext, 128, Encoding.UTF8))
				{
					throw new StringOutOfRangeException(128);
				}
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00003E3A File Offset: 0x0000203A
		// (set) Token: 0x06000089 RID: 137 RVA: 0x00003E44 File Offset: 0x00002044
		[JsonProperty("small_image", NullValueHandling = NullValueHandling.Ignore)]
		public string SmallImageKey
		{
			get
			{
				return this._smallimagekey;
			}
			set
			{
				if (!BaseRichPresence.ValidateString(value, out this._smallimagekey, 256, Encoding.UTF8))
				{
					throw new StringOutOfRangeException(256);
				}
				string smallimagekey = this._smallimagekey;
				this._issmallimagekeyexternal = (smallimagekey != null && smallimagekey.StartsWith("mp:external/"));
				this._smallimageID = null;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00003E9D File Offset: 0x0000209D
		[JsonIgnore]
		public bool IsSmallImageKeyExternal
		{
			get
			{
				return this._issmallimagekeyexternal;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00003EA5 File Offset: 0x000020A5
		// (set) Token: 0x0600008C RID: 140 RVA: 0x00003EAD File Offset: 0x000020AD
		[JsonProperty("small_text", NullValueHandling = NullValueHandling.Ignore)]
		public string SmallImageText
		{
			get
			{
				return this._smallimagetext;
			}
			set
			{
				if (!BaseRichPresence.ValidateString(value, out this._smallimagetext, 128, Encoding.UTF8))
				{
					throw new StringOutOfRangeException(128);
				}
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00003ED2 File Offset: 0x000020D2
		[JsonIgnore]
		public ulong? LargeImageID
		{
			get
			{
				return this._largeimageID;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00003EDA File Offset: 0x000020DA
		[JsonIgnore]
		public ulong? SmallImageID
		{
			get
			{
				return this._smallimageID;
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003EE4 File Offset: 0x000020E4
		internal void Merge(Assets other)
		{
			this._smallimagetext = other._smallimagetext;
			this._largeimagetext = other._largeimagetext;
			ulong value;
			if (ulong.TryParse(other._largeimagekey, out value))
			{
				this._largeimageID = new ulong?(value);
			}
			else
			{
				this._largeimagekey = other._largeimagekey;
				this._largeimageID = null;
			}
			ulong value2;
			if (ulong.TryParse(other._smallimagekey, out value2))
			{
				this._smallimageID = new ulong?(value2);
				return;
			}
			this._smallimagekey = other._smallimagekey;
			this._smallimageID = null;
		}

		// Token: 0x04000031 RID: 49
		private string _largeimagekey;

		// Token: 0x04000032 RID: 50
		private bool _islargeimagekeyexternal;

		// Token: 0x04000033 RID: 51
		private string _largeimagetext;

		// Token: 0x04000034 RID: 52
		private string _smallimagekey;

		// Token: 0x04000035 RID: 53
		private bool _issmallimagekeyexternal;

		// Token: 0x04000036 RID: 54
		private string _smallimagetext;

		// Token: 0x04000037 RID: 55
		private ulong? _largeimageID;

		// Token: 0x04000038 RID: 56
		private ulong? _smallimageID;
	}
}
