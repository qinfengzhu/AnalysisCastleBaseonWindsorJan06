namespace Castle.MicroKernel.Lifestyle.Pool
{
	using System;
	using System.Runtime.Serialization;

	[Serializable]
	public class PoolException : ApplicationException
	{
		public PoolException(string message) : base(message)
		{
		}

		public PoolException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
