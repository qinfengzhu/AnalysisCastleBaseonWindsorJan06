namespace Castle.MicroKernel.Lifestyle
{
	using System;

	/// <summary>
	/// ��������-������
	/// </summary>
	[Serializable]
	public class SingletonLifestyleManager : AbstractLifestyleManager
	{
		private Object instance;

		public override void Dispose()
		{
			if (instance != null) base.Release( instance );
		}

		public override object Resolve()
		{
			lock(ComponentActivator)
			{
				if (instance == null)
				{
					instance = base.Resolve();
				}
			}

			return instance;
		}

		public override void Release( object instance )
		{
			// Do nothing
		}
	}
}
