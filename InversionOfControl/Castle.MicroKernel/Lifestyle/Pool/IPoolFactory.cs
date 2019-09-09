namespace Castle.MicroKernel.Lifestyle.Pool
{
	using System;

    /// <summary>
    /// �ع����ӿ�
    /// </summary>
	public interface IPoolFactory
	{
		IPool Create(int initialsize, int maxSize, IComponentActivator activator);
	}
}
