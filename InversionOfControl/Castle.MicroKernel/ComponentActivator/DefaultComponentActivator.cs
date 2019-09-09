namespace Castle.MicroKernel.ComponentActivator
{
	using System;
	using System.Reflection;
	using Castle.Model;
	using Castle.MicroKernel.LifecycleConcerns;

    /// <summary>
    /// 标准的实现<see cref="IComponentActivator"/>是处理构造,填充可写的属性
    /// </summary>
    /// <remarks>
    /// 自定义组件激活器仅需要重写 CreateInstance方法
    /// </remarks>
    [Serializable]
	public class DefaultComponentActivator : AbstractComponentActivator
	{
		public DefaultComponentActivator(ComponentModel model, IKernel kernel, 
			ComponentInstanceDelegate onCreation, 
			ComponentInstanceDelegate onDestruction) : base(model, kernel, onCreation, onDestruction)
		{
		}

		#region 实现抽象函数的抽象成员
		protected override sealed object InternalCreate()
		{
			object instance = Instantiate(); //创建对象

			SetUpProperties(instance); //属性赋值

			ApplyCommissionConcerns(instance);//调用初始化关注点方法

			return instance;
		}

		protected override void InternalDestroy(object instance)
		{
			ApplyDecommissionConcerns(instance); //调用析构关注点方法
		}

		#endregion

		protected virtual object Instantiate()
		{
			ConstructorCandidate candidate = SelectEligibleConstructor();
	
			Type[] signature;
			object[] arguments = CreateConstructorArguments(candidate, out signature );
	
			return CreateInstance(arguments, signature);
		}

		protected virtual object CreateInstance(object[] arguments, Type[] signature)
		{
			object instance;

			if (Model.Interceptors.HasInterceptors)
			{
				try
				{
					instance = Kernel.ProxyFactory.Create(Kernel, Model, arguments);
				}
				catch(Exception ex)
				{
					throw new ComponentActivatorException("ComponentActivator: could not proxy " + Model.Implementation.FullName, ex);
				}
			}
			else
			{
				try
				{
					ConstructorInfo cinfo = Model.Implementation.GetConstructor(BindingFlags.Public|BindingFlags.Instance, null, signature, null);

					instance = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(Model.Implementation); //实例化对象,但是构造函数是不被调用

					cinfo.Invoke(instance, arguments); //使用System.Reflection中的Invoke速度很慢,Invoke方式调用构造函数中的方法
                }
				catch(Exception ex)
				{
					throw new ComponentActivatorException("ComponentActivator: could not instantiate " + Model.Implementation.FullName, ex);
				}
			}
			
			return instance;
		}

		protected virtual void ApplyCommissionConcerns(object instance )
		{
			object[] steps = Model.LifecycleSteps.GetCommissionSteps();
			ApplyConcerns(steps, instance);
		}

		protected virtual void ApplyDecommissionConcerns(object instance )
		{
			object[] steps = Model.LifecycleSteps.GetDecommissionSteps();
			ApplyConcerns(steps, instance);
		}

		protected virtual void ApplyConcerns(object[] steps, object instance )
		{
			foreach (ILifecycleConcern concern in steps)
			{
				concern.Apply(Model, instance);
			}
		}

        /// <summary>
        /// 选择合格的构造函数
        /// </summary>
		protected virtual ConstructorCandidate SelectEligibleConstructor()
		{
			if (Model.Constructors.Count == 0) //没有构造函数的情况
			{
				return null;
			}

			if (Model.Constructors.BestCandidate != null)
			{
				return Model.Constructors.BestCandidate;
			}

			if (Model.Constructors.Count == 1)
			{
				return Model.Constructors.FewerArgumentsCandidate;
			}

			ConstructorCandidate winnerCandidate = null; 

			foreach(ConstructorCandidate candidate in Model.Constructors)
			{
				foreach(DependencyModel dep in candidate.Dependencies)
				{
					if (CanSatisfyDependency(dep))
					{
						candidate.Points += 2;
					}
					else
					{
						candidate.Points -= 2;
					}
				}

				if (winnerCandidate == null) winnerCandidate = candidate;

				if (winnerCandidate.Points < candidate.Points)
				{
					winnerCandidate = candidate;
				}
			}

			if (winnerCandidate == null)
			{
				throw new ComponentActivatorException("Could not find eligible constructor.");
			}

			Model.Constructors.BestCandidate = winnerCandidate;

			return winnerCandidate;
		}

        /// <summary>
        /// 是否可以被解析进行评估
        /// </summary>
		protected virtual bool CanSatisfyDependency(DependencyModel dep)
		{
			return Kernel.Resolver.CanResolve(Model, dep);
		}

        /// <summary>
        /// 创建构造函数的参数数组
        /// </summary>
        /// <param name="constructor">构造函数候选者</param>
        /// <param name="signature">参数类型数组</param>
		protected virtual object[] CreateConstructorArguments(ConstructorCandidate constructor, out Type[] signature )
		{
			signature = null;

			if (constructor == null) return new object[0];

			object[] arguments = new object[constructor.Constructor.GetParameters().Length];
			signature = new Type[arguments.Length];

			int index = 0;

			foreach(DependencyModel dependency in constructor.Dependencies)
			{
				object value = Kernel.Resolver.Resolve(Model, dependency);
				arguments[index] = value;
				signature[index++] = dependency.TargetType;
			}

			return arguments;
		}

        /// <summary>
        /// 初始化属性值
        /// </summary>
		protected virtual void SetUpProperties(object instance)
		{
			foreach(PropertySet property in Model.Properties)
			{
				object value = Kernel.Resolver.Resolve(Model, property.Dependency);

				if (value == null) continue;

				MethodInfo setMethod = property.Property.GetSetMethod();
				setMethod.Invoke(instance, new object[] { value } ); //对属性方法并没有做最优化的处理,仅适用反射完成
			}
		}
	}
}
