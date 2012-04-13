using System;

namespace Sundown
{
	enum html_render_mode : uint
	{
		HTML_SKIP_HTML        = (1 << 0),
		HTML_SKIP_STYLE       = (1 << 1),
		HTML_SKIP_IMAGES      = (1 << 2),
		HTML_SKIP_LINKS       = (1 << 3),
		HTML_SKIP_EXPAND_TABS = (1 << 4),
		HTML_SAFELINK         = (1 << 5),
		HTML_TOC              = (1 << 6),
		HTML_HARD_WRAP        = (1 << 7),
		HTML_USE_XHTML        = (1 << 8),
		HTML_ESCAPE           = (1 << 9)
	}

	public class HtmlRenderMode
	{
		public bool SkipHtml       { get; set; }
		public bool SkipStyle      { get; set; }
		public bool SkipImages     { get; set; }
		public bool SkipLinks      { get; set; }
		public bool SkipExpandTabs { get; set; }
		public bool SafeLink       { get; set; }
		public bool TOC            { get; set; }
		public bool HardWrap       { get; set; }
		public bool UseXHTML       { get; set; }
		public bool Escape         { get; set; }

		internal uint ToUInt()
		{
			uint ret = 0;

			if (SkipHtml)       ret |= (uint)html_render_mode.HTML_SKIP_HTML;
			if (SkipStyle)      ret |= (uint)html_render_mode.HTML_SKIP_STYLE;
			if (SkipImages)     ret |= (uint)html_render_mode.HTML_SKIP_IMAGES;
			if (SkipLinks)      ret |= (uint)html_render_mode.HTML_SKIP_LINKS;
			if (SkipExpandTabs) ret |= (uint)html_render_mode.HTML_SKIP_EXPAND_TABS;
			if (SafeLink)       ret |= (uint)html_render_mode.HTML_SAFELINK;
			if (TOC)            ret |= (uint)html_render_mode.HTML_TOC;
			if (HardWrap)       ret |= (uint)html_render_mode.HTML_HARD_WRAP;
			if (UseXHTML)       ret |= (uint)html_render_mode.HTML_USE_XHTML;
			if (Escape)         ret |= (uint)html_render_mode.HTML_ESCAPE;

			return ret;
		}
	}
}

