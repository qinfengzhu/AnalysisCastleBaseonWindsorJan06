namespace Castle.MicroKernel.ModelBuilder.Inspectors
{
	using System;
	using System.Configuration;

	using Castle.Model;

    /// <summary>
    /// ��ѯ���������,�ҵ������������������
    /// </summary>
    /// <remarks>
    /// �����ģ�͹����߲��ܱ�֤���һ��������������
    /// ���ʲôҲ�Ҳ��������Ͳ����޸�ģ��;�����������,���ں���Ϊ�������Ĭ�ϵ�������������
    /// </remarks>
    [Serializable]
	public class LifestyleModelInspector : IContributeComponentModelConstruction
	{
		/// <summary>
        /// �������������������������,���û������Ҿ���ʵ�����͵�������������
		/// </summary>
		/// <param name="kernel">�ں�</param>
		/// <param name="model">���ģ��</param>
		public virtual void ProcessModel(IKernel kernel, ComponentModel model)
		{
			if (!ReadLifestyleFromConfiguration(model))
			{
				ReadLifestyleFromType(model);
			}
		}

        /// <summary>
        /// �������ж�ȡlifestyle����,Ȼ��ת��ΪLifestyleTypeö������<see cref="LifestyleType"/> 
        /// </summary>
        /// <param name="model">���ģ��</param>
        protected virtual bool ReadLifestyleFromConfiguration(ComponentModel model)
		{
			if (model.Configuration != null)
			{
				String lifestyle = model.Configuration.Attributes["lifestyle"];
				
				if (lifestyle != null)
				{
					try
					{
						LifestyleType type = (LifestyleType)Enum.Parse(typeof(LifestyleType), lifestyle, true);
						model.LifestyleType = type;

					}
					catch(Exception ex)
					{
						String message = String.Format("Could not convert the specified attribute value " + "{0} to a valid LifestyleType enum type", lifestyle);						
						throw new ConfigurationException(message, ex);
					}

					if (model.LifestyleType == LifestyleType.Pooled)
					{
						ExtractPoolConfig(model); //ϸ�·�����������Ϣ
					}
					else if(model.LifestyleType == LifestyleType.Custom)
					{
						ExtractCustomConfig(model);//ϸ�·����Զ���������Ϣ
					}

					return true;
				}
			}
			return false;
		}

        /// <summary>
        /// ϸ�·��������������Ϣ
        /// </summary>
        /// <param name="model">���ģ��</param>
		private void ExtractPoolConfig(ComponentModel model)
		{
			String initial = model.Configuration.Attributes["initialPoolSize"];
			String maxSize = model.Configuration.Attributes["maxPoolSize"];
	
			if (initial != null)
			{
				model.ExtendedProperties[ExtendedPropertiesConstants.Pool_InitialPoolSize] = Convert.ToInt32(initial);
			}
			if (maxSize != null)
			{
				model.ExtendedProperties[ExtendedPropertiesConstants.Pool_MaxPoolSize] = Convert.ToInt32(maxSize);
			}
		}

        /// <summary>
        /// ϸ�·����Զ���������Ϣ
        /// </summary>
        /// <param name="model">���ģ��</param>
		private void ExtractCustomConfig(ComponentModel model)
		{
			String customLifestyleType = model.Configuration.Attributes["customLifestyleType"];

			if(customLifestyleType != null)
			{
				try
				{
					model.CustomLifestyle = Type.GetType(customLifestyleType, true, false);
				}
				catch(Exception ex)
				{
					String message = String.Format(
						"The Type {0} specified  in the customLifestyleType attribute could not be loaded.", customLifestyleType);

					throw new ConfigurationException(message, ex);
				}
			}
			else
			{
				throw new ConfigurationException(@"The attribute 'customLifestyleType' must be specified in conjunction with the 'lifestyle' attribute set to ""custom"".");
			}
		}

        /// <summary>
        /// ���������ͱ�¶��Lifestyle���Ա��
        /// </summary>
        /// <param name="model">���ģ��</param>
        protected virtual void ReadLifestyleFromType(ComponentModel model)
		{
			object[] attributes = model.Implementation.GetCustomAttributes(typeof(LifestyleAttribute),true);

			if (attributes.Length != 0)
			{
				LifestyleAttribute attribute = (LifestyleAttribute)attributes[0];

				model.LifestyleType = attribute.Lifestyle;

				if (model.LifestyleType == LifestyleType.Custom)
				{
					CustomLifestyleAttribute custom = (CustomLifestyleAttribute)attribute;
					model.CustomLifestyle = custom.LifestyleHandlerType;
				}
				else if (model.LifestyleType == LifestyleType.Pooled)
				{
					PooledAttribute pooled = (PooledAttribute) attribute;
					model.ExtendedProperties[ExtendedPropertiesConstants.Pool_InitialPoolSize] = pooled.InitialPoolSize;
					model.ExtendedProperties[ExtendedPropertiesConstants.Pool_MaxPoolSize] = pooled.MaxPoolSize;
				}
			}
		}
	}
}
