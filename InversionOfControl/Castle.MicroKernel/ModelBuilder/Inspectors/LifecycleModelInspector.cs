namespace Castle.MicroKernel.ModelBuilder.Inspectors
{
	using System;
	using System.ComponentModel;
	using Castle.Model;
	using Castle.MicroKernel.LifecycleConcerns;

	/// <summary>
    /// ��������ҳ�ʵ���������սӿ�(�������ռ�Castle.Model��)
	/// </summary>
	[Serializable]
	public class LifecycleModelInspector : IContributeComponentModelConstruction
	{
		public LifecycleModelInspector()
		{
		}

        /// <summary>
        /// ��������Ƿ�ʵ���� <see cref="IInitializable"/> ��<see cref="ISupportInitialize"/> �� <see cref="IDisposable"/>  �ӿ�
        /// </summary>
        /// <param name="kernel">�ں�</param>
        /// <param name="model">���ģ��</param>
        public virtual void ProcessModel(IKernel kernel, ComponentModel model)
		{
			if (typeof(IInitializable).IsAssignableFrom(model.Implementation))
			{
				model.LifecycleSteps.Add(LifecycleStepType.Commission,InitializationConcern.Instance);
			}
			if (typeof(ISupportInitialize).IsAssignableFrom(model.Implementation))
			{
				model.LifecycleSteps.Add(LifecycleStepType.Commission,SupportInitializeConcern.Instance);
			}
			if (typeof(IDisposable).IsAssignableFrom(model.Implementation))
			{
				model.LifecycleSteps.Add(LifecycleStepType.Decommission,DisposalConcern.Instance);
			}
		}
	}
}