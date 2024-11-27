using System;
using System.Linq;
using System.Text;

namespace DiscordRPC.Helper
{
	// Token: 0x02000039 RID: 57
	public static class StringTools
	{
		// Token: 0x060001DB RID: 475 RVA: 0x00007AF5 File Offset: 0x00005CF5
		public static string GetNullOrString(this string str)
		{
			if (str.Length != 0 && !string.IsNullOrEmpty(str.Trim()))
			{
				return str;
			}
			return null;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00007B0F File Offset: 0x00005D0F
		public static bool WithinLength(this string str, int bytes)
		{
			return str.WithinLength(bytes, Encoding.UTF8);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00007B1D File Offset: 0x00005D1D
		public static bool WithinLength(this string str, int bytes, Encoding encoding)
		{
			return encoding.GetByteCount(str) <= bytes;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00007B2C File Offset: 0x00005D2C
		public static string ToCamelCase(this string str)
		{
			if (str == null)
			{
				return null;
			}
			return (from s in str.ToLowerInvariant().Split(new string[]
			{
				"_",
				" "
			}, StringSplitOptions.RemoveEmptyEntries)
			select char.ToUpper(s[0]).ToString() + s.Substring(1, s.Length - 1)).Aggregate(string.Empty, (string s1, string s2) => s1 + s2);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00007BAD File Offset: 0x00005DAD
		public static string ToSnakeCase(this string str)
		{
			if (str == null)
			{
				return null;
			}
			return string.Concat(str.Select(delegate(char x, int i)
			{
				if (i <= 0 || !char.IsUpper(x))
				{
					return x.ToString();
				}
				return "_" + x.ToString();
			}).ToArray<string>()).ToUpperInvariant();
		}
	}
}
