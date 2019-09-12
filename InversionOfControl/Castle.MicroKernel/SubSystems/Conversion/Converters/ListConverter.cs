namespace Castle.MicroKernel.SubSystems.Conversion
{
	using System;
	using System.Collections;

	using Castle.Model.Configuration;


	[Serializable]
	public class ListConverter : AbstractTypeConverter
	{
		public ListConverter()
		{
		}

		public override bool CanHandleType(Type type)
		{
			return (type == typeof(IList) || type == typeof(ArrayList));
		}

		public override object PerformConversion(String value, Type targetType)
		{
			throw new NotImplementedException();
		}

		public override object PerformConversion(IConfiguration configuration, Type targetType)
		{
			System.Diagnostics.Debug.Assert( targetType == typeof(IList) || targetType == typeof(ArrayList) );

			ArrayList list = new ArrayList();

			String itemType = configuration.Attributes["type"];
			Type convertTo = typeof(String);

			if (itemType != null)
			{
				convertTo = (Type) Context.Composition.PerformConversion( itemType, typeof(Type) );
			}

			foreach(IConfiguration itemConfig in configuration.Children)
			{
				list.Add( Context.Composition.PerformConversion(itemConfig.Value, convertTo) );
			}

			return list;
		}
	}
}
