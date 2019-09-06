namespace Castle.MicroKernel.Releasers
{
	using System;

	using Castle.Model;

	/// <summary>
	/// ��������ע������,���������ͷŲ���
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
