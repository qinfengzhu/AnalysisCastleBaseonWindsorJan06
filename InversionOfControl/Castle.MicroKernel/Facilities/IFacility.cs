namespace Castle.MicroKernel
{
	using Castle.Model.Configuration;

	/// <summary>
    /// 扩展单元,一个基础设施应该使用内核提供的扩展点加强自己的功能性
	/// </summary>
	public interface IFacility
	{
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="kernel">Castle内核</param>
		/// <param name="facilityConfig">基础设施配置</param>
		void Init(IKernel kernel, IConfiguration facilityConfig);

		/// <summary>
		/// 结束基础设置
		/// </summary>
		void Terminate();
	}
}
