namespace Castle.Facilities.TypedFactory
{
	using System;

	using Castle.Model;
	using Castle.Model.Interceptor;

	using Castle.MicroKernel;

	/// <summary>
	/// Summary description for FactoryInterceptor.
	/// </summary>
	[Transient]
	public class FactoryInterceptor : IMethodInterceptor, IOnBehalfAware
	{
		private FactoryEntry _entry;
		private IKernel _kernel;

		public FactoryInterceptor(IKernel kernel)
		{
			_kernel = kernel;
		}

		public void SetInterceptedComponentModel(ComponentModel target)
		{
			_entry = (FactoryEntry) target.ExtendedProperties["typed.fac.entry"];
		}

		public object Intercept(IMethodInvocation invocation, params object[] args)
		{
			String name = invocation.Method.Name;

			if (name.Equals(_entry.CreationMethod))
			{
				if (args.Length == 0 || args[0] == null)
				{
					return _kernel[ invocation.Method.ReturnType ];
				}
				else
				{
					return _kernel[ (String) args[0] ];
				}
			}
			else if (name.Equals(_entry.DestructionMethod))
			{
				if (args.Length == 1)
				{
					_kernel.ReleaseComponent( args[0] );
					
					return null;
				}
			}
			
			return invocation.Proceed(args);
		}
	}
}
