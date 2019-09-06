namespace Castle.Model.Resource
{
	using System;
	using System.IO;

	/// <summary>
	/// TODO: Find out how to open a file through UNC
	/// </summary>
	public class UncResource : AbstractStreamResource
	{
		private readonly Stream stream;
		private String basePath;

		public UncResource(Uri resource)
		{
			stream = CreateStreamFromUri(resource, DefaultBasePath);
		}

		public UncResource(Uri resource, String basePath)
		{
			stream = CreateStreamFromUri(resource, basePath);
		}

		public UncResource(String resourceName) : this(new Uri(resourceName))
		{
		}

		public UncResource(String resourceName, String basePath) : this(new Uri(resourceName), basePath)
		{
		}

		protected override Stream Stream
		{
			get { return stream; }
		}

		public override String FileBasePath
		{
			get { return basePath; }
		}

		public override IResource CreateRelative(String resourceName)
		{
			return new UncResource( Path.Combine(basePath, resourceName) );
		}

		private Stream CreateStreamFromUri(Uri resource, String basePath)
		{
			if (resource == null) 
				throw new ArgumentNullException("resource");
			if (!resource.IsUnc) 
				throw new ArgumentException("Resource must be an Unc", "resource");
			if (!resource.IsFile) 
				throw new ArgumentException("The specified resource is not a file", "resource");

			int count = Uri.UriSchemeFile.Length + Uri.SchemeDelimiter.Length;

			String filePath = @"\\" + resource.AbsoluteUri.Substring(count);

			if (!File.Exists(filePath) && basePath != null)
			{
				filePath = Path.Combine( basePath, filePath );
			}

			this.basePath = Path.GetDirectoryName(filePath);
			
			CheckFileExists(filePath);

			return File.OpenRead(filePath);
		}

		private void CheckFileExists(String path)
		{
			if (!File.Exists(path))
			{
				String message = String.Format("File {0} could not be found", path);
				throw new ResourceException(message);
			}
		}
	}
}
