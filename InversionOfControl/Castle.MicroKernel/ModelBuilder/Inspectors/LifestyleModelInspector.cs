namespace Castle.MicroKernel.ModelBuilder.Inspectors
{
	using System;
	using System.Configuration;

	using Castle.Model;

    /// <summary>
    /// 查询组件的配置,找到组件的生命周期类型
    /// </summary>
    /// <remarks>
    /// 该组件模型贡献者不能保证获得一种生命周期类型
    /// 如果什么也找不到，它就不会修改模型;在这种情况下,由内核来为组件建立默认的生命周期类型
    /// </remarks>
    [Serializable]
	public class LifestyleModelInspector : IContributeComponentModelConstruction
	{
		/// <summary>
        /// 优先配置里面的生命周期配置,如果没有则查找具体实现类型的生命周期特性
		/// </summary>
		/// <param name="kernel">内核</param>
		/// <param name="model">组件模型</param>
		public virtual void ProcessModel(IKernel kernel, ComponentModel model)
		{
			if (!ReadLifestyleFromConfiguration(model))
			{
				ReadLifestyleFromType(model);
			}
		}

        /// <summary>
        /// 从配置中读取lifestyle属性,然后转换为LifestyleType枚举类型<see cref="LifestyleType"/> 
        /// </summary>
        /// <param name="model">组件模型</param>
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
						ExtractPoolConfig(model); //细致分析池配置信息
					}
					else if(model.LifestyleType == LifestyleType.Custom)
					{
						ExtractCustomConfig(model);//细致分析自定义配置信息
					}

					return true;
				}
			}
			return false;
		}

        /// <summary>
        /// 细致分析对象池配置信息
        /// </summary>
        /// <param name="model">组件模型</param>
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
        /// 细致分析自定义配置信息
        /// </summary>
        /// <param name="model">组件模型</param>
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
        /// 检查组件类型暴露的Lifestyle属性标记
        /// </summary>
        /// <param name="model">组件模型</param>
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
