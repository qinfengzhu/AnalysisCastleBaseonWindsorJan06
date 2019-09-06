namespace Castle.MicroKernel.ModelBuilder.Inspectors
{
	using System;
	using System.Collections;
	using System.Configuration;
	using System.Reflection;
	using Castle.Model;
	using Castle.Model.Configuration;
	using Castle.MicroKernel.SubSystems.Conversion;

    /// <summary>
    /// ��������ļ��е�methods�ڵ�
    /// ÿһ��������Ӧһ��<see cref="MethodMetaModel"/> 
    /// </summary>
    public class MethodMetaInspector : IContributeComponentModelConstruction
	{
		private static readonly BindingFlags AllMethods = 
			BindingFlags.Public|BindingFlags.NonPublic|
			BindingFlags.Instance|BindingFlags.Static|
			BindingFlags.IgnoreCase|BindingFlags.IgnoreReturn;

		private ITypeConverter converter;

		public virtual void ProcessModel(IKernel kernel, ComponentModel model)
		{
			if (model.Configuration == null || model.Implementation == null) return;

			IConfiguration methodsNode = model.Configuration.Children["methods"];

			if (methodsNode == null) return;

			EnsureHasReferenceToConverter(kernel); //ת����

			foreach(IConfiguration methodNode in methodsNode.Children)
			{
				String name = methodNode.Name;

				if ("method".Equals(name))
				{
					name = methodNode.Attributes["name"];
				}

				AssertNameIsNotNull(name, model);

				MethodMetaModel metaModel = new MethodMetaModel(methodNode);

				model.MethodMetaModels.Add( metaModel );

				String signature = methodNode.Attributes["signature"]; //ǩ��,����Ϊ���͵�ȫ��(System.String;System.Collection.Genirec.List`)

				MethodInfo[] methods = GetMethods(model.Implementation, name, signature);

				RegisterMethodsForFastAccess(methods, signature, metaModel, model); //�ǼǷ��������ģ����
			}
		}

		private void RegisterMethodsForFastAccess(MethodInfo[] methods,String signature, MethodMetaModel metaModel, ComponentModel model)
		{
			foreach(MethodInfo method in methods)
			{
				if (signature != null && signature.Length != 0)
				{
					model.MethodMetaModels.MethodInfo2Model[method] = metaModel;
				}
				else
				{
					if (!model.MethodMetaModels.MethodInfo2Model.Contains(method))
					{
						model.MethodMetaModels.MethodInfo2Model[method] = metaModel;
					}
				}
			}
		}

		private void AssertNameIsNotNull(string name, ComponentModel model)
		{
			if (name == null)
			{
				String message = String.Format("The configuration nodes within 'methods' " + 
					"for the component '{0}' does not have a name. You can either name " + 
					"the node as the method name or provide an attribute 'name'", model.Name);
				throw new ConfigurationException(message);
			}
		}

		private void EnsureHasReferenceToConverter(IKernel kernel)
		{
			if (converter != null) return;

			converter = (ITypeConverter)kernel.GetSubSystem(SubSystemConstants.ConversionManagerKey);
		}

        /// <summary>
        /// ���Ȼ�ȡǩ���ķ���
        /// ��ǩ����ֵ��ʱ��,Ϊ�����Ĳ������ͼ�����';'���зָ�
        /// </summary>
        /// <param name="implementation">����������������</param>
        /// <param name="name">��������</param>
        /// <param name="signature">������������ǩ��</param>
		private MethodInfo[] GetMethods(Type implementation, String name, String signature)
		{
			if (signature == null || signature.Length == 0)
			{
				MethodInfo[] allmethods = implementation.GetMethods(AllMethods);

				ArrayList methods = new ArrayList();

				foreach(MethodInfo method in allmethods)
				{
					if (String.Compare(method.Name, name, true) == 0)
					{
						methods.Add(method);
					}
				}

				return (MethodInfo[]) methods.ToArray(typeof(MethodInfo));
			}
			else
			{
				MethodInfo methodInfo = implementation.GetMethod(name, AllMethods, null, ConvertSignature(signature), null ); //��ȡ�ض�ǩ����������

				if (methodInfo == null) return new MethodInfo[0];

				return new MethodInfo[] { methodInfo };
			}
		}

        /// <summary>
        /// ����ǩ����������ת��,����(System.String;System.Int)
        /// </summary>
        /// <param name="signature">����ǩ��</param>
        /// <returns>�������ͼ���</returns>
		private Type[] ConvertSignature(string signature)
		{
			String[] parameters = signature.Split(';');

			ArrayList types = new ArrayList();

			foreach(String param in parameters)
			{
				try
				{
					types.Add(converter.PerformConversion(param,typeof(Type)));
				}
				catch(Exception ex)
				{
					String message = String.Format("The signature {0} contains an entry type {1} " + 
						"that could not be converted to System.Type. Check the inner exception for " + 
						"details", signature, param);

					throw new ConfigurationException(message, ex);
				}
			}

			return (Type[]) types.ToArray( typeof(Type) );
		}
	}
}
