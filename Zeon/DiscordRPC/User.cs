using System;
using Newtonsoft.Json;

namespace DiscordRPC
{
	// Token: 0x0200000D RID: 13
	public class User
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x000046D9 File Offset: 0x000028D9
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x000046E1 File Offset: 0x000028E1
		[JsonProperty("id")]
		public ulong ID { get; private set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x000046EA File Offset: 0x000028EA
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x000046F2 File Offset: 0x000028F2
		[JsonProperty("username")]
		public string Username { get; private set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x000046FB File Offset: 0x000028FB
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x00004703 File Offset: 0x00002903
		[JsonProperty("discriminator")]
		[Obsolete("Discord no longer uses discriminators.")]
		public int Discriminator { get; private set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000CA RID: 202 RVA: 0x0000470C File Offset: 0x0000290C
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00004714 File Offset: 0x00002914
		[JsonProperty("global_name")]
		public string DisplayName { get; private set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000CC RID: 204 RVA: 0x0000471D File Offset: 0x0000291D
		// (set) Token: 0x060000CD RID: 205 RVA: 0x00004725 File Offset: 0x00002925
		[JsonProperty("avatar")]
		public string Avatar { get; private set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000CE RID: 206 RVA: 0x0000472E File Offset: 0x0000292E
		// (set) Token: 0x060000CF RID: 207 RVA: 0x00004736 File Offset: 0x00002936
		[JsonProperty("flags", NullValueHandling = NullValueHandling.Ignore)]
		public User.Flag Flags { get; private set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x0000473F File Offset: 0x0000293F
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x00004747 File Offset: 0x00002947
		[JsonProperty("premium_type", NullValueHandling = NullValueHandling.Ignore)]
		public User.PremiumType Premium { get; private set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00004750 File Offset: 0x00002950
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x00004758 File Offset: 0x00002958
		public string CdnEndpoint { get; private set; }

		// Token: 0x060000D4 RID: 212 RVA: 0x00004761 File Offset: 0x00002961
		internal User()
		{
			this.CdnEndpoint = "cdn.discordapp.com";
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00004774 File Offset: 0x00002974
		internal void SetConfiguration(Configuration configuration)
		{
			this.CdnEndpoint = configuration.CdnHost;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00004782 File Offset: 0x00002982
		public string GetAvatarURL(User.AvatarFormat format)
		{
			return this.GetAvatarURL(format, User.AvatarSize.x128);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00004790 File Offset: 0x00002990
		public string GetAvatarURL(User.AvatarFormat format, User.AvatarSize size)
		{
			string text = string.Format("/avatars/{0}/{1}", this.ID, this.Avatar);
			if (string.IsNullOrEmpty(this.Avatar))
			{
				if (format != User.AvatarFormat.PNG)
				{
					throw new BadImageFormatException("The user has no avatar and the requested format " + format.ToString() + " is not supported. (Only supports PNG).");
				}
				int num = (int)((this.ID >> 22) % 6UL);
				if (this.Discriminator > 0)
				{
					num = this.Discriminator % 5;
				}
				text = string.Format("/embed/avatars/{0}", num);
			}
			return string.Format("https://{0}{1}{2}?size={3}", new object[]
			{
				this.CdnEndpoint,
				text,
				this.GetAvatarExtension(format),
				(int)size
			});
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000484C File Offset: 0x00002A4C
		public string GetAvatarExtension(User.AvatarFormat format)
		{
			return "." + format.ToString().ToLowerInvariant();
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000486C File Offset: 0x00002A6C
		public override string ToString()
		{
			if (!string.IsNullOrEmpty(this.DisplayName))
			{
				return this.DisplayName;
			}
			if (this.Discriminator != 0)
			{
				return this.Username + "#" + this.Discriminator.ToString("D4");
			}
			return this.Username;
		}

		// Token: 0x0200004E RID: 78
		public enum AvatarFormat
		{
			// Token: 0x040000EE RID: 238
			PNG,
			// Token: 0x040000EF RID: 239
			JPEG,
			// Token: 0x040000F0 RID: 240
			WebP,
			// Token: 0x040000F1 RID: 241
			GIF
		}

		// Token: 0x0200004F RID: 79
		public enum AvatarSize
		{
			// Token: 0x040000F3 RID: 243
			x16 = 16,
			// Token: 0x040000F4 RID: 244
			x32 = 32,
			// Token: 0x040000F5 RID: 245
			x64 = 64,
			// Token: 0x040000F6 RID: 246
			x128 = 128,
			// Token: 0x040000F7 RID: 247
			x256 = 256,
			// Token: 0x040000F8 RID: 248
			x512 = 512,
			// Token: 0x040000F9 RID: 249
			x1024 = 1024,
			// Token: 0x040000FA RID: 250
			x2048 = 2048
		}

		// Token: 0x02000050 RID: 80
		[Flags]
		public enum Flag
		{
			// Token: 0x040000FC RID: 252
			None = 0,
			// Token: 0x040000FD RID: 253
			Employee = 1,
			// Token: 0x040000FE RID: 254
			Partner = 2,
			// Token: 0x040000FF RID: 255
			HypeSquad = 4,
			// Token: 0x04000100 RID: 256
			BugHunter = 8,
			// Token: 0x04000101 RID: 257
			HouseBravery = 64,
			// Token: 0x04000102 RID: 258
			HouseBrilliance = 128,
			// Token: 0x04000103 RID: 259
			HouseBalance = 256,
			// Token: 0x04000104 RID: 260
			EarlySupporter = 512,
			// Token: 0x04000105 RID: 261
			TeamUser = 1024
		}

		// Token: 0x02000051 RID: 81
		public enum PremiumType
		{
			// Token: 0x04000107 RID: 263
			None,
			// Token: 0x04000108 RID: 264
			NitroClassic,
			// Token: 0x04000109 RID: 265
			Nitro
		}
	}
}
