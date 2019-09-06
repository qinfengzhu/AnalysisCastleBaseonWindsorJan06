namespace Castle.MicroKernel
{
	using System;

	using Castle.Model;

    /// <summary>
    /// �������������Ϣ��ί��
    /// </summary>
    /// <param name="key">�����Ψһ��ʶ</param>
    /// <param name="handler">�������,���ܹ�ʵ�������</param>
    public delegate void ComponentDataDelegate( String key, IHandler handler );

    /// <summary>
    /// �������������Ϣ��ʵ����ί��
    /// </summary>
    /// <param name="model">�����Ԫ������Ϣ(meta)</param>
    /// <param name="instance">���ʵ��</param>
    public delegate void ComponentInstanceDelegate( ComponentModel model, object instance );

    /// <summary>
    /// �������Ԫ������Ϣ��ί��
    /// </summary>
    public delegate void ComponentModelDelegate( ComponentModel model );

    /// <summary>
    /// ���������������ί��
    /// </summary>
    /// <param name="handler">�������,���ܹ�ʵ�������</param>
    public delegate void HandlerDelegate( IHandler handler, ref bool stateChanged );

	/// <summary>
	/// ��������������ί��
	/// </summary>
	public delegate void DependencyDelegate(ComponentModel client, DependencyModel model, Object dependency);

	/// <summary>
	/// ���ں��¼��ӿڵĻ���
	/// </summary>
	public interface IKernelEvents
	{
		/// <summary>
        /// ��������ں���ע���ʱ��,�������¼�
		/// </summary>
		event ComponentDataDelegate ComponentRegistered;

		/// <summary>
        /// ��������ں����Ƴ���ʱ��,�������¼�
		/// </summary>
		event ComponentDataDelegate ComponentUnregistered;

		/// <summary>
        /// �����ģ�ͱ�������ʱ��,�������¼�
        /// ����һЩ����Ӱ�촦�����(handler)���Զ������
		/// </summary>
		event ComponentModelDelegate ComponentModelCreated;

		/// <summary>
        /// ���ں��Ժ��ӽڵ㷽ʽ���뵽��һ���ں��е�ʱ��,�������¼�
		/// </summary>
		event EventHandler AddedAsChildKernel;

		/// <summary>
        /// ���������ǰ,�������¼�
		/// </summary>
		event ComponentInstanceDelegate ComponentCreated;

		/// <summary>
        /// ���ʵ���������ٵ�ʱ��,�������¼�
		/// </summary>
		event ComponentInstanceDelegate ComponentDestroyed;

		/// <summary>
        /// �������(handler)��ע���ʱ��,�������¼�
		/// </summary>
		event HandlerDelegate HandlerRegistered;

		/// <summary>
        /// ��������ʱ��,�������¼�
        /// �����������ı�,�ͻ������ģ�Ͳ��ܱ��ı�
		/// </summary>
		event DependencyDelegate DependencyResolving;
	}
}
