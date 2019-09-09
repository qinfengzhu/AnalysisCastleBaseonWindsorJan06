namespace Castle.MicroKernel.Releasers
{
	using System;

	/// <summary>
	/// 不跟踪组件实例策略
	/// </summary>
	[Serializable]
	public class NoTrackingReleasePolicy : IReleasePolicy
	{
		public NoTrackingReleasePolicy()
		{
		}

		#region IReleasePolicy Members

		public void Track(object instance, IHandler handler)
		{
		}

		public bool HasTrack(object instance)
		{
			return false;
		}

		public void Release(object instance)
		{
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
		}

		#endregion
	}
}
