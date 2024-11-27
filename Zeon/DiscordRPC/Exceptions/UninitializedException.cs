using System;

namespace DiscordRPC.Exceptions
{
	// Token: 0x0200003E RID: 62
	public class UninitializedException : Exception
	{
		// Token: 0x060001EA RID: 490 RVA: 0x00007C76 File Offset: 0x00005E76
		internal UninitializedException(string message) : base(message)
		{
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00007C7F File Offset: 0x00005E7F
		internal UninitializedException() : this("Cannot perform action because the client has not been initialized yet or has been deinitialized.")
		{
		}
	}
}
