namespace Castle.MicroKernel.ModelBuilder
{
	using Castle.Model;

	/// <summary>
    /// �������ģ�͹����ӿ�
    /// ʵ���߱�����������Ҹ������ģ��(ComponentModel)��һЩ������Ϣ
	/// </summary>
	public interface IContributeComponentModelConstruction
	{
		/// <summary>
        /// ͨ��ʵ���߶���ȥ�鿴���ģ�ͻ�ӿڵ���������,����ȥ�鿴һЩ�����Ķ���
		/// </summary>
		/// <param name="kernel">�ں�ʵ������</param>
		/// <param name="model">���ģ��</param>
		void ProcessModel(IKernel kernel, ComponentModel model);
	}
}
