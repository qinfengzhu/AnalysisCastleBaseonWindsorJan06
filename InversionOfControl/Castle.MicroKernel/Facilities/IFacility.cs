namespace Castle.MicroKernel
{
	using Castle.Model.Configuration;

	/// <summary>
    /// ��չ��Ԫ,һ��������ʩӦ��ʹ���ں��ṩ����չ���ǿ�Լ��Ĺ�����
	/// </summary>
	public interface IFacility
	{
		/// <summary>
		/// ��ʼ��
		/// </summary>
		/// <param name="kernel">Castle�ں�</param>
		/// <param name="facilityConfig">������ʩ����</param>
		void Init(IKernel kernel, IConfiguration facilityConfig);

		/// <summary>
		/// ������������
		/// </summary>
		void Terminate();
	}
}
