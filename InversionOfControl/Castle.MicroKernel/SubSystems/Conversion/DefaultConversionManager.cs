namespace Castle.MicroKernel.SubSystems.Conversion
{
	using System;
	using System.Collections;

	using Castle.Model.Configuration;

	/// <summary>
	/// 所有可用转换器的组合
	/// </summary>
	[Serializable]
	public class DefaultConversionManager : AbstractSubSystem, IConversionManager, ITypeConverterContext
	{
		private IList converters;

		public DefaultConversionManager()
		{
			converters = new ArrayList();

			InitDefaultConverters();
		}

		protected virtual void InitDefaultConverters()
		{
			Add( new PrimitiveConverter() );
			Add( new TypeNameConverter() );
			Add( new EnumConverter() );
			Add( new ListConverter() );
			Add( new DictionaryConverter() );
			Add( new ArrayConverter() ); 
		}

		#region IConversionManager Members

		public void Add(ITypeConverter converter)
		{
			converter.Context = this;

			converters.Add(converter);
		}

		#endregion

		#region ITypeConverter Members

		public ITypeConverterContext Context
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public bool CanHandleType(Type type)
		{
			foreach(ITypeConverter converter in converters)
			{
				if (converter.CanHandleType(type)) return true;
			}

			return false;
		}

		public object PerformConversion(String value, Type targetType)
		{
			foreach(ITypeConverter converter in converters)
			{
				if (converter.CanHandleType(targetType)) 
					return converter.PerformConversion(value, targetType);
			}

			String message = String.Format("No converter registered to handle the type {0}", 
				targetType.FullName);

			throw new ConverterException(message);
		}

		public object PerformConversion(IConfiguration configuration, Type targetType)
		{
			foreach(ITypeConverter converter in converters)
			{
				if (converter.CanHandleType(targetType)) 
					return converter.PerformConversion(configuration, targetType);
			}

			String message = String.Format("No converter registered to handle the type {0}", 
				targetType.FullName);

			throw new ConverterException(message);
		}

		#endregion

		#region ITypeConverterContext Members

		public ITypeConverter Composition
		{
			get { return this; }
		}

		#endregion
	}
}
