namespace Castle.MicroKernel
{
	/// <summary>
	/// Implements the instance creation logic. The default
	/// implementation should rely on an ordinary call to 
	/// Activator.CreateInstance(). 
	/// </summary>
	/// <remarks>
	/// This interface is provided in order to allow custom components
	/// to be created using a different logic, such as using a specific factory
	/// or builder.
	/// <br/>
	/// The constructor for implementation have the following signature:
	/// <code>
	/// ComponentModel model, IKernel kernel, 
	/// ComponentInstanceDelegate onCreation, 
	/// ComponentInstanceDelegate onDestruction
	/// </code>
	/// The Activator should raise the events onCreation and onDestruction
	/// in order to correctly implement the contract. Usually the best
	/// way of creating a custom activator is by extending the existing ones.
	/// 
	/// <seealso cref="ComponentActivator.AbstractComponentActivator"/>
	/// <seealso cref="ComponentActivator.DefaultComponentActivator"/>
	/// </remarks>
	public interface IComponentActivator
	{
		/// <summary>
		/// Should return a new component instance.
		/// </summary>
		/// <returns></returns>
		object Create();

		/// <summary>
		/// Should perform all necessary work to dispose the instance
		/// and/or any resource related to it.
		/// </summary>
		/// <param name="instance"></param>
		void Destroy(object instance);
	}
}
