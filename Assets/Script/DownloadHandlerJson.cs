using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

/*
 * use MemoryStream to store buffer
 * reference: ()
 * */

public class DownloadHandlerJson : DownloadHandlerScript
{
	private string jsonString;
	private MemoryStream buffer = null;
//	private T unmarhal = default(T);

	public DownloadHandlerJson() : base()
	{
	}
	

	public DownloadHandlerJson(byte[] preallocatedBuffer) : base(preallocatedBuffer)
	{
	}

//	public T jsonObject
//	{
//		get { return this.unmarhal; }
//	}
	
	protected override string GetText()
	{
		return this.jsonString;
	}
	protected override byte[] GetData()
	{
		//Raw data access is not support
		return null;
	}

	protected override void ReceiveContentLength(int contentLength)
	{
		base.ReceiveContentLength(contentLength);
		this.buffer = new MemoryStream(contentLength);
	}
	protected override bool ReceiveData(byte[] data, int dataLength)
	{
		this.buffer.Write(data, 0, dataLength);
		return true;
	}
	protected override void CompleteContent()
	{
		base.CompleteContent();
		this.jsonString = System.Text.Encoding.UTF8.GetString(this.buffer.ToArray());
//		this.unmarhal = JsonUtility.FromJson<T>(this.jsonString);
//		Debug.LogFormat("content {0}", jsonString);
		//release memory
		this.buffer.Dispose();
	}

//	public static T GetContent(UnityWebRequest request)
//	{
//		return (DownloadHandler.
//			GetCheckedDownloader<DownloadHandlerJson<T>>(request).jsonObject);
//	}
	/// <summary>
	/// Gets the object from downloaded json string.
	/// </summary>
	/// <returns>object.</returns>
	/// <typeparam name="T">object Type.</typeparam>
	public T GetJson<T>()
	{
		return JsonUtility.FromJson<T>(this.jsonString);
	}

	public static string GetContent(UnityWebRequest request)
	{
		return (DownloadHandler.
			GetCheckedDownloader<DownloadHandlerJson>(request).jsonString);
	}

}


