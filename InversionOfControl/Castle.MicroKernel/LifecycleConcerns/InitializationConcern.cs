namespace Castle.MicroKernel.LifecycleConcerns
{
	using System;
	using Castle.Model;

	/// <summary>
	/// 组件初始化操作
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
