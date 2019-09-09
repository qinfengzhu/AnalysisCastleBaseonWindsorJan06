namespace Castle.MicroKernel.Lifestyle.Pool
{
	using System;

    /// <summary>
    /// 池工厂接口
    /// </summary>
	public interface IPoolFactory
	{
		IPool Create(int initialsize, int maxSize, IComponentActivator activator);
	}
}
