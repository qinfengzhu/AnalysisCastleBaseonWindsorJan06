namespace Castle.MicroKernel
{
	using System;

	/// <summary>
	/// Holds the keys used by Kernel to register/request 
	/// a subsystem.
	/// </summary>
	public abstract class SubSystemConstants
	{
		/// <summary>
		/// Key used for the configuration store subsystem
		/// </summary>
		public static readonly String ConfigurationStoreKey = "config.store";

		/// <summary>
		/// Key used for the conversion manager
		/// </summary>
		public static readonly String ConversionManagerKey  = "conversion.mng";

		/// <summary>
		/// Key used for the naming subsystem
		/// </summary>
		public static readonly String NamingKey             = "naming.sub.key";

		/// <summary>
		/// Key used for the resource subsystem
		/// </summary>
		public static readonly String ResourceKey           = "resource.sub.key";
	}
}
