using System;
using System.Collections.Generic;
using System.Diagnostics;
using DiscordRPC.Events;
using DiscordRPC.Exceptions;
using DiscordRPC.IO;
using DiscordRPC.Logging;
using DiscordRPC.Message;
using DiscordRPC.RPC;
using DiscordRPC.RPC.Commands;
using DiscordRPC.RPC.Payload;

namespace DiscordRPC
{
	// Token: 0x02000003 RID: 3
	public sealed class DiscordRpcClient : IDisposable
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000008 RID: 8 RVA: 0x0000208B File Offset: 0x0000028B
		// (set) Token: 0x06000009 RID: 9 RVA: 0x00002093 File Offset: 0x00000293
		public bool HasRegisteredUriScheme { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000A RID: 10 RVA: 0x0000209C File Offset: 0x0000029C
		// (set) Token: 0x0600000B RID: 11 RVA: 0x000020A4 File Offset: 0x000002A4
		public string ApplicationID { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000C RID: 12 RVA: 0x000020AD File Offset: 0x000002AD
		// (set) Token: 0x0600000D RID: 13 RVA: 0x000020B5 File Offset: 0x000002B5
		public string SteamID { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000020BE File Offset: 0x000002BE
		// (set) Token: 0x0600000F RID: 15 RVA: 0x000020C6 File Offset: 0x000002C6
		public int ProcessID { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000020CF File Offset: 0x000002CF
		// (set) Token: 0x06000011 RID: 17 RVA: 0x000020D7 File Offset: 0x000002D7
		public int MaxQueueSize { get; private set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000012 RID: 18 RVA: 0x000020E0 File Offset: 0x000002E0
		// (set) Token: 0x06000013 RID: 19 RVA: 0x000020E8 File Offset: 0x000002E8
		public bool IsDisposed { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000014 RID: 20 RVA: 0x000020F1 File Offset: 0x000002F1
		// (set) Token: 0x06000015 RID: 21 RVA: 0x000020F9 File Offset: 0x000002F9
		public ILogger Logger
		{
			get
			{
				return this._logger;
			}
			set
			{
				this._logger = value;
				if (this.connection != null)
				{
					this.connection.Logger = value;
				}
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002116 File Offset: 0x00000316
		// (set) Token: 0x06000017 RID: 23 RVA: 0x0000211E File Offset: 0x0000031E
		public bool AutoEvents { get; private set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002127 File Offset: 0x00000327
		// (set) Token: 0x06000019 RID: 25 RVA: 0x0000212F File Offset: 0x0000032F
		public bool SkipIdenticalPresence { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002138 File Offset: 0x00000338
		// (set) Token: 0x0600001B RID: 27 RVA: 0x00002140 File Offset: 0x00000340
		public int TargetPipe { get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002149 File Offset: 0x00000349
		// (set) Token: 0x0600001D RID: 29 RVA: 0x00002151 File Offset: 0x00000351
		public RichPresence CurrentPresence { get; private set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600001E RID: 30 RVA: 0x0000215A File Offset: 0x0000035A
		// (set) Token: 0x0600001F RID: 31 RVA: 0x00002162 File Offset: 0x00000362
		public EventType Subscription { get; private set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000020 RID: 32 RVA: 0x0000216B File Offset: 0x0000036B
		// (set) Token: 0x06000021 RID: 33 RVA: 0x00002173 File Offset: 0x00000373
		public User CurrentUser { get; private set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000022 RID: 34 RVA: 0x0000217C File Offset: 0x0000037C
		// (set) Token: 0x06000023 RID: 35 RVA: 0x00002184 File Offset: 0x00000384
		public Configuration Configuration { get; private set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000024 RID: 36 RVA: 0x0000218D File Offset: 0x0000038D
		// (set) Token: 0x06000025 RID: 37 RVA: 0x00002195 File Offset: 0x00000395
		public bool IsInitialized { get; private set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000026 RID: 38 RVA: 0x0000219E File Offset: 0x0000039E
		// (set) Token: 0x06000027 RID: 39 RVA: 0x000021A6 File Offset: 0x000003A6
		public bool ShutdownOnly
		{
			get
			{
				return this._shutdownOnly;
			}
			set
			{
				this._shutdownOnly = value;
				if (this.connection != null)
				{
					this.connection.ShutdownOnly = value;
				}
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000028 RID: 40 RVA: 0x000021C4 File Offset: 0x000003C4
		// (remove) Token: 0x06000029 RID: 41 RVA: 0x000021FC File Offset: 0x000003FC
		public event OnReadyEvent OnReady;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600002A RID: 42 RVA: 0x00002234 File Offset: 0x00000434
		// (remove) Token: 0x0600002B RID: 43 RVA: 0x0000226C File Offset: 0x0000046C
		public event OnCloseEvent OnClose;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600002C RID: 44 RVA: 0x000022A4 File Offset: 0x000004A4
		// (remove) Token: 0x0600002D RID: 45 RVA: 0x000022DC File Offset: 0x000004DC
		public event OnErrorEvent OnError;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600002E RID: 46 RVA: 0x00002314 File Offset: 0x00000514
		// (remove) Token: 0x0600002F RID: 47 RVA: 0x0000234C File Offset: 0x0000054C
		public event OnPresenceUpdateEvent OnPresenceUpdate;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000030 RID: 48 RVA: 0x00002384 File Offset: 0x00000584
		// (remove) Token: 0x06000031 RID: 49 RVA: 0x000023BC File Offset: 0x000005BC
		public event OnSubscribeEvent OnSubscribe;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000032 RID: 50 RVA: 0x000023F4 File Offset: 0x000005F4
		// (remove) Token: 0x06000033 RID: 51 RVA: 0x0000242C File Offset: 0x0000062C
		public event OnUnsubscribeEvent OnUnsubscribe;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000034 RID: 52 RVA: 0x00002464 File Offset: 0x00000664
		// (remove) Token: 0x06000035 RID: 53 RVA: 0x0000249C File Offset: 0x0000069C
		public event OnJoinEvent OnJoin;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000036 RID: 54 RVA: 0x000024D4 File Offset: 0x000006D4
		// (remove) Token: 0x06000037 RID: 55 RVA: 0x0000250C File Offset: 0x0000070C
		public event OnSpectateEvent OnSpectate;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000038 RID: 56 RVA: 0x00002544 File Offset: 0x00000744
		// (remove) Token: 0x06000039 RID: 57 RVA: 0x0000257C File Offset: 0x0000077C
		public event OnJoinRequestedEvent OnJoinRequested;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600003A RID: 58 RVA: 0x000025B4 File Offset: 0x000007B4
		// (remove) Token: 0x0600003B RID: 59 RVA: 0x000025EC File Offset: 0x000007EC
		public event OnConnectionEstablishedEvent OnConnectionEstablished;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600003C RID: 60 RVA: 0x00002624 File Offset: 0x00000824
		// (remove) Token: 0x0600003D RID: 61 RVA: 0x0000265C File Offset: 0x0000085C
		public event OnConnectionFailedEvent OnConnectionFailed;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600003E RID: 62 RVA: 0x00002694 File Offset: 0x00000894
		// (remove) Token: 0x0600003F RID: 63 RVA: 0x000026CC File Offset: 0x000008CC
		public event OnRpcMessageEvent OnRpcMessage;

		// Token: 0x06000040 RID: 64 RVA: 0x00002701 File Offset: 0x00000901
		public DiscordRpcClient(string applicationID) : this(applicationID, -1, null, true, null)
		{
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002710 File Offset: 0x00000910
		public DiscordRpcClient(string applicationID, int pipe = -1, ILogger logger = null, bool autoEvents = true, INamedPipeClient client = null)
		{
			if (string.IsNullOrEmpty(applicationID))
			{
				throw new ArgumentNullException("applicationID");
			}
			this.ApplicationID = applicationID.Trim();
			this.TargetPipe = pipe;
			this.ProcessID = Process.GetCurrentProcess().Id;
			this.HasRegisteredUriScheme = false;
			this.AutoEvents = autoEvents;
			this.SkipIdenticalPresence = true;
			this._logger = (logger ?? new NullLogger());
			this.connection = new RpcConnection(this.ApplicationID, this.ProcessID, this.TargetPipe, client ?? new ManagedNamedPipeClient(), autoEvents ? 0U : 128U, 512U)
			{
				ShutdownOnly = this._shutdownOnly,
				Logger = this._logger
			};
			this.connection.OnRpcMessage += delegate(object sender, IMessage msg)
			{
				if (this.OnRpcMessage != null)
				{
					this.OnRpcMessage(this, msg);
				}
				if (this.AutoEvents)
				{
					this.ProcessMessage(msg);
				}
			};
		}

        // Token: 0x06000042 RID: 66 RVA: 0x000027FC File Offset: 0x000009FC
        public IMessage[] Invoke()
        {
            if (this.AutoEvents)
            {
                this.Logger.Error("Cannot Invoke client when AutomaticallyInvokeEvents has been set.", Array.Empty<object>());
                return new IMessage[0];
            }

            List<IMessage> messages = new List<IMessage>();

            foreach (IMessage message in this.connection.DequeueMessages())
            {
                this.ProcessMessage(message);
                messages.Add(message);
            }

            return messages.ToArray();
        }


        // Token: 0x06000043 RID: 67 RVA: 0x00002854 File Offset: 0x00000A54
        private void ProcessMessage(IMessage message)
		{
			if (message == null)
			{
				return;
			}
			switch (message.Type)
			{
			case MessageType.Ready:
			{
				ReadyMessage readyMessage = message as ReadyMessage;
				if (readyMessage != null)
				{
					object sync = this._sync;
					lock (sync)
					{
						this.Configuration = readyMessage.Configuration;
						this.CurrentUser = readyMessage.User;
					}
					this.SynchronizeState();
				}
				if (this.OnReady != null)
				{
					this.OnReady(this, message as ReadyMessage);
					return;
				}
				break;
			}
			case MessageType.Close:
				if (this.OnClose != null)
				{
					this.OnClose(this, message as CloseMessage);
					return;
				}
				break;
			case MessageType.Error:
				if (this.OnError != null)
				{
					this.OnError(this, message as ErrorMessage);
					return;
				}
				break;
			case MessageType.PresenceUpdate:
			{
				object sync = this._sync;
				lock (sync)
				{
					PresenceMessage presenceMessage = message as PresenceMessage;
					if (presenceMessage != null)
					{
						if (presenceMessage.Presence == null)
						{
							this.CurrentPresence = null;
						}
						else if (this.CurrentPresence == null)
						{
							this.CurrentPresence = new RichPresence().Merge(presenceMessage.Presence);
						}
						else
						{
							this.CurrentPresence.Merge(presenceMessage.Presence);
						}
						presenceMessage.Presence = this.CurrentPresence;
					}
				}
				if (this.OnPresenceUpdate != null)
				{
					this.OnPresenceUpdate(this, message as PresenceMessage);
					return;
				}
				break;
			}
			case MessageType.Subscribe:
			{
				object sync = this._sync;
				lock (sync)
				{
					SubscribeMessage subscribeMessage = message as SubscribeMessage;
					this.Subscription |= subscribeMessage.Event;
				}
				if (this.OnSubscribe != null)
				{
					this.OnSubscribe(this, message as SubscribeMessage);
					return;
				}
				break;
			}
			case MessageType.Unsubscribe:
			{
				object sync = this._sync;
				lock (sync)
				{
					UnsubscribeMessage unsubscribeMessage = message as UnsubscribeMessage;
					this.Subscription &= ~unsubscribeMessage.Event;
				}
				if (this.OnUnsubscribe != null)
				{
					this.OnUnsubscribe(this, message as UnsubscribeMessage);
					return;
				}
				break;
			}
			case MessageType.Join:
				if (this.OnJoin != null)
				{
					this.OnJoin(this, message as JoinMessage);
					return;
				}
				break;
			case MessageType.Spectate:
				if (this.OnSpectate != null)
				{
					this.OnSpectate(this, message as SpectateMessage);
					return;
				}
				break;
			case MessageType.JoinRequest:
				if (this.Configuration != null)
				{
					JoinRequestMessage joinRequestMessage = message as JoinRequestMessage;
					if (joinRequestMessage != null)
					{
						joinRequestMessage.User.SetConfiguration(this.Configuration);
					}
				}
				if (this.OnJoinRequested != null)
				{
					this.OnJoinRequested(this, message as JoinRequestMessage);
					return;
				}
				break;
			case MessageType.ConnectionEstablished:
				if (this.OnConnectionEstablished != null)
				{
					this.OnConnectionEstablished(this, message as ConnectionEstablishedMessage);
					return;
				}
				break;
			case MessageType.ConnectionFailed:
				if (this.OnConnectionFailed != null)
				{
					this.OnConnectionFailed(this, message as ConnectionFailedMessage);
					return;
				}
				break;
			default:
				this.Logger.Error("Message was queued with no appropriate handle! {0}", new object[]
				{
					message.Type
				});
				break;
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002B98 File Offset: 0x00000D98
		public void Respond(JoinRequestMessage request, bool acceptRequest)
		{
			if (this.IsDisposed)
			{
				throw new ObjectDisposedException("Discord IPC Client");
			}
			if (this.connection == null)
			{
				throw new ObjectDisposedException("Connection", "Cannot initialize as the connection has been deinitialized");
			}
			if (!this.IsInitialized)
			{
				throw new UninitializedException();
			}
			this.connection.EnqueueCommand(new RespondCommand
			{
				Accept = acceptRequest,
				UserID = request.User.ID.ToString()
			});
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002C10 File Offset: 0x00000E10
		public void SetPresence(RichPresence presence)
		{
			if (this.IsDisposed)
			{
				throw new ObjectDisposedException("Discord IPC Client");
			}
			if (this.connection == null)
			{
				throw new ObjectDisposedException("Connection", "Cannot initialize as the connection has been deinitialized");
			}
			if (!this.IsInitialized)
			{
				this.Logger.Warning("The client is not yet initialized, storing the presence as a state instead.", Array.Empty<object>());
			}
			if (presence == null)
			{
				if (!this.SkipIdenticalPresence || this.CurrentPresence != null)
				{
					this.connection.EnqueueCommand(new PresenceCommand
					{
						PID = this.ProcessID,
						Presence = null
					});
				}
			}
			else
			{
				if (presence.HasSecrets() && !this.HasRegisteredUriScheme)
				{
					throw new BadPresenceException("Cannot send a presence with secrets as this object has not registered a URI scheme. Please enable the uri scheme registration in the DiscordRpcClient constructor.");
				}
				if (presence.HasParty() && presence.Party.Max < presence.Party.Size)
				{
					throw new BadPresenceException("Presence maximum party size cannot be smaller than the current size.");
				}
				if (presence.HasSecrets() && !presence.HasParty())
				{
					this.Logger.Warning("The presence has set the secrets but no buttons will show as there is no party available.", Array.Empty<object>());
				}
				if (!this.SkipIdenticalPresence || !presence.Matches(this.CurrentPresence))
				{
					this.connection.EnqueueCommand(new PresenceCommand
					{
						PID = this.ProcessID,
						Presence = presence.Clone()
					});
				}
			}
			object sync = this._sync;
			lock (sync)
			{
				this.CurrentPresence = ((presence != null) ? presence.Clone() : null);
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002D8C File Offset: 0x00000F8C
		public RichPresence UpdateButtons(Button[] button = null)
		{
			if (!this.IsInitialized)
			{
				throw new UninitializedException();
			}
			object sync = this._sync;
			RichPresence richPresence;
			lock (sync)
			{
				if (this.CurrentPresence == null)
				{
					richPresence = new RichPresence();
				}
				else
				{
					richPresence = this.CurrentPresence.Clone();
				}
			}
			richPresence.Buttons = button;
			this.SetPresence(richPresence);
			return richPresence;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002E00 File Offset: 0x00001000
		public RichPresence SetButton(Button button, int index = 0)
		{
			if (!this.IsInitialized)
			{
				throw new UninitializedException();
			}
			object sync = this._sync;
			RichPresence richPresence;
			lock (sync)
			{
				if (this.CurrentPresence == null)
				{
					richPresence = new RichPresence();
				}
				else
				{
					richPresence = this.CurrentPresence.Clone();
				}
			}
			richPresence.Buttons[index] = button;
			this.SetPresence(richPresence);
			return richPresence;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002E78 File Offset: 0x00001078
		public RichPresence UpdateDetails(string details)
		{
			if (!this.IsInitialized)
			{
				throw new UninitializedException();
			}
			object sync = this._sync;
			RichPresence richPresence;
			lock (sync)
			{
				if (this.CurrentPresence == null)
				{
					richPresence = new RichPresence();
				}
				else
				{
					richPresence = this.CurrentPresence.Clone();
				}
			}
			richPresence.Details = details;
			this.SetPresence(richPresence);
			return richPresence;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002EEC File Offset: 0x000010EC
		public RichPresence UpdateState(string state)
		{
			if (!this.IsInitialized)
			{
				throw new UninitializedException();
			}
			object sync = this._sync;
			RichPresence richPresence;
			lock (sync)
			{
				if (this.CurrentPresence == null)
				{
					richPresence = new RichPresence();
				}
				else
				{
					richPresence = this.CurrentPresence.Clone();
				}
			}
			richPresence.State = state;
			this.SetPresence(richPresence);
			return richPresence;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002F60 File Offset: 0x00001160
		public RichPresence UpdateParty(Party party)
		{
			if (!this.IsInitialized)
			{
				throw new UninitializedException();
			}
			object sync = this._sync;
			RichPresence richPresence;
			lock (sync)
			{
				if (this.CurrentPresence == null)
				{
					richPresence = new RichPresence();
				}
				else
				{
					richPresence = this.CurrentPresence.Clone();
				}
			}
			richPresence.Party = party;
			this.SetPresence(richPresence);
			return richPresence;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002FD4 File Offset: 0x000011D4
		public RichPresence UpdatePartySize(int size)
		{
			if (!this.IsInitialized)
			{
				throw new UninitializedException();
			}
			object sync = this._sync;
			RichPresence richPresence;
			lock (sync)
			{
				if (this.CurrentPresence == null)
				{
					richPresence = new RichPresence();
				}
				else
				{
					richPresence = this.CurrentPresence.Clone();
				}
			}
			if (richPresence.Party == null)
			{
				throw new BadPresenceException("Cannot set the size of the party if the party does not exist");
			}
			richPresence.Party.Size = size;
			this.SetPresence(richPresence);
			return richPresence;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003060 File Offset: 0x00001260
		public RichPresence UpdatePartySize(int size, int max)
		{
			if (!this.IsInitialized)
			{
				throw new UninitializedException();
			}
			object sync = this._sync;
			RichPresence richPresence;
			lock (sync)
			{
				if (this.CurrentPresence == null)
				{
					richPresence = new RichPresence();
				}
				else
				{
					richPresence = this.CurrentPresence.Clone();
				}
			}
			if (richPresence.Party == null)
			{
				throw new BadPresenceException("Cannot set the size of the party if the party does not exist");
			}
			richPresence.Party.Size = size;
			richPresence.Party.Max = max;
			this.SetPresence(richPresence);
			return richPresence;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000030F8 File Offset: 0x000012F8
		public RichPresence UpdateLargeAsset(string key = null, string tooltip = null)
		{
			if (!this.IsInitialized)
			{
				throw new UninitializedException();
			}
			object sync = this._sync;
			RichPresence richPresence;
			lock (sync)
			{
				if (this.CurrentPresence == null)
				{
					richPresence = new RichPresence();
				}
				else
				{
					richPresence = this.CurrentPresence.Clone();
				}
			}
			if (richPresence.Assets == null)
			{
				richPresence.Assets = new Assets();
			}
			richPresence.Assets.LargeImageKey = (key ?? richPresence.Assets.LargeImageKey);
			richPresence.Assets.LargeImageText = (tooltip ?? richPresence.Assets.LargeImageText);
			this.SetPresence(richPresence);
			return richPresence;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000031B0 File Offset: 0x000013B0
		public RichPresence UpdateSmallAsset(string key = null, string tooltip = null)
		{
			if (!this.IsInitialized)
			{
				throw new UninitializedException();
			}
			object sync = this._sync;
			RichPresence richPresence;
			lock (sync)
			{
				if (this.CurrentPresence == null)
				{
					richPresence = new RichPresence();
				}
				else
				{
					richPresence = this.CurrentPresence.Clone();
				}
			}
			if (richPresence.Assets == null)
			{
				richPresence.Assets = new Assets();
			}
			richPresence.Assets.SmallImageKey = (key ?? richPresence.Assets.SmallImageKey);
			richPresence.Assets.SmallImageText = (tooltip ?? richPresence.Assets.SmallImageText);
			this.SetPresence(richPresence);
			return richPresence;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003268 File Offset: 0x00001468
		public RichPresence UpdateSecrets(Secrets secrets)
		{
			if (!this.IsInitialized)
			{
				throw new UninitializedException();
			}
			object sync = this._sync;
			RichPresence richPresence;
			lock (sync)
			{
				if (this.CurrentPresence == null)
				{
					richPresence = new RichPresence();
				}
				else
				{
					richPresence = this.CurrentPresence.Clone();
				}
			}
			richPresence.Secrets = secrets;
			this.SetPresence(richPresence);
			return richPresence;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000032DC File Offset: 0x000014DC
		public RichPresence UpdateStartTime()
		{
			return this.UpdateStartTime(DateTime.UtcNow);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000032EC File Offset: 0x000014EC
		public RichPresence UpdateStartTime(DateTime time)
		{
			if (!this.IsInitialized)
			{
				throw new UninitializedException();
			}
			object sync = this._sync;
			RichPresence richPresence;
			lock (sync)
			{
				if (this.CurrentPresence == null)
				{
					richPresence = new RichPresence();
				}
				else
				{
					richPresence = this.CurrentPresence.Clone();
				}
			}
			if (richPresence.Timestamps == null)
			{
				richPresence.Timestamps = new Timestamps();
			}
			richPresence.Timestamps.Start = new DateTime?(time);
			this.SetPresence(richPresence);
			return richPresence;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x0000337C File Offset: 0x0000157C
		public RichPresence UpdateEndTime()
		{
			return this.UpdateEndTime(DateTime.UtcNow);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000338C File Offset: 0x0000158C
		public RichPresence UpdateEndTime(DateTime time)
		{
			if (!this.IsInitialized)
			{
				throw new UninitializedException();
			}
			object sync = this._sync;
			RichPresence richPresence;
			lock (sync)
			{
				if (this.CurrentPresence == null)
				{
					richPresence = new RichPresence();
				}
				else
				{
					richPresence = this.CurrentPresence.Clone();
				}
			}
			if (richPresence.Timestamps == null)
			{
				richPresence.Timestamps = new Timestamps();
			}
			richPresence.Timestamps.End = new DateTime?(time);
			this.SetPresence(richPresence);
			return richPresence;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000341C File Offset: 0x0000161C
		public RichPresence UpdateClearTime()
		{
			if (!this.IsInitialized)
			{
				throw new UninitializedException();
			}
			object sync = this._sync;
			RichPresence richPresence;
			lock (sync)
			{
				if (this.CurrentPresence == null)
				{
					richPresence = new RichPresence();
				}
				else
				{
					richPresence = this.CurrentPresence.Clone();
				}
			}
			richPresence.Timestamps = null;
			this.SetPresence(richPresence);
			return richPresence;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003490 File Offset: 0x00001690
		public void ClearPresence()
		{
			if (this.IsDisposed)
			{
				throw new ObjectDisposedException("Discord IPC Client");
			}
			if (!this.IsInitialized)
			{
				throw new UninitializedException();
			}
			if (this.connection == null)
			{
				throw new ObjectDisposedException("Connection", "Cannot initialize as the connection has been deinitialized");
			}
			this.SetPresence(null);
		}


		// Token: 0x06000057 RID: 87 RVA: 0x00003510 File Offset: 0x00001710
		public void Subscribe(EventType type)
		{
			this.SetSubscription(this.Subscription | type);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003520 File Offset: 0x00001720
		[Obsolete("Replaced with Unsubscribe", true)]
		public void Unubscribe(EventType type)
		{
			this.SetSubscription(this.Subscription & ~type);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003531 File Offset: 0x00001731
		public void Unsubscribe(EventType type)
		{
			this.SetSubscription(this.Subscription & ~type);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003544 File Offset: 0x00001744
		public void SetSubscription(EventType type)
		{
			if (this.IsInitialized)
			{
				this.SubscribeToTypes(this.Subscription & ~type, true);
				this.SubscribeToTypes(~this.Subscription & type, false);
			}
			else
			{
				this.Logger.Warning("Client has not yet initialized, but events are being subscribed too. Storing them as state instead.", Array.Empty<object>());
			}
			object sync = this._sync;
			lock (sync)
			{
				this.Subscription = type;
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000035C4 File Offset: 0x000017C4
		private void SubscribeToTypes(EventType type, bool isUnsubscribe)
		{
			if (type == EventType.None)
			{
				return;
			}
			if (this.IsDisposed)
			{
				throw new ObjectDisposedException("Discord IPC Client");
			}
			if (!this.IsInitialized)
			{
				throw new UninitializedException();
			}
			if (this.connection == null)
			{
				throw new ObjectDisposedException("Connection", "Cannot initialize as the connection has been deinitialized");
			}
			if (!this.HasRegisteredUriScheme)
			{
				throw new InvalidConfigurationException("Cannot subscribe/unsubscribe to an event as this application has not registered a URI Scheme. Call RegisterUriScheme().");
			}
			if ((type & EventType.Spectate) == EventType.Spectate)
			{
				this.connection.EnqueueCommand(new SubscribeCommand
				{
					Event = ServerEvent.ActivitySpectate,
					IsUnsubscribe = isUnsubscribe
				});
			}
			if ((type & EventType.Join) == EventType.Join)
			{
				this.connection.EnqueueCommand(new SubscribeCommand
				{
					Event = ServerEvent.ActivityJoin,
					IsUnsubscribe = isUnsubscribe
				});
			}
			if ((type & EventType.JoinRequest) == EventType.JoinRequest)
			{
				this.connection.EnqueueCommand(new SubscribeCommand
				{
					Event = ServerEvent.ActivityJoinRequest,
					IsUnsubscribe = isUnsubscribe
				});
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000368D File Offset: 0x0000188D
		public void SynchronizeState()
		{
			if (!this.IsInitialized)
			{
				throw new UninitializedException();
			}
			this.SetPresence(this.CurrentPresence);
			if (this.HasRegisteredUriScheme)
			{
				this.SubscribeToTypes(this.Subscription, false);
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000036C0 File Offset: 0x000018C0
		public bool Initialize()
		{
			if (this.IsDisposed)
			{
				throw new ObjectDisposedException("Discord IPC Client");
			}
			if (this.IsInitialized)
			{
				throw new UninitializedException("Cannot initialize a client that is already initialized");
			}
			if (this.connection == null)
			{
				throw new ObjectDisposedException("Connection", "Cannot initialize as the connection has been deinitialized");
			}
			return this.IsInitialized = this.connection.AttemptConnection();
		}

		// Token: 0x0600005E RID: 94 RVA: 0x0000371F File Offset: 0x0000191F
		public void Deinitialize()
		{
			if (!this.IsInitialized)
			{
				throw new UninitializedException("Cannot deinitialize a client that has not been initalized.");
			}
			this.connection.Close();
			this.IsInitialized = false;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003746 File Offset: 0x00001946
		public void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			if (this.IsInitialized)
			{
				this.Deinitialize();
			}
			this.IsDisposed = true;
		}

		// Token: 0x0400000A RID: 10
		private ILogger _logger;

		// Token: 0x0400000E RID: 14
		private RpcConnection connection;

		// Token: 0x04000014 RID: 20
		private bool _shutdownOnly = true;

		// Token: 0x04000015 RID: 21
		private object _sync = new object();
	}
}
