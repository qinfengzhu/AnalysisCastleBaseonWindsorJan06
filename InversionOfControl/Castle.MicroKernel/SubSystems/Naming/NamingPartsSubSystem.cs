namespace Castle.MicroKernel.SubSystems.Naming
{
	using System;

	/// <summary>
	/// Alternative <see cref="INamingSubSystem"/> implementation.
	/// Extends the default implementation replacing the 
	/// key support with a more complete ComponentName. Supports
	/// queries.
	/// </summary>
	/// <example>
	/// The user must register components using the following construction
	/// <code>
	///   service:properties
	/// </code>
	/// Where properties is a list of key value pairs (comma separated). Example:
	/// <code>
	///   protocol:secure=true,version=1.2
	/// </code>
	/// The user can then query for components using the same construction:
	/// <code>
	///   protocol:secure=true
	/// </code>
	/// Or to return all:
	/// <code>
	///   protocol:*
	/// </code>
	/// </example>
	[Serializable]
	public class NamingPartsSubSystem : DefaultNamingSubSystem
	{
		private BinaryTreeComponentName tree;

		public NamingPartsSubSystem()
		{
			tree = new BinaryTreeComponentName();
		}

		private ComponentName ToComponentName(String key)
		{
			return new ComponentName(key);
		}

		public override bool Contains(String key)
		{
			return tree.Contains( ToComponentName(key) );
		}

		public override void UnRegister(String key)
		{
			tree.Remove( ToComponentName(key) );
		}

		public override IHandler GetHandler(String key)
		{
			return tree.GetHandler( ToComponentName(key) );
		}

		public override IHandler[] GetHandlers(String query)
		{
			return tree.GetHandlers( ToComponentName(query) );
		}

		public override IHandler[] GetHandlers()
		{
			return tree.Handlers;
		}

		public override IHandler this[String key]
		{
			set { tree.Add( ToComponentName(key), value ); }
		}

		public override int ComponentCount
		{
			get { return tree.Count; }
		}
	}
}