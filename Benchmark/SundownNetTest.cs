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
			return md.Transform(str);
		}

		public override byte[] Transform(byte[] arr)
		{
			return md.Transform(arr);
		}
	}
}

