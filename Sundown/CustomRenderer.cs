using System;

namespace Sundown
{
	public abstract class CustomRenderer : Renderer
	{
		public CustomRenderer()
		{
			callbacks.blockcode  = mkd_renderer_blockcode;
			callbacks.blockquote = mkd_renderer_blockquote;
			callbacks.blockhtml  = mkd_renderer_blockhtml;
			callbacks.header     = mkd_renderer_header;
			callbacks.hrule      = mkd_renderer_hrule;
			callbacks.list       = mkd_renderer_list;
			callbacks.listitem   = mkd_renderer_listitem;
			callbacks.paragrah   = mkd_renderer_paragraph;
			callbacks.table      = mkd_renderer_table;
			callbacks.table_row  = mkd_renderer_table_row;
			callbacks.table_cell = mkd_renderer_table_cell;

			callbacks.autolink        = mkd_renderer_autolink;
			callbacks.codespan        = mkd_renderer_codespan;
			callbacks.double_emphasis = mkd_renderer_double_emphasis;
			callbacks.emphasis        = mkd_renderer_emphasis;
			callbacks.image           = mkd_renderer_image;
			callbacks.linebreak       = mkd_renderer_linebreak;
			callbacks.link            = mkd_renderer_link;
			callbacks.raw_html_tag    = mkd_renderer_raw_html_tag;
			callbacks.triple_emphasis = mkd_renderer_triple_emphasis;
			callbacks.strikethrough   = mkd_renderer_strikethrough;
			callbacks.superscript     = mkd_renderer_superscript;

			callbacks.entity      = mkd_renderer_entity;
			callbacks.normal_text = mkd_renderer_normal_text;

			callbacks.doc_header = mkd_renderer_doc_header;
			callbacks.doc_footer = mkd_renderer_doc_footer;
		}

		#region block level

		protected virtual void BlockCode(Buffer ob, Buffer text, Buffer language) { }
		void mkd_renderer_blockcode(IntPtr ob, IntPtr text, IntPtr lang, IntPtr opaque)
		{
			BlockCode(new Buffer(ob), new Buffer(text), new Buffer(lang));
		}

		protected virtual void BlockQuote(Buffer ob, Buffer text) { }
		void mkd_renderer_blockquote(IntPtr ob, IntPtr text, IntPtr opaque)
		{
			BlockQuote(new Buffer(ob), new Buffer(text));
		}

		protected virtual void BlockHtml(Buffer ob, Buffer text) { }
		void mkd_renderer_blockhtml(IntPtr ob, IntPtr text, IntPtr opaque)
		{
			BlockHtml(new Buffer(ob), new Buffer(text));
		}

		protected virtual void Header(Buffer ob, Buffer text, int level) { }
		void mkd_renderer_header(IntPtr ob, IntPtr text, int level, IntPtr opaque)
		{
			Header(new Buffer(ob), new Buffer(text), level);
		}

		protected virtual void HRule(Buffer ob) { }
		void mkd_renderer_hrule(IntPtr ob, IntPtr opaque)
		{
			HRule(new Buffer(ob));
		}

		protected virtual void List(Buffer ob, Buffer text, int flags) { }
		void mkd_renderer_list(IntPtr ob, IntPtr text, int flags, IntPtr opaque)
		{
			List(new Buffer(ob), new Buffer(text), flags);
		}

		protected virtual void ListItem(Buffer ob, Buffer text, int flags) { }
		void mkd_renderer_listitem(IntPtr ob, IntPtr text, int flags, IntPtr opaque)
		{
			ListItem(new Buffer(ob), new Buffer(text), flags);
		}

		protected virtual void Paragraph(Buffer ob, Buffer text) { }
		void mkd_renderer_paragraph(IntPtr ob, IntPtr text, IntPtr opaque)
		{
			Paragraph(new Buffer(ob), new Buffer(text));
		}

		protected virtual void Table(Buffer ob, Buffer header, Buffer body) { }
		void mkd_renderer_table(IntPtr ob, IntPtr header, IntPtr body, IntPtr opaque)
		{
			Table(new Buffer(ob), new Buffer(header), new Buffer(body));
		}

		protected virtual void TableRow(Buffer ob, Buffer text) { }
		void mkd_renderer_table_row(IntPtr ob, IntPtr text, IntPtr opaque)
		{
			TableRow(new Buffer(ob), new Buffer(text));
		}

