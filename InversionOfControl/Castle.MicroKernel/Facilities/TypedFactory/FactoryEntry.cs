namespace Castle.Facilities.TypedFactory
{
	using System;

	public class FactoryEntry
	{
		private String _id;
		private Type _factoryInterface;
		private String _creationMethod;
		private String _destructionMethod;

		public FactoryEntry(String id, Type factoryInterface, String creationMethod, String destructionMethod)
		{
			if (id == null || id.Length == 0) throw new ArgumentNullException("id");
			if (factoryInterface == null) throw new ArgumentNullException("factoryInterface");
			if (!factoryInterface.IsInterface) throw new ArgumentException("factoryInterface must be an interface");
			if (creationMethod == null || creationMethod.Length == 0) throw new ArgumentNullException("creationMethod");

			_id = id;
			_factoryInterface = factoryInterface;
			_creationMethod = creationMethod;
			_destructionMethod = destructionMethod;
		}

		public String Id
		{
			get { return _id; }
		}

		public Type FactoryInterface
		{
			get { return _factoryInterface; }
		}

		public String CreationMethod
		{
			get { return _creationMethod; }
		}

		public String DestructionMethod
		{
			get { return _destructionMethod; }
		}
	}
}
