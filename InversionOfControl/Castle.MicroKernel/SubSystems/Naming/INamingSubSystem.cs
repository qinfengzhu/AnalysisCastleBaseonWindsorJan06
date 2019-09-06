namespace Castle.MicroKernel
{
	using System;
	using System.Collections;

	/// <summary>
	/// Contract for SubSystem that wishes to keep and coordinate
	/// component registration.
	/// </summary>
	public interface INamingSubSystem : ISubSystem
	{
		/// <summary>
		/// Implementors should register the key and service pointing 
		/// to the specified handler
		/// </summary>
		/// <param name="key"></param>
		/// <param name="handler"></param>
		void Register(String key, IHandler handler);

		/// <summary>
		/// Unregister the handler by the given key
		/// </summary>
		/// <param name="key"></param>
		void UnRegister(String key);

		/// <summary>
		/// Unregister the handler by the given service
		/// </summary>
		/// <param name="service"></param>
		void UnRegister(Type service);

		/// <summary>
		/// Returns true if there is a component registered 
		/// for the specified key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		bool Contains(String key);

		/// <summary>
		/// Returns true if there is a component registered 
		/// for the specified service
		/// </summary>
		/// <param name="service"></param>
		/// <returns></returns>
		bool Contains(Type service);

		/// <summary>
		/// Returns the number of components registered.
		/// </summary>
		int ComponentCount { get; }

		/// <summary>
		/// Returns the <see cref="IHandler"/> associated with
		/// the specified key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		IHandler GetHandler(String key);

		/// <summary>
		/// Returns an array of <see cref="IHandler"/> that
		/// satisfies the specified query.
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		IHandler[] GetHandlers(String query);

		/// <summary>
		/// Returns the <see cref="IHandler"/> associated with
		/// the specified service.
		/// </summary>
		IHandler GetHandler(Type service);

		/// <summary>
		/// Returns an array of <see cref="IHandler"/> associated with
		/// the specified service.
		/// </summary>
		/// <param name="service"></param>
		/// <returns></returns>
		IHandler[] GetHandlers(Type service);

		/// <summary>
		/// Returns all <see cref="IHandler"/> registered.
		/// </summary>
		/// <returns></returns>
		IHandler[] GetHandlers();

		/// <summary>
		/// Return <see cref="IHandler"/>s where components are compatible
		/// with the specified service.
		/// </summary>
		/// <param name="service"></param>
		/// <returns></returns>
		IHandler[] GetAssignableHandlers(Type service);

		/// <summary>
		/// Associates a <see cref="IHandler"/> with 
		/// the specified service
		/// </summary>
		IHandler this[Type service] { set; }

		/// <summary>
		/// Associates a <see cref="IHandler"/> with
		/// the specified key
		/// </summary>
		IHandler this[String key] { set; }

		/// <summary>
		/// List of handler by key
		/// </summary>
		IDictionary GetKey2Handler();

		/// <summary>
		/// List of handler by service
		/// </summary>
		IDictionary GetService2Handler();
	}
}