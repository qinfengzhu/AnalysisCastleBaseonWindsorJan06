namespace Castle.MicroKernel.Facilities
{
	using System;
	using System.Runtime.Serialization;

	[Serializable]
	public class FacilityException : ApplicationException
	{
		public FacilityException(string message) : base(message)
		{
		}

		public FacilityException(string message, Exception innerException) : base(message, innerException)
		{
		}

		public FacilityException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
