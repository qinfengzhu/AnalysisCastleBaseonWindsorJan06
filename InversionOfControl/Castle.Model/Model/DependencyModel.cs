namespace Castle.Model
{
	using System;

    /// <summary>
    /// 依赖类型
    /// </summary>
	public enum DependencyType
	{
		Service,//服务
		Parameter//参数
	}

    /// <summary>
    /// 依赖模型(通过外部配置提供的固定值)
    /// </summary>
    [Serializable]
	public class DependencyModel
	{
		private String dependencyKey;
		private Type targetType;
		private bool isOptional;
		private DependencyType dependencyType;

		public DependencyModel( 
			DependencyType type, String dependencyKey, 
			Type targetType, bool isOptional)
		{
			this.dependencyType = type;
			this.dependencyKey = dependencyKey;
			this.targetType = targetType;
			this.isOptional = isOptional;
		}

		public DependencyType DependencyType
		{
			get { return dependencyType; }
		}

		public String DependencyKey
		{
			get { return dependencyKey; }
		}

		public Type TargetType
		{
			get { return targetType; }
		}

		public bool IsOptional
		{
			get { return isOptional; }
		}
	}
}
