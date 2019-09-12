namespace Castle.MicroKernel.Facilities
{
	using System;

	using Castle.Model.Configuration;

	/// <summary>
	/// �������ó�����
	/// </summary>
	public abstract class AbstractFacility : IFacility, IDisposable
	{
		private IKernel kernel;
		private IConfiguration facilityConfig;

		public IKernel Kernel
		{
			get { return kernel; }
		}

		public IConfiguration FacilityConfig
		{
			get { return facilityConfig; }
		}

        /// <summary>
        /// ����Ļ������ñ���ʵ�ֵķ���
        /// </summary>
		protected abstract void Init();

		#region IFacility Members

		public void Init(IKernel kernel, IConfiguration facilityConfig)
		{
			this.kernel = kernel;
			this.facilityConfig = facilityConfig;

			Init();
		}

		public void Terminate()
		{
			Dispose();
			
			kernel = null;//�ͷŵ���Kernel������
		}

		#endregion

		#region IDisposable Members

		public virtual void Dispose()
		{
		}

		#endregion
	}
}
