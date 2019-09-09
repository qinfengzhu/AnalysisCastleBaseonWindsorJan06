namespace Castle.MicroKernel.Lifestyle.Pool
{
	using System;

	/// <summary>
	/// Pool �ӿ���Լ
	/// </summary>
	public interface IPool : IDisposable
	{
		/// <summary>
		/// Implementors should return a component instance.
		/// </summary>
		/// <returns></returns>
		object Request( );

		/// <summary>
		/// Implementors should release the instance or put it on the pool
		/// </summary>
		/// <param name="instance"></param>
		void Release( object instance );
	}
}
