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
		internal html_renderopt options = new html_renderopt();
		internal GCHandle optionsgchandle;
		internal uint flags;

		HtmlRenderer(uint flags)
			: base(false)
		{
			this.flags = flags;
			Initialize();
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
		internal static extern void sdhtml_renderer(ref md_callbacks callbacks, IntPtr options, uint render_flags);

		protected override void Initialize()
		{
			optionsgchandle = GCHandle.Alloc(options, GCHandleType.Pinned);
			opaque = optionsgchandle.AddrOfPinnedObject();
			sdhtml_renderer(ref callbacks, opaque, flags);

			base.Initialize();
		}
	}
}

