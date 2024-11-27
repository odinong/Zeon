using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace DiscordRPC.IO
{
	// Token: 0x02000037 RID: 55
	public struct PipeFrame : IEquatable<PipeFrame>
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001BA RID: 442 RVA: 0x00007793 File Offset: 0x00005993
		// (set) Token: 0x060001BB RID: 443 RVA: 0x0000779B File Offset: 0x0000599B
		public Opcode Opcode { get; set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001BC RID: 444 RVA: 0x000077A4 File Offset: 0x000059A4
		public uint Length
		{
			get
			{
				return (uint)this.Data.Length;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001BD RID: 445 RVA: 0x000077AE File Offset: 0x000059AE
		// (set) Token: 0x060001BE RID: 446 RVA: 0x000077B6 File Offset: 0x000059B6
		public byte[] Data { get; set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001BF RID: 447 RVA: 0x000077BF File Offset: 0x000059BF
		// (set) Token: 0x060001C0 RID: 448 RVA: 0x000077C7 File Offset: 0x000059C7
		public string Message
		{
			get
			{
				return this.GetMessage();
			}
			set
			{
				this.SetMessage(value);
			}
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x000077D0 File Offset: 0x000059D0
		public PipeFrame(Opcode opcode, object data)
		{
			this.Opcode = opcode;
			this.Data = null;
			this.SetObject(data);
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x000077E7 File Offset: 0x000059E7
		public Encoding MessageEncoding
		{
			get
			{
				return Encoding.UTF8;
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x000077EE File Offset: 0x000059EE
		private void SetMessage(string str)
		{
			this.Data = this.MessageEncoding.GetBytes(str);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00007802 File Offset: 0x00005A02
		private string GetMessage()
		{
			return this.MessageEncoding.GetString(this.Data);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00007818 File Offset: 0x00005A18
		public void SetObject(object obj)
		{
			string message = JsonConvert.SerializeObject(obj);
			this.SetMessage(message);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00007833 File Offset: 0x00005A33
		public void SetObject(Opcode opcode, object obj)
		{
			this.Opcode = opcode;
			this.SetObject(obj);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00007843 File Offset: 0x00005A43
		public T GetObject<T>()
		{
			return JsonConvert.DeserializeObject<T>(this.GetMessage());
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00007850 File Offset: 0x00005A50
		public bool ReadStream(Stream stream)
		{
			uint opcode;
			if (!this.TryReadUInt32(stream, out opcode))
			{
				return false;
			}
			uint num;
			if (!this.TryReadUInt32(stream, out num))
			{
				return false;
			}
			uint num2 = num;
			bool result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				uint num3 = (uint)this.Min(2048, num);
				byte[] array = new byte[num3];
				int count;
				while ((count = stream.Read(array, 0, this.Min(array.Length, num2))) > 0)
				{
					num2 -= num3;
					memoryStream.Write(array, 0, count);
				}
				byte[] array2 = memoryStream.ToArray();
				if ((long)array2.Length != (long)((ulong)num))
				{
					result = false;
				}
				else
				{
					this.Opcode = (Opcode)opcode;
					this.Data = array2;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00007908 File Offset: 0x00005B08
		private int Min(int a, uint b)
		{
			if ((ulong)b >= (ulong)((long)a))
			{
				return a;
			}
			return (int)b;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00007914 File Offset: 0x00005B14
		private bool TryReadUInt32(Stream stream, out uint value)
		{
			byte[] array = new byte[4];
			if (stream.Read(array, 0, array.Length) != 4)
			{
				value = 0U;
				return false;
			}
			value = BitConverter.ToUInt32(array, 0);
			return true;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00007948 File Offset: 0x00005B48
		public void WriteStream(Stream stream)
		{
			byte[] bytes = BitConverter.GetBytes((uint)this.Opcode);
			byte[] bytes2 = BitConverter.GetBytes(this.Length);
			byte[] array = new byte[bytes.Length + bytes2.Length + this.Data.Length];
			bytes.CopyTo(array, 0);
			bytes2.CopyTo(array, bytes.Length);
			this.Data.CopyTo(array, bytes.Length + bytes2.Length);
			stream.Write(array, 0, array.Length);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x000079B3 File Offset: 0x00005BB3
		public bool Equals(PipeFrame other)
		{
			return this.Opcode == other.Opcode && this.Length == other.Length && this.Data == other.Data;
		}

		// Token: 0x040000DF RID: 223
		public static readonly int MAX_SIZE = 16384;
	}
}
