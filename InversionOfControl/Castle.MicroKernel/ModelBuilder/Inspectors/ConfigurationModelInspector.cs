// Copyright 2004-2006 Castle Project - http://www.castleproject.org/
namespace Castle.MicroKernel.ModelBuilder.Inspectors
{
	using System;

	using Castle.Model.Configuration;

    /// <summary>
    /// 使用在内核注册的配置仓储<see cref="IConfiguration"/>关联组件
    /// </summary>
    [Serializable]
	public class ConfigurationModelInspector : IContributeComponentModelConstruction
	{
		/// <summary>
        /// 通过内核中的配置仓储查询组件关联的配置信息
		/// </summary>
		/// <param name="kernel"></param>
		/// <param name="model"></param>
		public virtual void ProcessModel(IKernel kernel, Castle.Model.ComponentModel model)
		{
			model.Configuration = kernel.ConfigurationStore.GetComponentConfiguration(model.Name);
		}
	}
}
