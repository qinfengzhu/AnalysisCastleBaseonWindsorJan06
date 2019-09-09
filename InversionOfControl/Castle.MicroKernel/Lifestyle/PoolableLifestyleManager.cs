namespace Castle.MicroKernel.Lifestyle
{
	using System;
	using System.Collections;

	using Castle.MicroKernel.Lifestyle.Pool;

	/// <summary>
	/// 池类型-管理器
	/// </summary>
	[Serializable]
	public class PoolableLifestyleManager : AbstractLifestyleManager
	{
		private IPool pool;
		private int initialSize;
		private int maxSize;

		public PoolableLifestyleManager(int initialSize, int maxSize)
		{
			this.initialSize = initialSize;
			this.maxSize = maxSize;
		}

		public override void Init(IComponentActivator componentActivator, IKernel kernel)
		{
			base.Init(componentActivator, kernel);

			pool = InitPool(initialSize, maxSize);
		}

		protected IPool InitPool(int initialSize, int maxSize)
		{
			if (!Kernel.HasComponent( typeof(IPoolFactory) ))
			{
				Kernel.AddComponent("castle.internal.poolfactory",typeof(IPoolFactory), typeof(DefaultPoolFactory));
			}

			IPoolFactory factory = Kernel[ typeof(IPoolFactory) ] as IPoolFactory;

			return factory.Create( initialSize, maxSize, ComponentActivator );
		}

		public override object Resolve()
		{
			return pool.Request();
		}

		public override void Release(object instance)
		{
			pool.Release(instance);
		}	

		public override void Dispose()
		{
			pool.Dispose();
		}
	}
}
