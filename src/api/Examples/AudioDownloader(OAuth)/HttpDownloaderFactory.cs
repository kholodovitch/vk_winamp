namespace HttpDownloader
{
	internal class HttpDownloaderFactory
	{
		private readonly string _url;

		internal delegate void HttpDownloaderFactoryProgressDelegate(object sender, HttpDownloaderFactoryProgressDelegateArgs args);

		internal class HttpDownloaderFactoryProgressDelegateArgs
		{
			public int PercentComplete { get; set; }
		}

		public HttpDownloaderFactory(string url)
		{
			_url = url;
		}

		public event HttpDownloaderFactoryProgressDelegate DownloadProgress;

		public void DownloadToFile(string path)
		{
		}

		public void Stop()
		{
		}
	}
}