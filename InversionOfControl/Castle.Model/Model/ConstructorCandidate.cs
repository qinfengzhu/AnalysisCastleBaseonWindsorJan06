namespace Castle.Model
{
	using System;
	using System.Reflection;

	/// <summary>
	/// ���캯����ѡ��
	/// </summary>
	[Serializable]
	public class ConstructorCandidate
	{
		private ConstructorInfo constructorInfo; //���캯����Ϣ
		private DependencyModel[] dependencies; //��������ģ��
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
