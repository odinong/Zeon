using System;

namespace DiscordRPC.Helper
{
	// Token: 0x02000038 RID: 56
	internal class BackoffDelay
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001CE RID: 462 RVA: 0x000079F0 File Offset: 0x00005BF0
		// (set) Token: 0x060001CF RID: 463 RVA: 0x000079F8 File Offset: 0x00005BF8
		public int Maximum { get; private set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x00007A01 File Offset: 0x00005C01
		// (set) Token: 0x060001D1 RID: 465 RVA: 0x00007A09 File Offset: 0x00005C09
		public int Minimum { get; private set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x00007A12 File Offset: 0x00005C12
		public int Current
		{
			get
			{
				return this._current;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x00007A1A File Offset: 0x00005C1A
		public int Fails
		{
			get
			{
				return this._fails;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x00007A22 File Offset: 0x00005C22
		// (set) Token: 0x060001D5 RID: 469 RVA: 0x00007A2A File Offset: 0x00005C2A
		public Random Random { get; set; }

		// Token: 0x060001D6 RID: 470 RVA: 0x00007A33 File Offset: 0x00005C33
		private BackoffDelay()
		{
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00007A3B File Offset: 0x00005C3B
		public BackoffDelay(int min, int max) : this(min, max, new Random())
		{
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00007A4A File Offset: 0x00005C4A
		public BackoffDelay(int min, int max, Random random)
		{
			this.Minimum = min;
			this.Maximum = max;
			this._current = min;
			this._fails = 0;
			this.Random = random;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00007A75 File Offset: 0x00005C75
		public void Reset()
		{
			this._fails = 0;
			this._current = this.Minimum;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00007A8C File Offset: 0x00005C8C
		public int NextDelay()
		{
			this._fails++;
			double num = (double)((float)(this.Maximum - this.Minimum) / 100f);
			this._current = (int)Math.Floor(num * (double)this._fails) + this.Minimum;
			return Math.Min(Math.Max(this._current, this.Minimum), this.Maximum);
		}

		// Token: 0x040000E4 RID: 228
		private int _current;

		// Token: 0x040000E5 RID: 229
		private int _fails;
	}
}
