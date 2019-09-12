namespace Castle.MicroKernel.Facilities
{
	using System;

	using Castle.Model.Configuration;

	/// <summary>
	/// 基础设置抽象类
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
        /// 具体的基础设置必须实现的方法
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
			
			kernel = null;//释放掉对Kernel的引用
		}

		#endregion

		#region IDisposable Members

		public virtual void Dispose()
		{
		}

		#endregion
	}
}
