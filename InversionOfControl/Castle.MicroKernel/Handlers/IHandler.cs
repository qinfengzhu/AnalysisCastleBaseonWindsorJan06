namespace Castle.MicroKernel
{
	using System;

	using Castle.Model;

	/// <summary>
    /// ��ʶHandler��״̬
	/// </summary>
	public enum HandlerState
	{
		/// <summary>
		/// ������Ա�ʵ����
		/// </summary>
		Valid,
		/// <summary>
        /// ������ܱ�ʵ����,��Ϊ����һЩ������������
		/// </summary>
		WaitingDependency
	}

	/// <summary>
    /// ���������״̬,Э������Ĵ���(�ַ���Activators)������(�ַ�����������Manager)
	/// </summary>
	public interface IHandler
	{
		/// <summary>
		/// ��ʼ��Handler
		/// </summary>
		/// <param name="kernel">�ں�</param>
		void Init(IKernel kernel);

		/// <summary>
        /// ʵ���߱��뷵��һ�����õ����ʵ��,��������ܴ�����ʱ��Ӧ���׳��쳣
		/// </summary>
		object Resolve();

		/// <summary>
        /// ʵ���߱����ͷ����ʵ��
		/// </summary>
		/// <param name="instance"></param>
		void Release(object instance);

		/// <summary>
        /// Handler��ǰ״̬
		/// </summary>
		HandlerState CurrentState { get; }

		/// <summary>
        /// ���ģ��
		/// </summary>
		ComponentModel ComponentModel { get; }
	}
}
