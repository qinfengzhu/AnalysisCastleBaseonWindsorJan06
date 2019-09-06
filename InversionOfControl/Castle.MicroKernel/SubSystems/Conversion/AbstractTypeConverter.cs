
namespace Castle.MicroKernel.SubSystems.Conversion
{
	using System;

	using Castle.Model.Configuration;

	/// <summary>
	/// Base implementation of <see cref="ITypeConverter"/>
	/// </summary>
	[Serializable]
	public abstract class AbstractTypeConverter : ITypeConverter
	{
		private ITypeConverterContext context;

		public ITypeConverterContext Context
		{
			get { return context; }
			set { context = value; }
		}

		public abstract bool CanHandleType(Type type);

		public abstract object PerformConversion(String value, Type targetType);

		public abstract object PerformConversion(IConfiguration configuration, Type targetType);
	}
}