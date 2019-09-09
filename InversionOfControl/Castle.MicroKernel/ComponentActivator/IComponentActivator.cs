namespace Castle.MicroKernel
{
	/// <summary>
    /// 实现实例的创建
    /// 默认的实例创建调用 Activator.CreateInstance()
	/// </summary>
	/// <remarks>
    /// 接口允许自定义的组件创建,例如某些特定的工厂构建器
	/// <br/>
    /// 构造函数必须包含下面标记
	/// <code>
	/// ComponentModel model, IKernel kernel, 
	/// ComponentInstanceDelegate onCreation, 
	/// ComponentInstanceDelegate onDestruction
	/// </code>
    /// 构建器必须触发onCreation与onDestruction这两个事件
	/// <seealso cref="ComponentActivator.AbstractComponentActivator"/>
	/// <seealso cref="ComponentActivator.DefaultComponentActivator"/>
	/// </remarks>
	public interface IComponentActivator
	{
		/// <summary>
		/// 返回组件的实例对象
		/// </summary>
		/// <returns></returns>
		object Create();

        /// <summary>
        /// 应该执行所有必要的工作来释放实例和与其相关的任何资源
        /// </summary>
        /// <param name="instance">组件实例对象</param>
        void Destroy(object instance);
	}
}
