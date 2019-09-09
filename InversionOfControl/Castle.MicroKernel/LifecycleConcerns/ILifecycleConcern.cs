namespace Castle.MicroKernel.LifecycleConcerns
{
	using System;
	using Castle.Model;

	public interface ILifecycleConcern
	{
		void Apply( ComponentModel model, object component );
	}
}
