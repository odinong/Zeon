using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using DiscordRPC.Logging;

namespace DiscordRPC.IO
{
	// Token: 0x02000035 RID: 53
	public sealed class ManagedNamedPipeClient : INamedPipeClient, IDisposable
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00006CC3 File Offset: 0x00004EC3
		// (set) Token: 0x060001A9 RID: 425 RVA: 0x00006CCB File Offset: 0x00004ECB
		public ILogger Logger { get; set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00006CD4 File Offset: 0x00004ED4
		public bool IsConnected
		{
			get
			{
				if (this._isClosed)
				{
					return false;
				}
				object obj = this.l_stream;
				bool result;
				lock (obj)
				{
					result = (this._stream != null && this._stream.IsConnected);
				}
				return result;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001AB RID: 427 RVA: 0x00006D34 File Offset: 0x00004F34
		public int ConnectedPipe
		{
			get
			{
				return this._connectedPipe;
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00006D3C File Offset: 0x00004F3C
		public ManagedNamedPipeClient()
		{
			this._buffer = new byte[PipeFrame.MAX_SIZE];
			this.Logger = new NullLogger();
			this._stream = null;
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00006DAC File Offset: 0x00004FAC
		public bool Connect(int pipe)
		{
			this.Logger.Trace("ManagedNamedPipeClient.Connection({0})", new object[]
			{
				pipe
			});
			if (this._isDisposed)
			{
				throw new ObjectDisposedException("NamedPipe");
			}
			if (pipe > 9)
			{
				throw new ArgumentOutOfRangeException("pipe", "Argument cannot be greater than 9");
			}
			if (pipe < 0)
			{
				for (int i = 0; i < 10; i++)
				{
					if (this.AttemptConnection(i, false) || this.AttemptConnection(i, true))
					{
						this.BeginReadStream();
						return true;
					}
				}
			}
			else if (this.AttemptConnection(pipe, false) || this.AttemptConnection(pipe, true))
			{
				this.BeginReadStream();
				return true;
			}
			return false;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00006E50 File Offset: 0x00005050
		private bool AttemptConnection(int pipe, bool isSandbox = false)
		{
			if (this._isDisposed)
			{
				throw new ObjectDisposedException("_stream");
			}
			string text = isSandbox ? ManagedNamedPipeClient.GetPipeSandbox() : "";
			if (isSandbox && text == null)
			{
				this.Logger.Trace("Skipping sandbox connection.", Array.Empty<object>());
				return false;
			}
			this.Logger.Trace("Connection Attempt {0} ({1})", new object[]
			{
				pipe,
				text
			});
			string pipeName = ManagedNamedPipeClient.GetPipeName(pipe, text);
			try
			{
				object obj = this.l_stream;
				lock (obj)
				{
					this.Logger.Info("Attempting to connect to '{0}'", new object[]
					{
						pipeName
					});
					this._stream = new NamedPipeClientStream(".", pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
					this._stream.Connect(0);
					this.Logger.Trace("Waiting for connection...", Array.Empty<object>());
					do
					{
						Thread.Sleep(10);
					}
					while (!this._stream.IsConnected);
				}
				this.Logger.Info("Connected to '{0}'", new object[]
				{
					pipeName
				});
				this._connectedPipe = pipe;
				this._isClosed = false;
			}
			catch (Exception ex)
			{
				this.Logger.Error("Failed connection to {0}. {1}", new object[]
				{
					pipeName,
					ex.Message
				});
				this.Close();
			}
			this.Logger.Trace("Done. Result: {0}", new object[]
			{
				this._isClosed
			});
			return !this._isClosed;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00006FF4 File Offset: 0x000051F4
		private void BeginReadStream()
		{
			if (this._isClosed)
			{
				return;
			}
			try
			{
				object obj = this.l_stream;
				lock (obj)
				{
					if (this._stream != null && this._stream.IsConnected)
					{
						this.Logger.Trace("Begining Read of {0} bytes", new object[]
						{
							this._buffer.Length
						});
						this._stream.BeginRead(this._buffer, 0, this._buffer.Length, new AsyncCallback(this.EndReadStream), this._stream.IsConnected);
					}
				}
			}
			catch (ObjectDisposedException)
			{
				this.Logger.Warning("Attempted to start reading from a disposed pipe", Array.Empty<object>());
			}
			catch (InvalidOperationException)
			{
				this.Logger.Warning("Attempted to start reading from a closed pipe", Array.Empty<object>());
			}
			catch (Exception ex)
			{
				this.Logger.Error("An exception occured while starting to read a stream: {0}", new object[]
				{
					ex.Message
				});
				this.Logger.Error(ex.StackTrace, Array.Empty<object>());
			}
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00007140 File Offset: 0x00005340
		private void EndReadStream(IAsyncResult callback)
		{
			this.Logger.Trace("Ending Read", Array.Empty<object>());
			int num = 0;
			try
			{
				object framequeuelock = this.l_stream;
				lock (framequeuelock)
				{
					if (this._stream == null || !this._stream.IsConnected)
					{
						return;
					}
					num = this._stream.EndRead(callback);
				}
			}
			catch (IOException)
			{
				this.Logger.Warning("Attempted to end reading from a closed pipe", Array.Empty<object>());
				return;
			}
			catch (NullReferenceException)
			{
				this.Logger.Warning("Attempted to read from a null pipe", Array.Empty<object>());
				return;
			}
			catch (ObjectDisposedException)
			{
				this.Logger.Warning("Attemped to end reading from a disposed pipe", Array.Empty<object>());
				return;
			}
			catch (Exception ex)
			{
				this.Logger.Error("An exception occured while ending a read of a stream: {0}", new object[]
				{
					ex.Message
				});
				this.Logger.Error(ex.StackTrace, Array.Empty<object>());
				return;
			}
			this.Logger.Trace("Read {0} bytes", new object[]
			{
				num
			});
			if (num > 0)
			{
				using (MemoryStream memoryStream = new MemoryStream(this._buffer, 0, num))
				{
					try
					{
						PipeFrame item = default(PipeFrame);
						if (item.ReadStream(memoryStream))
						{
							this.Logger.Trace("Read a frame: {0}", new object[]
							{
								item.Opcode
							});
							object framequeuelock = this._framequeuelock;
							lock (framequeuelock)
							{
								this._framequeue.Enqueue(item);
								goto IL_19E;
							}
						}
						this.Logger.Error("Pipe failed to read from the data received by the stream.", Array.Empty<object>());
						this.Close();
						IL_19E:
						goto IL_218;
					}
					catch (Exception ex2)
					{
						this.Logger.Error("A exception has occured while trying to parse the pipe data: {0}", new object[]
						{
							ex2.Message
						});
						this.Close();
						goto IL_218;
					}
				}
			}
			if (ManagedNamedPipeClient.IsUnix())
			{
				this.Logger.Error("Empty frame was read on {0}, aborting.", new object[]
				{
					Environment.OSVersion
				});
				this.Close();
			}
			else
			{
				this.Logger.Warning("Empty frame was read. Please send report to Lachee.", Array.Empty<object>());
			}
			IL_218:
			if (!this._isClosed && this.IsConnected)
			{
				this.Logger.Trace("Starting another read", Array.Empty<object>());
				this.BeginReadStream();
			}
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x000073F8 File Offset: 0x000055F8
		public bool ReadFrame(out PipeFrame frame)
		{
			if (this._isDisposed)
			{
				throw new ObjectDisposedException("_stream");
			}
			object framequeuelock = this._framequeuelock;
			bool result;
			lock (framequeuelock)
			{
				if (this._framequeue.Count == 0)
				{
					frame = default(PipeFrame);
					result = false;
				}
				else
				{
					frame = this._framequeue.Dequeue();
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00007474 File Offset: 0x00005674
		public bool WriteFrame(PipeFrame frame)
		{
			if (this._isDisposed)
			{
				throw new ObjectDisposedException("_stream");
			}
			if (this._isClosed || !this.IsConnected)
			{
				this.Logger.Error("Failed to write frame because the stream is closed", Array.Empty<object>());
				return false;
			}
			try
			{
				frame.WriteStream(this._stream);
				return true;
			}
			catch (IOException ex)
			{
				this.Logger.Error("Failed to write frame because of a IO Exception: {0}", new object[]
				{
					ex.Message
				});
			}
			catch (ObjectDisposedException)
			{
				this.Logger.Warning("Failed to write frame as the stream was already disposed", Array.Empty<object>());
			}
			catch (InvalidOperationException)
			{
				this.Logger.Warning("Failed to write frame because of a invalid operation", Array.Empty<object>());
			}
			return false;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00007550 File Offset: 0x00005750
		public void Close()
		{
			if (this._isClosed)
			{
				this.Logger.Warning("Tried to close a already closed pipe.", Array.Empty<object>());
				return;
			}
			try
			{
				object obj = this.l_stream;
				lock (obj)
				{
					if (this._stream != null)
					{
						try
						{
							this._stream.Flush();
							this._stream.Dispose();
						}
						catch (Exception)
						{
						}
						this._stream = null;
						this._isClosed = true;
					}
					else
					{
						this.Logger.Warning("Stream was closed, but no stream was available to begin with!", Array.Empty<object>());
					}
				}
			}
			catch (ObjectDisposedException)
			{
				this.Logger.Warning("Tried to dispose already disposed stream", Array.Empty<object>());
			}
			finally
			{
				this._isClosed = true;
				this._connectedPipe = -1;
			}
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00007644 File Offset: 0x00005844
		public void Dispose()
		{
			if (this._isDisposed)
			{
				return;
			}
			if (!this._isClosed)
			{
				this.Close();
			}
			object obj = this.l_stream;
			lock (obj)
			{
				if (this._stream != null)
				{
					this._stream.Dispose();
					this._stream = null;
				}
			}
			this._isDisposed = true;
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x000076BC File Offset: 0x000058BC
		public static string GetPipeName(int pipe, string sandbox)
		{
			if (!ManagedNamedPipeClient.IsUnix())
			{
				return sandbox + string.Format("discord-ipc-{0}", pipe);
			}
			return Path.Combine(ManagedNamedPipeClient.GetTemporaryDirectory(), sandbox + string.Format("discord-ipc-{0}", pipe));
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x000076FC File Offset: 0x000058FC
		public static string GetPipeName(int pipe)
		{
			return ManagedNamedPipeClient.GetPipeName(pipe, "");
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00007709 File Offset: 0x00005909
		public static string GetPipeSandbox()
		{
			if (Environment.OSVersion.Platform != PlatformID.Unix)
			{
				return null;
			}
			return "snap.discord/";
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00007720 File Offset: 0x00005920
		private static string GetTemporaryDirectory()
		{
			return ((((null ?? Environment.GetEnvironmentVariable("XDG_RUNTIME_DIR")) ?? Environment.GetEnvironmentVariable("TMPDIR")) ?? Environment.GetEnvironmentVariable("TMP")) ?? Environment.GetEnvironmentVariable("TEMP")) ?? "/tmp";
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00007770 File Offset: 0x00005970
		public static bool IsUnix()
		{
			PlatformID platform = Environment.OSVersion.Platform;
			return platform == PlatformID.Unix || platform == PlatformID.MacOSX;
		}

		// Token: 0x040000CF RID: 207
		private const string PIPE_NAME = "discord-ipc-{0}";

		// Token: 0x040000D1 RID: 209
		private int _connectedPipe;

		// Token: 0x040000D2 RID: 210
		private NamedPipeClientStream _stream;

		// Token: 0x040000D3 RID: 211
		private byte[] _buffer = new byte[PipeFrame.MAX_SIZE];

		// Token: 0x040000D4 RID: 212
		private Queue<PipeFrame> _framequeue = new Queue<PipeFrame>();

		// Token: 0x040000D5 RID: 213
		private object _framequeuelock = new object();

		// Token: 0x040000D6 RID: 214
		private volatile bool _isDisposed;

		// Token: 0x040000D7 RID: 215
		private volatile bool _isClosed = true;

		// Token: 0x040000D8 RID: 216
		private object l_stream = new object();
	}
}
