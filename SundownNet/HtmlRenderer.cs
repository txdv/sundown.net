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

	public class HtmlRenderer : Renderer
	{
		internal html_renderopt options;

		HtmlRenderer(uint flags)
		{
			options = new html_renderopt();
			sdhtml_renderer(ref callbacks, ref options, flags);
		}

		public HtmlRenderer()
			: this(0)
		{
		}

		public HtmlRenderer(HtmlRenderMode mode)
			: this(mode.ToUInt())
		{
		}

		[DllImport("sundown")]
		internal static extern void sdhtml_renderer(ref sd_callbacks callbacks, ref html_renderopt options, uint render_flags);


		unsafe internal override void Initialize()
		{
			fixed (html_renderopt *ptr = &options) {
				opaque = new IntPtr(ptr);
			}
		}
	}
}

