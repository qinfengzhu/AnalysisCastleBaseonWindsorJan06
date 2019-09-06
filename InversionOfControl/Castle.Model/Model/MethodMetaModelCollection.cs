namespace Castle.Model
{
	using System;
	using System.Collections;
	using System.Collections.Specialized;

	[Serializable]
	public class MethodMetaModelCollection : ReadOnlyCollectionBase
	{
		private IDictionary methodInfo2Model;

		public void Add(MethodMetaModel model)
		{
			InnerList.Add(model);
		}

		public IDictionary MethodInfo2Model
		{
			get
			{
				if (methodInfo2Model == null) methodInfo2Model = new HybridDictionary();
				
				return methodInfo2Model;
			}
		}
	}
}
