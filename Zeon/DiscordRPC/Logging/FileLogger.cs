using System;
using System.IO;

namespace DiscordRPC.Logging
{
	// Token: 0x0200002F RID: 47
	public class FileLogger : ILogger
	{
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00006A88 File Offset: 0x00004C88
		// (set) Token: 0x06000185 RID: 389 RVA: 0x00006A90 File Offset: 0x00004C90
		public LogLevel Level { get; set; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000186 RID: 390 RVA: 0x00006A99 File Offset: 0x00004C99
		// (set) Token: 0x06000187 RID: 391 RVA: 0x00006AA1 File Offset: 0x00004CA1
		public string File { get; set; }

		// Token: 0x06000188 RID: 392 RVA: 0x00006AAA File Offset: 0x00004CAA
		public FileLogger(string path) : this(path, LogLevel.Info)
		{
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00006AB4 File Offset: 0x00004CB4
		public FileLogger(string path, LogLevel level)
		{
			this.Level = level;
			this.File = path;
			this.filelock = new object();
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00006AD8 File Offset: 0x00004CD8
		public void Trace(string message, params object[] args)
		{
			if (this.Level > LogLevel.Trace)
			{
				return;
			}
			object obj = this.filelock;
			lock (obj)
			{
				System.IO.File.AppendAllText(this.File, "\r\nTRCE: " + ((args.Length != 0) ? string.Format(message, args) : message));
			}
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00006B40 File Offset: 0x00004D40
		public void Info(string message, params object[] args)
		{
			if (this.Level > LogLevel.Info)
			{
				return;
			}
			object obj = this.filelock;
			lock (obj)
			{
				System.IO.File.AppendAllText(this.File, "\r\nINFO: " + ((args.Length != 0) ? string.Format(message, args) : message));
			}
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00006BA8 File Offset: 0x00004DA8
		public void Warning(string message, params object[] args)
		{
			if (this.Level > LogLevel.Warning)
			{
				return;
			}
			object obj = this.filelock;
			lock (obj)
			{
				System.IO.File.AppendAllText(this.File, "\r\nWARN: " + ((args.Length != 0) ? string.Format(message, args) : message));
			}
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00006C10 File Offset: 0x00004E10
		public void Error(string message, params object[] args)
		{
			if (this.Level > LogLevel.Error)
			{
				return;
			}
			object obj = this.filelock;
			lock (obj)
			{
				System.IO.File.AppendAllText(this.File, "\r\nERR : " + ((args.Length != 0) ? string.Format(message, args) : message));
			}
		}

		// Token: 0x040000C5 RID: 197
		private object filelock;
	}
}
