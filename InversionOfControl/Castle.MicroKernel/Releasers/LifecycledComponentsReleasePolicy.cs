namespace Castle.MicroKernel.Releasers
{
	using System;

	using Castle.Model;

	/// <summary>
	/// 仅跟踪已注册的组件,它包含有释放步骤
	/// </summary>
	[Serializable]
	public class LifecycledComponentsReleasePolicy : AllComponentsReleasePolicy
	{
		public LifecycledComponentsReleasePolicy()
		{
		}

		public override void Track(object instance, IHandler handler)
		{
			ComponentModel model = handler.ComponentModel;

			if (model.LifecycleSteps.HasDecommissionSteps || 
				model.LifestyleType == LifestyleType.Pooled)
			{
				base.Track(instance, handler);
			}
		}
	}
}
