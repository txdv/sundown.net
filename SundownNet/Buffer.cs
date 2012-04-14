using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace Sundown
{
	unsafe public abstract class Buffer : IDisposable
	{
		public static readonly int DefaultUnitSize = 1024;

		[StructLayout(LayoutKind.Sequential)]
		protected struct buffer
		{
			public IntPtr data;
			public IntPtr size;
			public IntPtr asize;
			public IntPtr unit;

			public IntPtr realloc;
			public IntPtr free;
		}

		protected buffer bufinstance = new buffer();
		internal IntPtr buf;
		internal bool release;

		protected buffer *cbuffer {
			get {
				return (buffer *)buf;
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

		protected Buffer(IntPtr size, bool alloc)
		{
			if (alloc) {
				Alloc(size);
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

		public static Buffer From(IntPtr ptr)
		{
			return new NativeBuffer(ptr, false);
		}

		public static Buffer Create()
		{
			return Create(DefaultUnitSize);
		}

		public static Buffer Create(int size)
		{
			return Create((IntPtr)size);
		}

		public static Buffer Create(long size)
		{
			return Create((IntPtr)size);
		}

		public static Buffer Create(IntPtr size)
		{
			return new NativeBuffer(size, true);
		}

		protected abstract void Alloc(IntPtr size);

		public byte[] GetBytes()
		{
			byte[] bytes = new byte[Size.ToInt64()];
			Marshal.Copy(Data, bytes, 0, bytes.Length);
			return bytes;
		}

		public BufferStream GetBufferStream()
		{
			return new BufferStream(this);
		}

		public Stream GetStream()
		{
			return new UnmanagedMemoryStream((byte *)Data, Size.ToInt64());
		}

		#region Put

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

		[DllImport("sundown")]
		private static extern void bufput(IntPtr buf, IntPtr buffer, IntPtr size);

		[DllImport("sundown")]
		private static extern void bufputs(IntPtr buf, IntPtr size);

		[DllImport("sundown")]
		private static extern void bufputc(IntPtr buf, byte c);

		#endregion

		#region Grow
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

		[DllImport("sundown")]
		private static extern int bufgrow(IntPtr buf, IntPtr size);
		#endregion

		#region Reset
		public void Reset()
		{
			bufreset(buf);
		}

		[DllImport("sundown")]
		private static extern void bufreset(IntPtr buf);
		#endregion

		#region Release
		void Release()
		{
			if (buf != IntPtr.Zero) {
				bufrelease(buf);
				buf = IntPtr.Zero;
			}
		}

		[DllImport("sundown")]
		private static extern void bufrelease(IntPtr buf);
		#endregion

		#region Slurp
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
		private static extern void bufslurp(IntPtr buf, IntPtr size);
		#endregion
	}
}

