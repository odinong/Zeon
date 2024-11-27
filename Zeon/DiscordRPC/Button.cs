using System;
using System.Text;
using DiscordRPC.Exceptions;
using Newtonsoft.Json;

namespace DiscordRPC
{
	// Token: 0x0200000A RID: 10
	public class Button
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00004264 File Offset: 0x00002464
		// (set) Token: 0x060000AD RID: 173 RVA: 0x0000426C File Offset: 0x0000246C
		[JsonProperty("label")]
		public string Label
		{
			get
			{
				return this._label;
			}
			set
			{
				if (!BaseRichPresence.ValidateString(value, out this._label, 32, Encoding.UTF8))
				{
					throw new StringOutOfRangeException(32);
				}
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000AE RID: 174 RVA: 0x0000428B File Offset: 0x0000248B
		// (set) Token: 0x060000AF RID: 175 RVA: 0x00004294 File Offset: 0x00002494
		[JsonProperty("url")]
		public string Url
		{
			get
			{
				return this._url;
			}
			set
			{
				if (!BaseRichPresence.ValidateString(value, out this._url, 512, Encoding.UTF8))
				{
					throw new StringOutOfRangeException(512);
				}
				Uri uri;
				if (!Uri.TryCreate(this._url, UriKind.Absolute, out uri))
				{
					throw new ArgumentException("Url must be a valid URI");
				}
			}
		}

		// Token: 0x0400003F RID: 63
		private string _label;

		// Token: 0x04000040 RID: 64
		private string _url;
	}
}
