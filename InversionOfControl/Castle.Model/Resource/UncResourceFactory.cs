namespace Castle.Model.Resource
{
	using System;

	public class UncResourceFactory : IResourceFactory
	{
		public UncResourceFactory()
		{
		}

		public bool Accept(Uri uri)
		{
			return uri.IsUnc;
		}

		public IResource Create(Uri uri)
		{
			return new UncResource(uri);
		}

		public IResource Create(Uri uri, String basePath)
		{
			return new UncResource(uri, basePath);
		}
	}
}
