namespace Castle.MicroKernel.Util
{
	using System;

	/// <summary>
	/// ���ñ��ʽ����
	/// </summary>
	public abstract class ReferenceExpressionUtil
	{
		/// <summary>
		/// �Ƿ�Ϊ���ñ��ʽ
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
        /// ��ȡ����ؼ���
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
