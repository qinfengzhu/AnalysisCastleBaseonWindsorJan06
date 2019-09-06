namespace Castle.Model.Resource
{
	using System;


	/// <summary>
	/// 
	/// </summary>
	public class FileResourceFactory : IResourceFactory
	{
		public FileResourceFactory()
		{
		}

		public bool Accept(Uri uri)
		{
			return "file".Equals(uri.Scheme);
		}

		public IResource Create(Uri uri)
		{
			return Create(uri, null);
		}

		public IResource Create(Uri uri, String basePath)
		{
			if (basePath != null)
				return new FileResource(uri, basePath);
			else
				return new FileResource(uri);
		}
	}
}
