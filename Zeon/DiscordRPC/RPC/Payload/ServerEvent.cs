using System;
using DiscordRPC.Converters;

namespace DiscordRPC.RPC.Payload
{
	// Token: 0x02000015 RID: 21
	internal enum ServerEvent
	{
		// Token: 0x04000087 RID: 135
		[EnumValue("READY")]
		Ready,
		// Token: 0x04000088 RID: 136
		[EnumValue("ERROR")]
		Error,
		// Token: 0x04000089 RID: 137
		[EnumValue("ACTIVITY_JOIN")]
		ActivityJoin,
		// Token: 0x0400008A RID: 138
		[EnumValue("ACTIVITY_SPECTATE")]
		ActivitySpectate,
		// Token: 0x0400008B RID: 139
		[EnumValue("ACTIVITY_JOIN_REQUEST")]
		ActivityJoinRequest
	}
}
