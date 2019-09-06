namespace Castle.MicroKernel.ModelBuilder.Inspectors
{
	using System;
	using System.Reflection;
	using Castle.Model;
	using Castle.MicroKernel.SubSystems.Conversion;

    /// <summary>
    /// 构造函数贡献者,实现<see cref="IContributeComponentModelConstruction"/>,
    /// 收集所有可用的构造函数,并将它们作为候选填充到模型中
    /// 内核将根据一个启发式方法来选择一个候选对象
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
                //我们注册每个公共构造函数,
                //让组件工厂(componentfactory)稍后在候选中选择一个合格的
                model.Constructors.Add(CreateConstructorCandidate(constructor));
			}
		}

        /// <summary>
        /// 创建构造函数候选者
        /// </summary>
        /// <param name="constructor">构造函数信息</param>
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
