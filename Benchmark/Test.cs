using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Benchmark
{
	public abstract class Test
	{
		public string Name { get; protected set; }

		public abstract string Transform(string str);
		public abstract byte[] Transform(byte[] arr);

		public long String { set; get; }
		public long Byte { set; get; }

		public double NString { get; protected set; }
		public double NByte { get; protected set; }

		public void Benchmark(int n, string text, byte[] textarr)
		{
			Stopwatch s;

			s = new Stopwatch();
			s.Start();
			for (int j = 0; j < n; j++) {
				Transform(text);
			}
			s.Stop();
			String = s.ElapsedMilliseconds;


			s = new Stopwatch();
			s.Start();
			for (int j = 0; j < n; j++) {
				Transform(textarr);
			}
			s.Stop();
			Byte = s.ElapsedMilliseconds;
		}

		public static void Normalize(IEnumerable<Test> tests)
		{
			long minString = long.MaxValue;
			long minByte = long.MaxValue;


			foreach (var test in tests) {
				minString = Math.Min(minString, test.String);
				minByte = Math.Min(minByte, test.Byte);
			}

			foreach (var test in tests) {
				test.NByte = (double)test.Byte/minByte;
				test.NString = (double)test.String/minString;
			}
		}

		public override string ToString()
		{
			return string.Format ("{0}:\t{1}\t{2}\t{3:0.00}\t{4:0.00}", Name, String, Byte, NString, NByte);
		}
	}
}

