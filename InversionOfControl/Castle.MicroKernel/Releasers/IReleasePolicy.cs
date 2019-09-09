namespace Castle.MicroKernel
{
	using System;

	/// <summary>
	/// �ͷŲ���
	/// </summary>
	public interface IReleasePolicy : IDisposable
	{
		void Track( object instance, IHandler handler );

		bool HasTrack( object instance );

		void Release( object instance );
	}
}
