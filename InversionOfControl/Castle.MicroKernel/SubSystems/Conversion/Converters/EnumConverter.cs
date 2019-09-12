namespace Castle.MicroKernel.SubSystems.Conversion
{
	using System;

	using Castle.Model.Configuration;

	/// <summary>
	/// Converts a string representation to an enum value
	/// </summary>
	[Serializable]
	public class EnumConverter : AbstractTypeConverter
	{
		public override bool CanHandleType(Type type)
		{
			return type.IsEnum;
		}

		public override object PerformConversion(String value, Type targetType)
		{
			try
			{
				return Enum.Parse( targetType, value, true );
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
