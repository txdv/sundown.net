using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace Sundown
{
	public class ClrBuffer : Buffer
	{
		buffer bufinstance = new buffer();
		GCHandle bufhandle;

		Func<IntPtr, IntPtr> malloc;
		Func<IntPtr, IntPtr, IntPtr> realloc;
		Action<IntPtr> free;

		IntPtr Malloc(IntPtr size)
		{
			bufhandle = GCHandle.Alloc(bufinstance, GCHandleType.Pinned);
			return bufhandle.AddrOfPinnedObject();
		}

		byte[] bytearr;
		GCHandle bytearrhandle;

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
			if (ptr == NativeHandle) {
				bufhandle.Free();
			} else {
				bytearrhandle.Free();
			}
		}

		public ClrBuffer()
			: this(DefaultUnitSize)
		{
		}

		public ClrBuffer(int size)
			: this((IntPtr)size)
		{
		}

		public ClrBuffer(long size)
			: this((IntPtr)size)
		{
		}

		public ClrBuffer(IntPtr size)
			: this((IntPtr)size, true)
		{
		}

		public ClrBuffer(IntPtr size, bool alloc)
			: base(size, alloc)
		{
		}

		[DllImport("sundown", CallingConvention=CallingConvention.Cdecl)]
		private static extern IntPtr bufnewcb(IntPtr size, IntPtr malloc, IntPtr realloc, IntPtr free);

		protected override void Alloc(IntPtr size)
		{
			malloc = Malloc;
			realloc = Realloc;
			free = Free;

			NativeHandle = bufnewcb(size,
				Marshal.GetFunctionPointerForDelegate(malloc),
				Marshal.GetFunctionPointerForDelegate(realloc),
				Marshal.GetFunctionPointerForDelegate(free));
		}

		public override string ToString()
		{
			return Encoding.GetString(bytearr, 0, Size.ToInt32());
		}

		public byte[] Buffer {
			get {
				return bytearr;
			}
		}
	}
}

