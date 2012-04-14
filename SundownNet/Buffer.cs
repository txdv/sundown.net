#define marshalcopy
//#define clrbuffer

using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace Sundown
{
	unsafe public class Buffer : IDisposable
	{
#if clrbuffer
		Func<IntPtr, IntPtr> malloc;
		Func<IntPtr, IntPtr, IntPtr> realloc;
		Action<IntPtr> free;

		IntPtr Malloc(IntPtr size)
		{
			bufhandle = GCHandle.Alloc(bufinstance, GCHandleType.Pinned);
			return bufhandle.AddrOfPinnedObject();
		}

		internal byte[] bytearr;
		internal GCHandle bytearrhandle;

		IntPtr Realloc(IntPtr ptr, IntPtr size)
		{
			if (ptr == IntPtr.Zero) {
				bytearr = new byte[size.ToInt64()];
				bytearrhandle = GCHandle.Alloc(bytearr, GCHandleType.Pinned);
				return bytearrhandle.AddrOfPinnedObject();
			} else {
				bytearrhandle.Free();
				Array.Resize(ref bytearr, size.ToInt32());
				bytearrhandle = GCHandle.Alloc(bytearr, GCHandleType.Pinned);
				return bytearrhandle.AddrOfPinnedObject();
			}
		}

		void Free(IntPtr ptr)
		{
			if (ptr == buf) {
				bufhandle.Free();
			} else {
				bytearrhandle.Free();
			}
		}
#endif

		[StructLayout(LayoutKind.Sequential)]
		internal struct buffer
		{
			public IntPtr data;
			public IntPtr size;
			public IntPtr asize;
			public IntPtr unit;

			public IntPtr realloc;
			public IntPtr free;
		}

		buffer bufinstance = new buffer();
		internal IntPtr buf;
		internal GCHandle bufhandle;

		internal buffer *cbuffer {
			get {
				return (buffer *)buf;
			}
		}

		bool release;

		public Buffer()
			: this(1024)
		{
		}

		public Buffer(int size)
			: this((IntPtr)size)
		{
		}

		public Buffer(long size)
			: this((IntPtr)size)
		{
		}

		public Buffer(IntPtr size)
			: this(size, true)
		{
		}

		internal Buffer(IntPtr size, bool alloc)
		{
			if (alloc) {
#if clrbuffer
				malloc = Malloc;
				realloc = Realloc;
				free = Free;

				buf = bufnewcb(size,
				               Marshal.GetFunctionPointerForDelegate(malloc),
				               Marshal.GetFunctionPointerForDelegate(realloc),
				               Marshal.GetFunctionPointerForDelegate(free));
#else
				buf = bufnew(size);
#endif
				release = true;
			} else {
				buf = size;
			}
			Encoding = Encoding.Default;
			release = false;
		}

		~Buffer()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (release) {
				Release();
				release = false;
			}
		}

		public IntPtr Data {
			get {
				return Marshal.ReadIntPtr(buf);
			}
		}

		public IntPtr Size {
			get {
				return cbuffer->size;
			}
			set {
				cbuffer->size = value;
			}
		}

		public IntPtr AllocatedSize {
			get {
				return cbuffer->asize;
			}
		}

		public IntPtr UnitSize {
			get {
				return cbuffer->unit;
			}
		}

		public Encoding Encoding { get; set; }

		public void Put(IntPtr data, int size)
		{
			Put(data, (IntPtr)size);
		}

		public void Put(IntPtr data, long size)
		{
			Put(data, (IntPtr)size);
		}

		public void Put(IntPtr data, IntPtr size)
		{
			bufput(buf, data, size);
		}

		public void Put(byte[] bytes, int size)
		{
			Put(bytes, (IntPtr)size);
		}

		public void Put(byte[] bytes, long size)
		{
			Put(bytes, (IntPtr)size);
		}

		public void Put(byte[] bytes, IntPtr size)
		{
			var gchandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
			Put(gchandle.AddrOfPinnedObject(), size);
			gchandle.Free();
		}

		public void Put(byte[] bytes, int offset, int count)
		{
			Put(bytes, (IntPtr)offset, (IntPtr)count);
		}

		public void Put(byte[] bytes, long offset, long count)
		{
			Put(bytes, (IntPtr)offset, (IntPtr)count);
		}

		public void Put(byte[] bytes, IntPtr offset, IntPtr count)
		{
			var gchandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
			Put(new IntPtr(gchandle.AddrOfPinnedObject().ToInt64() + offset.ToInt64()), count);
			gchandle.Free();
		}

		public void Put(byte[] bytes)
		{
			Put(bytes, bytes.LongLength);
		}

		public void Put(Encoding encoding, string str)
		{
			Put(encoding.GetBytes(str));
		}

		public void Put(string str)
		{
			Put(Encoding, str);
		}

		public void Put(Encoding encoding, string str, params object[] param)
		{
			Put(encoding, string.Format(str, param));
		}

		public void Put(string str, params object[] param)
		{
			Put(Encoding, str, param);
		}

		public void Put(Buffer buffer)
		{
			Put(buffer.Data, buffer.Size);
		}

		public void Put(byte c)
		{
			bufputc(buf, c);
		}

		public void Grow(int size)
		{
			Grow(new IntPtr(size));
		}

		public void Grow(long size)
		{
			Grow(new IntPtr(size));
		}

		public void Grow(IntPtr size)
		{
			bufgrow(buf, size);
		}

		public override string ToString()
		{
			if (Size == IntPtr.Zero) {
				return string.Empty;
			}
#if clrbuffer
			return Encoding.GetString(bytearr, 0, Size.ToInt32());
#else
#if marshalcopy
			return Encoding.GetString(GetBytes());
#else
			return new StreamReader(GetStream()).ReadToEnd();
#endif
#endif
		}

		public BufferStream GetBufferStream()
		{
			return new BufferStream(this);
		}

		public Stream GetStream()
		{
			return new UnmanagedMemoryStream((byte *)Data, Size.ToInt64());
		}

		public byte[] GetBytes()
		{
			byte[] bytes = new byte[Size.ToInt64()];
			Marshal.Copy(Data, bytes, 0, bytes.Length);
			return bytes;
		}

		public int Prefix(byte[] prefix)
		{
			return bufprefix(buf, prefix);
		}

		public int Prefix(string prefix)
		{
			return Prefix(Encoding.GetBytes(prefix));
		}

		void Release()
		{
			if (buf != IntPtr.Zero) {
				bufrelease(buf);
				buf = IntPtr.Zero;
			}
		}

		public void Reset()
		{
			bufreset(buf);
		}

		public void Slurp(IntPtr size)
		{
			bufslurp(buf, size);
		}

		public void Slurp(int size)
		{
			Slurp(new IntPtr(size));
		}

		public void Slurp(long size)
		{
			Slurp(new IntPtr(size));
		}

		[DllImport("sundown")]
		private static extern int bufgrow(IntPtr buf, IntPtr size);

		[DllImport("sundown")]
		private static extern IntPtr bufnew(IntPtr size);

		[DllImport("sundown")]
		private static extern IntPtr bufnewcb(IntPtr size, IntPtr malloc, IntPtr realloc, IntPtr free);

		[DllImport("sundown")]
		private static extern int bufprefix(IntPtr buf, byte[] prefix);

		[DllImport("sundown")]
		private static extern void bufput(IntPtr buf, IntPtr buffer, IntPtr size);

		[DllImport("sundown")]
		private static extern void bufputs(IntPtr buf, IntPtr size);

		[DllImport("sundown")]
		private static extern void bufputc(IntPtr buf, byte c);

		[DllImport("sundown")]
		private static extern void bufrelease(IntPtr buf);

		[DllImport("sundown")]
		private static extern void bufreset(IntPtr buf);

		[DllImport("sundown")]
		private static extern void bufslurp(IntPtr buf, IntPtr size);
	}
}

