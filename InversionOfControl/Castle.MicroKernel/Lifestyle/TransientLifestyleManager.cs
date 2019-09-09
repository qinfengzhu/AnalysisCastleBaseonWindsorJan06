namespace Castle.MicroKernel.Lifestyle
{
	using System;

	/// <summary>
	/// 瞬变类型-管理器
	/// </summary>
	[Serializable]
	public class TransientLifestyleManager : AbstractLifestyleManager
	{
		public override void Dispose()
		{
		}
	}
}
