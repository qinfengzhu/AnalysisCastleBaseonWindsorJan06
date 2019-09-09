namespace Castle.MicroKernel.Lifestyle
{
	using System;

	/// <summary>
	/// ≥ÈœÛπ‹¿Ì∆˜
	/// </summary>
	[Serializable]
	public abstract class AbstractLifestyleManager : ILifestyleManager
	{
		private IKernel kernel;
		private IComponentActivator componentActivator;

		public virtual void Init(IComponentActivator componentActivator, IKernel kernel)
		{
			this.componentActivator = componentActivator;
			this.kernel = kernel;
		}

		public virtual object Resolve()
		{
			return componentActivator.Create();
		}

		public virtual void Release(object instance)
		{
			componentActivator.Destroy( instance );
		}	

		public abstract void Dispose();

		protected IKernel Kernel
		{
			get { return kernel; }
		}

		protected IComponentActivator ComponentActivator
		{
			get { return componentActivator; }
		}
	}
}
