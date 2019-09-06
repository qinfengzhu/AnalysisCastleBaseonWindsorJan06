namespace Castle.MicroKernel.ModelBuilder.Inspectors
{
	using System;
	using System.Reflection;
	using Castle.Model;
	using Castle.MicroKernel.SubSystems.Conversion;

	/// <summary>
	/// ���Թ����� <see cref="IContributeComponentModelConstruction"/>
    /// �ռ���������Ŀ�д����,���ҷ��뵽���ģ����
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
        /// ���������Ϊ������Ŀ�ѡ������.
        /// </summary>
        /// <param name="kernel">�ں�</param>
        /// <param name="model">���ģ��</param>
        public virtual void ProcessModel(IKernel kernel, ComponentModel model)
		{
			if (converter == null)
			{
				converter = (ITypeConverter)kernel.GetSubSystem(SubSystemConstants.ConversionManagerKey);
			}
			InspectProperties(model);
		}

        /// <summary>
        /// ������Լ���
        /// </summary>
        /// <param name="model">���ģ��</param>
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

				//���е��������Ǽ򵥵Ĳ²�
				//����������Ϊ����ǿ�ѡ��(���һ������Ϊtrue)

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