namespace Castle.MicroKernel
{
	/// <summary>
	/// A subsystem is used by the MicroKernel to deal 
	/// with a specific concern.  
	/// </summary>
	public interface ISubSystem
	{
		/// <summary>
		/// Initializes the subsystem
		/// </summary>
		/// <param name="kernel"></param>
		void Init(IKernel kernel);

		/// <summary>
		/// Should perform the termination
		/// of the subsystem instance.
		/// </summary>
		void Terminate();
	}
}
