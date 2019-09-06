namespace Castle.Model.Resource
{
	using System;
	using System.IO;
	using System.Text;

	/// <summary>
	/// 
	/// </summary>
	public interface IResource : IDisposable
	{
		String FileBasePath { get; }

		TextReader GetStreamReader();

		TextReader GetStreamReader(Encoding encoding);

		IResource CreateRelative(String relativePath);
	}
}
