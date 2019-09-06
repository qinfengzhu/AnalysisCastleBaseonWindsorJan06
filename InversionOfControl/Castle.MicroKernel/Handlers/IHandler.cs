namespace Castle.MicroKernel
{
	using System;

	using Castle.Model;

	/// <summary>
    /// 标识Handler的状态
	/// </summary>
	public enum HandlerState
	{
		/// <summary>
		/// 组件可以被实例化
		/// </summary>
		Valid,
		/// <summary>
        /// 组件不能被实例化,因为它的一些依赖还不可用
		/// </summary>
		WaitingDependency
	}

	/// <summary>
    /// 管理组件的状态,协调组件的创建(分发给Activators)与销毁(分发给生命周期Manager)
	/// </summary>
	public interface IHandler
	{
		/// <summary>
		/// 初始化Handler
		/// </summary>
		/// <param name="kernel">内核</param>
		void Init(IKernel kernel);

		/// <summary>
        /// 实现者必须返回一个可用的组件实例,当组件不能创建的时候应该抛出异常
		/// </summary>
		object Resolve();

		/// <summary>
        /// 实现者必须释放组件实例
		/// </summary>
		/// <param name="instance"></param>
		void Release(object instance);

		/// <summary>
        /// Handler当前状态
		/// </summary>
		HandlerState CurrentState { get; }

		/// <summary>
        /// 组件模型
		/// </summary>
		ComponentModel ComponentModel { get; }
	}
}
