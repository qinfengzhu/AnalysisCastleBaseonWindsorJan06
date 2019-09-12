namespace Castle.Model.Interceptor
{
	using System;
	using System.Reflection;

	/// <summary>
	/// Supply information about the current method invocation.
	/// </summary>
	public interface IMethodInvocation
	{
		/// <summary>
		/// The proxy instance.
		/// </summary>
		object Proxy { get; }

		/// <summary>
		/// The target of this invocation, which usually is the
		/// instance of the class being proxy. 
		/// You might change the target, but be aware of the 
		/// implications.
		/// </summary>
		object InvocationTarget { get;set; }

		/// <summary>
		/// The Method being invoked.
		/// </summary>
		MethodInfo Method { get; }

		/// <summary>
		/// The Method being invoked on the target.
		/// </summary>
		MethodInfo MethodInvocationTarget { get; }

		/// <summary>
		/// The Proceed go on with the actual invocation.
		/// </summary>
		/// <param name="args">The arguments of the method</param>
		/// <returns></returns>
		object Proceed( params object[] args );
	}
}
