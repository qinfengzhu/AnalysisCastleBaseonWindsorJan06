namespace Castle.MicroKernel
{
	using System;

	public abstract class AbstractSubSystem : MarshalByRefObject, ISubSystem
	{
		private IKernel kernel;

		public override object InitializeLifetimeService()
		{
			return null;
		}

		public virtual void Init(IKernel kernel)
		{
			this.kernel = kernel;
		}

		public virtual void Terminate()
		{
		}

		protected IKernel Kernel
		{
			get { return kernel; }
		}
	}
}
