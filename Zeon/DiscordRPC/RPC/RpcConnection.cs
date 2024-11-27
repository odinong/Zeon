using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using DiscordRPC.Converters;
using DiscordRPC.Events;
using DiscordRPC.Helper;
using DiscordRPC.IO;
using DiscordRPC.Logging;
using DiscordRPC.Message;
using DiscordRPC.RPC.Commands;
using DiscordRPC.RPC.Payload;
using Newtonsoft.Json;

namespace DiscordRPC.RPC
{
	// Token: 0x0200000E RID: 14
	internal class RpcConnection : IDisposable
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000DA RID: 218 RVA: 0x000048BF File Offset: 0x00002ABF
		// (set) Token: 0x060000DB RID: 219 RVA: 0x000048C7 File Offset: 0x00002AC7
		public ILogger Logger
		{
			get
			{
				return this._logger;
			}
			set
			{
				this._logger = value;
				if (this.namedPipe != null)
				{
					this.namedPipe.Logger = value;
				}
			}
		}

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060000DC RID: 220 RVA: 0x000048E4 File Offset: 0x00002AE4
		// (remove) Token: 0x060000DD RID: 221 RVA: 0x0000491C File Offset: 0x00002B1C
		public event OnRpcMessageEvent OnRpcMessage;

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00004954 File Offset: 0x00002B54
		public RpcState State
		{
			get
			{
				object obj = this.l_states;
				RpcState state;
				lock (obj)
				{
					state = this._state;
				}
				return state;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000DF RID: 223 RVA: 0x00004998 File Offset: 0x00002B98
		public Configuration Configuration
		{
			get
			{
				Configuration result = null;
				object obj = this.l_config;
				lock (obj)
				{
					result = this._configuration;
				}
				return result;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x000049DC File Offset: 0x00002BDC
		public bool IsRunning
		{
			get
			{
				return this.thread != null;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x000049E7 File Offset: 0x00002BE7
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x000049EF File Offset: 0x00002BEF
		public bool ShutdownOnly { get; set; }

		// Token: 0x060000E3 RID: 227 RVA: 0x000049F8 File Offset: 0x00002BF8
		public RpcConnection(string applicationID, int processID, int targetPipe, INamedPipeClient client, uint maxRxQueueSize = 128U, uint maxRtQueueSize = 512U)
		{
			this.applicationID = applicationID;
			this.processID = processID;
			this.targetPipe = targetPipe;
			this.namedPipe = client;
			this.ShutdownOnly = true;
			this.Logger = new ConsoleLogger();
			this.delay = new BackoffDelay(500, 60000);
			this._maxRtQueueSize = maxRtQueueSize;
			this._rtqueue = new Queue<ICommand>((int)(this._maxRtQueueSize + 1U));
			this._maxRxQueueSize = maxRxQueueSize;
			this._rxqueue = new Queue<IMessage>((int)(this._maxRxQueueSize + 1U));
			this.nonce = 0L;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00004AC5 File Offset: 0x00002CC5
		private long GetNextNonce()
		{
			this.nonce += 1L;
			return this.nonce;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00004ADC File Offset: 0x00002CDC
		internal void EnqueueCommand(ICommand command)
		{
			this.Logger.Trace("Enqueue Command: {0}", new object[]
			{
				command.GetType().FullName
			});
			if (this.aborting || this.shutdown)
			{
				return;
			}
			object obj = this.l_rtqueue;
			lock (obj)
			{
				if ((long)this._rtqueue.Count == (long)((ulong)this._maxRtQueueSize))
				{
					this.Logger.Error("Too many enqueued commands, dropping oldest one. Maybe you are pushing new presences to fast?", Array.Empty<object>());
					this._rtqueue.Dequeue();
				}
				this._rtqueue.Enqueue(command);
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00004B94 File Offset: 0x00002D94
		private void EnqueueMessage(IMessage message)
		{
			try
			{
				if (this.OnRpcMessage != null)
				{
					this.OnRpcMessage(this, message);
				}
			}
			catch (Exception ex)
			{
				this.Logger.Error("Unhandled Exception while processing event: {0}", new object[]
				{
					ex.GetType().FullName
				});
				this.Logger.Error(ex.Message, Array.Empty<object>());
				this.Logger.Error(ex.StackTrace, Array.Empty<object>());
			}
			if (this._maxRxQueueSize <= 0U)
			{
				this.Logger.Trace("Enqueued Message, but queue size is 0.", Array.Empty<object>());
				return;
			}
			this.Logger.Trace("Enqueue Message: {0}", new object[]
			{
				message.Type
			});
			object obj = this.l_rxqueue;
			lock (obj)
			{
				if ((long)this._rxqueue.Count == (long)((ulong)this._maxRxQueueSize))
				{
					this.Logger.Warning("Too many enqueued messages, dropping oldest one.", Array.Empty<object>());
					this._rxqueue.Dequeue();
				}
				this._rxqueue.Enqueue(message);
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00004CCC File Offset: 0x00002ECC
		internal IMessage DequeueMessage()
		{
			object obj = this.l_rxqueue;
			IMessage result;
			lock (obj)
			{
				if (this._rxqueue.Count == 0)
				{
					result = null;
				}
				else
				{
					result = this._rxqueue.Dequeue();
				}
			}
			return result;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00004D24 File Offset: 0x00002F24
		internal IMessage[] DequeueMessages()
		{
			object obj = this.l_rxqueue;
			IMessage[] result;
			lock (obj)
			{
				IMessage[] array = this._rxqueue.ToArray();
				this._rxqueue.Clear();
				result = array;
			}
			return result;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00004D78 File Offset: 0x00002F78
		private void MainLoop()
		{
			this.Logger.Info("RPC Connection Started", Array.Empty<object>());
			if (this.Logger.Level <= LogLevel.Trace)
			{
				this.Logger.Trace("============================", Array.Empty<object>());
				this.Logger.Trace("Assembly:             " + Assembly.GetAssembly(typeof(RichPresence)).FullName, Array.Empty<object>());
				this.Logger.Trace("Pipe:                 " + this.namedPipe.GetType().FullName, Array.Empty<object>());
				this.Logger.Trace("Platform:             " + Environment.OSVersion.ToString(), Array.Empty<object>());
				this.Logger.Trace("applicationID:        " + this.applicationID, Array.Empty<object>());
				this.Logger.Trace("targetPipe:           " + this.targetPipe.ToString(), Array.Empty<object>());
				this.Logger.Trace("POLL_RATE:            " + RpcConnection.POLL_RATE.ToString(), Array.Empty<object>());
				this.Logger.Trace("_maxRtQueueSize:      " + this._maxRtQueueSize.ToString(), Array.Empty<object>());
				this.Logger.Trace("_maxRxQueueSize:      " + this._maxRxQueueSize.ToString(), Array.Empty<object>());
				this.Logger.Trace("============================", Array.Empty<object>());
			}
			while (!this.aborting && !this.shutdown)
			{
				try
				{
					if (this.namedPipe == null)
					{
						this.Logger.Error("Something bad has happened with our pipe client!", Array.Empty<object>());
						this.aborting = true;
						return;
					}
					this.Logger.Trace("Connecting to the pipe through the {0}", new object[]
					{
						this.namedPipe.GetType().FullName
					});
					if (this.namedPipe.Connect(this.targetPipe))
					{
						this.Logger.Trace("Connected to the pipe. Attempting to establish handshake...", Array.Empty<object>());
						this.EnqueueMessage(new ConnectionEstablishedMessage
						{
							ConnectedPipe = this.namedPipe.ConnectedPipe
						});
						this.EstablishHandshake();
						this.Logger.Trace("Connection Established. Starting reading loop...", Array.Empty<object>());
						bool flag = true;
						while (flag && !this.aborting && !this.shutdown && this.namedPipe.IsConnected)
						{
							PipeFrame frame;
							if (this.namedPipe.ReadFrame(out frame))
							{
								this.Logger.Trace("Read Payload: {0}", new object[]
								{
									frame.Opcode
								});
								switch (frame.Opcode)
								{
								case Opcode.Frame:
								{
									if (this.shutdown)
									{
										this.Logger.Warning("Skipping frame because we are shutting down.", Array.Empty<object>());
										goto IL_46B;
									}
									if (frame.Data == null)
									{
										this.Logger.Error("We received no data from the frame so we cannot get the event payload!", Array.Empty<object>());
										goto IL_46B;
									}
									EventPayload eventPayload = null;
									try
									{
										eventPayload = frame.GetObject<EventPayload>();
									}
									catch (Exception ex)
									{
										this.Logger.Error("Failed to parse event! {0}", new object[]
										{
											ex.Message
										});
										this.Logger.Error("Data: {0}", new object[]
										{
											frame.Message
										});
									}
									try
									{
										if (eventPayload != null)
										{
											this.ProcessFrame(eventPayload);
										}
										goto IL_46B;
									}
									catch (Exception ex2)
									{
										this.Logger.Error("Failed to process event! {0}", new object[]
										{
											ex2.Message
										});
										this.Logger.Error("Data: {0}", new object[]
										{
											frame.Message
										});
										goto IL_46B;
									}
									break;
								}
								case Opcode.Close:
								{
									ClosePayload @object = frame.GetObject<ClosePayload>();
									this.Logger.Warning("We have been told to terminate by discord: ({0}) {1}", new object[]
									{
										@object.Code,
										@object.Reason
									});
									this.EnqueueMessage(new CloseMessage
									{
										Code = @object.Code,
										Reason = @object.Reason
									});
									flag = false;
									goto IL_46B;
								}
								case Opcode.Ping:
									this.Logger.Trace("PING", Array.Empty<object>());
									frame.Opcode = Opcode.Pong;
									this.namedPipe.WriteFrame(frame);
									goto IL_46B;
								case Opcode.Pong:
									this.Logger.Trace("PONG", Array.Empty<object>());
									goto IL_46B;
								}
								this.Logger.Error("Invalid opcode: {0}", new object[]
								{
									frame.Opcode
								});
								flag = false;
							}
							IL_46B:
							if (!this.aborting && this.namedPipe.IsConnected)
							{
								this.ProcessCommandQueue();
								this.queueUpdatedEvent.WaitOne(RpcConnection.POLL_RATE);
							}
						}
						this.Logger.Trace("Left main read loop for some reason. Aborting: {0}, Shutting Down: {1}", new object[]
						{
							this.aborting,
							this.shutdown
						});
					}
					else
					{
						this.Logger.Error("Failed to connect for some reason.", Array.Empty<object>());
						this.EnqueueMessage(new ConnectionFailedMessage
						{
							FailedPipe = this.targetPipe
						});
					}
					if (!this.aborting && !this.shutdown)
					{
						long num = (long)this.delay.NextDelay();
						this.Logger.Trace("Waiting {0}ms before attempting to connect again", new object[]
						{
							num
						});
						Thread.Sleep(this.delay.NextDelay());
					}
				}
				catch (Exception ex3)
				{
					this.Logger.Error("Unhandled Exception: {0}", new object[]
					{
						ex3.GetType().FullName
					});
					this.Logger.Error(ex3.Message, Array.Empty<object>());
					this.Logger.Error(ex3.StackTrace, Array.Empty<object>());
				}
				finally
				{
					if (this.namedPipe.IsConnected)
					{
						this.Logger.Trace("Closing the named pipe.", Array.Empty<object>());
						this.namedPipe.Close();
					}
					this.SetConnectionState(RpcState.Disconnected);
				}
			}
			this.Logger.Trace("Left Main Loop", Array.Empty<object>());
			if (this.namedPipe != null)
			{
				this.namedPipe.Dispose();
			}
			this.Logger.Info("Thread Terminated, no longer performing RPC connection.", Array.Empty<object>());
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00005444 File Offset: 0x00003644
		private void ProcessFrame(EventPayload response)
		{
			this.Logger.Info("Handling Response. Cmd: {0}, Event: {1}", new object[]
			{
				response.Command,
				response.Event
			});
			if (response.Event != null && response.Event.Value == ServerEvent.Error)
			{
				this.Logger.Error("Error received from the RPC", Array.Empty<object>());
				ErrorMessage @object = response.GetObject<ErrorMessage>();
				this.Logger.Error("Server responded with an error message: ({0}) {1}", new object[]
				{
					@object.Code.ToString(),
					@object.Message
				});
				this.EnqueueMessage(@object);
				return;
			}
			if (this.State == RpcState.Connecting && response.Command == Command.Dispatch && response.Event != null && response.Event.Value == ServerEvent.Ready)
			{
				this.Logger.Info("Connection established with the RPC", Array.Empty<object>());
				this.SetConnectionState(RpcState.Connected);
				this.delay.Reset();
				ReadyMessage object2 = response.GetObject<ReadyMessage>();
				object obj = this.l_config;
				lock (obj)
				{
					this._configuration = object2.Configuration;
					object2.User.SetConfiguration(this._configuration);
				}
				this.EnqueueMessage(object2);
				return;
			}
			if (this.State != RpcState.Connected)
			{
				this.Logger.Trace("Received a frame while we are disconnected. Ignoring. Cmd: {0}, Event: {1}", new object[]
				{
					response.Command,
					response.Event
				});
				return;
			}
			switch (response.Command)
			{
			case Command.Dispatch:
				this.ProcessDispatch(response);
				return;
			case Command.SetActivity:
			{
				if (response.Data == null)
				{
					this.EnqueueMessage(new PresenceMessage());
					return;
				}
				RichPresenceResponse object3 = response.GetObject<RichPresenceResponse>();
				this.EnqueueMessage(new PresenceMessage(object3));
				return;
			}
			case Command.Subscribe:
			case Command.Unsubscribe:
			{
				new JsonSerializer().Converters.Add(new EnumSnakeCaseConverter());
				ServerEvent value = response.GetObject<EventPayload>().Event.Value;
				if (response.Command == Command.Subscribe)
				{
					this.EnqueueMessage(new SubscribeMessage(value));
					return;
				}
				this.EnqueueMessage(new UnsubscribeMessage(value));
				return;
			}
			case Command.SendActivityJoinInvite:
				this.Logger.Trace("Got invite response ack.", Array.Empty<object>());
				return;
			case Command.CloseActivityJoinRequest:
				this.Logger.Trace("Got invite response reject ack.", Array.Empty<object>());
				return;
			default:
				this.Logger.Error("Unkown frame was received! {0}", new object[]
				{
					response.Command
				});
				return;
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000056F8 File Offset: 0x000038F8
		private void ProcessDispatch(EventPayload response)
		{
			if (response.Command != Command.Dispatch)
			{
				return;
			}
			if (response.Event == null)
			{
				return;
			}
			switch (response.Event.Value)
			{
			case ServerEvent.ActivityJoin:
			{
				JoinMessage @object = response.GetObject<JoinMessage>();
				this.EnqueueMessage(@object);
				return;
			}
			case ServerEvent.ActivitySpectate:
			{
				SpectateMessage object2 = response.GetObject<SpectateMessage>();
				this.EnqueueMessage(object2);
				return;
			}
			case ServerEvent.ActivityJoinRequest:
			{
				JoinRequestMessage object3 = response.GetObject<JoinRequestMessage>();
				this.EnqueueMessage(object3);
				return;
			}
			default:
				this.Logger.Warning("Ignoring {0}", new object[]
				{
					response.Event.Value
				});
				return;
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000057A0 File Offset: 0x000039A0
		private void ProcessCommandQueue()
		{
			if (this.State != RpcState.Connected)
			{
				return;
			}
			if (this.aborting)
			{
				this.Logger.Warning("We have been told to write a queue but we have also been aborted.", Array.Empty<object>());
			}
			bool flag = true;
			ICommand command = null;
			while (flag && this.namedPipe.IsConnected)
			{
				object obj = this.l_rtqueue;
				lock (obj)
				{
					flag = (this._rtqueue.Count > 0);
					if (!flag)
					{
						break;
					}
					command = this._rtqueue.Peek();
				}
				if (this.shutdown || (!this.aborting && RpcConnection.LOCK_STEP))
				{
					flag = false;
				}
				IPayload payload = command.PreparePayload(this.GetNextNonce());
				this.Logger.Trace("Attempting to send payload: {0}", new object[]
				{
					payload.Command
				});
				PipeFrame frame = default(PipeFrame);
				if (command is CloseCommand)
				{
					this.SendHandwave();
					this.Logger.Trace("Handwave sent, ending queue processing.", Array.Empty<object>());
					obj = this.l_rtqueue;
					lock (obj)
					{
						this._rtqueue.Dequeue();
					}
					return;
				}
				if (this.aborting)
				{
					this.Logger.Warning("- skipping frame because of abort.", Array.Empty<object>());
					obj = this.l_rtqueue;
					lock (obj)
					{
						this._rtqueue.Dequeue();
						continue;
					}
				}
				frame.SetObject(Opcode.Frame, payload);
				this.Logger.Trace("Sending payload: {0}", new object[]
				{
					payload.Command
				});
				if (this.namedPipe.WriteFrame(frame))
				{
					this.Logger.Trace("Sent Successfully.", Array.Empty<object>());
					obj = this.l_rtqueue;
					lock (obj)
					{
						this._rtqueue.Dequeue();
						continue;
					}
				}
				this.Logger.Warning("Something went wrong during writing!", Array.Empty<object>());
				return;
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000059F8 File Offset: 0x00003BF8
		private void EstablishHandshake()
		{
			this.Logger.Trace("Attempting to establish a handshake...", Array.Empty<object>());
			if (this.State != RpcState.Disconnected)
			{
				this.Logger.Error("State must be disconnected in order to start a handshake!", Array.Empty<object>());
				return;
			}
			this.Logger.Trace("Sending Handshake...", Array.Empty<object>());
			if (!this.namedPipe.WriteFrame(new PipeFrame(Opcode.Handshake, new Handshake
			{
				Version = RpcConnection.VERSION,
				ClientID = this.applicationID
			})))
			{
				this.Logger.Error("Failed to write a handshake.", Array.Empty<object>());
				return;
			}
			this.SetConnectionState(RpcState.Connecting);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00005A9C File Offset: 0x00003C9C
		private void SendHandwave()
		{
			this.Logger.Info("Attempting to wave goodbye...", Array.Empty<object>());
			if (this.State == RpcState.Disconnected)
			{
				this.Logger.Error("State must NOT be disconnected in order to send a handwave!", Array.Empty<object>());
				return;
			}
			if (!this.namedPipe.WriteFrame(new PipeFrame(Opcode.Close, new Handshake
			{
				Version = RpcConnection.VERSION,
				ClientID = this.applicationID
			})))
			{
				this.Logger.Error("failed to write a handwave.", Array.Empty<object>());
				return;
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00005B24 File Offset: 0x00003D24
		public bool AttemptConnection()
		{
			this.Logger.Info("Attempting a new connection", Array.Empty<object>());
			if (this.thread != null)
			{
				this.Logger.Error("Cannot attempt a new connection as the previous connection thread is not null!", Array.Empty<object>());
				return false;
			}
			if (this.State != RpcState.Disconnected)
			{
				this.Logger.Warning("Cannot attempt a new connection as the previous connection hasn't changed state yet.", Array.Empty<object>());
				return false;
			}
			if (this.aborting)
			{
				this.Logger.Error("Cannot attempt a new connection while aborting!", Array.Empty<object>());
				return false;
			}
			this.thread = new Thread(new ThreadStart(this.MainLoop));
			this.thread.Name = "Discord IPC Thread";
			this.thread.IsBackground = true;
			this.thread.Start();
			return true;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00005BE4 File Offset: 0x00003DE4
		private void SetConnectionState(RpcState state)
		{
			this.Logger.Trace("Setting the connection state to {0}", new object[]
			{
				state.ToString().ToSnakeCase().ToUpperInvariant()
			});
			object obj = this.l_states;
			lock (obj)
			{
				this._state = state;
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00005C58 File Offset: 0x00003E58
		public void Shutdown()
		{
			this.Logger.Trace("Initiated shutdown procedure", Array.Empty<object>());
			this.shutdown = true;
			object obj = this.l_rtqueue;
			lock (obj)
			{
				this._rtqueue.Clear();
				if (RpcConnection.CLEAR_ON_SHUTDOWN)
				{
					this._rtqueue.Enqueue(new PresenceCommand
					{
						PID = this.processID,
						Presence = null
					});
				}
				this._rtqueue.Enqueue(new CloseCommand());
			}
			this.queueUpdatedEvent.Set();
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00005D04 File Offset: 0x00003F04
		public void Close()
		{
			if (this.thread == null)
			{
				this.Logger.Error("Cannot close as it is not available!", Array.Empty<object>());
				return;
			}
			if (this.aborting)
			{
				this.Logger.Error("Cannot abort as it has already been aborted", Array.Empty<object>());
				return;
			}
			if (this.ShutdownOnly)
			{
				this.Shutdown();
				return;
			}
			this.Logger.Trace("Updating Abort State...", Array.Empty<object>());
			this.aborting = true;
			this.queueUpdatedEvent.Set();
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00005D88 File Offset: 0x00003F88
		public void Dispose()
		{
			this.ShutdownOnly = false;
			this.Close();
		}

		// Token: 0x0400004C RID: 76
		public static readonly int VERSION = 1;

		// Token: 0x0400004D RID: 77
		public static readonly int POLL_RATE = 1000;

		// Token: 0x0400004E RID: 78
		private static readonly bool CLEAR_ON_SHUTDOWN = true;

		// Token: 0x0400004F RID: 79
		private static readonly bool LOCK_STEP = false;

		// Token: 0x04000050 RID: 80
		private ILogger _logger;

		// Token: 0x04000052 RID: 82
		private RpcState _state;

		// Token: 0x04000053 RID: 83
		private readonly object l_states = new object();

		// Token: 0x04000054 RID: 84
		private Configuration _configuration;

		// Token: 0x04000055 RID: 85
		private readonly object l_config = new object();

		// Token: 0x04000056 RID: 86
		private volatile bool aborting;

		// Token: 0x04000057 RID: 87
		private volatile bool shutdown;

		// Token: 0x04000059 RID: 89
		private string applicationID;

		// Token: 0x0400005A RID: 90
		private int processID;

		// Token: 0x0400005B RID: 91
		private long nonce;

		// Token: 0x0400005C RID: 92
		private Thread thread;

		// Token: 0x0400005D RID: 93
		private INamedPipeClient namedPipe;

		// Token: 0x0400005E RID: 94
		private int targetPipe;

		// Token: 0x0400005F RID: 95
		private readonly object l_rtqueue = new object();

		// Token: 0x04000060 RID: 96
		private readonly uint _maxRtQueueSize;

		// Token: 0x04000061 RID: 97
		private Queue<ICommand> _rtqueue;

		// Token: 0x04000062 RID: 98
		private readonly object l_rxqueue = new object();

		// Token: 0x04000063 RID: 99
		private readonly uint _maxRxQueueSize;

		// Token: 0x04000064 RID: 100
		private Queue<IMessage> _rxqueue;

		// Token: 0x04000065 RID: 101
		private AutoResetEvent queueUpdatedEvent = new AutoResetEvent(false);

		// Token: 0x04000066 RID: 102
		private BackoffDelay delay;
	}
}
