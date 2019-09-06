namespace Castle.MicroKernel.SubSystems.Conversion
{
	using System;

	using Castle.Model.Configuration;

	/// <summary>
	/// Implements a conversion logic to a type of a
	/// set of types. 
	/// </summary>
	public interface ITypeConverter
	{
		ITypeConverterContext Context { get; set; }

		/// <summary>
		/// Returns true if this instance of <c>ITypeConverter</c>
		/// is able to handle the specified type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		bool CanHandleType( Type type );

		/// <summary>
		/// Should perform the conversion from the
		/// string representation specified to the type
		/// specified.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <returns></returns>
		object PerformConversion( String value, Type targetType );

		/// <summary>
		/// Should perform the conversion from the
		/// configuration node specified to the type
		/// specified.
		/// </summary>
		/// <param name="configuration"></param>
		/// <param name="targetType"></param>
		/// <returns></returns>
		object PerformConversion( IConfiguration configuration, Type targetType );
	}
}
