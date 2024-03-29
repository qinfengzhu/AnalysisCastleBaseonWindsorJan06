namespace Castle.MicroKernel
{
	using System;
	using System.ComponentModel;
	using System.Runtime.Serialization;

	using Castle.Model;

	/// <summary>
	/// Summary description for KernelEventSupport.
	/// </summary>
	[Serializable]
	public abstract class KernelEventSupport : MarshalByRefObject, IKernelEvents, ISerializable
	{
		private static readonly object HandlerRegisteredEvent = new object();
		private static readonly object ComponentRegisteredEvent = new object();
		private static readonly object ComponentUnregisteredEvent = new object();
		private static readonly object ComponentCreatedEvent = new object();
		private static readonly object ComponentDestroyedEvent = new object();
		private static readonly object AddedAsChildKernelEvent = new object();
		private static readonly object ComponentModelCreatedEvent = new object();
		private static readonly object DependencyResolvingEvent = new object();

		[NonSerialized]
		private EventHandlerList events;

		public KernelEventSupport()
		{
			events = new EventHandlerList();
		}

		public KernelEventSupport(SerializationInfo info, StreamingContext context)
		{
			events = new EventHandlerList();

			events[HandlerRegisteredEvent] = (Delegate) 
				 info.GetValue("HandlerRegisteredEvent", typeof(Delegate));
		}

		public override object InitializeLifetimeService()
		{
			return null;
		}

		/// <summary>
		/// Pending
		/// </summary>
		public event HandlerDelegate HandlerRegistered
		{
			add { events.AddHandler(HandlerRegisteredEvent, value); }
			remove { events.RemoveHandler(HandlerRegisteredEvent, value); }
		}

		/// <summary>
		/// Pending
		/// </summary>
		/// <value></value>
		public event ComponentDataDelegate ComponentRegistered
		{
			add { events.AddHandler(ComponentRegisteredEvent, value); }
			remove { events.RemoveHandler(ComponentRegisteredEvent, value); }
		}

		/// <summary>
		/// Pending
		/// </summary>
		/// <value></value>
		public event ComponentDataDelegate ComponentUnregistered
		{
			add { events.AddHandler(ComponentUnregisteredEvent, value); }
			remove { events.RemoveHandler(ComponentUnregisteredEvent, value); }
		}


		/// <summary>
		/// Pending
		/// </summary>
		/// <value></value>
		public event ComponentInstanceDelegate ComponentCreated
		{
			add { events.AddHandler(ComponentCreatedEvent, value); }
			remove { events.RemoveHandler(ComponentCreatedEvent, value); }
		}

		/// <summary>
		/// Pending
		/// </summary>
		/// <value></value>
		public event ComponentInstanceDelegate ComponentDestroyed
		{
			add { events.AddHandler(ComponentDestroyedEvent, value); }
			remove { events.RemoveHandler(ComponentDestroyedEvent, value); }
		}

		/// <summary>
		/// Pending
		/// </summary>
		/// <value></value>
		public event EventHandler AddedAsChildKernel
		{
			add { events.AddHandler(AddedAsChildKernelEvent, value); }
			remove { events.RemoveHandler(AddedAsChildKernelEvent, value); }
		}

		/// <summary>
		/// Pending
		/// </summary>
		/// <value></value>
		public event ComponentModelDelegate ComponentModelCreated
		{
			add { events.AddHandler(ComponentModelCreatedEvent, value); }
			remove { events.RemoveHandler(ComponentModelCreatedEvent, value); }
		}


		public event DependencyDelegate DependencyResolving
		{
			add { events.AddHandler(DependencyResolvingEvent, value); }
			remove { events.RemoveHandler(DependencyResolvingEvent, value); }
		}


		protected virtual void RaiseComponentRegistered(String key, IHandler handler)
		{
			ComponentDataDelegate eventDelegate = (ComponentDataDelegate) events[ComponentRegisteredEvent];
			if (eventDelegate != null) eventDelegate(key, handler);
		}


		protected virtual void RaiseComponentUnregistered(String key, IHandler handler)
		{
			ComponentDataDelegate eventDelegate = (ComponentDataDelegate) events[ComponentUnregisteredEvent];
			if (eventDelegate != null) eventDelegate(key, handler);
		}

		public virtual void RaiseComponentCreated(ComponentModel model, object instance)
		{
			ComponentInstanceDelegate eventDelegate = (ComponentInstanceDelegate) events[ComponentCreatedEvent];
			if (eventDelegate != null) eventDelegate(model, instance);
		}

		public virtual void RaiseComponentDestroyed(ComponentModel model, object instance)
		{
			ComponentInstanceDelegate eventDelegate = (ComponentInstanceDelegate) events[ComponentDestroyedEvent];
			if (eventDelegate != null) eventDelegate(model, instance);
		}

		protected virtual void RaiseAddedAsChildKernel()
		{
			EventHandler eventDelegate = (EventHandler) events[AddedAsChildKernelEvent];
			if (eventDelegate != null) eventDelegate(this, EventArgs.Empty);
		}

		protected virtual void RaiseComponentModelCreated(ComponentModel model)
		{
			ComponentModelDelegate eventDelegate = (ComponentModelDelegate) events[ComponentModelCreatedEvent];
			if (eventDelegate != null) eventDelegate(model);
		}

		protected virtual void RaiseHandlerRegistered(IHandler handler)
		{
			bool stateChanged = true;

			while(stateChanged)
			{
				stateChanged = false;
				HandlerDelegate eventDelegate = (HandlerDelegate) events[HandlerRegisteredEvent];
				if (eventDelegate != null) eventDelegate(handler, ref stateChanged);
			}
		}
		protected virtual void RaiseDependencyResolving(ComponentModel client, DependencyModel model, Object dependency)
		{
			DependencyDelegate eventDelegate = (DependencyDelegate) events[DependencyResolvingEvent];
			if (eventDelegate != null) eventDelegate(client, model, dependency);
		}

		#region IDeserializationCallback Members

		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("HandlerRegisteredEvent", events[HandlerRegisteredEvent]);
		}

		#endregion
	}
}
