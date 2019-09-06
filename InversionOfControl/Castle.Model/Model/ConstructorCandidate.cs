namespace Castle.Model
{
	using System;
	using System.Reflection;

	/// <summary>
	/// 构造函数候选者
	/// </summary>
	[Serializable]
	public class ConstructorCandidate
	{
		private ConstructorInfo constructorInfo; //构造函数信息
		private DependencyModel[] dependencies; //依赖对象模型
		private int points;

		public ConstructorCandidate(ConstructorInfo constructorInfo, params DependencyModel[] dependencies )
		{
			this.constructorInfo = constructorInfo;
			this.dependencies = dependencies;
		}

		public ConstructorInfo Constructor
		{
			get { return constructorInfo; }
		}

		public DependencyModel[] Dependencies
		{
			get { return dependencies; }
		}

		public int Points
		{
			get { return points; }
			set { points = value; }
		}
	}
}
