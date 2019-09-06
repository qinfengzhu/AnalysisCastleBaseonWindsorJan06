namespace Castle.MicroKernel.Handlers
{
	using System;
	using Castle.Model;

    /// <summary>
    /// 默认的组件处理程序工厂
    /// </summary>
	[Serializable]
	public class DefaultHandlerFactory : IHandlerFactory
	{
		private IKernel kernel;

		public DefaultHandlerFactory(IKernel kernel)
		{
			this.kernel = kernel;
		}

		public virtual IHandler Create(ComponentModel model)
		{
			IHandler handler = new DefaultHandler(model);
			handler.Init(kernel); //组件初始化
			return handler;
		}
	}
}
