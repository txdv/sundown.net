using System;
using System.Runtime.InteropServices;

namespace Sundown
{
	struct html_renderopt
	{
		public int header_count;
		public int current_level;
		public int level_offset;
		public uint flags;
		public IntPtr link_attributes;
	}

	public sealed class HtmlRenderer : Renderer
	{
		internal html_renderopt options = new html_renderopt();
		internal GCHandle optionsgchandle;

		HtmlRenderer(uint flags)
		{
			callbacksgchandle = GCHandle.Alloc(callbacks, GCHandleType.Pinned);

			optionsgchandle = GCHandle.Alloc(options, GCHandleType.Pinned);
			opaque = optionsgchandle.AddrOfPinnedObject();
			sdhtml_renderer(callbacksgchandle.AddrOfPinnedObject(), opaque, flags);
		}

		public HtmlRenderer()
			: this(0)
		{
		}

		public HtmlRenderer(HtmlRenderMode mode)
			: this(mode.ToUInt())
		{
		}

		~HtmlRenderer()
		{
			if (optionsgchandle.IsAllocated) {
				optionsgchandle.Free();
			}
		}

		[DllImport("sundown")]
		internal static extern void sdhtml_renderer(IntPtr callbacks, IntPtr options, uint render_flags);
	}
}

