namespace Castle.MicroKernel.SubSystems.Conversion
{
	using System;

	using Castle.Model.Configuration;

	/// <summary>
	/// Implements all standard conversions.
	/// </summary>
	[Serializable]
	public class PrimitiveConverter : AbstractTypeConverter
	{
		private Type[] types;

		public PrimitiveConverter()
		{
			types = new Type[]
				{
					typeof (Char),
					typeof (DateTime),
					typeof (Decimal),
					typeof (Boolean),
					typeof (Int16),
					typeof (Int32),
					typeof (Int64),
					typeof (UInt16),
					typeof (UInt32),
					typeof (UInt64),
					typeof (Byte),
					typeof (SByte),
					typeof (Single),
					typeof (Double),
					typeof (String)
				};
		}

		public override bool CanHandleType(Type type)
		{
			return Array.IndexOf(types, type) != -1;
		}

		public override object PerformConversion(String value, Type targetType)
		{
			if (targetType == typeof(String)) return value;

			try
			{
				return Convert.ChangeType(value, targetType);
			}
			catch(Exception ex)
			{
				String message = String.Format(
					"Could not convert from '{0}' to {1}", 
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