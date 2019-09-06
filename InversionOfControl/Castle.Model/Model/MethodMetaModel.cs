namespace Castle.Model
{
	using System;
	using Castle.Model.Configuration;

	[Serializable]
	public class MethodMetaModel
	{
		private readonly IConfiguration configNode;

		public MethodMetaModel(IConfiguration configNode)
		{
			this.configNode = configNode;
		}

		public IConfiguration ConfigNode
		{
			get { return configNode; }
		}
	}
}
