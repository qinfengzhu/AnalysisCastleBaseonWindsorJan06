namespace Castle.MicroKernel.ComponentActivator
{
	using System;
	using Castle.Model;

	public class ExternalInstanceActivator : AbstractComponentActivator
	{
		public ExternalInstanceActivator(ComponentModel model, IKernel kernel, ComponentInstanceDelegate onCreation, ComponentInstanceDelegate onDestruction) : base(model, kernel, onCreation, onDestruction)
		{
		}

		protected override object InternalCreate()
		{
			return base.Model.ExtendedProperties["instance"];
		}

		protected override void InternalDestroy(object instance)
		{
			// Nothing to do
		}
	}
}
