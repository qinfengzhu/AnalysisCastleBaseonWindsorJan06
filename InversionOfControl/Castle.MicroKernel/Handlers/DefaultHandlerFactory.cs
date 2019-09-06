namespace Castle.MicroKernel.Handlers
{
	using System;
	using Castle.Model;

    /// <summary>
    /// Ĭ�ϵ����������򹤳�
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
			handler.Init(kernel); //�����ʼ��
			return handler;
		}
	}
}
