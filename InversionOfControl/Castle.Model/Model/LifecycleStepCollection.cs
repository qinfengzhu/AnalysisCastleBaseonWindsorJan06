namespace Castle.Model
{
	using System;
	using System.Collections;

    /// <summary>
    /// 生命周期步骤类型
    /// </summary>
	public enum LifecycleStepType
	{
		Commission,//实例化后的使用阶段类型
		Decommission//析构阶段类型
	}

	/// <summary>
	/// Represents a collection of ordered lifecycle steps.
	/// </summary>
	[Serializable]
	public class LifecycleStepCollection : ICollection
	{
		private IList commissionSteps;
		private IList decommissionSteps;

		public LifecycleStepCollection()
		{
			commissionSteps = new ArrayList();
			decommissionSteps = new ArrayList();
		}

		/// <summary>
		/// Returns all steps for the commission phase
		/// </summary>
		/// <returns></returns>
		public object[] GetCommissionSteps()
		{
			object[] steps = new object[commissionSteps.Count];
			commissionSteps.CopyTo( steps, 0 );
			return steps;
		}

		/// <summary>
		/// Returns all steps for the decommission phase
		/// </summary>
		/// <returns></returns>
		public object[] GetDecommissionSteps()
		{
			object[] steps = new object[decommissionSteps.Count];
			decommissionSteps.CopyTo( steps, 0 );
			return steps;
		}

		public bool HasCommissionSteps
		{
			get { return commissionSteps.Count != 0; }
		}

		public bool HasDecommissionSteps
		{
			get { return decommissionSteps.Count != 0; }
		}

		/// <summary>
		/// Adds a step to the commission or decomission phases.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="stepImplementation"></param>
		public void Add( LifecycleStepType type, object stepImplementation )
		{
			if (stepImplementation == null) throw new ArgumentNullException("stepImplementation");

			if (type == LifecycleStepType.Commission)
			{
				commissionSteps.Add( stepImplementation );				
			}
			else
			{
				decommissionSteps.Add( stepImplementation );
			}
		}

		public void CopyTo(Array array, int index)
		{
			throw new NotImplementedException();
		}

		public int Count
		{
			get { return commissionSteps.Count + decommissionSteps.Count; }
		}

		public object SyncRoot
		{
			get { return commissionSteps.SyncRoot; }
		}

		public bool IsSynchronized
		{
			get { return commissionSteps.IsSynchronized; }
		}

		public IEnumerator GetEnumerator()
		{
			ArrayList newList = new ArrayList(commissionSteps);
			newList.AddRange(decommissionSteps);
			return newList.GetEnumerator();
		}
	}
}
