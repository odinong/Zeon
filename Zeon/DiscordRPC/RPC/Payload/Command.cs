using System;
using DiscordRPC.Converters;

namespace DiscordRPC.RPC.Payload
{
	// Token: 0x02000011 RID: 17
	internal enum Command
	{
		// Token: 0x0400006E RID: 110
		[EnumValue("DISPATCH")]
		Dispatch,
		// Token: 0x0400006F RID: 111
		[EnumValue("SET_ACTIVITY")]
		SetActivity,
		// Token: 0x04000070 RID: 112
		[EnumValue("SUBSCRIBE")]
		Subscribe,
		// Token: 0x04000071 RID: 113
		[EnumValue("UNSUBSCRIBE")]
		Unsubscribe,
		// Token: 0x04000072 RID: 114
		[EnumValue("SEND_ACTIVITY_JOIN_INVITE")]
		SendActivityJoinInvite,
		// Token: 0x04000073 RID: 115
		[EnumValue("CLOSE_ACTIVITY_JOIN_REQUEST")]
		CloseActivityJoinRequest,
		// Token: 0x04000074 RID: 116
		[Obsolete("This value is appart of the RPC API and is not supported by this library.", true)]
		Authorize,
		// Token: 0x04000075 RID: 117
		[Obsolete("This value is appart of the RPC API and is not supported by this library.", true)]
		Authenticate,
		// Token: 0x04000076 RID: 118
		[Obsolete("This value is appart of the RPC API and is not supported by this library.", true)]
		GetGuild,
		// Token: 0x04000077 RID: 119
		[Obsolete("This value is appart of the RPC API and is not supported by this library.", true)]
		GetGuilds,
		// Token: 0x04000078 RID: 120
		[Obsolete("This value is appart of the RPC API and is not supported by this library.", true)]
		GetChannel,
		// Token: 0x04000079 RID: 121
		[Obsolete("This value is appart of the RPC API and is not supported by this library.", true)]
		GetChannels,
		// Token: 0x0400007A RID: 122
		[Obsolete("This value is appart of the RPC API and is not supported by this library.", true)]
		SetUserVoiceSettings,
		// Token: 0x0400007B RID: 123
		[Obsolete("This value is appart of the RPC API and is not supported by this library.", true)]
		SelectVoiceChannel,
		// Token: 0x0400007C RID: 124
		[Obsolete("This value is appart of the RPC API and is not supported by this library.", true)]
		GetSelectedVoiceChannel,
		// Token: 0x0400007D RID: 125
		[Obsolete("This value is appart of the RPC API and is not supported by this library.", true)]
		SelectTextChannel,
		// Token: 0x0400007E RID: 126
		[Obsolete("This value is appart of the RPC API and is not supported by this library.", true)]
		GetVoiceSettings,
		// Token: 0x0400007F RID: 127
		[Obsolete("This value is appart of the RPC API and is not supported by this library.", true)]
		SetVoiceSettings,
		// Token: 0x04000080 RID: 128
		[Obsolete("This value is appart of the RPC API and is not supported by this library.", true)]
		CaptureShortcut
	}
}
