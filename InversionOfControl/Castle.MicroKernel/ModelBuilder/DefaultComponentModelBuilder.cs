namespace Castle.MicroKernel.ModelBuilder
{
	using System;
	using System.Collections;
	using Castle.Model;
	using Castle.MicroKernel.ModelBuilder.Inspectors;

	/// <summary>
	/// Ĭ�ϵ����ģ�͹�����
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
        /// ��ʼ���������ģ�͹�����(IContributeComponentModelConstruction)
        /// </summary>
		protected virtual void InitializeContributors()
		{
			AddContributor(new ConfigurationModelInspector()); //�����������Ϣ
			AddContributor(new LifestyleModelInspector()); //�������������������Ϣ
			AddContributor(new ConstructorDependenciesModelInspector());//����Ĺ��캯������
			AddContributor(new PropertiesDependenciesModelInspector());//������Լ���
			AddContributor(new MethodMetaInspector());//�����������
			AddContributor(new LifecycleModelInspector());//������������в���ģ��(IInitializable,ISupportInitialize,IDisposable)�׶��Կ���
            AddContributor(new ConfigurationParametersInspector());//��������ò���
			AddContributor(new InterceptorInspector());//�����������
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