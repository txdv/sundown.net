using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using NDesk.Options;

namespace Benchmark
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			int n = 1000;
			bool help = false;
			OptionSet options = new OptionSet()
				.Add("i|iterations=", "number of iteration, default 1000",
				    (int iterations) => n = Math.Max(iterations, 1))
				.Add("h|?|help", "show help", (_) => help = true)
					;

			var files = options.Parse(args);
			if (help) {
				Console.WriteLine("benchmark MarkdownSharp, MarkdownDeep and SundownNet on specific files!");
				options.WriteOptionDescriptions(Console.Out);
				return;
			}
			if (files.Count < 1) {
				Console.WriteLine ("You have to specify a file on which to benchmark");
				return;
			}

			var file = files[0];
			string text;
			byte[] textBytes;
			using (var sr = new StreamReader(File.OpenRead(file))) {
				text = sr.ReadToEnd();
				textBytes = Encoding.ASCII.GetBytes(text);
			}

			var tests = new Test[] {
				new SundownNetTest(),
				new MarkdownDeepTest(),
				new MarkdownSharpTest(),
			};

			foreach (var test in tests) {
				Console.WriteLine("Starting {0}", test.Name);
				test.Benchmark(n, text, textBytes);
				Console.WriteLine("Ending {0}", test.Name);
			}

			Test.Normalize(tests);

			foreach (var test in tests) {
				Console.WriteLine(test);
			}

		}
	}
}
