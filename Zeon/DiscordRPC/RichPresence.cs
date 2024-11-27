using System;
using Newtonsoft.Json;

namespace DiscordRPC
{
	// Token: 0x0200000B RID: 11
	public sealed class RichPresence : BaseRichPresence
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x000042E7 File Offset: 0x000024E7
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x000042EF File Offset: 0x000024EF
		[JsonProperty("buttons", NullValueHandling = NullValueHandling.Ignore)]
		public Button[] Buttons { get; set; }

		// Token: 0x060000B3 RID: 179 RVA: 0x000042F8 File Offset: 0x000024F8
		public bool HasButtons()
		{
			return this.Buttons != null && this.Buttons.Length != 0;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000430E File Offset: 0x0000250E
		public RichPresence WithState(string state)
		{
			base.State = state;
			return this;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00004318 File Offset: 0x00002518
		public RichPresence WithDetails(string details)
		{
			base.Details = details;
			return this;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004322 File Offset: 0x00002522
		public RichPresence WithTimestamps(Timestamps timestamps)
		{
			base.Timestamps = timestamps;
			return this;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000432C File Offset: 0x0000252C
		public RichPresence WithAssets(Assets assets)
		{
			base.Assets = assets;
			return this;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00004336 File Offset: 0x00002536
		public RichPresence WithParty(Party party)
		{
			base.Party = party;
			return this;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00004340 File Offset: 0x00002540
		public RichPresence WithSecrets(Secrets secrets)
		{
			base.Secrets = secrets;
			return this;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0000434C File Offset: 0x0000254C
		public RichPresence Clone()
		{
			RichPresence richPresence = new RichPresence();
			richPresence.State = ((this._state != null) ? (this._state.Clone() as string) : null);
			richPresence.Details = ((this._details != null) ? (this._details.Clone() as string) : null);
			richPresence.Buttons = ((!this.HasButtons()) ? null : (this.Buttons.Clone() as Button[]));
			Secrets secrets2;
			if (base.HasSecrets())
			{
				Secrets secrets = new Secrets();
				secrets.JoinSecret = ((base.Secrets.JoinSecret != null) ? (base.Secrets.JoinSecret.Clone() as string) : null);
				secrets2 = secrets;
				secrets.SpectateSecret = ((base.Secrets.SpectateSecret != null) ? (base.Secrets.SpectateSecret.Clone() as string) : null);
			}
			else
			{
				secrets2 = null;
			}
			richPresence.Secrets = secrets2;
			Timestamps timestamps2;
			if (base.HasTimestamps())
			{
				Timestamps timestamps = new Timestamps();
				timestamps.Start = base.Timestamps.Start;
				timestamps2 = timestamps;
				timestamps.End = base.Timestamps.End;
			}
			else
			{
				timestamps2 = null;
			}
			richPresence.Timestamps = timestamps2;
			Assets assets2;
			if (base.HasAssets())
			{
				Assets assets = new Assets();
				assets.LargeImageKey = ((base.Assets.LargeImageKey != null) ? (base.Assets.LargeImageKey.Clone() as string) : null);
				assets.LargeImageText = ((base.Assets.LargeImageText != null) ? (base.Assets.LargeImageText.Clone() as string) : null);
				assets.SmallImageKey = ((base.Assets.SmallImageKey != null) ? (base.Assets.SmallImageKey.Clone() as string) : null);
				assets2 = assets;
				assets.SmallImageText = ((base.Assets.SmallImageText != null) ? (base.Assets.SmallImageText.Clone() as string) : null);
			}
			else
			{
				assets2 = null;
			}
			richPresence.Assets = assets2;
			Party party2;
			if (base.HasParty())
			{
				Party party = new Party();
				party.ID = base.Party.ID;
				party.Size = base.Party.Size;
				party.Max = base.Party.Max;
				party2 = party;
				party.Privacy = base.Party.Privacy;
			}
			else
			{
				party2 = null;
			}
			richPresence.Party = party2;
			return richPresence;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004584 File Offset: 0x00002784
		internal RichPresence Merge(BaseRichPresence presence)
		{
			this._state = presence.State;
			this._details = presence.Details;
			base.Party = presence.Party;
			base.Timestamps = presence.Timestamps;
			base.Secrets = presence.Secrets;
			if (presence.HasAssets())
			{
				if (!base.HasAssets())
				{
					base.Assets = presence.Assets;
				}
				else
				{
					base.Assets.Merge(presence.Assets);
				}
			}
			else
			{
				base.Assets = null;
			}
			return this;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00004608 File Offset: 0x00002808
		internal override bool Matches(RichPresence other)
		{
			if (!base.Matches(other))
			{
				return false;
			}
			if (this.Buttons == null ^ other.Buttons == null)
			{
				return false;
			}
			if (this.Buttons != null)
			{
				if (this.Buttons.Length != other.Buttons.Length)
				{
					return false;
				}
				for (int i = 0; i < this.Buttons.Length; i++)
				{
					Button button = this.Buttons[i];
					Button button2 = other.Buttons[i];
					if (button.Label != button2.Label || button.Url != button2.Url)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000046A1 File Offset: 0x000028A1
		public static implicit operator bool(RichPresence presesnce)
		{
			return presesnce != null;
		}
	}
}
