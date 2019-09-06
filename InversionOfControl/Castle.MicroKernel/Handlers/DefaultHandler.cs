namespace Castle.MicroKernel.Handlers
{
	using System;
	using Castle.Model;

    /// <summary>
    /// 默认组件处理程序
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
			//转交给LifeStyleManager进行创建组件实例
			return lifestyleManager.Resolve();
		}

		public override void Release(object instance)
		{
            //释放组件实例
			lifestyleManager.Release(instance);
		}
	}
}