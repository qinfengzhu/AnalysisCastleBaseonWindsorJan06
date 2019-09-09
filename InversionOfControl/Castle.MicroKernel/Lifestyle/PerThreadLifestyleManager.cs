
namespace Castle.MicroKernel.Lifestyle
{
	using System;
	using System.Collections;
	using System.Threading;
	using System.Runtime.Serialization;

	/// <summary>
	/// 单线程-管理器
	/// </summary>
	[Serializable]
	public class PerThreadLifestyleManager : AbstractLifestyleManager, IDeserializationCallback
	{
		[NonSerialized]
		private static LocalDataStoreSlot slot = Thread.AllocateNamedDataSlot("CastlePerThread"); //线程数据槽

		[NonSerialized]
		private IList instances = new ArrayList();

		/// <summary>
		/// 
		/// </summary>
		public override void Dispose()
		{
			foreach( object instance in instances )
			{
				base.Release( instance );
			}

			instances.Clear();

			Thread.FreeNamedDataSlot( "CastlePerThread" );
		}

		#region IResolver Members

		public override object Resolve()
		{
			lock(slot)
			{
				Hashtable map = (Hashtable) Thread.GetData( slot );

				if (map == null)
				{
					map = new Hashtable();

					Thread.SetData( slot, map );
				}

				Object instance = map[ ComponentActivator ];

				if ( instance == null )
				{
					instance = base.Resolve();
					map.Add( ComponentActivator, instance );
					instances.Add( instance );
				}

				return instance;
			}
		}

		public override void Release( object instance )
		{
			// Do nothing.
		}

		#endregion

		public void OnDeserialization(object sender)
		{
			slot = Thread.AllocateNamedDataSlot("CastlePerThread");
			instances = new ArrayList();
		}
	}
}
