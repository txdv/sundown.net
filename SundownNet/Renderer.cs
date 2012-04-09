using System;
using System.Runtime.InteropServices;

namespace Sundown
{

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

	public abstract class Renderer
	{
		internal md_callbacks callbacks = new md_callbacks();
		internal GCHandle callbacksgchandle;
		internal IntPtr opaque;

		~Renderer()
		{
			if (callbacksgchandle.IsAllocated) {
				callbacksgchandle.Free();
			}
		}
	}
}

