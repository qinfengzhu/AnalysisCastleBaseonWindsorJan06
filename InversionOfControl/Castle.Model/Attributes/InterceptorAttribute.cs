namespace Castle.Model
{
	using System;

	/// <summary>
    /// 组件使用拦截器作用于它
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
	public class InterceptorAttribute : Attribute
	{
		private InterceptorReference interceptorRef;

		/// <summary>
		/// Constructs the InterceptorAttribute pointing to
		/// a key to a interceptor
		/// </summary>
		/// <param name="componentKey"></param>
		public InterceptorAttribute( String componentKey )
		{
			this.interceptorRef = new InterceptorReference(componentKey);
		}

		/// <summary>
		/// Constructs the InterceptorAttribute pointing to
		/// a service
		/// </summary>
		/// <param name="interceptorType"></param>
		public InterceptorAttribute( Type interceptorType )
		{
			this.interceptorRef = new InterceptorReference(interceptorType);
		}

		public InterceptorReference Interceptor
		{
			get { return interceptorRef; }
		}
	}
}
