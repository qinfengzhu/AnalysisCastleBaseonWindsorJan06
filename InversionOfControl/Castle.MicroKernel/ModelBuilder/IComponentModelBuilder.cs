
namespace Castle.MicroKernel
{
	using System;
	using System.Collections;

	using Castle.Model;

	using Castle.MicroKernel.ModelBuilder;

    /// <summary>
    /// 实现者必须通过检查组件或配置来构造组件模型
    /// </summary>
    public interface IComponentModelBuilder
	{
        /// <summary>
        /// 通过调用注册的组件模型贡献者(实现了IContributeComponentModelConstruction)来构造一个新的组件模型
        /// </summary>
        /// <param name="key">组件唯一标识</param>
        /// <param name="service">组件服务类型</param>
        /// <param name="classType">组件实现类型</param>
        /// <param name="extendedProperties">组件扩展属性</param>
        /// <returns>组件模型</returns>
        ComponentModel BuildModel( String key, Type service, Type classType, IDictionary extendedProperties );

        /// <summary>
        /// 组件模型贡献者应该检查组件，甚至与组件相关联的配置，以便在模型中添加或更改可供以后使用的信息
        /// </summary>
        void AddContributor(IContributeComponentModelConstruction contributor );

        /// <summary>
        /// 移除指定的组件模型贡献者(实现了IContributeComponentModelConstruction)
        /// </summary>
        /// <param name="contributor">组件模型贡献者</param>
        void RemoveContributor(IContributeComponentModelConstruction contributor );
	}
}
