namespace Castle.MicroKernel.ComponentActivator
{
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// ��������쳣
	/// </summary>
	[Serializable]
	public class ComponentActivatorException : ApplicationException
	{
		public ComponentActivatorException(string message) : base(message)
		{
		}

		public ComponentActivatorException(string message, Exception innerException) : base(message, innerException)
		{
		}

		public ComponentActivatorException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
