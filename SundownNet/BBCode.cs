using System;
using System.Collections.Generic;
using System.IO;

namespace Sundown
{
	public class BBCodeOptions
	{
		public BBCodeOptions()
		{
			DefaultHeaderSize = 10;
			HeaderSizes = new Dictionary<int, int>() {
				{ 1, 16 },
				{ 2, 14 },
				{ 3, 12 },
			};
		}

		public int DefaultHeaderSize { get; set; }
		public Dictionary<int, int> HeaderSizes { get; set; }

		internal int GetSize(int level)
		{
			int val;
			if (HeaderSizes != null && HeaderSizes.TryGetValue(level, out val)) {
				return val;
			} else {
				return DefaultHeaderSize;
			}
		}
	}

	public class BBCodeRenderer : Renderer
	{
		BBCodeOptions options;

		public BBCodeRenderer()
			: this(new BBCodeOptions())
		{
		}

		public BBCodeRenderer(BBCodeOptions options)
		{
			this.options = options;
		}

		protected override void Paragraph(Buffer ob, Buffer text)
		{
			ob.Put("\n");
			ob.Put(text);
			ob.Put("\n");
			ob.Put("\n");
		}

		protected override void Header(Buffer ob, Buffer text, int level)
		{
			ob.Put("[size={0}pt]{1}[/size]\n", options.GetSize(level), text);
		}

		protected override bool DoubleEmphasis(Buffer ob, Buffer text)
		{
			ob.Put("[b]{0}[/b]", text);
			return true;
		}

		protected override bool Emphasis(Buffer ob, Buffer text)
		{
			ob.Put("[u]{0}[/u]", text);
			return true;
		}

		protected override bool Link(Buffer ob, Buffer link, Buffer title, Buffer content)
		{
			ob.Put("[url={0}]{1}[/url]", link, content);
			return true;
		}

		protected override void List(Buffer ob, Buffer text, int flags)
		{
			ob.Put("\n[list type=decimal]\n");
			ob.Put(text);
			ob.Put("[/list]");
		}

		protected override void ListItem(Buffer ob, Buffer text, int flags)
		{
			ob.Put("[li]{0}[/li]\n", text);
		}

		protected override void BlockCode(Buffer ob, Buffer text, Buffer language)
		{
			ob.Put("\n[quote author={0}]{1}[/quote]\n", language, text);
		}
	}
}

