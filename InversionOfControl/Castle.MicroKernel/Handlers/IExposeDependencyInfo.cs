namespace Castle.MicroKernel.Handlers
{
	using System;

	/// <summary>
    /// 可以被Handler继承,暴露对依赖信息的访问,收集一些有意义的错误信息
	/// </summary>
	public interface IExposeDependencyInfo
	{
		/// <summary>
		/// Returns human readable list of dependencies 
		/// this handler is waiting for.
		/// </summary>
		/// <returns></returns>
		String ObtainDependencyDetails();
	}
}
