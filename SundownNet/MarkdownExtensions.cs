using System;

namespace Sundown
{
	enum mkd_extensions : uint
	{
		MKDEXT_NO_INTRA_EMPHASIS = (1 << 0),
		MKDEXT_TABLES            = (1 << 1),
		MKDEXT_FENCED_CODE       = (1 << 2),
		MKDEXT_AUTOLINK          = (1 << 3),
		MKDEXT_STRIKETHROUGH     = (1 << 4),
		MKDEXT_LAX_HTML_BLOCKS   = (1 << 5),
		MKDEXT_SPACE_HEADERS     = (1 << 6),
		MKDEXT_SUPERSCRIPT       = (1 << 7),
	}

	public class MarkdownExtensions
	{
		public bool NoIntraEmphasis { get; set; }
		public bool Tables          { get; set; }
		public bool FencedCode      { get; set; }
		public bool Autolink        { get; set; }
		public bool Strikethrough   { get; set; }
		public bool LaxHTMLBlocks   { get; set; }
		public bool SpaceHeaders    { get; set; }
		public bool SuperScript     { get; set; }

		internal uint ToUInt()
		{
			uint ret = 0;

			if (NoIntraEmphasis) ret |= (uint)mkd_extensions.MKDEXT_NO_INTRA_EMPHASIS;
			if (Tables)          ret |= (uint)mkd_extensions.MKDEXT_TABLES;
			if (FencedCode)      ret |= (uint)mkd_extensions.MKDEXT_FENCED_CODE;
			if (Autolink)        ret |= (uint)mkd_extensions.MKDEXT_AUTOLINK;
			if (Strikethrough)   ret |= (uint)mkd_extensions.MKDEXT_STRIKETHROUGH;
			if (LaxHTMLBlocks)   ret |= (uint)mkd_extensions.MKDEXT_LAX_HTML_BLOCKS;
			if (SpaceHeaders)    ret |= (uint)mkd_extensions.MKDEXT_SPACE_HEADERS;
			if (SuperScript)     ret |= (uint)mkd_extensions.MKDEXT_SUPERSCRIPT;

			return ret;
		}
	}
}

