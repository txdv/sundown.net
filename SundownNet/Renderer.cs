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

	internal struct md_sharpcallbacks
	{
		public mkd_renderer_blockcode  blockcode;
		public mkd_renderer_blockquote blockquote;
		public mkd_renderer_blockhtml  blockhtml;
		public mkd_renderer_header     header;
		public mkd_renderer_hrule      hrule;
		public mkd_renderer_list       list;
		public mkd_renderer_listitem   listitem;
		public mkd_renderer_paragraph  paragraph;
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

		md_sharpcallbacks base_callbacks;

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
			// load up the base definitions
			if (callbacks.blockcode  != IntPtr.Zero) base_callbacks.blockcode  = Marshal.GetDelegateForFunctionPointer(callbacks.blockcode,  typeof(mkd_renderer_blockcode))  as mkd_renderer_blockcode;
			if (callbacks.blockquote != IntPtr.Zero) base_callbacks.blockquote = Marshal.GetDelegateForFunctionPointer(callbacks.blockquote, typeof(mkd_renderer_blockquote)) as mkd_renderer_blockquote;
			if (callbacks.blockhtml  != IntPtr.Zero) base_callbacks.blockhtml  = Marshal.GetDelegateForFunctionPointer(callbacks.blockhtml,  typeof(mkd_renderer_blockhtml))  as mkd_renderer_blockhtml;
			if (callbacks.header     != IntPtr.Zero) base_callbacks.header     = Marshal.GetDelegateForFunctionPointer(callbacks.header,     typeof(mkd_renderer_header))     as mkd_renderer_header;
			if (callbacks.hrule      != IntPtr.Zero) base_callbacks.hrule      = Marshal.GetDelegateForFunctionPointer(callbacks.hrule,      typeof(mkd_renderer_hrule))      as mkd_renderer_hrule;
			if (callbacks.list       != IntPtr.Zero) base_callbacks.list       = Marshal.GetDelegateForFunctionPointer(callbacks.list,       typeof(mkd_renderer_list))       as mkd_renderer_list;
			if (callbacks.listitem   != IntPtr.Zero) base_callbacks.listitem   = Marshal.GetDelegateForFunctionPointer(callbacks.listitem,   typeof(mkd_renderer_listitem))   as mkd_renderer_listitem;
			if (callbacks.paragraph  != IntPtr.Zero) base_callbacks.paragraph  = Marshal.GetDelegateForFunctionPointer(callbacks.paragraph,  typeof(mkd_renderer_paragraph))  as mkd_renderer_paragraph;
			if (callbacks.table      != IntPtr.Zero) base_callbacks.table      = Marshal.GetDelegateForFunctionPointer(callbacks.table,      typeof(mkd_renderer_table))      as mkd_renderer_table;
			if (callbacks.table_row  != IntPtr.Zero) base_callbacks.table_row  = Marshal.GetDelegateForFunctionPointer(callbacks.table_row,  typeof(mkd_renderer_table_row))  as mkd_renderer_table_row;
			if (callbacks.table_cell != IntPtr.Zero) base_callbacks.table_cell = Marshal.GetDelegateForFunctionPointer(callbacks.table_cell, typeof(mkd_renderer_table_cell)) as mkd_renderer_table_cell;

			if (callbacks.autolink        != IntPtr.Zero) base_callbacks.autolink        = Marshal.GetDelegateForFunctionPointer(callbacks.autolink,        typeof(mkd_renderer_autolink))        as mkd_renderer_autolink;
			if (callbacks.codespan        != IntPtr.Zero) base_callbacks.codespan        = Marshal.GetDelegateForFunctionPointer(callbacks.codespan,        typeof(mkd_renderer_codespan))        as mkd_renderer_codespan;
			if (callbacks.double_emphasis != IntPtr.Zero) base_callbacks.double_emphasis = Marshal.GetDelegateForFunctionPointer(callbacks.double_emphasis, typeof(mkd_renderer_double_emphasis)) as mkd_renderer_double_emphasis;
			if (callbacks.emphasis        != IntPtr.Zero) base_callbacks.emphasis        = Marshal.GetDelegateForFunctionPointer(callbacks.emphasis,        typeof(mkd_renderer_emphasis))        as mkd_renderer_emphasis;
			if (callbacks.image           != IntPtr.Zero) base_callbacks.image           = Marshal.GetDelegateForFunctionPointer(callbacks.image,           typeof(mkd_renderer_image))           as mkd_renderer_image;
			if (callbacks.linebreak       != IntPtr.Zero) base_callbacks.linebreak       = Marshal.GetDelegateForFunctionPointer(callbacks.linebreak,       typeof(mkd_renderer_linebreak))       as mkd_renderer_linebreak;
			if (callbacks.link            != IntPtr.Zero) base_callbacks.link            = Marshal.GetDelegateForFunctionPointer(callbacks.link,            typeof(mkd_renderer_link))            as mkd_renderer_link;
			if (callbacks.raw_html_tag    != IntPtr.Zero) base_callbacks.raw_html_tag    = Marshal.GetDelegateForFunctionPointer(callbacks.raw_html_tag,    typeof(mkd_renderer_raw_html_tag))    as mkd_renderer_raw_html_tag;
			if (callbacks.triple_emphasis != IntPtr.Zero) base_callbacks.triple_emphasis = Marshal.GetDelegateForFunctionPointer(callbacks.triple_emphasis, typeof(mkd_renderer_triple_emphasis)) as mkd_renderer_triple_emphasis;
			if (callbacks.strikethrough   != IntPtr.Zero) base_callbacks.strikethrough   = Marshal.GetDelegateForFunctionPointer(callbacks.strikethrough,   typeof(mkd_renderer_strikethrough))   as mkd_renderer_strikethrough;
			if (callbacks.superscript     != IntPtr.Zero) base_callbacks.superscript     = Marshal.GetDelegateForFunctionPointer(callbacks.superscript,     typeof(mkd_renderer_superscript))     as mkd_renderer_superscript;

			if (callbacks.entity != IntPtr.Zero) base_callbacks.entity = Marshal.GetDelegateForFunctionPointer(callbacks.entity, typeof(mkd_renderer_entity)) as mkd_renderer_entity;
			if (callbacks.normal_text != IntPtr.Zero) base_callbacks.normal_text = Marshal.GetDelegateForFunctionPointer(callbacks.normal_text, typeof(mkd_renderer_normal_text)) as mkd_renderer_normal_text;

			if (callbacks.doc_header != IntPtr.Zero) base_callbacks.doc_header = Marshal.GetDelegateForFunctionPointer(callbacks.doc_header, typeof(mkd_renderer_doc_header)) as mkd_renderer_doc_header;
			if (callbacks.doc_footer != IntPtr.Zero) base_callbacks.doc_footer = Marshal.GetDelegateForFunctionPointer(callbacks.doc_footer, typeof(mkd_renderer_doc_footer)) as mkd_renderer_doc_footer;

			// override if we have overriden functions
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

		protected virtual void BlockCode(Buffer ob, Buffer text, Buffer language)
		{
			if (base_callbacks.blockcode != null) {
				base_callbacks.blockcode(ob.buf, text.buf, language.buf, opaque);
			}
		}
		void mkd_renderer_blockcode(IntPtr ob, IntPtr text, IntPtr lang, IntPtr opaque)
		{
			BlockCode(new Buffer(ob), new Buffer(text), new Buffer(lang));
		}

		protected virtual void BlockQuote(Buffer ob, Buffer text)
		{
			if (base_callbacks.blockquote != null) {
				base_callbacks.blockquote(ob.buf, text.buf, opaque);
			}
		}
		void mkd_renderer_blockquote(IntPtr ob, IntPtr text, IntPtr opaque)
		{
			BlockQuote(new Buffer(ob), new Buffer(text));
		}

		protected virtual void BlockHtml(Buffer ob, Buffer text)
		{
			if (base_callbacks.blockhtml != null) {
				base_callbacks.blockhtml(ob.buf, text.buf, opaque);
			}
		}
		void mkd_renderer_blockhtml(IntPtr ob, IntPtr text, IntPtr opaque)
		{
			BlockHtml(new Buffer(ob), new Buffer(text));
		}

		protected virtual void Header(Buffer ob, Buffer text, int level)
		{
			if (base_callbacks.header != null) {
				base_callbacks.header(ob.buf, text.buf, level, opaque);
			}
		}
		void mkd_renderer_header(IntPtr ob, IntPtr text, int level, IntPtr opaque)
		{
			Header(new Buffer(ob), new Buffer(text), level);
		}

		protected virtual void HRule(Buffer ob)
		{
			if (base_callbacks.hrule != null) {
				base_callbacks.hrule(ob.buf, opaque);
			}
		}
		void mkd_renderer_hrule(IntPtr ob, IntPtr opaque)
		{
			HRule(new Buffer(ob));
		}

		protected virtual void List(Buffer ob, Buffer text, int flags)
		{
			if (base_callbacks.list != null) {
				base_callbacks.list(ob.buf, text.buf, flags, opaque);
			}
		}
		void mkd_renderer_list(IntPtr ob, IntPtr text, int flags, IntPtr opaque)
		{
			List(new Buffer(ob), new Buffer(text), flags);
		}

		protected virtual void ListItem(Buffer ob, Buffer text, int flags)
		{
			if (base_callbacks.listitem != null) {
				base_callbacks.listitem(ob.buf, text.buf, flags, opaque);
			}
		}
		void mkd_renderer_listitem(IntPtr ob, IntPtr text, int flags, IntPtr opaque)
		{
			ListItem(new Buffer(ob), new Buffer(text), flags);
		}

		protected virtual void Paragraph(Buffer ob, Buffer text)
		{
			if (base_callbacks.paragraph != null) {
				base_callbacks.paragraph(ob.buf, text.buf, opaque);
			}
		}
		void mkd_renderer_paragraph(IntPtr ob, IntPtr text, IntPtr opaque)
		{
			Paragraph(new Buffer(ob), new Buffer(text));
		}

		protected virtual void Table(Buffer ob, Buffer header, Buffer body)
		{
			if (base_callbacks.table != null) {
				base_callbacks.table(ob.buf, header.buf, body.buf, opaque);
			}
		}
		void mkd_renderer_table(IntPtr ob, IntPtr header, IntPtr body, IntPtr opaque)
		{
			Table(new Buffer(ob), new Buffer(header), new Buffer(body));
		}

		protected virtual void TableRow(Buffer ob, Buffer text)
		{
			if (base_callbacks.table_row != null) {
				base_callbacks.table_row(ob.buf, text.buf, opaque);
			}
		}
		void mkd_renderer_table_row(IntPtr ob, IntPtr text, IntPtr opaque)
		{
			TableRow(new Buffer(ob), new Buffer(text));
		}

		protected virtual void TableCell(Buffer ob, Buffer text, TableFlags flags)
		{
			if (base_callbacks.table_cell != null) {
				base_callbacks.table_cell(ob.buf, text.buf, (int)flags, opaque);
			}
		}
		void mkd_renderer_table_cell(IntPtr ob, IntPtr text, int flags, IntPtr opaque)
		{
			TableCell(new Buffer(ob), new Buffer(text), (TableFlags)flags);
		}

		#endregion

		#region span level

		protected virtual bool Autolink(Buffer ob, Buffer link, AutolinkType type)
		{
			if (base_callbacks.autolink != null) {
				return base_callbacks.autolink(ob.buf, link.buf, (int)type, opaque) != 0;
			}
			return false;
		}
		int mkd_renderer_autolink(IntPtr ob, IntPtr link, int type, IntPtr opaque)
		{
			return Autolink(new Buffer(ob), new Buffer(link), (AutolinkType)type) ? 1 : 0;
		}

		protected virtual bool Codespan(Buffer ob, Buffer text)
		{
			if (base_callbacks.codespan != null) {
				return base_callbacks.codespan(ob.buf, text.buf, opaque) != 0;
			}
			return false;
		}
		int mkd_renderer_codespan(IntPtr ob, IntPtr text, IntPtr opaque)
		{
			return Codespan(new Buffer(ob), new Buffer(text)) ? 1 : 0;
		}

		protected virtual bool DoubleEmphasis(Buffer ob, Buffer text)
		{
			if (base_callbacks.double_emphasis != null) {
				return base_callbacks.double_emphasis(ob.buf, text.buf, opaque) != 0;
			}
			return false;
		}
		int mkd_renderer_double_emphasis(IntPtr ob, IntPtr text, IntPtr opaque)
		{
			return DoubleEmphasis(new Buffer(ob), new Buffer(text)) ? 1 : 0;
		}

		protected virtual bool Emphasis(Buffer ob, Buffer text)
		{
			if (base_callbacks.emphasis != null) {
				return base_callbacks.emphasis(ob.buf, text.buf, opaque) != 0;
			}
			return false;
		}
		int mkd_renderer_emphasis(IntPtr ob, IntPtr text, IntPtr opaque)
		{
			return Emphasis(new Buffer(ob), new Buffer(text)) ? 1 : 0;
		}

		protected virtual bool Image(Buffer ob, Buffer link, Buffer title, Buffer alt)
		{
			if (base_callbacks.image != null) {
				return base_callbacks.image(ob.buf, link.buf, title.buf, alt.buf, opaque) != 0;
			}
			return false;
		}
		int mkd_renderer_image(IntPtr ob, IntPtr link, IntPtr title, IntPtr alt, IntPtr opaque)
		{
			return Image(new Buffer(ob), new Buffer(link), new Buffer(title), new Buffer(alt)) ? 1 : 0;
		}

		protected virtual bool Linebreak(Buffer ob)
		{
			if (base_callbacks.linebreak != null) {
				return base_callbacks.linebreak(ob.buf, opaque) != 0;
			}
			return false;
		}
		int mkd_renderer_linebreak(IntPtr ob, IntPtr opaque)
		{
			return Linebreak(new Buffer(ob)) ? 1 : 0;
		}

		protected virtual bool Link(Buffer ob, Buffer link, Buffer title, Buffer content)
		{
			if (base_callbacks.link != null) {
				return base_callbacks.link(ob.buf, link.buf, title.buf, content.buf, opaque) != 0;
			}
			return false;
		}
		int mkd_renderer_link(IntPtr ob, IntPtr link, IntPtr title, IntPtr content, IntPtr opaque)
		{
			return Link(new Buffer(ob), new Buffer(link), new Buffer(title), new Buffer(content)) ? 1 : 0;
		}

		protected virtual bool RawHtmlTag(Buffer ob, Buffer tag)
		{
			if (base_callbacks.raw_html_tag != null) {
				return base_callbacks.raw_html_tag(ob.buf, tag.buf, opaque) != 0;
			}
			return false;
		}
		int mkd_renderer_raw_html_tag(IntPtr ob, IntPtr tag, IntPtr opaque)
		{
			return RawHtmlTag(new Buffer(ob), new Buffer(tag)) ? 1 : 0;
		}

		protected virtual bool TripleEmphasis(Buffer ob, Buffer text)
		{
			if (base_callbacks.triple_emphasis != null) {
				return base_callbacks.triple_emphasis(ob.buf, text.buf, opaque) != 0;
			}
			return false;
		}
		int mkd_renderer_triple_emphasis(IntPtr buf, IntPtr text, IntPtr opaque)
		{
			return TripleEmphasis(new Buffer(buf), new Buffer(text)) ? 1 : 0;
		}

		protected virtual bool Strikethrough(Buffer ob, Buffer text)
		{
			if (base_callbacks.strikethrough != null) {
				return base_callbacks.strikethrough(ob.buf, text.buf, opaque) != 0;
			}
			return false;
		}
		int mkd_renderer_strikethrough(IntPtr ob, IntPtr text, IntPtr opaque)
		{
			return Strikethrough(new Buffer(ob), new Buffer(text)) ? 1 : 0;
		}

		protected virtual bool SuperScript(Buffer ob, Buffer text)
		{
			if (base_callbacks.superscript != null) {
				return base_callbacks.superscript(ob.buf, text.buf, opaque) != 0;
			}
			return false;
		}
		int mkd_renderer_superscript(IntPtr ob, IntPtr buffer, IntPtr opaque)
		{
			return SuperScript(new Buffer(ob), new Buffer(buffer)) ? 1 : 0;
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

		public virtual void DocumentHeader(Buffer buffer)
		{
		}
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

