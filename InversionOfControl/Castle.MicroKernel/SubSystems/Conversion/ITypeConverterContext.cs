namespace Castle.MicroKernel.SubSystems.Conversion
{
	using System;

	public interface ITypeConverterContext
	{
		ITypeConverter Composition { get; }
	}
}
