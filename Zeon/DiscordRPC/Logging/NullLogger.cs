using System;

namespace DiscordRPC.Logging
{
	// Token: 0x02000032 RID: 50
	public class NullLogger : ILogger
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00006C78 File Offset: 0x00004E78
		// (set) Token: 0x06000195 RID: 405 RVA: 0x00006C80 File Offset: 0x00004E80
		public LogLevel Level { get; set; }

		// Token: 0x06000196 RID: 406 RVA: 0x00006C89 File Offset: 0x00004E89
		public void Trace(string message, params object[] args)
		{
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00006C8B File Offset: 0x00004E8B
		public void Info(string message, params object[] args)
		{
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00006C8D File Offset: 0x00004E8D
		public void Warning(string message, params object[] args)
		{
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00006C8F File Offset: 0x00004E8F
		public void Error(string message, params object[] args)
		{
		}
	}
}
