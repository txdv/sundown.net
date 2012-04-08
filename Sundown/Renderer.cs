using System;
using System.Runtime.InteropServices;

namespace Sundown
{
	delegate void mkd_renderer_blockcode (IntPtr ob, IntPtr text, IntPtr lang, IntPtr opaque);
	delegate void mkd_renderer_blockquote(IntPtr ob, IntPtr text,              IntPtr opaque);
	delegate void mkd_renderer_blockhtml (IntPtr ob, IntPtr text,              IntPtr opaque);
	delegate void mkd_renderer_header    (IntPtr ob, IntPtr text, int level,   IntPtr opaque);
	delegate void mkd_renderer_hrule     (IntPtr ob,                           IntPtr opaque);
	delegate void mkd_renderer_list      (IntPtr ob, IntPtr text, int flags,   IntPtr opaque);
	delegate void mkd_renderer_listitem  (IntPtr ob, IntPtr text, int flags,   IntPtr opaque);
	delegate void mkd_renderer_paragraph (IntPtr ob, IntPtr text,              IntPtr opaque);
	delegate void mkd_renderer_table     (IntPtr ob, IntPtr head, IntPtr body, IntPtr opaque);
	delegate void mkd_renderer_table_row (IntPtr ob, IntPtr text,              IntPtr opaque);
	delegate void mkd_renderer_table_cell(IntPtr ob, IntPtr text, int flags,   IntPtr opaque);

	delegate int mkd_renderer_autolink       (IntPtr ob, IntPtr link, int type, IntPtr opaque);
	delegate int mkd_renderer_codespan       (IntPtr ob, IntPtr text,           IntPtr opaque);
	delegate int mkd_renderer_double_emphasis(IntPtr ob, IntPtr text,           IntPtr opaque);
	delegate int mkd_renderer_emphasis       (IntPtr ob, IntPtr text,           IntPtr opaque);
	delegate int mkd_renderer_image          (IntPtr ob, IntPtr link, IntPtr title, IntPtr alt, IntPtr opaque);
	delegate int mkd_renderer_linebreak      (IntPtr ob,                        IntPtr opaque);
	delegate int mkd_renderer_link           (IntPtr ob, IntPtr link, IntPtr title, IntPtr content, IntPtr opaque);
	delegate int mkd_renderer_raw_html_tag   (IntPtr ob, IntPtr tag,            IntPtr opaque);
	delegate int mkd_renderer_triple_emphasis(IntPtr ob, IntPtr text,           IntPtr opaque);
	delegate int mkd_renderer_strikethrough  (IntPtr ob, IntPtr text,           IntPtr opaque);
	delegate int mkd_renderer_superscript    (IntPtr ob, IntPtr text,           IntPtr opaque);

	delegate void mkd_renderer_entity     (IntPtr ob, IntPtr entity, IntPtr opaque);
	delegate void mkd_renderer_normal_text(IntPtr ob, IntPtr text,   IntPtr opaque);

	delegate void mkd_renderer_doc_header(IntPtr ob, IntPtr opaque);
	delegate void mkd_renderer_doc_footer(IntPtr ob, IntPtr opaque);

	[StructLayout(LayoutKind.Sequential)]
	internal struct sd_callbacks
	{
		public mkd_renderer_blockcode  blockcode;
		public mkd_renderer_blockquote blockquote;
		public mkd_renderer_blockhtml  blockhtml;
		public mkd_renderer_header     header;
		public mkd_renderer_hrule      hrule;
		public mkd_renderer_list       list;
		public mkd_renderer_listitem   listitem;
		public mkd_renderer_paragraph  paragrah;
		public mkd_renderer_table      table;
		public mkd_renderer_table_row  table_row;
		public mkd_renderer_table_cell table_cell;


		public mkd_renderer_autolink        autolink;
		public mkd_renderer_codespan        codespan;
		public mkd_renderer_double_emphasis double_emphasis;
		public mkd_renderer_emphasis        emphasis;
		public mkd_renderer_image           image;
		public mkd_renderer_linebreak       linebreak;
		public mkd_renderer_link            link;
		public mkd_renderer_raw_html_tag    raw_html_tag;
		public mkd_renderer_triple_emphasis triple_emphasis;
		public mkd_renderer_strikethrough   strikethrough;
		public mkd_renderer_superscript     superscript;

		public mkd_renderer_entity entity;
		public mkd_renderer_normal_text normal_text;

		public mkd_renderer_doc_header doc_header;
		public mkd_renderer_doc_footer doc_footer;
	}

	public abstract class Renderer
	{
		internal sd_callbacks callbacks;
	}
}

