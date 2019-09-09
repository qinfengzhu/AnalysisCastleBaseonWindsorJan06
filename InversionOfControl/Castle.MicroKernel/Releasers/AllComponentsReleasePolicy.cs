namespace Castle.MicroKernel.Releasers
{
	using System;
	using System.Collections;

	/// <summary>
	/// 默认所有组件释放策略
	/// </summary>
	[Serializable]
	public class AllComponentsReleasePolicy : IReleasePolicy
	{
		private IDictionary instance2Handler = Hashtable.Synchronized(new Hashtable(CaseInsensitiveHashCodeProvider.Default, new Util.ReferenceComparer()));

		public AllComponentsReleasePolicy()
		{
		}

		#region IReleasePolicy Members

		public virtual void Track(object instance, IHandler handler)
		{
			instance2Handler[instance] = handler;
		}

		public bool HasTrack(object instance)
		{
			return instance2Handler.Contains(instance);
		}

		public void Release(object instance)
		{
			IHandler handler = (IHandler) instance2Handler[instance];
			
			if (handler != null)
			{
				instance2Handler.Remove(instance);

				handler.Release(instance);
			}
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			foreach(DictionaryEntry entry in instance2Handler)
			{
				object instance = entry.Key;
				IHandler handler = (IHandler) entry.Value;
				handler.Release(instance);
			}

			instance2Handler.Clear();
		}

		#endregion
	}
}
