namespace Castle.MicroKernel.ModelBuilder.Inspectors
{
	using System;
	using System.Configuration;
	using Castle.MicroKernel.Util;
	using Castle.Model;
	using Castle.Model.Configuration;

    /// <summary>
    /// 检查组件是否使用 <c>InterceptorAttribute</c>
    /// 或者配置信息中有interceptors
    /// </summary>
    [Serializable]
	public class InterceptorInspector : IContributeComponentModelConstruction
	{
		public virtual void ProcessModel(IKernel kernel, ComponentModel model)
		{
			CollectFromAttributes(model); //收集来自属性标记
			CollectFromConfiguration(model);//收集来自配置文件
		}

		protected virtual void CollectFromConfiguration(ComponentModel model)
		{
			if (model.Configuration == null) return;

			IConfiguration interceptors = model.Configuration.Children["interceptors"];

			if (interceptors == null) return;

			foreach(IConfiguration interceptor in interceptors.Children)
			{
				String value = interceptor.Value;

				if (!ReferenceExpressionUtil.IsReference(value))
				{
					String message = String.Format(
						"The value for the interceptor must be a reference " + 
						"to a component (Currently {0})", 
						value);

					throw new ConfigurationException(message);
				}

				InterceptorReference interceptorRef = new InterceptorReference(ReferenceExpressionUtil.ExtractComponentKey(value));
				
				model.Interceptors.Add(interceptorRef);
				model.Dependencies.Add(CreateDependencyModel(interceptorRef));
			}
		}

		protected virtual void CollectFromAttributes(ComponentModel model)
		{
			if (!model.Implementation.IsDefined(typeof(InterceptorAttribute),true))
			{
				return;
			}

			object[] attributes = model.Implementation.GetCustomAttributes(true);

			foreach(object attribute in attributes)
			{
				if (attribute is InterceptorAttribute)
				{
					InterceptorAttribute attr = (attribute as InterceptorAttribute);
					AddInterceptor(attr.Interceptor,model.Interceptors);
					model.Dependencies.Add(CreateDependencyModel(attr.Interceptor));
				}
			}
		}

		protected DependencyModel CreateDependencyModel(InterceptorReference interceptor)
		{
			return new DependencyModel(DependencyType.Service, interceptor.ComponentKey,interceptor.ServiceType, false);
		}

		protected void AddInterceptor(InterceptorReference interceptorRef, InterceptorReferenceCollection interceptors)
		{
			interceptors.Add(interceptorRef);
		}
	}
}
