using System;

namespace UWebRequest
{
	public class UWebResponse<T>
	{
		//
		private string url;
		private T responseData = default(T);
		private long responseCode;
		private string errorMsg;

		public bool isError { get { return !string.IsNullOrEmpty(errorMsg);} }

		public UWebResponse(string url, long code, string error, T data)
		{
			this.url = url;
			this.responseCode = code;
			this.responseData = data;
			this.errorMsg = error;
		}
		public string RequestURL
		{
			get {return this.url;}
		}
		public long ResponseCode
		{
			get {return this.responseCode;}
		}
		public string ErrorMsg
		{
			get {return this.errorMsg;}
		}
		public T GetResponseData()
		{
			return this.responseData;
		}

	//	public string ResponseToString()
	//	{
	//		//byte data to UTF8 encoding string
	//		return System.Text.Encoding.UTF8.GetString(this.responseData);
	//	}

	}
}
