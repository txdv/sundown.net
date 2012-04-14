using System;
using System.Runtime.InteropServices;

namespace Sundown
{
	public class TableOfContentRenderer : Renderer
	{
		internal html_renderopt options = new html_renderopt();
		internal GCHandle optionsgchandle;
		internal uint flags;

		public TableOfContentRenderer()
		{
		}

		~TableOfContentRenderer()
		{
			if (optionsgchandle.IsAllocated) {
				optionsgchandle.Free();
			}
		}

		[DllImport("sundown")]
		internal static extern void sdhtml_toc_renderer(ref md_callbacks callbacks, IntPtr options, uint render_flags);

		protected override void Initialize()
		{
			optionsgchandle = GCHandle.Alloc(options, GCHandleType.Pinned);
			opaque = optionsgchandle.AddrOfPinnedObject();
			sdhtml_toc_renderer(ref callbacks, opaque, 0);
			base.Initialize();
		}
	}
}

