
namespace Castle.MicroKernel
{
	using System;
	using System.Collections;

	using Castle.Model;

	using Castle.MicroKernel.ModelBuilder;

    /// <summary>
    /// ʵ���߱���ͨ�����������������������ģ��
    /// </summary>
    public interface IComponentModelBuilder
	{
        /// <summary>
        /// ͨ������ע������ģ�͹�����(ʵ����IContributeComponentModelConstruction)������һ���µ����ģ��
        /// </summary>
        /// <param name="key">���Ψһ��ʶ</param>
        /// <param name="service">�����������</param>
        /// <param name="classType">���ʵ������</param>
        /// <param name="extendedProperties">�����չ����</param>
        /// <returns>���ģ��</returns>
        ComponentModel BuildModel( String key, Type service, Type classType, IDictionary extendedProperties );

        /// <summary>
        /// ���ģ�͹�����Ӧ�ü��������������������������ã��Ա���ģ������ӻ���Ŀɹ��Ժ�ʹ�õ���Ϣ
        /// </summary>
        void AddContributor(IContributeComponentModelConstruction contributor );

        /// <summary>
        /// �Ƴ�ָ�������ģ�͹�����(ʵ����IContributeComponentModelConstruction)
        /// </summary>
        /// <param name="contributor">���ģ�͹�����</param>
        void RemoveContributor(IContributeComponentModelConstruction contributor );
	}
}
