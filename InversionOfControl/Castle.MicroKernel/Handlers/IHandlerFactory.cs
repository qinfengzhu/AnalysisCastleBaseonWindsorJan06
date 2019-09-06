namespace Castle.MicroKernel
{
	using System;
	using Castle.Model;

    /// <summary>
    /// IHandlerFacotry是一个扩展点,允许开发者使用自己的<see cref="IHandler"/>实现
    /// </summary>
    public interface IHandlerFactory
	{
		IHandler Create( ComponentModel model );
	}
}
