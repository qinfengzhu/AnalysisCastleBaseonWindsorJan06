namespace Castle.MicroKernel.SubSystems.Conversion
{
	using System;

	using Castle.Model.Configuration;

	/// <summary>
	/// Convert a type name to a Type instance.
	/// </summary>
	[Serializable]
	public class TypeNameConverter : AbstractTypeConverter
	{
		public override bool CanHandleType(Type type)
		{
			return type == typeof(Type);
		}

		public override object PerformConversion(String value, Type targetType)
		{
			try
			{
				Type type = Type.GetType(value, true, false);

				if (type == null)
				{
					String message = String.Format(
						"Could not convert from '{0}' to {1} - Maybe type could not be found", 
						value, targetType.FullName);
				
					throw new ConverterException(message);
				}

				return type;
			}
			catch(ConverterException ex)
			{
				throw ex;
			}
			catch(Exception ex)
			{
				String message = String.Format(
					"Could not convert from '{0}' to {1}.", 
					value, targetType.FullName);

				throw new ConverterException(message, ex);
			}
		}
		
		public override object PerformConversion(IConfiguration configuration, Type targetType)
		{
			return PerformConversion(configuration.Value, targetType);
		}
	}
}
