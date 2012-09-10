
namespace Sundown
{
	public class Version
	{
		public Version(int major, int minor, int revision)
		{
			Major = major;
			Minor = minor;
			Revision = revision;
		}

		public int Major { get; protected set; }
		public int Minor { get; protected set; }
		public int Revision { get; protected set; }

		public override string ToString()
		{
			return string.Format("{0}.{1}.{2}", Major, Minor, Revision);
		}
	}
}

