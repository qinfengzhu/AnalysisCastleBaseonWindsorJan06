namespace Castle.MicroKernel.Resolvers
{
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// Summary description for DependencyResolverException.
	/// </summary>
	[Serializable]
	public class DependencyResolverException : ApplicationException
	{
		public DependencyResolverException(string message) : base(message)
		{
		}

		public DependencyResolverException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
