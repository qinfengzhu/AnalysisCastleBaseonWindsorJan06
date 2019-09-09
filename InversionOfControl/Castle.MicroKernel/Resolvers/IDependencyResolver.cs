namespace Castle.MicroKernel
{
	using System;

	using Castle.Model;

    /// <summary>
    /// ʵ����Ӧ��ʹ�ò�������ȡ��������ϵģ������������Ի�������Ч����
    /// </summary>
    public interface IDependencyResolver : ISubDependencyResolver
	{
		/// <summary>
		/// This method is called with a delegate for firing the
		/// IKernelEvents.DependencyResolving event.
		/// </summary>
		/// <param name="resolving">The delegate used to fire the event</param>
		void Initialize(DependencyDelegate resolving);

		void AddSubResolver(ISubDependencyResolver subResolver);

		void RemoveSubResolver(ISubDependencyResolver subResolver);
	}
}
