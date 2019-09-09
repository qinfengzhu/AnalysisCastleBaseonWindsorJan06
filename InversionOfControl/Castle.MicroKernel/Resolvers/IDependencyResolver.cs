namespace Castle.MicroKernel
{
	using System;

	using Castle.Model;

    /// <summary>
    /// 实现者应该使用策略来获取对依赖关系模型中请求的属性或服务的有效引用
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
