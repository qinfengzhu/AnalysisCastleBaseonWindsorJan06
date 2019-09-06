namespace Castle.MicroKernel.ModelBuilder.Inspectors
{
	using System;
	using System.Reflection;
	using Castle.Model;
	using Castle.MicroKernel.SubSystems.Conversion;

    /// <summary>
    /// ���캯��������,ʵ��<see cref="IContributeComponentModelConstruction"/>,
    /// �ռ����п��õĹ��캯��,����������Ϊ��ѡ��䵽ģ����
    /// �ں˽�����һ������ʽ������ѡ��һ����ѡ����
    /// </summary>
    [Serializable]
	public class ConstructorDependenciesModelInspector : IContributeComponentModelConstruction
	{
		[NonSerialized]
		private ITypeConverter converter;

		public ConstructorDependenciesModelInspector()
		{
		}

		public virtual void ProcessModel(IKernel kernel, ComponentModel model)
		{
			if (converter == null)
			{
				converter = (ITypeConverter)kernel.GetSubSystem( SubSystemConstants.ConversionManagerKey );
			}

			Type targetType = model.Implementation;

			ConstructorInfo[] constructors = targetType.GetConstructors(BindingFlags.Public|BindingFlags.Instance);

			foreach(ConstructorInfo constructor in constructors)
			{
                //����ע��ÿ���������캯��,
                //���������(componentfactory)�Ժ��ں�ѡ��ѡ��һ���ϸ��
                model.Constructors.Add(CreateConstructorCandidate(constructor));
			}
		}

        /// <summary>
        /// �������캯����ѡ��
        /// </summary>
        /// <param name="constructor">���캯����Ϣ</param>
		protected virtual ConstructorCandidate CreateConstructorCandidate(ConstructorInfo constructor )
		{
			ParameterInfo[] parameters = constructor.GetParameters();

			DependencyModel[] dependencies = new DependencyModel[parameters.Length];

			for (int i = 0; i < parameters.Length; i++)
			{
				ParameterInfo parameter = parameters[i];

				Type paramType = parameter.ParameterType;

				if (converter.CanHandleType(paramType))
				{
					dependencies[i] = new DependencyModel(DependencyType.Parameter, parameter.Name, paramType, false );
				}
				else
				{
					dependencies[i] = new DependencyModel(DependencyType.Service, parameter.Name, paramType, false );
				}
			}

			return new ConstructorCandidate(constructor, dependencies);
		}
	}
}
