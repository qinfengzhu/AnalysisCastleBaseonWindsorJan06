namespace Castle.MicroKernel
{
	/// <summary>
    /// ʵ��ʵ���Ĵ���
    /// Ĭ�ϵ�ʵ���������� Activator.CreateInstance()
	/// </summary>
	/// <remarks>
    /// �ӿ������Զ�����������,����ĳЩ�ض��Ĺ���������
	/// <br/>
    /// ���캯���������������
	/// <code>
	/// ComponentModel model, IKernel kernel, 
	/// ComponentInstanceDelegate onCreation, 
	/// ComponentInstanceDelegate onDestruction
	/// </code>
    /// ���������봥��onCreation��onDestruction�������¼�
	/// <seealso cref="ComponentActivator.AbstractComponentActivator"/>
	/// <seealso cref="ComponentActivator.DefaultComponentActivator"/>
	/// </remarks>
	public interface IComponentActivator
	{
		/// <summary>
		/// ���������ʵ������
		/// </summary>
		/// <returns></returns>
		object Create();

        /// <summary>
        /// Ӧ��ִ�����б�Ҫ�Ĺ������ͷ�ʵ����������ص��κ���Դ
        /// </summary>
        /// <param name="instance">���ʵ������</param>
        void Destroy(object instance);
	}
}
