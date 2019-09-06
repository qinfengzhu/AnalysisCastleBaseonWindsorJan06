namespace Castle.MicroKernel.SubSystems.Resource
{
	using System;

	using Castle.Model.Resource;

	/// <summary>
	/// 
	/// </summary>
	public interface IResourceSubSystem : ISubSystem
	{
		IResource CreateResource(Uri uri);

		IResource CreateResource(Uri uri, String basePath);

		IResource CreateResource(String resource);

		IResource CreateResource(String resource, String basePath);

		void RegisterResourceFactory(IResourceFactory resourceFactory);
	}
}
