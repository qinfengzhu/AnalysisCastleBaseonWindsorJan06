namespace Castle.MicroKernel
{
	using System;

	using Castle.Model;

	/// <summary>
	/// Implementors should use a strategy to obtain 
	/// valid references to properties and/or services 
	/// requested in the dependency model.
	/// </summary>
	public interface ISubDependencyResolver
	{
		/// <summary>
		/// Should return an instance of a service or property values as
		/// specified by the dependency model instance. 
		/// It is also the responsability of <see cref="IDependencyResolver"/>
		/// to throw an exception in the case a non-optional dependency 
		/// could not be resolved.
		/// </summary>
		/// <param name="model"></param>
		/// <param name="dependency"></param>
		/// <returns></returns>
		object Resolve(ComponentModel model, DependencyModel dependency);

		/// <summary>
		/// Returns true if the resolver is able to satisfy this dependency.
		/// </summary>
		/// <param name="model"></param>
		/// <param name="dependency"></param>
		/// <returns></returns>
		bool CanResolve(ComponentModel model, DependencyModel dependency);
	}
}
