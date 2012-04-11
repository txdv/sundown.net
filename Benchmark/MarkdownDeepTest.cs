using System;
using System.Text;
using MarkdownDeep;

namespace Benchmark
{
	public class MarkdownDeepTest : Test
	{
		Markdown md;
		public MarkdownDeepTest()
		{
			md = new Markdown();
			Name = "MarkdownDeep";
		}

		public override string Transform(string str)
		{
			return md.Transform(str);
		}

		public override byte[] Transform(byte[] arr)
		{
			return Encoding.ASCII.GetBytes(md.Transform(Encoding.ASCII.GetString(arr)));
		}
	}
}

