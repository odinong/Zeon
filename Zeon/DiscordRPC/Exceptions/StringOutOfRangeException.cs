using System;

namespace DiscordRPC.Exceptions
{
	// Token: 0x0200003D RID: 61
	public class StringOutOfRangeException : Exception
	{
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x00007C03 File Offset: 0x00005E03
		// (set) Token: 0x060001E4 RID: 484 RVA: 0x00007C0B File Offset: 0x00005E0B
		public int MaximumLength { get; private set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x00007C14 File Offset: 0x00005E14
		// (set) Token: 0x060001E6 RID: 486 RVA: 0x00007C1C File Offset: 0x00005E1C
		public int MinimumLength { get; private set; }

		// Token: 0x060001E7 RID: 487 RVA: 0x00007C25 File Offset: 0x00005E25
		internal StringOutOfRangeException(string message, int min, int max) : base(message)
		{
			this.MinimumLength = min;
			this.MaximumLength = max;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00007C3C File Offset: 0x00005E3C
		internal StringOutOfRangeException(int minumum, int max) : this(string.Format("Length of string is out of range. Expected a value between {0} and {1}", minumum, max), minumum, max)
		{
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00007C5C File Offset: 0x00005E5C
		internal StringOutOfRangeException(int max) : this(string.Format("Length of string is out of range. Expected a value with a maximum length of {0}", max), 0, max)
		{
		}
	}
}
