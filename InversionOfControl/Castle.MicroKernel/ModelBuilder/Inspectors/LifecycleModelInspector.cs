namespace Castle.MicroKernel.ModelBuilder.Inspectors
{
	using System;
	using System.ComponentModel;
	using Castle.Model;
	using Castle.MicroKernel.LifecycleConcerns;

	/// <summary>
    /// 检查类型找出实现生命回收接口(在命名空间Castle.Model下)
	/// </summary>
	[Serializable]
	public class LifecycleModelInspector : IContributeComponentModelConstruction
	{
		public LifecycleModelInspector()
		{
		}

        /// <summary>
        /// 检查类型是否实现了 <see cref="IInitializable"/> 或<see cref="ISupportInitialize"/> 或 <see cref="IDisposable"/>  接口
        /// </summary>
        /// <param name="kernel">内核</param>
        /// <param name="model">组件模型</param>
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