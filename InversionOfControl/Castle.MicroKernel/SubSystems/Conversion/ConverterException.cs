namespace Castle.MicroKernel.SubSystems.Conversion
{
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// Summary description for ConverterException.
	/// </summary>
	[Serializable]
	public class ConverterException : ApplicationException
	{
		public ConverterException(string message) : base(message)
		{
		}

		public ConverterException(string message, Exception innerException) : base(message, innerException)
		{
		}

		public ConverterException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
