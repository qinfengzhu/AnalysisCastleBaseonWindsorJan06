namespace Castle.MicroKernel.Handlers
{
	using System;
	using Castle.Model;

    /// <summary>
    /// Ĭ������������
    /// </summary>
	[Serializable]
	public class DefaultHandler : AbstractHandler
	{
		public DefaultHandler(ComponentModel model) : base(model)
		{
		}

		public override object Resolve()
		{
			if (CurrentState == HandlerState.WaitingDependency)
			{
				String message = 
					String.Format("Can't create component '{1}' as it has dependencies to be satisfied. {0}", 
						ObtainDependencyDetails(), ComponentModel.Name );

				throw new HandlerException(message);
			}
			//ת����LifeStyleManager���д������ʵ��
			return lifestyleManager.Resolve();
		}

		public override void Release(object instance)
		{
            //�ͷ����ʵ��
			lifestyleManager.Release(instance);
		}
	}
}