namespace Castle.Facilities.TypedFactory
{
	using System;
	using System.Configuration;
	
	using Castle.Model;
	using Castle.Model.Configuration;

	using Castle.MicroKernel;
	using Castle.MicroKernel.Facilities;
	using Castle.MicroKernel.SubSystems.Conversion;

	/// <summary>
	/// Summary description for TypedFactoryFacility.
	/// </summary>
	public class TypedFactoryFacility : AbstractFacility
	{
		public void AddTypedFactoryEntry( FactoryEntry entry )
		{
			ComponentModel model = new ComponentModel(entry.Id, entry.FactoryInterface, typeof(Empty));
			
			model.LifestyleType = LifestyleType.Singleton;
			model.ExtendedProperties["typed.fac.entry"] = entry;
			model.Interceptors.Add( new InterceptorReference( typeof(FactoryInterceptor) ) );

			Kernel.AddCustomComponent( model );
		}

		protected override void Init()
		{
			Kernel.AddComponent( "typed.fac.interceptor", typeof(FactoryInterceptor) );

			ITypeConverter converter = (ITypeConverter)
				Kernel.GetSubSystem( SubSystemConstants.ConversionManagerKey );

			AddFactories(FacilityConfig, converter);
		}

		protected virtual void AddFactories(IConfiguration facilityConfig, ITypeConverter converter)
		{
			if (facilityConfig != null)
			{
				foreach(IConfiguration config in facilityConfig.Children["factories"].Children)
				{
					String id = config.Attributes["id"];
					String creation = config.Attributes["creation"];
					String destruction = config.Attributes["destruction"];
					Type factoryType = (Type)
						converter.PerformConversion( config.Attributes["interface"], typeof(Type) );

					try
					{
						AddTypedFactoryEntry( 
							new FactoryEntry(id, factoryType, creation, destruction) );
					}
					catch(Exception ex)
					{
						throw new ConfigurationException("Invalid factory entry in configuration", ex);
					}
				}
			}
		}
	}
}
