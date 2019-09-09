namespace Castle.MicroKernel.ComponentActivator
{
	using System;

	using Castle.Model;

    /// <summary>
    /// Abstract implementation of <see cref="IComponentActivator"/>.
    /// 实现必须重写 InternalCreate 与  InternalDestroy 方法
    /// 执行他们自己的构建与销毁逻辑
    /// </summary>
    [Serializable]
	public abstract class AbstractComponentActivator : IComponentActivator
	{
		private IKernel kernel;
		private ComponentModel model; 
		private ComponentInstanceDelegate onCreation;
		private ComponentInstanceDelegate onDestruction;

		/// <summary>
		/// 构造函数
		/// </summary>
		public AbstractComponentActivator(ComponentModel model, IKernel kernel, 
			ComponentInstanceDelegate onCreation, 
			ComponentInstanceDelegate onDestruction)
		{
			this.model = model;
			this.kernel = kernel;
			this.onCreation = onCreation;
			this.onDestruction = onDestruction;
		}

		public IKernel Kernel
		{
			get { return kernel; }
		}

		public ComponentModel Model
		{
			get { return model; }
		}

		public ComponentInstanceDelegate OnCreation
		{
			get { return onCreation; }
		}

		public ComponentInstanceDelegate OnDestruction
		{
			get { return onDestruction; }
		}

		protected abstract object InternalCreate();

		protected abstract void InternalDestroy(object instance);

		#region 抽象构造函数成员

		public virtual object Create()
		{
			object instance = InternalCreate();

			onCreation(model, instance);

			return instance;
		}

		public virtual void Destroy(object instance)
		{
			InternalDestroy(instance);

			onDestruction(model, instance);
		}

		#endregion
	}
}
