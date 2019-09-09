namespace Castle.MicroKernel.ComponentActivator
{
	using System;

	using Castle.Model;

    /// <summary>
    /// Abstract implementation of <see cref="IComponentActivator"/>.
    /// ʵ�ֱ�����д InternalCreate ��  InternalDestroy ����
    /// ִ�������Լ��Ĺ����������߼�
    /// </summary>
    [Serializable]
	public abstract class AbstractComponentActivator : IComponentActivator
	{
		private IKernel kernel;
		private ComponentModel model; 
		private ComponentInstanceDelegate onCreation;
		private ComponentInstanceDelegate onDestruction;

		/// <summary>
		/// ���캯��
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

		#region �����캯����Ա

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
