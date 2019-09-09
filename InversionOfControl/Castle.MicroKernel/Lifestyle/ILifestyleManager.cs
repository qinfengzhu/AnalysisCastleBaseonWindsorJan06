namespace Castle.MicroKernel
{
	using System;

	/// <summary>
	/// The <c>ILifestyleManager</c> implements 
	/// a strategy for a given lifestyle, like singleton, perthread
	/// and transient.
	/// </summary>
	/// <remarks>
	/// The responsability of <c>ILifestyleManager</c>
	/// is only the management of lifestyle. It should rely on
	/// <see cref="IComponentActivator"/> to obtain a new component instance
	/// </remarks>
	public interface ILifestyleManager : IDisposable
	{
		/// <summary>
		/// Initializes the <c>ILifestyleManager</c> with the 
		/// <see cref="IComponentActivator"/>
		/// </summary>
		/// <param name="componentActivator"></param>
		/// <param name="kernel"></param>
		void Init(IComponentActivator componentActivator, IKernel kernel);

		/// <summary>
		/// Implementors should return the component instance based on the lifestyle semantic.
		/// </summary>
		/// <returns></returns>
		object Resolve();

		/// <summary>
		/// Implementors should release the component instance based
		/// on the lifestyle semantic, for example, singleton components
		/// should not be released on a call for release, instead they should
		/// release them when disposed is invoked.
		/// </summary>
		/// <param name="instance"></param>
		void Release(object instance);
	}
}
