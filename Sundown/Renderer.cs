using System;
using System.IO;
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
	internal struct mkd_renderer
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

		public IntPtr opaque;
	}

	public abstract class Renderer
	{
		public static Version LibraryVersion {
			get {
				int major, minor, revision;
				sd_version(out major, out minor, out revision);
				return new Version(major, minor, revision);
			}
		}

		public abstract void Markdown(Buffer outBuffer, Buffer inBuffer);

		internal void Markdown(Buffer outBuffer, Buffer inBuffer, ref mkd_renderer renderer)
		{
			uint i = 0;
			sd_markdown(outBuffer.buf, inBuffer.buf, ref renderer, ~i);
		}

		[DllImport("sundown")]
		internal static extern void sd_markdown(IntPtr outBuffer, IntPtr inBuffer, ref mkd_renderer renderer, uint extensions);

		[DllImport("sundown")]
		internal static extern void sd_version(out int major, out int minor, out int revision);
	}

	// TODO: implement this
	public abstract class CustomRenderer : Renderer
	{
		mkd_renderer renderer;

		public CustomRenderer()
		{
			renderer.blockcode  = mkd_renderer_blockcode;
			renderer.blockquote = mkd_renderer_blockquote;
			renderer.blockhtml  = mkd_renderer_blockhtml;
			renderer.header     = mkd_renderer_header;
			renderer.hrule      = mkd_renderer_hrule;
			renderer.list       = mkd_renderer_list;
			renderer.listitem   = mkd_renderer_listitem;
			renderer.paragrah   = mkd_renderer_paragraph;
			renderer.table      = mkd_renderer_table;
			renderer.table_row  = mkd_renderer_table_row;
			renderer.table_cell = mkd_renderer_table_cell;

			renderer.autolink        = mkd_renderer_autolink;
			renderer.codespan        = mkd_renderer_codespan;
			renderer.double_emphasis = mkd_renderer_double_emphasis;
			renderer.emphasis        = mkd_renderer_emphasis;
			renderer.image           = mkd_renderer_image;
			renderer.linebreak       = mkd_renderer_linebreak;
			renderer.link            = mkd_renderer_link;
			renderer.raw_html_tag    = mkd_renderer_raw_html_tag;
			renderer.triple_emphasis = mkd_renderer_triple_emphasis;
			renderer.strikethrough   = mkd_renderer_strikethrough;
			renderer.superscript     = mkd_renderer_superscript;

			renderer.entity      = mkd_renderer_entity;
			renderer.normal_text = mkd_renderer_normal_text;

			renderer.doc_header = mkd_renderer_doc_header;
			renderer.doc_footer = mkd_renderer_doc_footer;
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

		public override void Markdown(Buffer outBuffer, Buffer inBuffer)
		{
			Markdown(outBuffer, inBuffer, ref renderer);
		}
	}

	public class HtmlRenderer : Renderer, IDisposable
	{
		mkd_renderer renderer;

		public HtmlRenderer()
		{
			renderer = new mkd_renderer();
			sdhtml_renderer(ref renderer, 0, IntPtr.Zero);
		}

		public override void Markdown(Buffer outBuffer, Buffer inBuffer)
		{
			Markdown(outBuffer, inBuffer, ref renderer);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public virtual void Dispose(bool dispose)
		{
			sdhtml_free_renderer(ref renderer);
		}

		[DllImport("sundown")]
		internal static extern void sdhtml_renderer(ref mkd_renderer renderer, int size, IntPtr ptr);

		[DllImport("sundown")]
		internal static extern void sdhtml_free_renderer(ref mkd_renderer renderer);

	}
}

