namespace Castle.MicroKernel.Util
{
	using System;
	using System.Collections;
	
	/// <summary>
	/// Compares if the reference of two objects are equals.
	/// </summary>
	[Serializable]
	public class ReferenceComparer : IComparer
	{
		public ReferenceComparer()
		{
		}

		public int Compare(object x, object y)
		{
			return object.ReferenceEquals(x, y) ? 0 : 1;
		}
	}
}
