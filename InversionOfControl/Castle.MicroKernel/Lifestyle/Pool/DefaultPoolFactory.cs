namespace Castle.MicroKernel.Lifestyle.Pool
{
	using System;

	[Serializable]
	public class DefaultPoolFactory : IPoolFactory
	{
		public DefaultPoolFactory()
		{
		}

		public IPool Create(int initialsize, int maxSize, IComponentActivator activator)
		{
			return new DefaultPool(initialsize, maxSize, activator);
		}
	}
}
