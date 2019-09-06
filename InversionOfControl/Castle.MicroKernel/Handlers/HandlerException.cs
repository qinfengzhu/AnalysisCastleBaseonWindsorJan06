namespace Castle.MicroKernel.Handlers
{
	using System;
	using System.Runtime.Serialization;

	[Serializable]
	public class HandlerException : ApplicationException
	{
		public HandlerException(string message) : base(message)
		{
		}

		public HandlerException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
