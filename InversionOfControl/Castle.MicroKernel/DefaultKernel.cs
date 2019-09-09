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
		#region �ֶ�

		/// <summary>
		/// �����ں�
		/// </summary>
		private IKernel parentKernel;

		/// <summary>
		/// ������򹤳� <see cref="IHandlerFactory"/>
		/// </summary>
		private IHandlerFactory handlerFactory;

		/// <summary>
		/// ���ģ�͹����� <see cref="IComponentModelBuilder"/>
		/// </summary>
		private IComponentModelBuilder modelBuilder;

		/// <summary>
		/// �����ֽ���
		/// </summary>
		private IDependencyResolver resolver;

		/// <summary>
		/// ������ƹ���
		/// </summary>
		private IReleasePolicy releaserPolicy;

		/// <summary>
		/// ������ <see cref="IProxyFactory"/>
		/// </summary>
		private IProxyFactory proxyFactory;

		/// <summary>
		/// ������ʩ���� <see cref="IFacility"/> registered.
		/// </summary>
		private IList facilities;

		/// <summary>
		/// ��ϵͳ
		/// </summary>
		private IDictionary subsystems;
		
		/// <summary>
		/// ���ں�
		/// </summary>
		private IList childKernels;

		#endregion

		#region ���췽��

		/// <summary>
		/// Ĭ�Ϲ�����,��֧�ִ�����
		/// </summary>
		public DefaultKernel() : this(new NotSupportedProxyFactory())
		{
		}

        /// <summary>
        /// �ں�ָ�����캯�� <see cref="IProxyFactory"/> and <see cref="IDependencyResolver"/>
        /// </summary>
        /// <param name="resolver">�����ֽ���</param>
        /// <param name="proxyFactory">������</param>
        public DefaultKernel(IDependencyResolver resolver, IProxyFactory proxyFactory) : this(proxyFactory)
		{
			this.resolver = resolver;
			this.resolver.Initialize(new DependencyDelegate(RaiseDependencyResolving));
		}

		/// <summary>
		/// �ں�ָ�����캯�� <see cref="IProxyFactory"/>
		/// </summary>
		public DefaultKernel(IProxyFactory proxyFactory)
		{
			this.proxyFactory = proxyFactory;  //������

			this.childKernels = new ArrayList(); //���ں�
			this.facilities = new ArrayList(); //������ʩ
			this.subsystems = new Hashtable(); //��ϵͳ

			RegisterSubSystems();

			this.releaserPolicy = new LifecycledComponentsReleasePolicy(); //�ͷŹ���
			this.handlerFactory = new DefaultHandlerFactory(this); //������򹤳�
			this.modelBuilder = new DefaultComponentModelBuilder(this);//ģ�͹�����
			this.resolver = new DefaultDependencyResolver(this);//�����ֽ���
            this.resolver.Initialize(new DependencyDelegate(RaiseDependencyResolving));//�����ֽ�����ʼ������
		}

		public DefaultKernel(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			MemberInfo[] members = FormatterServices.GetSerializableMembers( GetType(), context );
			
			object[] kernelmembers = (object[]) info.GetValue( "members", typeof(object[]) );
			
			FormatterServices.PopulateObjectMembers( this, members, kernelmembers );
		}

		#endregion

		#region �����ط���

        /// <summary>
        /// ע����ϵͳ
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

		#region �ں˽ӿڳ�Ա

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="key">�����Ψһ��ʶ</param>
        /// <param name="classType">�������</param>
		public virtual void AddComponent(String key, Type classType)
		{
			if (key == null) throw new ArgumentNullException("key");
			if (classType == null) throw new ArgumentNullException("classType");

			ComponentModel model = ComponentModelBuilder.BuildModel(key, classType, classType, null);
			RaiseComponentModelCreated(model);//�������ģ���Ѿ������¼�
			IHandler handler = HandlerFactory.Create(model);
			RegisterHandler(key, handler);
		}

        /// <summary>
        /// ��Ӵ��г�����ʵ�ֵ����
        /// </summary>
        /// <param name="key">���Ψһ��ʶ</param>
        /// <param name="serviceType">�����������</param>
        /// <param name="classType">����ʵ������</param>
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
		/// ��Ӵ���Ĭ�����Ե����
		/// </summary>
		/// <param name="key">���Ψһ��ʶ</param>
		/// <param name="classType">�������</param>
		/// <param name="parameters">�����ֵ�</param>
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
        /// ��Ӵ��г�����ʵ�ֵ����,����Ĭ������
        /// </summary>
        /// <param name="key">���Ψһ��ʶ</param>
        /// <param name="serviceType">�����������</param>
        /// <param name="classType">����ʵ������</param>
        /// <param name="parameters">�����ֵ�</param>
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
		/// ������ģ��
		/// </summary>
		/// <param name="model">���ģ��</param>
		public virtual void AddCustomComponent(ComponentModel model )
		{
			if (model == null) throw new ArgumentNullException("model");

			RaiseComponentModelCreated(model);
			IHandler handler = HandlerFactory.Create(model);
			RegisterHandler(model.Name, handler);
		}

		/// <summary>
        /// �����ڻ�����ʩ(facilities),���һ��ʵ���������ʹ��
		/// </summary>
		/// <param name="key">���Ψһ��ʶ</param>
		/// <param name="instance">ʵ������</param>
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
        /// �����ڻ�����ʩ(facilities),���һ��ʵ���������ʹ��
        /// </summary>
        /// <param name="key">���Ψһ��ʶ</param>
        /// <param name="serviceType">�����������</param>
        /// <param name="instance">�������ʵ������</param>
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
        /// ���������Ψһ��ʶ,�Ƴ����
        /// </summary>
        /// <param name="key">�����Ψһ��ʶ</param>
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
        /// �ж��Ƿ�������,Ψһ��ʶ�ж�
        /// </summary>
        /// <param name="key">Ψһ��ʶ</param>
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
        /// �ж��Ƿ�������,�����������
        /// </summary>
        /// <param name="serviceType">�����������</param>
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
        /// ����Ϊһ��ʶ,��ȡ�����ʵ��
        /// </summary>
        /// <param name="key">���Ψһ��ʶ</param>
        /// <returns>�����ʵ��</returns>
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
        /// �����������,��ȡ�����ʵ��
        /// </summary>
        /// <param name="service">�������</param>
        /// <returns>�����ʵ��</returns>
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
        /// �ͷ����ʵ��
        /// </summary>
        /// <param name="instance">���ʵ��</param>
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
        /// �������Ψһ��ʶ,��ȡ����������
        /// </summary>
        /// <param name="key">���Ψһ��ʶ</param>
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
        /// ���������������,��ȡ����������
        /// </summary>
        /// <param name="service">�������</param>
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
        /// ��ȡ���е�����������,���������������
		/// </summary>
		/// <param name="service">�����������</param>
		public virtual IHandler[] GetHandlers(Type service)
		{
			return NamingSubSystem.GetHandlers(service);
		}

        /// <summary>
        /// ��ȡ���е�����������,���������������(IsAssignableFrom)
        /// </summary>
        /// <param name="service">�����������</param>
        public virtual IHandler[] GetAssignableHandlers(Type service)
		{
			return NamingSubSystem.GetAssignableHandlers(service);
		}

		public virtual IReleasePolicy ReleasePolicy
		{
			get { return releaserPolicy; }
		}

        /// <summary>
        /// ��ӻ�����ʩ
        /// </summary>
        /// <param name="key">������ʩΨһ��ʶ</param>
        /// <param name="facility">������ʩ</param>
		public virtual void AddFacility(String key, IFacility facility)
		{
			if (key == null) throw new ArgumentNullException("key");
			if (facility == null) throw new ArgumentNullException("facility");

			facility.Init(this, ConfigurationStore.GetFacilityConfiguration(key));

			facilities.Add(facility);
		}

		/// <summary>
        /// ���������Ѿ����ں�ע����Ļ�����ʩ
		/// </summary>
		public virtual IFacility[] GetFacilities()
		{
			IFacility[] list = new IFacility[ facilities.Count ];
			facilities.CopyTo(list, 0);
			return list;
		}

        /// <summary>
        /// �����ϵͳ
        /// </summary>
        /// <param name="key">Ψһ��ʶ</param>
        /// <param name="subsystem">��ϵͳ</param>
		public virtual void AddSubSystem(String key, ISubSystem subsystem)
		{
			if (key == null) throw new ArgumentNullException("key");
			if (subsystem == null) throw new ArgumentNullException("subsystem");

			subsystem.Init(this);
			subsystems[key] = subsystem;
		}

        /// <summary>
        /// ����Ψһ��ʶ,��ȡ��ϵͳ
        /// </summary>
        /// <param name="key">Ψһ��ʶ</param>
		public virtual ISubSystem GetSubSystem(String key)
		{
			if (key == null) throw new ArgumentNullException("key");

			return subsystems[key] as ISubSystem;
		}

        /// <summary>
        /// ������ں�
        /// </summary>
        /// <param name="childKernel">���ں�</param>
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
        /// �������ģ�ʹ������������
        /// ��AbstractHandlerʹ��,�����ʵ����������IComponentActivator.Create,�ͷ�����Destroy
        /// </summary>
        /// <param name="model">���ģ��</param>
        /// <returns>���������</returns>
		public virtual IComponentActivator CreateComponentActivator(ComponentModel model)
		{
			if (model == null) throw new ArgumentNullException("model");

			IComponentActivator activator = null;

			if (model.CustomComponentActivator == null)  //Ĭ�����������
			{
				activator = new DefaultComponentActivator(model, this,
								new ComponentInstanceDelegate(RaiseComponentCreated),
								new ComponentInstanceDelegate(RaiseComponentDestroyed));
			}
			else
			{
				try
				{
					activator = (IComponentActivator) //�Զ������������
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
		/// �����ͼ�ṹ
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

		#region ������Ա

		/// <summary>
		/// �ں��ͷ�
		/// </summary>
		public virtual void Dispose()
		{
			DisposeSubKernels();//�ͷ����ں�
			TerminateFacilities();//����������ʩ
			DisposeComponentsInstancesWithinTracker();//�ͷ�������������
			DisposeHandlers();//�����������ͷ�
			UnsubscribeFromParentKernel();//������ڵ���¼�����
		}

        /// <summary>
        /// ��ֹ���еĻ�����ʩ
        /// </summary>
		private void TerminateFacilities()
		{
			foreach(IFacility facility in facilities)
			{
				facility.Terminate();
			}
		}

        /// <summary>
        /// �ͷ����е�����������
        /// </summary>
		private void DisposeHandlers()
		{
			GraphNode[] nodes = GraphNodes;
			IVertex[] vertices = TopologicalSortAlgo.Sort( nodes );
	
			for(int i=0; i < vertices.Length; i++)
			{
				ComponentModel model = (ComponentModel) vertices[i];

				//��ֹ�Ƴ������������������

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

		#region ������Ա

        /// <summary>
        /// ������ϵͳ
        /// </summary>
		protected INamingSubSystem NamingSubSystem
		{
			get { return GetSubSystem(SubSystemConstants.NamingKey) as INamingSubSystem; }
		}

        /// <summary>
        /// ע�ᴦ����������
        /// </summary>
        /// <param name="key">���Ψһ��ʶ</param>
        /// <param name="handler">����������</param>
		protected void RegisterHandler(String key, IHandler handler)
		{
			NamingSubSystem.Register(key, handler);

			base.RaiseHandlerRegistered(handler);
			base.RaiseComponentRegistered(key, handler);
		}

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="handler">�����������ӿ�</param>
        /// <returns>�������</returns>
		protected object ResolveComponent(IHandler handler)
		{
			object instance = handler.Resolve();

			ReleasePolicy.Track(instance, handler);

			return instance;
		}
		#endregion

		#region ���л��뷴���л�

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
