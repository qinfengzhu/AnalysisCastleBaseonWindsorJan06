namespace Castle.MicroKernel.ModelBuilder
{
	using Castle.Model;

	/// <summary>
    /// 贡献组件模型构建接口
    /// 实现者必须检查组件并且给出组件模型(ComponentModel)的一些参数信息
	/// </summary>
	public interface IContributeComponentModelConstruction
	{
		/// <summary>
        /// 通常实现者都会去查看组件模型或接口的配置属性,或者去查看一些其他的东西
		/// </summary>
		/// <param name="kernel">内核实例对象</param>
		/// <param name="model">组件模型</param>
		void ProcessModel(IKernel kernel, ComponentModel model);
	}
}
