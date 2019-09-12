namespace Castle.MicroKernel.SubSystems.Conversion
{
	using System;
	using System.Collections;

	using Castle.Model.Configuration;

	/// <summary>
	/// ���п���ת���������
	/// </summary>
	[Serializable]
	public class DefaultConversionManager : AbstractSubSystem, IConversionManager, ITypeConverterContext
	{
		private IList converters;

		public DefaultConversionManager()
		{
			converters = new ArrayList();

			InitDefaultConverters();
		}

		protected virtual void InitDefaultConverters()
		{
            //{char,DateTime,Decimal,Boolean,Int16,Int32,Int64,UInt16,UInt32,UInt64,Byte,SByte,Single,Double,String}
            Add( new PrimitiveConverter() ); //ֵ����ת�� Convert.ChangeType(value, targetType) string,type 

            //�ַ���ת��ΪType
            Add( new TypeNameConverter() ); //�ַ���:�������ƶ��� System.String ת��Ϊ Type

            //ö��ת��  Enum.Parse( targetType, value, true )
            Add( new EnumConverter() );

            //���ַ���ת��,���ܹ�֧�ִ�������Ϣ��ת��
			Add( new ListConverter() );

            //���ַ���ת��,���ܹ�֧�ִ�������Ϣ��ת��
			Add( new DictionaryConverter() );

            //���ַ���ת��,���ܹ�֧�ִ�������Ϣ��ת��
			Add( new ArrayConverter() ); 
		}

		#region IConversionManager Members

		public void Add(ITypeConverter converter)
		{
			converter.Context = this;

			converters.Add(converter);
		}

		#endregion

		#region ITypeConverter Members

		public ITypeConverterContext Context
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public bool CanHandleType(Type type)
		{
			foreach(ITypeConverter converter in converters)
			{
				if (converter.CanHandleType(type)) return true;
			}

			return false;
		}

		public object PerformConversion(String value, Type targetType)
		{
			foreach(ITypeConverter converter in converters)
			{
				if (converter.CanHandleType(targetType)) 
					return converter.PerformConversion(value, targetType);
			}

			String message = String.Format("No converter registered to handle the type {0}", 
				targetType.FullName);

			throw new ConverterException(message);
		}

		public object PerformConversion(IConfiguration configuration, Type targetType)
		{
			foreach(ITypeConverter converter in converters)
			{
				if (converter.CanHandleType(targetType)) 
					return converter.PerformConversion(configuration, targetType);
			}

			String message = String.Format("No converter registered to handle the type {0}", 
				targetType.FullName);

			throw new ConverterException(message);
		}

		#endregion

		#region ITypeConverterContext Members

		public ITypeConverter Composition
		{
			get { return this; }
		}

		#endregion
	}
}
