namespace Castle.MicroKernel.ComponentActivator
{
	using System;
	using System.Reflection;
	using Castle.Model;
	using Castle.MicroKernel.LifecycleConcerns;

    /// <summary>
    /// ��׼��ʵ��<see cref="IComponentActivator"/>�Ǵ�����,����д������
    /// </summary>
    /// <remarks>
    /// �Զ����������������Ҫ��д CreateInstance����
    /// </remarks>
    [Serializable]
	public class DefaultComponentActivator : AbstractComponentActivator
	{
		public DefaultComponentActivator(ComponentModel model, IKernel kernel, 
			ComponentInstanceDelegate onCreation, 
			ComponentInstanceDelegate onDestruction) : base(model, kernel, onCreation, onDestruction)
		{
		}

		#region ʵ�ֳ������ĳ����Ա
		protected override sealed object InternalCreate()
		{
			object instance = Instantiate(); //��������

			SetUpProperties(instance); //���Ը�ֵ

			ApplyCommissionConcerns(instance);//���ó�ʼ����ע�㷽��

			return instance;
		}

		protected override void InternalDestroy(object instance)
		{
			ApplyDecommissionConcerns(instance); //����������ע�㷽��
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

					instance = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(Model.Implementation); //ʵ��������,���ǹ��캯���ǲ�������

					cinfo.Invoke(instance, arguments); //ʹ��System.Reflection�е�Invoke�ٶȺ���,Invoke��ʽ���ù��캯���еķ���
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
        /// ѡ��ϸ�Ĺ��캯��
        /// </summary>
		protected virtual ConstructorCandidate SelectEligibleConstructor()
		{
			if (Model.Constructors.Count == 0) //û�й��캯�������
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
        /// �Ƿ���Ա�������������
        /// </summary>
		protected virtual bool CanSatisfyDependency(DependencyModel dep)
		{
			return Kernel.Resolver.CanResolve(Model, dep);
		}

        /// <summary>
        /// �������캯���Ĳ�������
        /// </summary>
        /// <param name="constructor">���캯����ѡ��</param>
        /// <param name="signature">������������</param>
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
        /// ��ʼ������ֵ
        /// </summary>
		protected virtual void SetUpProperties(object instance)
		{
			foreach(PropertySet property in Model.Properties)
			{
				object value = Kernel.Resolver.Resolve(Model, property.Dependency);

				if (value == null) continue;

				MethodInfo setMethod = property.Property.GetSetMethod();
				setMethod.Invoke(instance, new object[] { value } ); //�����Է�����û�������Ż��Ĵ���,�����÷������
			}
		}
	}
}
