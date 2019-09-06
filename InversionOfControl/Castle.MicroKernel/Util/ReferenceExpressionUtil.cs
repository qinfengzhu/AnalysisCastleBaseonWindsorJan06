namespace Castle.MicroKernel.Util
{
	using System;

	/// <summary>
	/// 引用表达式工具
	/// </summary>
	public abstract class ReferenceExpressionUtil
	{
		/// <summary>
		/// 是否为引用表达式
		/// </summary>
		public static bool IsReference(String value)
		{
			if (value == null || value.Length <= 3 || 
				!value.StartsWith("${") || !value.EndsWith("}"))
			{
				return false;
			}

			return true;
		}

        /// <summary>
        /// 提取组件关键字
        /// </summary>
		public static String ExtractComponentKey(String value)
		{
			if (IsReference(value))
			{
				return value.Substring( 2, value.Length - 3 );
			}

			return null;
		}
	}
}
