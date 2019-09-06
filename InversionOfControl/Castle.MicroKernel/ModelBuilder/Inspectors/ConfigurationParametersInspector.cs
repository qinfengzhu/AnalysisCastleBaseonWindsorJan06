namespace Castle.MicroKernel.ModelBuilder.Inspectors
{
	using System;
	using Castle.Model;
	using Castle.Model.Configuration;

	/// <summary>
    /// ��������е�'parameters'�ڵ�,
    /// ����ÿһ���ӽڵ㶼�ᴴ��һ������ģ��(ParameterModel)
    /// ����ģ�ͽ�����ӵ����ģ�͵Ĳ���������(Parameters)
	/// </summary>
	[Serializable]
	public class ConfigurationParametersInspector : IContributeComponentModelConstruction
	{
		/// <summary>
        /// ���ҹ�������������еĲ�����Ϣ,׷�ӵ����ģ�͵Ĳ�������(Parameters)��
		/// </summary>
		/// <param name="kernel">�ں�</param>
		/// <param name="model">���ģ��</param>
		public virtual void ProcessModel(IKernel kernel, ComponentModel model)
		{
			if (model.Configuration == null) return;

			IConfiguration parameters = model.Configuration.Children["parameters"];

			if (parameters == null) return;

			foreach(IConfiguration parameter in parameters.Children)
			{
				String name = parameter.Name;
				String value = parameter.Value;

				if (value == null && parameter.Children.Count != 0)
				{
					IConfiguration parameterValue = parameter.Children[0];
					model.Parameters.Add( name, parameterValue );
				}
				else
				{
					model.Parameters.Add( name, value );
				}
			}
		}
	}
}
