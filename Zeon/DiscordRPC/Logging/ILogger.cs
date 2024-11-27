using System;

namespace DiscordRPC.Logging
{
	// Token: 0x02000030 RID: 48
	public interface ILogger
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600018E RID: 398
		// (set) Token: 0x0600018F RID: 399
		LogLevel Level { get; set; }

		// Token: 0x06000190 RID: 400
		void Trace(string message, params object[] args);

		// Token: 0x06000191 RID: 401
		void Info(string message, params object[] args);

		// Token: 0x06000192 RID: 402
		void Warning(string message, params object[] args);

		// Token: 0x06000193 RID: 403
		void Error(string message, params object[] args);
	}
}
