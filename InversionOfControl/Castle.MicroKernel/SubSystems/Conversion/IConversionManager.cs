namespace Castle.MicroKernel.SubSystems.Conversion
{
	using System;

	/// <summary>
	/// Establish a composition interface and a subsystem.
	/// Implementors should delegate the conversion to 
	/// a instance of a type converter.
	/// </summary>
	public interface IConversionManager : ITypeConverter, ISubSystem
	{
		/// <summary>
		/// Register a type converter instance.
		/// </summary>
		/// <param name="converter"></param>
		void Add( ITypeConverter converter );
	}
}
