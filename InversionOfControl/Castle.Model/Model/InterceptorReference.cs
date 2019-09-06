namespace Castle.Model
{
	using System;

    /// <summary>
    /// ¿πΩÿ¿‡–Õ
    /// </summary>
	public enum InterceptorReferenceType
	{
		Interface,
		Key
	}

	/// <summary>
	/// Represents an reference to a Interceptor component.
	/// </summary>
	[Serializable]
	public class InterceptorReference
	{
		private readonly InterceptorReferenceType refType;
		private Type serviceType;
		private String componentKey;

		public InterceptorReference( String componentKey )
		{
			if (componentKey == null)
			{
				throw new ArgumentNullException( "componentKey cannot be null" );
			}

			this.refType = InterceptorReferenceType.Key;
			this.componentKey = componentKey;
		}

		public InterceptorReference( Type serviceType )
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException( "'serviceType' cannot be null" );
			}		

			this.refType = InterceptorReferenceType.Interface;
			this.serviceType = serviceType;
		}

		public Type ServiceType
		{
			get { return serviceType; }
		}

		public String ComponentKey
		{
			get { return componentKey; }
		}

		public InterceptorReferenceType ReferenceType
		{
			get { return refType; }
		}
	}
}
