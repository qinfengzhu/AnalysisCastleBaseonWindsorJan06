namespace Castle.MicroKernel.SubSystems.Conversion
{
	using System;

	using Castle.Model.Configuration;


	[Serializable]
	public class ArrayConverter : AbstractTypeConverter
	{
		public ArrayConverter()
		{
		}

		public override bool CanHandleType(Type type)
		{
			if (!type.IsArray) 
			{
				return false;
			}

			return Context.Composition.CanHandleType(type.GetElementType());
		}

		public override object PerformConversion(String value, Type targetType)
		{
			throw new NotImplementedException();
		}

		public override object PerformConversion(IConfiguration configuration, Type targetType)
		{
			System.Diagnostics.Debug.Assert( targetType.IsArray );

			int count = configuration.Children.Count;
			Type itemType = targetType.GetElementType();

			Array array = Array.CreateInstance( itemType, count );

			int index = 0;
			foreach(IConfiguration itemConfig in configuration.Children)
			{
				object value = Context.Composition.PerformConversion(itemConfig.Value, itemType);
				array.SetValue( value, index++ ); 
			}

			return array;
		}
	}
}