		protected virtual void TableCell(Buffer ob, Buffer text, int flags) { }
		void mkd_renderer_table_cell(IntPtr ob, IntPtr text, int flags, IntPtr opaque)
		{
			TableCell(new Buffer(ob), new Buffer(text), flags);
		}

		#endregion

		#region span level

		protected virtual int Autolink(Buffer ob, Buffer link, int type) { return 0; }
		int mkd_renderer_autolink(IntPtr ob, IntPtr link, int type, IntPtr opaque)
		{
			return Autolink(new Buffer(ob), new Buffer(link), type);
		}

		protected virtual int Codespan(Buffer ob, Buffer text) { return 0; }
		int mkd_renderer_codespan(IntPtr ob, IntPtr text, IntPtr opaque)
		{
			return Codespan(new Buffer(ob), new Buffer(text));
		}

		protected virtual int DoubleEmphasis(Buffer ob, Buffer text) { return 0; }
		int mkd_renderer_double_emphasis(IntPtr ob, IntPtr text, IntPtr opaque)
		{
			return DoubleEmphasis(new Buffer(ob), new Buffer(text));
		}

		protected virtual int Emphasis(Buffer ob, Buffer text) { return 0; }
		int mkd_renderer_emphasis(IntPtr ob, IntPtr text, IntPtr opaque)
		{
			return Emphasis(new Buffer(ob), new Buffer(text));
		}

		protected virtual int Image(Buffer ob, Buffer link, Buffer title, Buffer alt) { return 0; }
		int mkd_renderer_image(IntPtr ob, IntPtr link, IntPtr title, IntPtr alt, IntPtr opaque)
		{
			return Image(new Buffer(ob), new Buffer(link), new Buffer(title), new Buffer(alt));
		}

		protected virtual int Linebreak(Buffer ob) { return 0; }
		int mkd_renderer_linebreak(IntPtr ob, IntPtr opaque)
		{
			return Linebreak(new Buffer(ob));
		}

		protected virtual int Link(Buffer ob, Buffer link, Buffer title, Buffer content) { return 0; }
		int mkd_renderer_link(IntPtr ob, IntPtr link, IntPtr title, IntPtr content, IntPtr opaque)
		{
			return Link(new Buffer(ob), new Buffer(link), new Buffer(title), new Buffer(content));
		}

		protected virtual int RawHtmlTag(Buffer ob, Buffer tag) { return 0; }
		int mkd_renderer_raw_html_tag(IntPtr ob, IntPtr tag, IntPtr opaque)
		{
			return RawHtmlTag(new Buffer(ob), new Buffer(tag));
		}

		protected virtual int TripleEmphasis(Buffer buf, Buffer text) { return 0; }
		int mkd_renderer_triple_emphasis(IntPtr buf, IntPtr text, IntPtr opaque)
		{
			return TripleEmphasis(new Buffer(buf), new Buffer(text));
		}

		protected virtual int Strikethrough(Buffer buf, Buffer text) { return 0; }
		int mkd_renderer_strikethrough(IntPtr buf, IntPtr text, IntPtr opaque)
		{
			return Strikethrough(new Buffer(buf), new Buffer(text));
		}

		protected virtual int Superscript(Buffer buf, Buffer text) { return 0; }
		int mkd_renderer_superscript(IntPtr buf, IntPtr buffer, IntPtr opaque)
		{
			return Superscript(new Buffer(buf), new Buffer(buffer));
		}

		#endregion

		#region low level callbacks

		public virtual void NormalText(Buffer ob, Buffer text) { ob.Put(text.ToString()); }
		void mkd_renderer_normal_text(IntPtr ob, IntPtr text, IntPtr opaque)
		{
			NormalText(new Buffer(ob), new Buffer(text));
		}

		public virtual void Entity(Buffer ob, Buffer entity) { ob.Put(entity.ToString()); }
		void mkd_renderer_entity(IntPtr ob, IntPtr entity, IntPtr opaque)
		{
			Entity(new Buffer(ob), new Buffer(entity));
		}

		#endregion

		#region header and footer

		public virtual void DocumentHeader(Buffer buffer) { }
		void mkd_renderer_doc_header(IntPtr ob, IntPtr opaque)
		{
			DocumentHeader(new Buffer(ob));
		}

		public virtual void DocumentFooter(Buffer buffer) { }
		void mkd_renderer_doc_footer(IntPtr obj, IntPtr opaque)
		{
			DocumentFooter(new Buffer(obj));
		}

		#endregion
	}
}

