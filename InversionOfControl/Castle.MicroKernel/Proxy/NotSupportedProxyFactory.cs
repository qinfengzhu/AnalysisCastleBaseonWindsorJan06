namespace Castle.MicroKernel.Proxy
{
	using System;

	using Castle.Model;

	/// <summary>
	/// This is a placeholder implementation of <see cref="IProxyFactory"/>.
	/// </summary>
	/// <remarks>
	/// The decision to supply no implementation for <see cref="IProxyFactory"/>
	/// is supported by the fact that the MicroKernel should be a thin
	/// assembly with the minimal set of features, although extensible.
	/// Providing the support for this interface would obligate 
	/// the user to import another assembly, even if the large majority of
	/// simple cases, no use use of interceptors will take place.
	/// If you want to use however, see the Windsor container.
	/// </remarks>
	[Serializable]
	public class NotSupportedProxyFactory : IProxyFactory
	{
		#region IProxyFactory Members

		public object Create(IKernel kernel, ComponentModel mode, params object[] constructorArguments)
		{
			throw new NotImplementedException(
				"You must supply an implementation of IProxyFactory " + 
				"to use interceptors on the Microkernel");
		}

		#endregion
	}
}
