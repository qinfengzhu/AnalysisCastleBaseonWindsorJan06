namespace Castle.MicroKernel
{
	using System;

	/// <summary>
	/// ÊÍ·Å²ßÂÔ
	/// </summary>
	public interface IReleasePolicy : IDisposable
	{
		void Track( object instance, IHandler handler );

		bool HasTrack( object instance );

		void Release( object instance );
	}
}
