// Copyright 2004-2006 Castle Project - http://www.castleproject.org/
namespace Castle.MicroKernel.ModelBuilder.Inspectors
{
	using System;

	using Castle.Model.Configuration;

    /// <summary>
    /// ʹ�����ں�ע������òִ�<see cref="IConfiguration"/>�������
    /// </summary>
    [Serializable]
	public class ConfigurationModelInspector : IContributeComponentModelConstruction
	{
		/// <summary>
        /// ͨ���ں��е����òִ���ѯ���������������Ϣ
		/// </summary>
		/// <param name="kernel"></param>
		/// <param name="model"></param>
		public virtual void ProcessModel(IKernel kernel, Castle.Model.ComponentModel model)
		{
			model.Configuration = kernel.ConfigurationStore.GetComponentConfiguration(model.Name);
		}
	}
}
