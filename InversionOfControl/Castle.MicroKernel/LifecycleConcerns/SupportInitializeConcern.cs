namespace Castle.MicroKernel.LifecycleConcerns
{
	using System;
	using System.ComponentModel;

	using Castle.Model;

	/// <summary>
	/// Summary description for SupportInitializeConcern.
	/// </summary>
	[Serializable]
	public class SupportInitializeConcern : ILifecycleConcern
	{
		private static readonly SupportInitializeConcern instance = new SupportInitializeConcern();

		public static SupportInitializeConcern Instance
		{
			get { return instance; }
		}

		protected SupportInitializeConcern()
		{
		}

		public void Apply(ComponentModel model, object component)
		{
			(component as ISupportInitialize).BeginInit();
			(component as ISupportInitialize).EndInit();
		}
	}
}
