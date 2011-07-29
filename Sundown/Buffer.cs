#define marshalcopy

using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace Sundown
{
	unsafe public class Buffer : IDisposable
	{
		internal struct buffer
		{
			public byte *str;
			public int size;
			public int asize;
			public int unit;
			public int refcount;
		}

		internal IntPtr buf;

		buffer *cbuffer {
			get {
				return (buffer *)buf.ToPointer();
			}
		}

		internal Buffer(IntPtr ptr)
		{
			buf = ptr;
			Encoding = Encoding.Default;
		}

		public Buffer(int size)
			: this(bufnew(size))
		{
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			Release();
		}

		public int Size {
			get {
				return cbuffer->size;
			}
			set {
				cbuffer->size = value;
			}
		}

		public int AllocatedSize {
			get {
				return cbuffer->asize;
			}
		}

		public Encoding Encoding { get; set; }

		public void Put(byte[] bytes, int size)
		{
			bufput(buf, bytes, size);
		}

		public void Put(byte[] bytes)
		{
			Put(bytes, bytes.Length);
		}

		public void Put(string str)
		{
			Put(Encoding.GetBytes(str));
		}

		public void Put(string str, params object[] param)
		{
			Put(string.Format(str, param));
		}

		public void Puts(byte[] bytes, int size)
		{
			bufputs(buf, bytes, size);
		}

		public void Puts(byte[] bytes)
		{
			Puts(bytes, bytes.Length);
		}

		public void Puts(string str)
		{
			Puts(Encoding.GetBytes(str));
		}

		public void Puts(string str, params object[] param)
		{
			Puts(string.Format(str, param));
		}

		public void Putc(byte c)
		{
			bufputc(buf, c);
		}

		public void Grow(int size)
		{
			bufgrow(buf, size);
		}

		public void End()
		{
			bufnullterm(buf);
		}

		public override string ToString()
		{
			if (cbuffer->size == 0) {
				return string.Empty;
			}
#if marshalcopy
			byte[] bytes = new byte[cbuffer->size];
			Marshal.Copy(new IntPtr(cbuffer->str), bytes, 0, bytes.Length);
			return Encoding.GetString(bytes);
#else
			using (UnmanagedMemoryStream ums = new UnmanagedMemoryStream((byte *)cbuffer->str, cbuffer->size))
			{
				TextReader tr = new StreamReader(ums);
				return tr.ReadToEnd();
			}
#endif
		}

		public static int CaseCompare(Buffer buffer1, Buffer buffer2)
		{
			return bufcasecmp(buffer1.buf, buffer2.buf);
		}

		public int CaseCompare(Buffer buffer)
		{
			return CaseCompare(this, buffer);
		}

		public int Compare(Buffer buffer1, Buffer buffer2)
		{
			return bufcmp(buffer1.buf, buffer2.buf);
		}

		public int Compare(Buffer buffer)
		{
			return Compare(this, buffer);
		}

		public int Prefix(byte[] prefix)
		{
			return bufprefix(buf, prefix);
		}

		public int Prefix(string prefix)
		{
			return Prefix(Encoding.GetBytes(prefix));
		}

		public Buffer Duplicate(int size)
		{
			return new Buffer(bufdup(buf, size));
		}

		public Buffer Duplicate()
		{
			return Duplicate(cbuffer->size);
		}

		void Release()
		{
			bufrelease(buf);
		}

		public void Reset()
		{
			bufreset(buf);
		}

		public void Set(Buffer buffer)
		{
			bufset(buf, buffer.buf);
		}

		public void Slurp(int size)
		{
			bufslurp(buf, size);
		}

		[DllImport("sundown")]
		private static extern int bufcasecmp(IntPtr buf1, IntPtr buf2);

		[DllImport("sundown")]
		private static extern int bufcmp(IntPtr buf1, IntPtr buf2);

		[DllImport("sundown")]
		private static extern int bufcmps(IntPtr buf1, byte[] buffer);

		[DllImport("sundown")]
		private static extern int bufprefix(IntPtr buf1, byte[] prefix);

		[DllImport("sundown")]
		private static extern IntPtr bufnew(int size);

		[DllImport("sundown")]
		private static extern IntPtr bufdup(IntPtr buffer, int size);

		[DllImport("sundown")]
		private static extern int bufgrow(IntPtr buf, int size);

		[DllImport("sundown")]
		private static extern void bufnullterm(IntPtr buf);

		[DllImport("sundown")]
		private static extern void bufput(IntPtr buf, byte[] buffer, int size);

		[DllImport("sundown")]
		private static extern void bufputs(IntPtr buf, byte[] buffer, int size);

		[DllImport("sundown")]
		private static extern void bufputc(IntPtr buf, byte c);

		[DllImport("sundown")]
		private static extern void bufrelease(IntPtr buf);

		[DllImport("sundown")]
		private static extern void bufreset(IntPtr buf);

		[DllImport("sundown")]
		private static extern void bufset(IntPtr buf1, IntPtr buf2);

		[DllImport("sundown")]
		private static extern void bufslurp(IntPtr buf, int size);

		// TODO: implement this
		[DllImport("sundown")]
		private static extern int buftoi(IntPtr buf, int size, out int res);
	}

}

