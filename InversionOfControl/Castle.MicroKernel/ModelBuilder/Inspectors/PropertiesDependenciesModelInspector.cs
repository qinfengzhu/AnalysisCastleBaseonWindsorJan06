namespace Castle.MicroKernel.ModelBuilder.Inspectors
{
	using System;
	using System.Reflection;
	using Castle.Model;
	using Castle.MicroKernel.SubSystems.Conversion;

	/// <summary>
	/// 属性贡献者 <see cref="IContributeComponentModelConstruction"/>
    /// 收集组件公开的可写属性,并且放入到组件模型中
	/// </summary>
	[Serializable]
	public class PropertiesDependenciesModelInspector : IContributeComponentModelConstruction
	{
		[NonSerialized]
		private ITypeConverter converter;

		public PropertiesDependenciesModelInspector()
		{
		}

        /// <summary>
        /// 添加属性作为此组件的可选依赖项.
        /// </summary>
        /// <param name="kernel">内核</param>
        /// <param name="model">组件模型</param>
        public virtual void ProcessModel(IKernel kernel, ComponentModel model)
		{
			if (converter == null)
			{
				converter = (ITypeConverter)kernel.GetSubSystem(SubSystemConstants.ConversionManagerKey);
			}
			InspectProperties(model);
		}

        /// <summary>
        /// 检查属性集合
        /// </summary>
        /// <param name="model">组件模型</param>
		protected virtual void InspectProperties(ComponentModel model)
		{
			Type targetType = model.Implementation;
	
			PropertyInfo[] properties = targetType.GetProperties(BindingFlags.Public|BindingFlags.Instance);
	
			foreach(PropertyInfo property in properties)
			{
				if (!property.CanWrite)
				{
					continue;
				}

				DependencyModel dependency = null;

				Type propertyType = property.PropertyType;

				//所有的依赖都是简单的猜测
				//所以我们认为这个是可选的(最后一个参数为true)

				if (converter.CanHandleType(propertyType))
				{
					dependency = new DependencyModel(DependencyType.Parameter, property.Name, propertyType, true);
				}
				else if(propertyType.IsInterface || propertyType.IsClass)
				{
					dependency = new DependencyModel(DependencyType.Service, property.Name, propertyType, true);
				}
				else
				{
					continue;
				}

				model.Properties.Add( new PropertySet(property, dependency) );
			}
		}
	}
}