namespace Castle.MicroKernel.ModelBuilder
{
	using System;
	using System.Collections;
	using Castle.Model;
	using Castle.MicroKernel.ModelBuilder.Inspectors;

	/// <summary>
	/// 默认的组件模型构建者
	/// </summary>
	[Serializable]
	public class DefaultComponentModelBuilder : IComponentModelBuilder
	{
		private readonly IKernel kernel;
		private readonly IList contributors;

		public DefaultComponentModelBuilder(IKernel kernel)
		{
			this.kernel = kernel;
			this.contributors = new ArrayList();

			InitializeContributors();
		}

        /// <summary>
        /// 初始化组件构建模型贡献者(IContributeComponentModelConstruction)
        /// </summary>
		protected virtual void InitializeContributors()
		{
			AddContributor(new ConfigurationModelInspector()); //组件的配置信息
			AddContributor(new LifestyleModelInspector()); //组件的生命周期类型信息
			AddContributor(new ConstructorDependenciesModelInspector());//组件的构造函数集合
			AddContributor(new PropertiesDependenciesModelInspector());//组件属性集合
			AddContributor(new MethodMetaInspector());//组件方法集合
			AddContributor(new LifecycleModelInspector());//组件生命周期中步骤模型(IInitializable,ISupportInitialize,IDisposable)阶段性控制
            AddContributor(new ConfigurationParametersInspector());//组件的配置参数
			AddContributor(new InterceptorInspector());//组件的拦截器
		}

		public ComponentModel BuildModel(String key,Type service,Type classType,IDictionary extendedProperties)
		{
			ComponentModel model = new ComponentModel(key, service, classType);
			
			if (extendedProperties != null)
			{
				model.ExtendedProperties = extendedProperties;
			}

			foreach(IContributeComponentModelConstruction contributor in contributors)
			{
				contributor.ProcessModel( kernel, model );
			}
			
			return model;
		}

		public void AddContributor(IContributeComponentModelConstruction contributor)
		{
			contributors.Add(contributor);
		}

		public void RemoveContributor(IContributeComponentModelConstruction contributor)
		{
			contributors.Remove(contributor);
		}
	}
}