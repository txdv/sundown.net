using System;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Sundown
{
	public enum AutolinkType : uint
	{
		NotAutolink,
		Normal,
		Email
	}

	public enum TableFlags : uint
	{
		AlignLeft,
		AligntRight,
		AlignCenter,
		AlignHeader
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct md_callbacks
	{
		public IntPtr blockcode;
		public IntPtr blockquote;
		public IntPtr blockhtml;
		public IntPtr header;
		public IntPtr hrule;
		public IntPtr list;
		public IntPtr listitem;
		public IntPtr paragraph;
		public IntPtr table;
		public IntPtr table_row;
		public IntPtr table_cell;


		public IntPtr autolink;
		public IntPtr codespan;
		public IntPtr double_emphasis;
		public IntPtr emphasis;
		public IntPtr image;
		public IntPtr linebreak;
		public IntPtr link;
		public IntPtr raw_html_tag;
		public IntPtr triple_emphasis;
		public IntPtr strikethrough;
		public IntPtr superscript;

		public IntPtr entity;
		public IntPtr normal_text;

		public IntPtr doc_header;
		public IntPtr doc_footer;
	}

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

	public class Renderer
	{
		internal md_callbacks callbacks = new md_callbacks();
		internal GCHandle callbacksgchandle;
		internal IntPtr opaque;

		mkd_renderer_blockcode  blockcode;
		mkd_renderer_blockquote blockquote;
		mkd_renderer_blockhtml  blockhtml;
		mkd_renderer_header     header;
		mkd_renderer_hrule      hrule;
		mkd_renderer_list       list;
		mkd_renderer_listitem   listitem;
		mkd_renderer_paragraph  paragraph;
		mkd_renderer_table      table;
		mkd_renderer_table_row  table_row;
		mkd_renderer_table_cell table_cell;

		mkd_renderer_autolink        autolink;
		mkd_renderer_codespan        codespan;
		mkd_renderer_double_emphasis double_emphasis;
		mkd_renderer_emphasis        emphasis;
		mkd_renderer_image           image;
		mkd_renderer_linebreak       linebreak;
		mkd_renderer_link            link;
		mkd_renderer_raw_html_tag    raw_html_tag;
		mkd_renderer_triple_emphasis triple_emphasis;
		mkd_renderer_strikethrough   strikethrough;
		mkd_renderer_superscript     superscript;

		mkd_renderer_entity entity;
		mkd_renderer_normal_text normal_text;

		mkd_renderer_doc_header doc_header;
		mkd_renderer_doc_footer doc_footer;

		public Renderer()
		{
			blockcode  = mkd_renderer_blockcode;
			blockquote = mkd_renderer_blockquote;
			blockhtml  = mkd_renderer_blockhtml;
			header     = mkd_renderer_header;
			hrule      = mkd_renderer_hrule;
			list       = mkd_renderer_list;
			listitem   = mkd_renderer_listitem;
			paragraph  = mkd_renderer_paragraph;
			table      = mkd_renderer_table;
			table_row  = mkd_renderer_table_row;
			table_cell = mkd_renderer_table_cell;

			autolink        = mkd_renderer_autolink;
			codespan        = mkd_renderer_codespan;
			double_emphasis = mkd_renderer_double_emphasis;
			emphasis        = mkd_renderer_emphasis;
			image           = mkd_renderer_image;
			linebreak       = mkd_renderer_linebreak;
			link            = mkd_renderer_link;
			raw_html_tag    = mkd_renderer_raw_html_tag;
			triple_emphasis = mkd_renderer_triple_emphasis;
			strikethrough   = mkd_renderer_strikethrough;
			superscript     = mkd_renderer_superscript;

			entity      = mkd_renderer_entity;
			normal_text = mkd_renderer_normal_text;

			doc_header = mkd_renderer_doc_header;
			doc_footer = mkd_renderer_doc_footer;

			Initialize();
		}

		~Renderer()
		{
			if (callbacksgchandle.IsAllocated) {
				callbacksgchandle.Free();
			}
		}

		bool IsOverriden(string name)
		{
			for (Type type = this.GetType(); type != typeof(Renderer); type = type.BaseType) {
				if (IsOverriden(type, name)) {
					return true;
				}
			}
			return false;
		}

		bool IsOverriden(Type type, string name)
		{
			return type.GetMethod(name, BindingFlags.NonPublic|BindingFlags.Instance|BindingFlags.DeclaredOnly) != null;
		}

		protected virtual void Initialize()
		{
			if (IsOverriden("BlockCode"))  callbacks.blockcode  = Marshal.GetFunctionPointerForDelegate(blockcode);
			if (IsOverriden("BlockQuote")) callbacks.blockquote = Marshal.GetFunctionPointerForDelegate(blockquote);
			if (IsOverriden("BlockHtml"))  callbacks.blockhtml  = Marshal.GetFunctionPointerForDelegate(blockhtml);
			if (IsOverriden("Header"))     callbacks.header     = Marshal.GetFunctionPointerForDelegate(header);
			if (IsOverriden("HRule"))      callbacks.hrule      = Marshal.GetFunctionPointerForDelegate(hrule);
			if (IsOverriden("List"))       callbacks.list       = Marshal.GetFunctionPointerForDelegate(list);
			if (IsOverriden("ListItem"))   callbacks.listitem   = Marshal.GetFunctionPointerForDelegate(listitem);
			if (IsOverriden("Paragraph"))  callbacks.paragraph  = Marshal.GetFunctionPointerForDelegate(paragraph);
			if (IsOverriden("Table"))      callbacks.table      = Marshal.GetFunctionPointerForDelegate(table);
			if (IsOverriden("TableRow"))   callbacks.table_row  = Marshal.GetFunctionPointerForDelegate(table_row);
			if (IsOverriden("TableCell"))  callbacks.table_cell = Marshal.GetFunctionPointerForDelegate(table_cell);

			if (IsOverriden("Autolink"))       callbacks.autolink        = Marshal.GetFunctionPointerForDelegate(autolink);
			if (IsOverriden("Codespan"))       callbacks.codespan        = Marshal.GetFunctionPointerForDelegate(codespan);
			if (IsOverriden("DoubleEmphasis")) callbacks.double_emphasis = Marshal.GetFunctionPointerForDelegate(double_emphasis);
			if (IsOverriden("Emphasis"))       callbacks.emphasis        = Marshal.GetFunctionPointerForDelegate(emphasis);
			if (IsOverriden("Image"))          callbacks.image           = Marshal.GetFunctionPointerForDelegate(image);
			if (IsOverriden("Linebreak"))      callbacks.linebreak       = Marshal.GetFunctionPointerForDelegate(linebreak);
			if (IsOverriden("Link"))           callbacks.link            = Marshal.GetFunctionPointerForDelegate(link);
			if (IsOverriden("RawHtmlTag"))     callbacks.raw_html_tag    = Marshal.GetFunctionPointerForDelegate(raw_html_tag);
			if (IsOverriden("TripleEmphasis")) callbacks.triple_emphasis = Marshal.GetFunctionPointerForDelegate(triple_emphasis);
			if (IsOverriden("Strikethrough"))  callbacks.strikethrough   = Marshal.GetFunctionPointerForDelegate(strikethrough);
			if (IsOverriden("SuperScript"))    callbacks.superscript     = Marshal.GetFunctionPointerForDelegate(superscript);

			if (IsOverriden("Entity"))     callbacks.entity      = Marshal.GetFunctionPointerForDelegate(entity);
			if (IsOverriden("NormalText")) callbacks.normal_text = Marshal.GetFunctionPointerForDelegate(normal_text);

			if (IsOverriden("DocumentHeader")) callbacks.doc_header = Marshal.GetFunctionPointerForDelegate(doc_header);
			if (IsOverriden("DocumentFooter")) callbacks.doc_footer = Marshal.GetFunctionPointerForDelegate(doc_footer);

			callbacksgchandle = GCHandle.Alloc(callbacks, GCHandleType.Pinned);
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

		protected virtual void TableCell(Buffer ob, Buffer text, TableFlags flags) { }
		void mkd_renderer_table_cell(IntPtr ob, IntPtr text, int flags, IntPtr opaque)
		{
			TableCell(new Buffer(ob), new Buffer(text), (TableFlags)flags);
		}

		#endregion

		#region span level

		protected virtual bool Autolink(Buffer ob, Buffer link, AutolinkType type) { return false; }
		int mkd_renderer_autolink(IntPtr ob, IntPtr link, int type, IntPtr opaque)
		{
			return Autolink(new Buffer(ob), new Buffer(link), (AutolinkType)type) ? 1 : 0;
		}

		protected virtual bool Codespan(Buffer ob, Buffer text) { return false; }
		int mkd_renderer_codespan(IntPtr ob, IntPtr text, IntPtr opaque)
		{
			return Codespan(new Buffer(ob), new Buffer(text)) ? 1 : 0;
		}

		protected virtual bool DoubleEmphasis(Buffer ob, Buffer text) { return false; }
		int mkd_renderer_double_emphasis(IntPtr ob, IntPtr text, IntPtr opaque)
		{
			return DoubleEmphasis(new Buffer(ob), new Buffer(text)) ? 1 : 0;
		}

		protected virtual bool Emphasis(Buffer ob, Buffer text) { return false; }
		int mkd_renderer_emphasis(IntPtr ob, IntPtr text, IntPtr opaque)
		{
			return Emphasis(new Buffer(ob), new Buffer(text)) ? 1 : 0;
		}

		protected virtual bool Image(Buffer ob, Buffer link, Buffer title, Buffer alt) { return false; }
		int mkd_renderer_image(IntPtr ob, IntPtr link, IntPtr title, IntPtr alt, IntPtr opaque)
		{
			return Image(new Buffer(ob), new Buffer(link), new Buffer(title), new Buffer(alt)) ? 1 : 0;
		}

		protected virtual bool Linebreak(Buffer ob) { return false; }
		int mkd_renderer_linebreak(IntPtr ob, IntPtr opaque)
		{
			return Linebreak(new Buffer(ob)) ? 1 : 0;
		}

		protected virtual bool Link(Buffer ob, Buffer link, Buffer title, Buffer content) { return false; }
		int mkd_renderer_link(IntPtr ob, IntPtr link, IntPtr title, IntPtr content, IntPtr opaque)
		{
			return Link(new Buffer(ob), new Buffer(link), new Buffer(title), new Buffer(content)) ? 1 : 0;
		}

		protected virtual bool RawHtmlTag(Buffer ob, Buffer tag) { return false; }
		int mkd_renderer_raw_html_tag(IntPtr ob, IntPtr tag, IntPtr opaque)
		{
			return RawHtmlTag(new Buffer(ob), new Buffer(tag)) ? 1 : 0;
		}

		protected virtual bool TripleEmphasis(Buffer buf, Buffer text) { return false; }
		int mkd_renderer_triple_emphasis(IntPtr buf, IntPtr text, IntPtr opaque)
		{
			return TripleEmphasis(new Buffer(buf), new Buffer(text)) ? 1 : 0;
		}

		protected virtual bool Strikethrough(Buffer buf, Buffer text) { return false; }
		int mkd_renderer_strikethrough(IntPtr buf, IntPtr text, IntPtr opaque)
		{
			return Strikethrough(new Buffer(buf), new Buffer(text)) ? 1 : 0;
		}

		protected virtual bool SuperScript(Buffer buf, Buffer text) { return false; }
		int mkd_renderer_superscript(IntPtr buf, IntPtr buffer, IntPtr opaque)
		{
			return SuperScript(new Buffer(buf), new Buffer(buffer)) ? 1 : 0;
		}

		#endregion

		#region low level callbacks

		public virtual void NormalText(Buffer ob, Buffer text) { ob.Put(text); }
		void mkd_renderer_normal_text(IntPtr ob, IntPtr text, IntPtr opaque)
		{
			NormalText(new Buffer(ob), new Buffer(text));
		}

		public virtual void Entity(Buffer ob, Buffer entity) { ob.Put(entity); }
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

