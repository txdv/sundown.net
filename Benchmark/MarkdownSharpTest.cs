using System;
using System.Text;
using MarkdownSharp;

namespace Benchmark
{
	public class MarkdownSharpTest : Test
	{
		Markdown md;
		public MarkdownSharpTest()
		{
			md = new Markdown();
			Name = "MarkdownSharp";
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

