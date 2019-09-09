namespace Castle.MicroKernel
{
	using System;
	using System.Reflection;
	using System.Collections;
	using System.Runtime.Serialization;

	using Castle.Model;
	using Castle.Model.Internal;
	
	using Castle.MicroKernel.Handlers;
	using Castle.MicroKernel.ModelBuilder;
	using Castle.MicroKernel.Resolvers;
	using Castle.MicroKernel.Releasers;
	using Castle.MicroKernel.ComponentActivator;
	using Castle.MicroKernel.Proxy;
	using Castle.MicroKernel.SubSystems.Configuration;

	[Serializable]
	public class DefaultKernel : KernelEventSupport, IKernel, IDeserializationCallback
	{
		#region 字段

		/// <summary>
		/// 父级内核
		/// </summary>
		private IKernel parentKernel;

		/// <summary>
		/// 处理程序工厂 <see cref="IHandlerFactory"/>
		/// </summary>
		private IHandlerFactory handlerFactory;

		/// <summary>
		/// 组件模型构建器 <see cref="IComponentModelBuilder"/>
		/// </summary>
		private IComponentModelBuilder modelBuilder;

		/// <summary>
		/// 依赖分解器
		/// </summary>
		private IDependencyResolver resolver;

		/// <summary>
		/// 组件控制规则
		/// </summary>
		private IReleasePolicy releaserPolicy;

		/// <summary>
		/// 代理工厂 <see cref="IProxyFactory"/>
		/// </summary>
		private IProxyFactory proxyFactory;

		/// <summary>
		/// 基础设施集合 <see cref="IFacility"/> registered.
		/// </summary>
		private IList facilities;

		/// <summary>
		/// 子系统
		/// </summary>
		private IDictionary subsystems;
		
		/// <summary>
		/// 子内核
		/// </summary>
		private IList childKernels;

		#endregion

		#region 构造方法

		/// <summary>
		/// 默认构造器,不支持代理工厂
		/// </summary>
		public DefaultKernel() : this(new NotSupportedProxyFactory())
		{
		}

        /// <summary>
        /// 内核指定构造函数 <see cref="IProxyFactory"/> and <see cref="IDependencyResolver"/>
        /// </summary>
        /// <param name="resolver">依赖分解器</param>
        /// <param name="proxyFactory">代理工厂</param>
        public DefaultKernel(IDependencyResolver resolver, IProxyFactory proxyFactory) : this(proxyFactory)
		{
			this.resolver = resolver;
			this.resolver.Initialize(new DependencyDelegate(RaiseDependencyResolving));
		}

		/// <summary>
		/// 内核指定构造函数 <see cref="IProxyFactory"/>
		/// </summary>
		public DefaultKernel(IProxyFactory proxyFactory)
		{
			this.proxyFactory = proxyFactory;  //代理工厂

			this.childKernels = new ArrayList(); //子内核
			this.facilities = new ArrayList(); //基础设施
			this.subsystems = new Hashtable(); //子系统

			RegisterSubSystems();

			this.releaserPolicy = new LifecycledComponentsReleasePolicy(); //释放规则
			this.handlerFactory = new DefaultHandlerFactory(this); //处理程序工厂
			this.modelBuilder = new DefaultComponentModelBuilder(this);//模型构建器
			this.resolver = new DefaultDependencyResolver(this);//依赖分解器
            this.resolver.Initialize(new DependencyDelegate(RaiseDependencyResolving));//依赖分解器初始化操作
		}

		public DefaultKernel(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			MemberInfo[] members = FormatterServices.GetSerializableMembers( GetType(), context );
			
			object[] kernelmembers = (object[]) info.GetValue( "members", typeof(object[]) );
			
			FormatterServices.PopulateObjectMembers( this, members, kernelmembers );
		}

		#endregion

		#region 可重载方法

        /// <summary>
        /// 注册子系统
        /// </summary>
		protected virtual void RegisterSubSystems()
		{
			AddSubSystem( SubSystemConstants.ConfigurationStoreKey, 
				new DefaultConfigurationStore() );
	
			AddSubSystem( SubSystemConstants.ConversionManagerKey, 
				new SubSystems.Conversion.DefaultConversionManager() );
	
			AddSubSystem( SubSystemConstants.NamingKey, 
				new SubSystems.Naming.DefaultNamingSubSystem() );

			AddSubSystem( SubSystemConstants.ResourceKey, 
				new SubSystems.Resource.DefaultResourceSubSystem() );
		}

		#endregion

		#region 内核接口成员

        /// <summary>
        /// 添加组件
        /// </summary>
        /// <param name="key">组件的唯一标识</param>
        /// <param name="classType">组件类型</param>
		public virtual void AddComponent(String key, Type classType)
		{
			if (key == null) throw new ArgumentNullException("key");
			if (classType == null) throw new ArgumentNullException("classType");

			ComponentModel model = ComponentModelBuilder.BuildModel(key, classType, classType, null);
			RaiseComponentModelCreated(model);//触发组件模型已经创建事件
			IHandler handler = HandlerFactory.Create(model);
			RegisterHandler(key, handler);
		}

        /// <summary>
        /// 添加带有抽象与实现的组件
        /// </summary>
        /// <param name="key">组件唯一标识</param>
        /// <param name="serviceType">抽象服务类型</param>
        /// <param name="classType">具体实现类型</param>
		public virtual void AddComponent(String key, Type serviceType, Type classType)
		{
			if (key == null) throw new ArgumentNullException("key");
			if (serviceType == null) throw new ArgumentNullException("serviceType");
			if (classType == null) throw new ArgumentNullException("classType");

			ComponentModel model = ComponentModelBuilder.BuildModel(key, serviceType, classType, null);
			RaiseComponentModelCreated(model);
			IHandler handler = HandlerFactory.Create(model);
			RegisterHandler(key, handler);
		}

		/// <summary>
		/// 添加带有默认属性的组件
		/// </summary>
		/// <param name="key">组件唯一标识</param>
		/// <param name="classType">组件类型</param>
		/// <param name="parameters">属性字典</param>
		public virtual void AddComponentWithProperties( String key, Type classType, IDictionary parameters )
		{
			if (key == null) throw new ArgumentNullException("key");
			if (parameters == null) throw new ArgumentNullException("parameters");
			if (classType == null) throw new ArgumentNullException("classType");

			ComponentModel model = ComponentModelBuilder.BuildModel(key, classType, classType, parameters);
			RaiseComponentModelCreated(model);
			IHandler handler = HandlerFactory.Create(model);
			RegisterHandler(key, handler);
		}

        /// <summary>
        /// 添加带有抽象与实现的组件,带有默认属性
        /// </summary>
        /// <param name="key">组件唯一标识</param>
        /// <param name="serviceType">抽象服务类型</param>
        /// <param name="classType">具体实现类型</param>
        /// <param name="parameters">属性字典</param>
        public virtual void AddComponentWithProperties( String key, Type serviceType, Type classType, IDictionary parameters )
		{
			if (key == null) throw new ArgumentNullException("key");
			if (parameters == null) throw new ArgumentNullException("parameters");
			if (serviceType == null) throw new ArgumentNullException("serviceType");
			if (classType == null) throw new ArgumentNullException("classType");

			ComponentModel model = ComponentModelBuilder.BuildModel(key, serviceType, classType, parameters);
			RaiseComponentModelCreated(model);
			IHandler handler = HandlerFactory.Create(model);
			RegisterHandler(key, handler);
		}

		/// <summary>
		/// 添加组件模型
		/// </summary>
		/// <param name="model">组件模型</param>
		public virtual void AddCustomComponent(ComponentModel model )
		{
			if (model == null) throw new ArgumentNullException("model");

			RaiseComponentModelCreated(model);
			IHandler handler = HandlerFactory.Create(model);
			RegisterHandler(model.Name, handler);
		}

		/// <summary>
        /// 常用于基础设施(facilities),添加一个实例当组件来使用
		/// </summary>
		/// <param name="key">组件唯一标识</param>
		/// <param name="instance">实例对象</param>
		public void AddComponentInstance( String key, object instance )
		{
			if (key == null) throw new ArgumentNullException("key");
			if (instance == null) throw new ArgumentNullException("instance");
			
			Type classType = instance.GetType();

			ComponentModel model = new ComponentModel(key, classType, classType);
			model.CustomComponentActivator = typeof(ExternalInstanceActivator);
			model.ExtendedProperties["instance"] = instance;
			
			RaiseComponentModelCreated(model);
			IHandler handler = HandlerFactory.Create(model);
			RegisterHandler(key, handler);
		}

        /// <summary>
        /// 常用于基础设施(facilities),添加一个实例当组件来使用
        /// </summary>
        /// <param name="key">组件唯一标识</param>
        /// <param name="serviceType">组件服务类型</param>
        /// <param name="instance">组件具体实例对象</param>
        public void AddComponentInstance( String key, Type serviceType, object instance )
		{
			if (key == null) throw new ArgumentNullException("key");
			if (serviceType == null) throw new ArgumentNullException("serviceType");
			if (instance == null) throw new ArgumentNullException("instance");
			
			Type classType = instance.GetType();

			ComponentModel model = new ComponentModel(key, serviceType, classType);
			model.CustomComponentActivator = typeof(ExternalInstanceActivator);
			model.ExtendedProperties["instance"] = instance;

			RaiseComponentModelCreated(model);
			IHandler handler = HandlerFactory.Create(model);
			RegisterHandler(key, handler);
		}

        /// <summary>
        /// 根据组件的唯一标识,移除组件
        /// </summary>
        /// <param name="key">组件的唯一标识</param>
		public virtual bool RemoveComponent(String key)
		{
			if (key == null) throw new ArgumentNullException("key");

			if (NamingSubSystem.Contains(key))
			{
				IHandler handler = GetHandler(key);

				if (handler.ComponentModel.Dependers.Length == 0)
				{
					NamingSubSystem.UnRegister(key);

					if (GetHandler(handler.ComponentModel.Service) == handler)
					{
						NamingSubSystem.UnRegister(handler.ComponentModel.Service);
					}

					foreach(ComponentModel model in handler.ComponentModel.Dependents)
					{
						model.RemoveDepender(handler.ComponentModel);
					}

					RaiseComponentUnregistered(key, handler);

					DisposeHandler(handler);

					return true;
				}
				else
				{
					// We can't remove this component as there are
					// others which depends on it

					return false;
				}
			}
			
			if (Parent != null)
			{
				return Parent.RemoveComponent(key);
			}

			return false;
		}

        /// <summary>
        /// 判断是否包含组件,唯一标识判断
        /// </summary>
        /// <param name="key">唯一标识</param>
		public virtual bool HasComponent(String key)
		{
			if (key == null) throw new ArgumentNullException("key");
			
			if (NamingSubSystem.Contains(key))
			{
				return true;
			}

			if (Parent != null)
			{
				return Parent.HasComponent(key);
			}

			return false;
		}

        /// <summary>
        /// 判断是否包含组件,组件服务类型
        /// </summary>
        /// <param name="serviceType">组件服务类型</param>
		public virtual bool HasComponent(Type serviceType)
		{
			if (serviceType == null) throw new ArgumentNullException("serviceType");

			if (NamingSubSystem.Contains(serviceType))
			{
				return true;
			}

			if (Parent != null)
			{
				return Parent.HasComponent(serviceType);
			}

			return false;
		}

        /// <summary>
        /// 根据为一标识,获取组件的实例
        /// </summary>
        /// <param name="key">组件唯一标识</param>
        /// <returns>组件的实例</returns>
		public virtual object this[String key]
		{
			get
			{
				if (key == null) throw new ArgumentNullException("key");

				if (!HasComponent(key))
				{
					throw new ComponentNotFoundException(key);
				}

				IHandler handler = GetHandler(key);

				return ResolveComponent(handler);
			}
		}

        /// <summary>
        /// 根据组件类型,获取组件的实例
        /// </summary>
        /// <param name="service">组件类型</param>
        /// <returns>组件的实例</returns>
		public virtual object this[Type service]
		{
			get
			{
				if (service == null) throw new ArgumentNullException("service");

				if (!HasComponent(service))
				{
					throw new ComponentNotFoundException(service);
				}

				IHandler handler = GetHandler(service);

				return ResolveComponent(handler);
			}
		}

        /// <summary>
        /// 释放组件实例
        /// </summary>
        /// <param name="instance">组件实例</param>
		public virtual void ReleaseComponent(object instance)
		{
			if (ReleasePolicy.HasTrack(instance))
			{
				ReleasePolicy.Release(instance);
			}
			else
			{
				if (Parent != null)
				{
					Parent.ReleaseComponent(instance);
				}
			}
		}

		public IHandlerFactory HandlerFactory
		{
			get { return handlerFactory; }
		}

		public IComponentModelBuilder ComponentModelBuilder
		{
			get { return modelBuilder; }
			set { modelBuilder = value; }
		}

		public IProxyFactory ProxyFactory
		{
			get { return proxyFactory; }
			set { proxyFactory = value; }
		}

		public virtual IConfigurationStore ConfigurationStore
		{
			get { return GetSubSystem(SubSystemConstants.ConfigurationStoreKey) as IConfigurationStore; }
			set { AddSubSystem(SubSystemConstants.ConfigurationStoreKey, value); }
		}

        /// <summary>
        /// 根据组件唯一标识,获取组件处理程序
        /// </summary>
        /// <param name="key">组件唯一标识</param>
		public virtual IHandler GetHandler(String key)
		{
			if (key == null) throw new ArgumentNullException("key");

			IHandler handler = NamingSubSystem.GetHandler(key);

			if (handler == null && Parent != null)
			{
				handler = Parent.GetHandler(key);
			}

			return handler;
		}

        /// <summary>
        /// 根据组件服务类型,获取组件处理程序
        /// </summary>
        /// <param name="service">组件类型</param>
		public virtual IHandler GetHandler(Type service)
		{
			if (service == null) throw new ArgumentNullException("service");

			IHandler handler = NamingSubSystem.GetHandler(service);

			if (handler == null && Parent != null)
			{
				handler = Parent.GetHandler(service);
			}

			return handler;
		}

		/// <summary>
        /// 获取所有的组件处理程序,根据组件服务类型
		/// </summary>
		/// <param name="service">组件服务类型</param>
		public virtual IHandler[] GetHandlers(Type service)
		{
			return NamingSubSystem.GetHandlers(service);
		}

        /// <summary>
        /// 获取所有的组件处理程序,根据组件服务类型(IsAssignableFrom)
        /// </summary>
        /// <param name="service">组件服务类型</param>
        public virtual IHandler[] GetAssignableHandlers(Type service)
		{
			return NamingSubSystem.GetAssignableHandlers(service);
		}

		public virtual IReleasePolicy ReleasePolicy
		{
			get { return releaserPolicy; }
		}

        /// <summary>
        /// 添加基础设施
        /// </summary>
        /// <param name="key">基础设施唯一标识</param>
        /// <param name="facility">基础设施</param>
		public virtual void AddFacility(String key, IFacility facility)
		{
			if (key == null) throw new ArgumentNullException("key");
			if (facility == null) throw new ArgumentNullException("facility");

			facility.Init(this, ConfigurationStore.GetFacilityConfiguration(key));

			facilities.Add(facility);
		}

		/// <summary>
        /// 返回所有已经在内核注册过的基础设施
		/// </summary>
		public virtual IFacility[] GetFacilities()
		{
			IFacility[] list = new IFacility[ facilities.Count ];
			facilities.CopyTo(list, 0);
			return list;
		}

        /// <summary>
        /// 添加子系统
        /// </summary>
        /// <param name="key">唯一标识</param>
        /// <param name="subsystem">子系统</param>
		public virtual void AddSubSystem(String key, ISubSystem subsystem)
		{
			if (key == null) throw new ArgumentNullException("key");
			if (subsystem == null) throw new ArgumentNullException("subsystem");

			subsystem.Init(this);
			subsystems[key] = subsystem;
		}

        /// <summary>
        /// 根据唯一标识,获取子系统
        /// </summary>
        /// <param name="key">唯一标识</param>
		public virtual ISubSystem GetSubSystem(String key)
		{
			if (key == null) throw new ArgumentNullException("key");

			return subsystems[key] as ISubSystem;
		}

        /// <summary>
        /// 添加子内核
        /// </summary>
        /// <param name="childKernel">子内核</param>
		public virtual void AddChildKernel(IKernel childKernel)
		{
			if (childKernel == null) throw new ArgumentNullException("childKernel");

			childKernel.Parent = this;
			childKernels.Add(childKernel);
		}

		public virtual IKernel Parent
		{
			get { return parentKernel; }
			set
			{
				parentKernel = value;

				parentKernel.ComponentRegistered += new ComponentDataDelegate(RaiseComponentRegistered);
				parentKernel.ComponentUnregistered += new ComponentDataDelegate(RaiseComponentUnregistered);

				RaiseAddedAsChildKernel();
			}
		}

		public IDependencyResolver Resolver
		{
			get { return resolver; }
		}

        /// <summary>
        /// 根据组件模型创建组件激活器
        /// 被AbstractHandler使用,组件的实例化都落在IComponentActivator.Create,释放落在Destroy
        /// </summary>
        /// <param name="model">组件模型</param>
        /// <returns>组件激活器</returns>
		public virtual IComponentActivator CreateComponentActivator(ComponentModel model)
		{
			if (model == null) throw new ArgumentNullException("model");

			IComponentActivator activator = null;

			if (model.CustomComponentActivator == null)  //默认组件激活器
			{
				activator = new DefaultComponentActivator(model, this,
								new ComponentInstanceDelegate(RaiseComponentCreated),
								new ComponentInstanceDelegate(RaiseComponentDestroyed));
			}
			else
			{
				try
				{
					activator = (IComponentActivator) //自定义组件激活器
						Activator.CreateInstance(model.CustomComponentActivator,
						    new object[]
						    {
						        model, this,
						        new ComponentInstanceDelegate(RaiseComponentCreated),
						        new ComponentInstanceDelegate(RaiseComponentDestroyed)
						    });
				}
				catch (Exception e)
				{
					throw new KernelException("Could not instantiate custom activator", e);
				}
			}

			return activator;
		}

		/// <summary>
		/// 组件的图结构
		/// </summary>
		public GraphNode[] GraphNodes 
		{ 
			get
			{
				GraphNode[] nodes = new GraphNode[ NamingSubSystem.ComponentCount ];

				int index = 0;

				IHandler[] handlers = NamingSubSystem.GetHandlers();

				foreach(IHandler handler in handlers)
				{
					nodes[index++] = handler.ComponentModel;
				}

				return nodes;
			} 
		}

		#endregion

		#region 析构成员

		/// <summary>
		/// 内核释放
		/// </summary>
		public virtual void Dispose()
		{
			DisposeSubKernels();//释放子内核
			TerminateFacilities();//结束基础设施
			DisposeComponentsInstancesWithinTracker();//释放组件规则跟踪器
			DisposeHandlers();//组件处理程序释放
			UnsubscribeFromParentKernel();//解除父节点的事件订阅
		}

        /// <summary>
        /// 终止所有的基础设施
        /// </summary>
		private void TerminateFacilities()
		{
			foreach(IFacility facility in facilities)
			{
				facility.Terminate();
			}
		}

        /// <summary>
        /// 释放所有的组件处理程序
        /// </summary>
		private void DisposeHandlers()
		{
			GraphNode[] nodes = GraphNodes;
			IVertex[] vertices = TopologicalSortAlgo.Sort( nodes );
	
			for(int i=0; i < vertices.Length; i++)
			{
				ComponentModel model = (ComponentModel) vertices[i];

				//防止移除属于其他容器的组件

				if (!NamingSubSystem.Contains(model.Name)) continue;
				
				bool successOnRemoval = RemoveComponent( model.Name );

				System.Diagnostics.Debug.Assert( successOnRemoval );
			}
		}

		private void UnsubscribeFromParentKernel()
		{
			if (Parent != null)
			{
				Parent.ComponentRegistered -= new ComponentDataDelegate(RaiseComponentRegistered);
				Parent.ComponentUnregistered -= new ComponentDataDelegate(RaiseComponentUnregistered);
			}
		}

		private void DisposeComponentsInstancesWithinTracker()
		{
			ReleasePolicy.Dispose();
		}

		private void DisposeSubKernels()
		{
			foreach(IKernel childKernel in childKernels)
			{
				childKernel.Dispose();
			}
		}

		protected void DisposeHandler(IHandler handler)
		{
			if (handler == null) return;

			if (handler is IDisposable)
			{
				((IDisposable) handler).Dispose();
			}
		}

		#endregion

		#region 保护成员

        /// <summary>
        /// 命名子系统
        /// </summary>
		protected INamingSubSystem NamingSubSystem
		{
			get { return GetSubSystem(SubSystemConstants.NamingKey) as INamingSubSystem; }
		}

        /// <summary>
        /// 注册处理的组件程序
        /// </summary>
        /// <param name="key">组件唯一标识</param>
        /// <param name="handler">组件处理程序</param>
		protected void RegisterHandler(String key, IHandler handler)
		{
			NamingSubSystem.Register(key, handler);

			base.RaiseHandlerRegistered(handler);
			base.RaiseComponentRegistered(key, handler);
		}

        /// <summary>
        /// 解析组件
        /// </summary>
        /// <param name="handler">处理组件程序接口</param>
        /// <returns>组件对象</returns>
		protected object ResolveComponent(IHandler handler)
		{
			object instance = handler.Resolve();

			ReleasePolicy.Track(instance, handler);

			return instance;
		}
		#endregion

		#region 序列化与反序列化

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);

			MemberInfo[] members = FormatterServices.GetSerializableMembers( GetType(), context );

			object[] kernelmembers = FormatterServices.GetObjectData(this, members);

			info.AddValue( "members", kernelmembers, typeof(object[]) );
		}

		void IDeserializationCallback.OnDeserialization(object sender)
		{
		}

		#endregion
	}
}
