using System;
using System.Text;
using System.Runtime.InteropServices;

namespace Sundown
{
	public sealed class Markdown : IDisposable
	{
		[DllImport("sundown")]
		internal static extern void sd_version(out int major, out int minor, out int revision);

		public static Version Version {
			get {
				int major, minor, revision;
				sd_version(out major, out minor, out revision);
				return new Version(major, minor, revision);
			}
		}

		IntPtr ptr;
		Renderer renderer;

		[DllImport("sundown")]
		internal static extern IntPtr sd_markdown_new(uint extensions, IntPtr max_nesting, ref sd_callbacks callbacks, IntPtr opaque);

		public Markdown(Renderer renderer)
			: this(renderer, null)
		{
		}

		public Markdown(Renderer renderer, MarkdownExtensions extensions)
			: this(renderer, extensions, 16)
		{
		}

		public Markdown(Renderer renderer, int maxNesting)
			: this(renderer, null, maxNesting)
		{
		}

		public Markdown(Renderer renderer, MarkdownExtensions extensions, int maxNesting)
		{
			this.renderer = renderer;
			renderer.Pin();

			ptr = sd_markdown_new((extensions == null ? 0 : extensions.ToUInt()), (IntPtr)maxNesting,
				ref renderer.callbacks, renderer.opaque);
		}

		~Markdown()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
		}

		[DllImport("sundown")]
		internal static extern void sd_markdown_free(IntPtr ptr);

		void Dispose(bool disposing)
		{
			if (disposing) {
				GC.SuppressFinalize(this);
			}

			if (ptr != IntPtr.Zero) {
				sd_markdown_free(ptr);
				ptr = IntPtr.Zero;
			}

			if (renderer != null) {
				renderer.Unpin();
				renderer = null;
			}
		}

		public void Render(Buffer @out, string str)
		{
			Render(@out, @out.Encoding, str);
		}

		public void Render(Buffer @out, Encoding encoding, string str)
		{
			Render(@out, encoding.GetBytes(str));
		}

		public void Render(Buffer @out, Buffer @in)
		{
			sd_markdown_render(@out.buf, @in.Data, @in.Size, ptr);
		}

		public void Render(Buffer @out, byte[] array)
		{
			Render(@out, array, array.LongLength);
		}

		public void Render(Buffer @out, byte[] array, int length)
		{
			Render(@out, array, (IntPtr)length);
		}

		public void Render(Buffer @out, byte[] array, long length)
		{
			Render(@out, array, (IntPtr)length);
		}

		[DllImport("sundown")]
		internal static extern void sd_markdown_render(IntPtr buf, IntPtr document, IntPtr documentSize, IntPtr md);

		public void Render(Buffer @out, byte[] array, IntPtr length)
		{
			var handle = GCHandle.Alloc(array, GCHandleType.Pinned);
			sd_markdown_render(@out.buf, handle.AddrOfPinnedObject(), length, ptr);
			handle.Free();
		}

		#region SmartyPants

		public static void SmartyPants(Buffer @out, string str)
		{
			SmartyPants(@out, Encoding.Default, str);
		}

		public static void SmartyPants(Buffer @out, Encoding encoding, string str)
		{
			SmartyPants(@out, encoding.GetBytes(str));
		}

		public static void SmartyPants(Buffer @out, byte[] array)
		{
			SmartyPants(@out, array, array.LongLength);
		}

		public static void SmartyPants(Buffer @out, byte[] array, int length)
		{
			SmartyPants(@out, array, (IntPtr)length);
		}

		public static void SmartyPants(Buffer @out, byte[] array, long length)
		{
			SmartyPants(@out, array, (IntPtr)length);
		}

		[DllImport("sundown")]
		internal static extern void sdhtml_smartypants(IntPtr buf, IntPtr text, IntPtr size);

		public static void SmartyPants(Buffer @out, byte[] array, IntPtr length)
		{
			var handle = GCHandle.Alloc(array, GCHandleType.Pinned);
			sdhtml_smartypants(@out.buf, handle.AddrOfPinnedObject(), (IntPtr)length);
			handle.Free();
		}

		#endregion
	}
}

