using System;

namespace DiscordRPC.Logging
{
	// Token: 0x0200002E RID: 46
	public class ConsoleLogger : ILogger
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00006909 File Offset: 0x00004B09
		// (set) Token: 0x06000178 RID: 376 RVA: 0x00006911 File Offset: 0x00004B11
		public LogLevel Level { get; set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000179 RID: 377 RVA: 0x0000691A File Offset: 0x00004B1A
		// (set) Token: 0x0600017A RID: 378 RVA: 0x00006922 File Offset: 0x00004B22
		public bool Coloured { get; set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600017B RID: 379 RVA: 0x0000692B File Offset: 0x00004B2B
		// (set) Token: 0x0600017C RID: 380 RVA: 0x00006933 File Offset: 0x00004B33
		[Obsolete("Use Coloured")]
		public bool Colored
		{
			get
			{
				return this.Coloured;
			}
			set
			{
				this.Coloured = value;
			}
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000693C File Offset: 0x00004B3C
		public ConsoleLogger()
		{
			this.Level = LogLevel.Info;
			this.Coloured = false;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00006952 File Offset: 0x00004B52
		public ConsoleLogger(LogLevel level) : this()
		{
			this.Level = level;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00006961 File Offset: 0x00004B61
		public ConsoleLogger(LogLevel level, bool coloured)
		{
			this.Level = level;
			this.Coloured = coloured;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00006978 File Offset: 0x00004B78
		public void Trace(string message, params object[] args)
		{
			if (this.Level > LogLevel.Trace)
			{
				return;
			}
			if (this.Coloured)
			{
				Console.ForegroundColor = ConsoleColor.Gray;
			}
			string text = "TRACE: " + message;
			if (args.Length != 0)
			{
				Console.WriteLine(text, args);
				return;
			}
			Console.WriteLine(text);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x000069BC File Offset: 0x00004BBC
		public void Info(string message, params object[] args)
		{
			if (this.Level > LogLevel.Info)
			{
				return;
			}
			if (this.Coloured)
			{
				Console.ForegroundColor = ConsoleColor.White;
			}
			string text = "INFO: " + message;
			if (args.Length != 0)
			{
				Console.WriteLine(text, args);
				return;
			}
			Console.WriteLine(text);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00006A00 File Offset: 0x00004C00
		public void Warning(string message, params object[] args)
		{
			if (this.Level > LogLevel.Warning)
			{
				return;
			}
			if (this.Coloured)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
			}
			string text = "WARN: " + message;
			if (args.Length != 0)
			{
				Console.WriteLine(text, args);
				return;
			}
			Console.WriteLine(text);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00006A44 File Offset: 0x00004C44
		public void Error(string message, params object[] args)
		{
			if (this.Level > LogLevel.Error)
			{
				return;
			}
			if (this.Coloured)
			{
				Console.ForegroundColor = ConsoleColor.Red;
			}
			string text = "ERR : " + message;
			if (args.Length != 0)
			{
				Console.WriteLine(text, args);
				return;
			}
			Console.WriteLine(text);
		}
	}
}
