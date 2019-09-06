namespace Castle.MicroKernel
{
	using System;
	using Castle.Model;

    /// <summary>
    /// IHandlerFacotry��һ����չ��,��������ʹ���Լ���<see cref="IHandler"/>ʵ��
    /// </summary>
    public interface IHandlerFactory
	{
		IHandler Create( ComponentModel model );
	}
}
