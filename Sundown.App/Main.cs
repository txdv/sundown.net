using System;
using System.IO;

namespace Sundown.App
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			byte[] bytes = new byte[512];

			using (Buffer buffer = new Buffer(bytes.Length))
			{
				if (args[0].Length == 0) {
					Console.WriteLine("You have to provide a file as an argument");
				}

				string inputfile = args[0];

				try {
					using (FileStream fs = File.OpenRead(inputfile))
					{
						for (int size = 0; (size = fs.Read(bytes, 0, bytes.Length)) != 0;) {
							buffer.Grow(bytes.Length);
							buffer.Put(bytes, size);
						}
						buffer.End();
					}
				} catch (Exception exception) {
					Console.WriteLine("Unable to open input file {0: {1}", inputfile, exception.Message);
					return;
				}

				// Strangest bug ever, without printing something, it simply
				// will stop executing at the next line
				// puting this into the constructor doesn't work either...

				Console.Write(new char[] { });

				Renderer r = new HtmlRenderer();
				using (Buffer output = new Buffer(bytes.Length))
				{
					r.Markdown(output, buffer);
					Console.WriteLine(output.ToString());
				}

			}
		}

	}
}
