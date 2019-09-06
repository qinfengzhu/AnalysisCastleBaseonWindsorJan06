namespace Castle.MicroKernel
{
	using System;

	using Castle.Model;

    /// <summary>
    /// 组件包含基本信息的委托
    /// </summary>
    /// <param name="key">组件的唯一标识</param>
    /// <param name="handler">处理程序,它能够实例化组件</param>
    public delegate void ComponentDataDelegate( String key, IHandler handler );

    /// <summary>
    /// 组件包含基本信息与实例的委托
    /// </summary>
    /// <param name="model">组件的元数据信息(meta)</param>
    /// <param name="instance">组件实例</param>
    public delegate void ComponentInstanceDelegate( ComponentModel model, object instance );

    /// <summary>
    /// 组件包含元数据信息的委托
    /// </summary>
    public delegate void ComponentModelDelegate( ComponentModel model );

    /// <summary>
    /// 组件包含处理程序的委托
    /// </summary>
    /// <param name="handler">处理程序,它能够实例化组件</param>
    public delegate void HandlerDelegate( IHandler handler, ref bool stateChanged );

	/// <summary>
	/// 包含依赖解析的委托
	/// </summary>
	public delegate void DependencyDelegate(ComponentModel client, DependencyModel model, Object dependency);

	/// <summary>
	/// 对内核事件接口的汇总
	/// </summary>
	public interface IKernelEvents
	{
		/// <summary>
        /// 当组件在内核中注册的时候,触发该事件
		/// </summary>
		event ComponentDataDelegate ComponentRegistered;

		/// <summary>
        /// 当组件从内核中移除的时候,触发该事件
		/// </summary>
		event ComponentDataDelegate ComponentUnregistered;

		/// <summary>
        /// 当组件模型被创建的时候,触发该事件
        /// 允许一些后续影响处理程序(handler)的自定义操作
		/// </summary>
		event ComponentModelDelegate ComponentModelCreated;

		/// <summary>
        /// 当内核以孩子节点方式加入到另一个内核中的时候,触发该事件
		/// </summary>
		event EventHandler AddedAsChildKernel;

		/// <summary>
        /// 组件被创建前,触发该事件
		/// </summary>
		event ComponentInstanceDelegate ComponentCreated;

		/// <summary>
        /// 组件实例对象被销毁的时候,触发该事件
		/// </summary>
		event ComponentInstanceDelegate ComponentDestroyed;

		/// <summary>
        /// 处理程序(handler)被注册的时候,触发该事件
		/// </summary>
		event HandlerDelegate HandlerRegistered;

		/// <summary>
        /// 依赖项被解决时候,触发该事件
        /// 允许依赖被改变,客户端组件模型不能被改变
		/// </summary>
		event DependencyDelegate DependencyResolving;
	}
}
