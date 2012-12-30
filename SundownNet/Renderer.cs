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

		md_sharpcallbacks this_callbacks;

		md_sharpcallbacks base_callbacks;

		internal Renderer(bool initialize = false)
		{
			this_callbacks.blockcode  = mkd_renderer_blockcode;
			this_callbacks.blockquote = mkd_renderer_blockquote;
			this_callbacks.blockhtml  = mkd_renderer_blockhtml;
			this_callbacks.header     = mkd_renderer_header;
			this_callbacks.hrule      = mkd_renderer_hrule;
			this_callbacks.list       = mkd_renderer_list;
			this_callbacks.listitem   = mkd_renderer_listitem;
			this_callbacks.paragraph  = mkd_renderer_paragraph;
			this_callbacks.table      = mkd_renderer_table;
			this_callbacks.table_row  = mkd_renderer_table_row;
			this_callbacks.table_cell = mkd_renderer_table_cell;

			this_callbacks.autolink        = mkd_renderer_autolink;
			this_callbacks.codespan        = mkd_renderer_codespan;
			this_callbacks.double_emphasis = mkd_renderer_double_emphasis;
			this_callbacks.emphasis        = mkd_renderer_emphasis;
			this_callbacks.image           = mkd_renderer_image;
			this_callbacks.linebreak       = mkd_renderer_linebreak;
			this_callbacks.link            = mkd_renderer_link;
			this_callbacks.raw_html_tag    = mkd_renderer_raw_html_tag;
			this_callbacks.triple_emphasis = mkd_renderer_triple_emphasis;
			this_callbacks.strikethrough   = mkd_renderer_strikethrough;
			this_callbacks.superscript     = mkd_renderer_superscript;

			this_callbacks.entity      = mkd_renderer_entity;
			this_callbacks.normal_text = mkd_renderer_normal_text;

			this_callbacks.doc_header = mkd_renderer_doc_header;
			this_callbacks.doc_footer = mkd_renderer_doc_footer;

			if (initialize) {
				Initialize();
			}
		}

		public Renderer()
			: this(true)
		{
		}

		~Renderer()
		{
			if (callbacksgchandle.IsAllocated) {
				callbacksgchandle.Free();
			}
		}

		bool IsOverriden(string name)
		{
			return IsOverriden(name, typeof(Renderer));
		}

		protected bool IsOverriden(string name, Type baseType)
		{
			for (Type type = this.GetType(); type != baseType; type = type.BaseType) {
				if (IsOverriden(type, name)) {
					return true;
				}
			}
			return false;
		}

		protected bool IsOverriden(Type type, string name)
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
			if (IsOverriden("BlockCode"))  callbacks.blockcode  = Marshal.GetFunctionPointerForDelegate(this_callbacks.blockcode);
			if (IsOverriden("BlockQuote")) callbacks.blockquote = Marshal.GetFunctionPointerForDelegate(this_callbacks.blockquote);
			if (IsOverriden("BlockHtml"))  callbacks.blockhtml  = Marshal.GetFunctionPointerForDelegate(this_callbacks.blockhtml);
			if (IsOverriden("Header"))     callbacks.header     = Marshal.GetFunctionPointerForDelegate(this_callbacks.header);
			if (IsOverriden("HRule"))      callbacks.hrule      = Marshal.GetFunctionPointerForDelegate(this_callbacks.hrule);
			if (IsOverriden("List"))       callbacks.list       = Marshal.GetFunctionPointerForDelegate(this_callbacks.list);
			if (IsOverriden("ListItem"))   callbacks.listitem   = Marshal.GetFunctionPointerForDelegate(this_callbacks.listitem);
			if (IsOverriden("Paragraph"))  callbacks.paragraph  = Marshal.GetFunctionPointerForDelegate(this_callbacks.paragraph);
			if (IsOverriden("Table"))      callbacks.table      = Marshal.GetFunctionPointerForDelegate(this_callbacks.table);
			if (IsOverriden("TableRow"))   callbacks.table_row  = Marshal.GetFunctionPointerForDelegate(this_callbacks.table_row);
			if (IsOverriden("TableCell"))  callbacks.table_cell = Marshal.GetFunctionPointerForDelegate(this_callbacks.table_cell);

			if (IsOverriden("Autolink"))       callbacks.autolink        = Marshal.GetFunctionPointerForDelegate(this_callbacks.autolink);
			if (IsOverriden("Codespan"))       callbacks.codespan        = Marshal.GetFunctionPointerForDelegate(this_callbacks.codespan);
			if (IsOverriden("DoubleEmphasis")) callbacks.double_emphasis = Marshal.GetFunctionPointerForDelegate(this_callbacks.double_emphasis);
			if (IsOverriden("Emphasis"))       callbacks.emphasis        = Marshal.GetFunctionPointerForDelegate(this_callbacks.emphasis);
			if (IsOverriden("Image"))          callbacks.image           = Marshal.GetFunctionPointerForDelegate(this_callbacks.image);
			if (IsOverriden("Linebreak"))      callbacks.linebreak       = Marshal.GetFunctionPointerForDelegate(this_callbacks.linebreak);
			if (IsOverriden("Link"))           callbacks.link            = Marshal.GetFunctionPointerForDelegate(this_callbacks.link);
			if (IsOverriden("RawHtmlTag"))     callbacks.raw_html_tag    = Marshal.GetFunctionPointerForDelegate(this_callbacks.raw_html_tag);
			if (IsOverriden("TripleEmphasis")) callbacks.triple_emphasis = Marshal.GetFunctionPointerForDelegate(this_callbacks.triple_emphasis);
			if (IsOverriden("Strikethrough"))  callbacks.strikethrough   = Marshal.GetFunctionPointerForDelegate(this_callbacks.strikethrough);
			if (IsOverriden("SuperScript"))    callbacks.superscript     = Marshal.GetFunctionPointerForDelegate(this_callbacks.superscript);

			if (IsOverriden("Entity"))     callbacks.entity      = Marshal.GetFunctionPointerForDelegate(this_callbacks.entity);
			if (IsOverriden("NormalText")) callbacks.normal_text = Marshal.GetFunctionPointerForDelegate(this_callbacks.normal_text);

			if (IsOverriden("DocumentHeader")) callbacks.doc_header = Marshal.GetFunctionPointerForDelegate(this_callbacks.doc_header);
			if (IsOverriden("DocumentFooter")) callbacks.doc_footer = Marshal.GetFunctionPointerForDelegate(this_callbacks.doc_footer);

			callbacksgchandle = GCHandle.Alloc(callbacks, GCHandleType.Pinned);
		}

		#region block level

		protected virtual void BlockCode(Buffer ob, Buffer text, Buffer language)
		{
			if (base_callbacks.blockcode != null) {
				base_callbacks.blockcode(ob.NativeHandle, text.NativeHandle, language.NativeHandle, opaque);
			}
		}
		void mkd_renderer_blockcode(IntPtr ob, IntPtr text, IntPtr lang, IntPtr opaque)
		{
			BlockCode(Buffer.From(ob), Buffer.From(text), Buffer.From(lang));
		}

		protected virtual void BlockQuote(Buffer ob, Buffer text)
		{
			if (base_callbacks.blockquote != null) {
				base_callbacks.blockquote(ob.NativeHandle, text.NativeHandle, opaque);
			}
		}
		void mkd_renderer_blockquote(IntPtr ob, IntPtr text, IntPtr opaque)
		{
			BlockQuote(Buffer.From(ob), Buffer.From(text));
		}

		protected virtual void BlockHtml(Buffer ob, Buffer text)
		{
			if (base_callbacks.blockhtml != null) {
				base_callbacks.blockhtml(ob.NativeHandle, text.NativeHandle, opaque);
			}
		}
		void mkd_renderer_blockhtml(IntPtr ob, IntPtr text, IntPtr opaque)
		{
			BlockHtml(Buffer.From(ob), Buffer.From(text));
		}

		protected virtual void Header(Buffer ob, Buffer text, int level)
		{
			if (base_callbacks.header != null) {
				base_callbacks.header(ob.NativeHandle, text.NativeHandle, level, opaque);
			}
		}
		void mkd_renderer_header(IntPtr ob, IntPtr text, int level, IntPtr opaque)
		{
			Header(Buffer.From(ob), Buffer.From(text), level);
		}

		protected virtual void HRule(Buffer ob)
		{
			if (base_callbacks.hrule != null) {
				base_callbacks.hrule(ob.NativeHandle, opaque);
			}
		}
		void mkd_renderer_hrule(IntPtr ob, IntPtr opaque)
		{
			HRule(Buffer.From(ob));
		}

		protected virtual void List(Buffer ob, Buffer text, int flags)
		{
			if (base_callbacks.list != null) {
				base_callbacks.list(ob.NativeHandle, text.NativeHandle, flags, opaque);
			}
		}
		void mkd_renderer_list(IntPtr ob, IntPtr text, int flags, IntPtr opaque)
		{
			List(Buffer.From(ob), Buffer.From(text), flags);
		}

		protected virtual void ListItem(Buffer ob, Buffer text, int flags)
		{
			if (base_callbacks.listitem != null) {
				base_callbacks.listitem(ob.NativeHandle, text.NativeHandle, flags, opaque);
			}
		}
		void mkd_renderer_listitem(IntPtr ob, IntPtr text, int flags, IntPtr opaque)
		{
			ListItem(Buffer.From(ob), Buffer.From(text), flags);
		}

		protected virtual void Paragraph(Buffer ob, Buffer text)
		{
			if (base_callbacks.paragraph != null) {
				base_callbacks.paragraph(ob.NativeHandle, text.NativeHandle, opaque);
			}
		}
		void mkd_renderer_paragraph(IntPtr ob, IntPtr text, IntPtr opaque)
		{
			Paragraph(Buffer.From(ob), Buffer.From(text));
		}

		protected virtual void Table(Buffer ob, Buffer header, Buffer body)
		{
			if (base_callbacks.table != null) {
				base_callbacks.table(ob.NativeHandle, header.NativeHandle, body.NativeHandle, opaque);
			}
		}
		void mkd_renderer_table(IntPtr ob, IntPtr header, IntPtr body, IntPtr opaque)
		{
			Table(Buffer.From(ob), Buffer.From(header), Buffer.From(body));
		}

		protected virtual void TableRow(Buffer ob, Buffer text)
		{
			if (base_callbacks.table_row != null) {
				base_callbacks.table_row(ob.NativeHandle, text.NativeHandle, opaque);
			}
		}
		void mkd_renderer_table_row(IntPtr ob, IntPtr text, IntPtr opaque)
		{
			TableRow(Buffer.From(ob), Buffer.From(text));
		}

		protected virtual void TableCell(Buffer ob, Buffer text, TableFlags flags)
		{
			if (base_callbacks.table_cell != null) {
				base_callbacks.table_cell(ob.NativeHandle, text.NativeHandle, (int)flags, opaque);
			}
		}
		void mkd_renderer_table_cell(IntPtr ob, IntPtr text, int flags, IntPtr opaque)
		{
			TableCell(Buffer.From(ob), Buffer.From(text), (TableFlags)flags);
		}

		#endregion

		#region span level

		protected virtual bool Autolink(Buffer ob, Buffer link, AutolinkType type)
		{
			if (base_callbacks.autolink != null) {
				return base_callbacks.autolink(ob.NativeHandle, link.NativeHandle, (int)type, opaque) != 0;
			}
			return false;
		}
		int mkd_renderer_autolink(IntPtr ob, IntPtr link, int type, IntPtr opaque)
		{
			return Autolink(Buffer.From(ob), Buffer.From(link), (AutolinkType)type) ? 1 : 0;
		}

		protected virtual bool Codespan(Buffer ob, Buffer text)
		{
			if (base_callbacks.codespan != null) {
				return base_callbacks.codespan(ob.NativeHandle, text.NativeHandle, opaque) != 0;
			}
			return false;
		}
		int mkd_renderer_codespan(IntPtr ob, IntPtr text, IntPtr opaque)
		{
			return Codespan(Buffer.From(ob), Buffer.From(text)) ? 1 : 0;
		}

		protected virtual bool DoubleEmphasis(Buffer ob, Buffer text)
		{
			if (base_callbacks.double_emphasis != null) {
				return base_callbacks.double_emphasis(ob.NativeHandle, text.NativeHandle, opaque) != 0;
			}
			return false;
		}
		int mkd_renderer_double_emphasis(IntPtr ob, IntPtr text, IntPtr opaque)
		{
			return DoubleEmphasis(Buffer.From(ob), Buffer.From(text)) ? 1 : 0;
		}

		protected virtual bool Emphasis(Buffer ob, Buffer text)
		{
			if (base_callbacks.emphasis != null) {
				return base_callbacks.emphasis(ob.NativeHandle, text.NativeHandle, opaque) != 0;
			}
			return false;
		}
		int mkd_renderer_emphasis(IntPtr ob, IntPtr text, IntPtr opaque)
		{
			return Emphasis(Buffer.From(ob), Buffer.From(text)) ? 1 : 0;
		}

		protected virtual bool Image(Buffer ob, Buffer link, Buffer title, Buffer alt)
		{
			if (base_callbacks.image != null) {
				return base_callbacks.image(ob.NativeHandle, link.NativeHandle, title.NativeHandle, alt.NativeHandle, opaque) != 0;
			}
			return false;
		}
		int mkd_renderer_image(IntPtr ob, IntPtr link, IntPtr title, IntPtr alt, IntPtr opaque)
		{
			return Image(Buffer.From(ob), Buffer.From(link), Buffer.From(title), Buffer.From(alt)) ? 1 : 0;
		}

		protected virtual bool Linebreak(Buffer ob)
		{
			if (base_callbacks.linebreak != null) {
				return base_callbacks.linebreak(ob.NativeHandle, opaque) != 0;
			}
			return false;
		}
		int mkd_renderer_linebreak(IntPtr ob, IntPtr opaque)
		{
			return Linebreak(Buffer.From(ob)) ? 1 : 0;
		}

		protected virtual bool Link(Buffer ob, Buffer link, Buffer title, Buffer content)
		{
			if (base_callbacks.link != null) {
				return base_callbacks.link(ob.NativeHandle, link.NativeHandle, title.NativeHandle, content.NativeHandle, opaque) != 0;
			}
			return false;
		}
		int mkd_renderer_link(IntPtr ob, IntPtr link, IntPtr title, IntPtr content, IntPtr opaque)
		{
			return Link(Buffer.From(ob), Buffer.From(link), Buffer.From(title), Buffer.From(content)) ? 1 : 0;
		}

		protected virtual bool RawHtmlTag(Buffer ob, Buffer tag)
		{
			if (base_callbacks.raw_html_tag != null) {
				return base_callbacks.raw_html_tag(ob.NativeHandle, tag.NativeHandle, opaque) != 0;
			}
			return false;
		}
		int mkd_renderer_raw_html_tag(IntPtr ob, IntPtr tag, IntPtr opaque)
		{
			return RawHtmlTag(Buffer.From(ob), Buffer.From(tag)) ? 1 : 0;
		}

		protected virtual bool TripleEmphasis(Buffer ob, Buffer text)
		{
			if (base_callbacks.triple_emphasis != null) {
				return base_callbacks.triple_emphasis(ob.NativeHandle, text.NativeHandle, opaque) != 0;
			}
			return false;
		}
		int mkd_renderer_triple_emphasis(IntPtr buf, IntPtr text, IntPtr opaque)
		{
			return TripleEmphasis(Buffer.From(buf), Buffer.From(text)) ? 1 : 0;
		}

		protected virtual bool Strikethrough(Buffer ob, Buffer text)
		{
			if (base_callbacks.strikethrough != null) {
				return base_callbacks.strikethrough(ob.NativeHandle, text.NativeHandle, opaque) != 0;
			}
			return false;
		}
		int mkd_renderer_strikethrough(IntPtr ob, IntPtr text, IntPtr opaque)
		{
			return Strikethrough(Buffer.From(ob), Buffer.From(text)) ? 1 : 0;
		}

		protected virtual bool SuperScript(Buffer ob, Buffer text)
		{
			if (base_callbacks.superscript != null) {
				return base_callbacks.superscript(ob.NativeHandle, text.NativeHandle, opaque) != 0;
			}
			return false;
		}
		int mkd_renderer_superscript(IntPtr ob, IntPtr buffer, IntPtr opaque)
		{
			return SuperScript(Buffer.From(ob), Buffer.From(buffer)) ? 1 : 0;
		}

		#endregion

		#region low level callbacks

		public virtual void NormalText(Buffer ob, Buffer text) {
			if (base_callbacks.normal_text != null) {
				base_callbacks.normal_text(ob.NativeHandle, text.NativeHandle, opaque);
			}
		}
		void mkd_renderer_normal_text(IntPtr ob, IntPtr text, IntPtr opaque)
		{
			NormalText(Buffer.From(ob), Buffer.From(text));
		}

		public virtual void Entity(Buffer ob, Buffer entity) {
			if (base_callbacks.entity != null) {
				base_callbacks.entity(ob.NativeHandle, entity.NativeHandle, opaque);
			}
		}
		void mkd_renderer_entity(IntPtr ob, IntPtr entity, IntPtr opaque)
		{
			Entity(Buffer.From(ob), Buffer.From(entity));
		}

		#endregion

		#region header and footer

		public virtual void DocumentHeader(Buffer buffer)
		{
			if (base_callbacks.doc_header != null) {
				base_callbacks.doc_header(buffer.NativeHandle, opaque);
			}
		}
		void mkd_renderer_doc_header(IntPtr ob, IntPtr opaque)
		{
			DocumentHeader(Buffer.From(ob));
		}

		public virtual void DocumentFooter(Buffer buffer)
		{
			if (base_callbacks.doc_footer != null) {
				base_callbacks.doc_footer(buffer.NativeHandle, opaque);
			}
		}
		void mkd_renderer_doc_footer(IntPtr obj, IntPtr opaque)
		{
			DocumentFooter(Buffer.From(obj));
		}

		#endregion
	}
}

