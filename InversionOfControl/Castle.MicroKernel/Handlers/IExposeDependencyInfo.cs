namespace Castle.MicroKernel.Handlers
{
	using System;

	/// <summary>
    /// ���Ա�Handler�̳�,��¶��������Ϣ�ķ���,�ռ�һЩ������Ĵ�����Ϣ
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
