using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Sundown.App
{
	class MainClass
	{
		static readonly int BufferSize = 1024;

		public static void Main(string[] args)
		{
			Method2(args);
		}

		static void Method1(string[] args)
		{
			if (args.Length < 1) {
				Console.WriteLine("You have to provide a file as an argument");
				return;
			}

			string inputfile = args[0];

			byte[] bytes = new byte[BufferSize];

			using (Buffer buffer = new Buffer(bytes.Length))
			{
				try {
					using (FileStream fs = File.OpenRead(inputfile))
					for (int size = 0; (size = fs.Read(bytes, 0, bytes.Length)) != 0;) {
						buffer.Grow(bytes.Length);
						buffer.Put(bytes, size);
					}
				} catch (Exception exception) {
					Console.WriteLine("Unable to open input file {0: {1}", inputfile, exception.Message);
					return;
				}

				using (Buffer output = new Buffer())
				{
					var md = new Markdown(new HtmlRenderer());
					md.Render(output, buffer);
					Console.WriteLine(output);
				}
			}
		}

		static void Method2(string[] args)
		{
			if (args.Length < 1) {
				Console.WriteLine("You have to provide a file as an argument");
				return;
			}

			string inputfile = args[0];

			var md = new Markdown(new HtmlRenderer());

			using (Buffer buffer = new Buffer())
			try {
				using (var sr = new StreamReader(File.OpenRead(inputfile)))
				md.Render(buffer, sr.ReadToEnd());
			} catch (Exception exception) {
				Console.WriteLine("Unable to open input file {0: {1}", inputfile, exception.Message);
				return;
			} finally {
				Console.WriteLine(buffer);
			}
		}
	}
}
