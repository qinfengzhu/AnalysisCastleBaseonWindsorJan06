namespace Castle.Facilities.FactorySupport
{
	using System;
	using System.Collections;
	using System.Reflection;

	using Castle.Model;
	
	using Castle.MicroKernel;
	using Castle.MicroKernel.ComponentActivator;
	using Castle.MicroKernel.Facilities;
	using Castle.MicroKernel.SubSystems.Conversion;


	/// <summary>
	/// 
	/// </summary>
	public class FactoryActivator : DefaultComponentActivator
	{
		public FactoryActivator(ComponentModel model, IKernel kernel, ComponentInstanceDelegate onCreation, ComponentInstanceDelegate onDestruction) : base(model, kernel, onCreation, onDestruction)
		{
		}

		protected override object Instantiate()
		{
			String factoryId = (String) Model.ExtendedProperties["factoryId"];
			String factoryCreate = (String) Model.ExtendedProperties["factoryCreate"];

			if (!Kernel.HasComponent( factoryId ))
			{
				String message = String.Format("You have specified a factory ('{2}') " + 
					"for the component '{0}' {1} but the kernel does not have this " + 
					"factory registered", 
					Model.Name, Model.Implementation.FullName, factoryId);
				throw new FacilityException(message);
			}

			IHandler factoryHandler = Kernel.GetHandler( factoryId );

			// Let's find out whether the create method is a static or instance method

			Type factoryType = factoryHandler.ComponentModel.Implementation;

			MethodInfo staticCreateMethod = 
				factoryType.GetMethod( factoryCreate, 
					BindingFlags.Public|BindingFlags.Static );

			MethodInfo instanceCreateMethod = 
				factoryType.GetMethod( factoryCreate, 
					BindingFlags.Public|BindingFlags.Instance );

			if (staticCreateMethod != null)
			{
				return Create(null, factoryId, staticCreateMethod, factoryCreate);
			}
			else if (instanceCreateMethod != null)
			{
				object factoryInstance = Kernel[ factoryId ];

				return Create(factoryInstance, factoryId, instanceCreateMethod, factoryCreate);
			}
			else
			{
				String message = String.Format("You have specified a factory " +
					"('{2}' - method to be called: {3}) " + 
					"for the component '{0}' {1} but we couldn't find the creation method" + 
					"(neither instance or static method with the name '{3}')", 
					Model.Name, Model.Implementation.FullName, factoryId, factoryCreate);
				throw new FacilityException(message);
			}
		}

		private object Create(object factoryInstance, string factoryId, 
			MethodInfo instanceCreateMethod, string factoryCreate)
		{
			ITypeConverter converter = (ITypeConverter) Kernel.GetSubSystem( SubSystemConstants.ConversionManagerKey );

			try
			{
				ParameterInfo[] parameters = instanceCreateMethod.GetParameters();

				ArrayList methodArgs = new ArrayList();

				foreach(ParameterInfo parameter in parameters)
				{
					Type paramType = parameter.ParameterType;

					DependencyModel depModel = null;

					if ( converter.CanHandleType(paramType) )
					{
						depModel = new DependencyModel( 
							DependencyType.Parameter, parameter.Name, paramType, false );
					}
					else
					{
						depModel = new DependencyModel(
							DependencyType.Service, parameter.Name, paramType, false );
					}

					if (!Kernel.Resolver.CanResolve(Model, depModel))
					{
						String message = String.Format(
							"Factory Method {0}.{1} requires an argument '{2}' that could not be resolved", 
								instanceCreateMethod.DeclaringType.FullName, instanceCreateMethod.Name, parameter.Name);
						throw new FacilityException(message);
					}

					object arg = Kernel.Resolver.Resolve(Model, depModel );

					methodArgs.Add(arg);
				}

				return instanceCreateMethod.Invoke( factoryInstance, methodArgs.ToArray() );
			}
			catch(Exception ex)
			{
				String message = String.Format("You have specified a factory " +
					"('{2}' - method to be called: {3}) " + 
					"for the component '{0}' {1} that failed during invoke.", 
						Model.Name, Model.Implementation.FullName, factoryId, factoryCreate);

				throw new FacilityException(message, ex);
			}
		}
	}
}