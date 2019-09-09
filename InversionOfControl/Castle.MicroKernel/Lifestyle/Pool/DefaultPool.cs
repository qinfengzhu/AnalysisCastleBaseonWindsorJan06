namespace Castle.MicroKernel.Lifestyle.Pool
{
	using System;
	using System.Threading;
	using System.Collections;

	using Castle.Model;

	[Serializable]
	public class DefaultPool : IPool, IDisposable
	{
		private readonly Stack available = Stack.Synchronized(new Stack());
		private readonly IList inUse = ArrayList.Synchronized(new ArrayList());
		private readonly int initialsize;
		private readonly int maxsize;
		private readonly ReaderWriterLock rwlock;
		private readonly IComponentActivator componentActivator;

		public DefaultPool(int initialsize, int maxsize, IComponentActivator componentActivator)
		{
			this.initialsize = initialsize;
			this.maxsize = maxsize;
			this.componentActivator = componentActivator;

			this.rwlock = new ReaderWriterLock();

			InitPool();
		}

		#region IPool Members

		public virtual object Request()
		{
			rwlock.AcquireWriterLock(-1);

			object instance = null;

			try
			{

				if (available.Count != 0)
				{
					instance = available.Pop();

					if (instance == null)
					{
						throw new PoolException("Invalid instance on the pool stack");
					}
				}
				else
				{
					instance = componentActivator.Create();

					if (instance == null)
					{
						throw new PoolException("Activator didn't return a valid instance");
					}
				}

				inUse.Add(instance);
			}
			finally
			{
				rwlock.ReleaseWriterLock();
			}

			return instance;
		}

		public virtual void Release(object instance)
		{
			rwlock.AcquireWriterLock(-1);

			try
			{
				if (!inUse.Contains(instance))
				{
					throw new PoolException("Trying to release a component that does not belong to this pool");
				}

				inUse.Remove(instance);

				if (available.Count < maxsize)
				{
					if (instance is IRecyclable)
					{
						(instance as IRecyclable).Recycle();
					}

					available.Push(instance);
				}
				else
				{
					// Pool is full
					componentActivator.Destroy(instance);
				}
			}
			finally
			{
				rwlock.ReleaseWriterLock();
			}
		}

		#endregion

		#region 销毁成员

		public virtual void Dispose()
		{
			foreach(object instance in available)
			{
				componentActivator.Destroy(instance);
			}
		}

		#endregion

		/// <summary>
		/// 初始化池大小
		/// </summary>
		private void InitPool()
		{
			ArrayList tempInstance = new ArrayList();

			for(int i=0; i < initialsize; i++)
			{
				tempInstance.Add( Request() );
			}

			for(int i=0; i < initialsize; i++)
			{
				Release( tempInstance[i] );
			}
		}
	}
}
