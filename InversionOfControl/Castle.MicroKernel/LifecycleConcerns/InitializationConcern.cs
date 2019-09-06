namespace Castle.MicroKernel.LifecycleConcerns
{
	using System;
	using Castle.Model;

	/// <summary>
	/// �����ʼ������
	/// </summary>
	[Serializable]
	public class InitializationConcern : ILifecycleConcern
	{
		private static readonly InitializationConcern instance = new InitializationConcern();

		public static InitializationConcern Instance
		{
			get { return instance; }
		}

		protected InitializationConcern()
		{
		}

		public void Apply(ComponentModel model, object component)
		{
			(component as IInitializable).Initialize();
		}
	}
}
