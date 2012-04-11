using System;
using Sundown;

namespace Benchmark
{
	public class SundownNetTest : Test
	{
		Markdown md;
		public SundownNetTest()
		{
			md = new Markdown(new HtmlRenderer());
			Name = "SundownNet";
		}

		public override string Transform(string str)
		{
			var buf = new Sundown.Buffer();
			md.Render(buf, str);
			return buf.ToString();
		}

		public override byte[] Transform(byte[] arr)
		{
			var buf = new Sundown.Buffer();
			md.Render(buf, arr);
			return buf.GetBytes();
		}
	}
}

