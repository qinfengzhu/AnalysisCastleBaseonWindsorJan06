namespace Castle.Model.Resource
{
	using System;

	public class ConfigResourceFactory : IResourceFactory
	{
		public ConfigResourceFactory()
		{
		}

		public bool Accept(Uri uri)
		{
			return "config".Equals(uri.Scheme);
		}

		public IResource Create(Uri uri)
		{
			return new ConfigResource(uri);
		}

		public IResource Create(Uri uri, String basePath)
		{
			return Create(uri);
		}
	}
}
