namespace Castle.MicroKernel.SubSystems.Resource
{
	using System;
	using System.Collections;

	using Castle.Model.Resource;

	/// <summary>
	/// 
	/// </summary>
	public class DefaultResourceSubSystem : AbstractSubSystem, IResourceSubSystem
	{
		private readonly IList resourceFactories = new ArrayList();

		public DefaultResourceSubSystem()
		{
			InitDefaultResourceFactories();
		}

		protected virtual void InitDefaultResourceFactories()
		{
			RegisterResourceFactory( new AssemblyResourceFactory() );
			RegisterResourceFactory( new FileResourceFactory() );
			RegisterResourceFactory( new UncResourceFactory() );
			RegisterResourceFactory( new ConfigResourceFactory() );
		}

		public void RegisterResourceFactory(IResourceFactory resourceFactory)
		{
			if (resourceFactory == null) throw new ArgumentNullException("resourceFactory");

			resourceFactories.Add( resourceFactory );
		}

		public IResource CreateResource(String resource)
		{
			if (resource == null) throw new ArgumentNullException("resource");

			return CreateResource(new Uri(resource));
		}

		public IResource CreateResource(String resource, String basePath)
		{
			if (resource == null) throw new ArgumentNullException("resource");

			return CreateResource(new Uri(resource), basePath);
		}

		public IResource CreateResource(Uri uri)
		{
			if (uri == null) throw new ArgumentNullException("uri");

			foreach(IResourceFactory resFactory in resourceFactories)
			{
				if (resFactory.Accept(uri))
				{
					return resFactory.Create(uri);
				}
			}

			throw new KernelException("No Resource factory was able to " + 
				"deal with Uri " + uri.ToString());
		}

		public IResource CreateResource(Uri uri, String basePath)
		{
			if (uri == null) throw new ArgumentNullException("uri");
			if (basePath == null) throw new ArgumentNullException("basePath");

			foreach(IResourceFactory resFactory in resourceFactories)
			{
				if (resFactory.Accept(uri))
				{
					return resFactory.Create(uri, basePath);
				}
			}

			throw new KernelException("No Resource factory was able to " + 
				"deal with Uri " + uri.ToString());
		}
	}
}
