namespace Castle.MicroKernel.ModelBuilder.Inspectors
{
	using System;
	using Castle.Model;
	using Castle.Model.Configuration;

	/// <summary>
    /// 检查配置中的'parameters'节点,
    /// 它的每一个子节点都会创建一个参数模型(ParameterModel)
    /// 参数模型将被添加到组件模型的参数集合上(Parameters)
	/// </summary>
	[Serializable]
	public class ConfigurationParametersInspector : IContributeComponentModelConstruction
	{
		/// <summary>
        /// 查找关联组件的配置中的参数信息,追加到组件模型的参数属性(Parameters)中
		/// </summary>
		/// <param name="kernel">内核</param>
		/// <param name="model">组件模型</param>
		public virtual void ProcessModel(IKernel kernel, ComponentModel model)
		{
			if (model.Configuration == null) return;

			IConfiguration parameters = model.Configuration.Children["parameters"];

			if (parameters == null) return;

			foreach(IConfiguration parameter in parameters.Children)
			{
				String name = parameter.Name;
				String value = parameter.Value;

				if (value == null && parameter.Children.Count != 0)
				{
					IConfiguration parameterValue = parameter.Children[0];
					model.Parameters.Add( name, parameterValue );
				}
				else
				{
					model.Parameters.Add( name, value );
				}
			}
		}
	}
}
