using System;
using System.IO;

namespace Sundown
{
	public class BufferStream : Stream
	{
		public Buffer Buffer { get; protected set; }

		public BufferStream()
			: this(new Buffer())
		{
		}

		public BufferStream(int unit)
			: this(new Buffer(unit))
		{
		}

		public BufferStream(Buffer buffer)
		{
			Buffer = buffer;
		}

		#region implemented abstract members of System.IO.Stream
		public override void Flush()
		{
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new System.NotImplementedException ();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new System.NotImplementedException ();
		}

		public override void SetLength(long value)
		{
			throw new System.NotImplementedException ();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			Buffer.Put(buffer, offset, count);
		}

		public override bool CanRead {
			get {
				return false;
			}
		}

		public override bool CanSeek {
			get {
				return false;
			}
		}

		public override bool CanWrite {
			get {
				return true;
			}
		}

		public override long Length {
			get {
				return Buffer.Size.ToInt64();
			}
		}

		public override long Position {
			get {
				return Buffer.Size.ToInt64();
			}
			set {
				throw new System.NotImplementedException ();
			}
		}
		#endregion
	}
}

