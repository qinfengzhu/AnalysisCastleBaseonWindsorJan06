namespace Castle.MicroKernel.ComponentActivator
{
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// 组件构建异常
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
