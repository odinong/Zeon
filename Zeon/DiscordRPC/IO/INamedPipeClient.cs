using System;
using DiscordRPC.Logging;

namespace DiscordRPC.IO
{
	// Token: 0x02000034 RID: 52
	public interface INamedPipeClient : IDisposable
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001A0 RID: 416
		// (set) Token: 0x060001A1 RID: 417
		ILogger Logger { get; set; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001A2 RID: 418
		bool IsConnected { get; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001A3 RID: 419
		int ConnectedPipe { get; }

		// Token: 0x060001A4 RID: 420
		bool Connect(int pipe);

		// Token: 0x060001A5 RID: 421
		bool ReadFrame(out PipeFrame frame);

		// Token: 0x060001A6 RID: 422
		bool WriteFrame(PipeFrame frame);

		// Token: 0x060001A7 RID: 423
		void Close();
	}
}
