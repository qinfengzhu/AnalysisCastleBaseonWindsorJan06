namespace Castle.Model.Resource
{
	using System;

	/// <summary>
	/// 
	/// </summary>
	public class AssemblyResourceFactory : IResourceFactory
	{
		public AssemblyResourceFactory()
		{
		}

		public bool Accept(Uri uri)
		{
			return "assembly".Equals(uri.Scheme);
		}

		public IResource Create(Uri uri)
		{
			return Create(uri, null);
		}

		public IResource Create(Uri uri, String basePath)
		{
			if (basePath != null)
				return new AssemblyResource(uri, basePath);
			else
				return new AssemblyResource(uri);
		}		
	}
}
