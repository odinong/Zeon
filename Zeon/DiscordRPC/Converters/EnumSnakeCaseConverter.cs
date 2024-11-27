using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace DiscordRPC.Converters
{
	// Token: 0x0200004B RID: 75
	internal class EnumSnakeCaseConverter : JsonConverter
	{
		// Token: 0x0600021C RID: 540 RVA: 0x00007C8C File Offset: 0x00005E8C
		public override bool CanConvert(Type objectType)
		{
			return objectType.IsEnum;
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00007C94 File Offset: 0x00005E94
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.Value == null)
			{
				return null;
			}
			object result = null;
			if (this.TryParseEnum(objectType, (string)reader.Value, out result))
			{
				return result;
			}
			return existingValue;
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00007CC8 File Offset: 0x00005EC8
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			Type type = value.GetType();
			string value2 = Enum.GetName(type, value);
			foreach (MemberInfo memberInfo in type.GetMembers(BindingFlags.Static | BindingFlags.Public))
			{
				if (memberInfo.Name.Equals(value2))
				{
					object[] customAttributes = memberInfo.GetCustomAttributes(typeof(EnumValueAttribute), true);
					if (customAttributes.Length != 0)
					{
						value2 = ((EnumValueAttribute)customAttributes[0]).Value;
					}
				}
			}
			writer.WriteValue(value2);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00007D3C File Offset: 0x00005F3C
		public bool TryParseEnum(Type enumType, string str, out object obj)
		{
			if (str == null)
			{
				obj = null;
				return false;
			}
			Type type = enumType;
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				type = type.GetGenericArguments().First<Type>();
			}
			if (!type.IsEnum)
			{
				obj = null;
				return false;
			}
			foreach (MemberInfo memberInfo in type.GetMembers(BindingFlags.Static | BindingFlags.Public))
			{
				foreach (EnumValueAttribute enumValueAttribute in memberInfo.GetCustomAttributes(typeof(EnumValueAttribute), true))
				{
					if (str.Equals(enumValueAttribute.Value))
					{
						obj = Enum.Parse(type, memberInfo.Name, true);
						return true;
					}
				}
			}
			obj = null;
			return false;
		}
	}
}
