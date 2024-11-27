using System;
using System.Text;
using DiscordRPC.Exceptions;
using Newtonsoft.Json;

namespace DiscordRPC
{
	// Token: 0x02000006 RID: 6
	[Serializable]
	public class Secrets
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00003C92 File Offset: 0x00001E92
		// (set) Token: 0x06000079 RID: 121 RVA: 0x00003C9A File Offset: 0x00001E9A
		[Obsolete("This feature has been deprecated my Mason in issue #152 on the offical library. Was originally used as a Notify Me feature, it has been replaced with Join / Spectate.")]
		[JsonProperty("match", NullValueHandling = NullValueHandling.Ignore)]
		public string MatchSecret
		{
			get
			{
				return this._matchSecret;
			}
			set
			{
				if (!BaseRichPresence.ValidateString(value, out this._matchSecret, 128, Encoding.UTF8))
				{
					throw new StringOutOfRangeException(128);
				}
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00003CBF File Offset: 0x00001EBF
		// (set) Token: 0x0600007B RID: 123 RVA: 0x00003CC7 File Offset: 0x00001EC7
		[JsonProperty("join", NullValueHandling = NullValueHandling.Ignore)]
		public string JoinSecret
		{
			get
			{
				return this._joinSecret;
			}
			set
			{
				if (!BaseRichPresence.ValidateString(value, out this._joinSecret, 128, Encoding.UTF8))
				{
					throw new StringOutOfRangeException(128);
				}
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00003CEC File Offset: 0x00001EEC
		// (set) Token: 0x0600007D RID: 125 RVA: 0x00003CF4 File Offset: 0x00001EF4
		[JsonProperty("spectate", NullValueHandling = NullValueHandling.Ignore)]
		public string SpectateSecret
		{
			get
			{
				return this._spectateSecret;
			}
			set
			{
				if (!BaseRichPresence.ValidateString(value, out this._spectateSecret, 128, Encoding.UTF8))
				{
					throw new StringOutOfRangeException(128);
				}
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00003D19 File Offset: 0x00001F19
		public static Encoding Encoding
		{
			get
			{
				return Encoding.UTF8;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00003D20 File Offset: 0x00001F20
		public static int SecretLength
		{
			get
			{
				return 128;
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003D28 File Offset: 0x00001F28
		public static string CreateSecret(Random random)
		{
			byte[] array = new byte[Secrets.SecretLength];
			random.NextBytes(array);
			return Secrets.Encoding.GetString(array);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003D54 File Offset: 0x00001F54
		public static string CreateFriendlySecret(Random random)
		{
			string text = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < Secrets.SecretLength; i++)
			{
				stringBuilder.Append(text[random.Next(text.Length)]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400002E RID: 46
		private string _matchSecret;

		// Token: 0x0400002F RID: 47
		private string _joinSecret;

		// Token: 0x04000030 RID: 48
		private string _spectateSecret;
	}
}
