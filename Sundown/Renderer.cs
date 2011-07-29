using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Sundown
{
	public class Renderer
	{
		IntPtr renderer = new IntPtr();

		public Renderer()
		{
			sdhtml_renderer(out renderer, 0, IntPtr.Zero);
		}

		public Buffer Markdown(Buffer outBuffer, Buffer inBuffer, int size)
		{
			uint i = 0;

			sd_markdown(outBuffer.buf, inBuffer.buf, ref renderer, ~i);

			return outBuffer;
		}

		public static Version LibraryVersion {
			get {
				int major, minor, revision;
				sd_version(out major, out minor, out revision);
				return new Version(major, minor, revision);
			}
		}

		[DllImport("sundown")]
		private static extern void sdhtml_renderer(out IntPtr renderer, int size, IntPtr ptr);

		[DllImport("sundown")]
		private static extern void sdhtml_free_renderer(IntPtr renderer);


		[DllImport("sundown")]
		private static extern void sd_markdown(IntPtr outBuffer, IntPtr inBuffer, ref IntPtr renderer, uint extensions);

		[DllImport("sundown")]
		private static extern void sd_version(out int major, out int minor, out int revision);
	}
}

