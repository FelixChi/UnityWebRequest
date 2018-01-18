using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace UWebRequest
{
	/// <summary>
	/// Unity web request creator.
	/// Create web request simple
	/// </summary>
	public static class UWebRequestCreator
	{


		/*------------------------------------------------------------------*/
		#region Get Request
		//Create Get Request
		public static UnityWebRequest GetRequest(string uri)
		{
			return UWebRequestCreator.GetRequest(uri, new DownloadHandlerBuffer());
		}
		public static UnityWebRequest GetRequest(string uri, DownloadHandler downloader)
		{
			return new UnityWebRequest(uri, UnityWebRequest.kHttpVerbGET, downloader, null);
		}
		#endregion

		/*-----------------------------------------------------------------*/
		//Create Post Request
		public static UnityWebRequest PostRequest(string uri, byte[] data, DownloadHandler downloader)
		{
			UnityWebRequest request = new UnityWebRequest(uri, UnityWebRequest.kHttpVerbPOST);
			request.uploadHandler = new UploadHandlerRaw(data);
			request.downloadHandler = downloader;

			return request;
		}
		public static UnityWebRequest PostRequest(string uri, string postData, DownloadHandler downloader)
		{
			//string to byte[]
			byte[] data = null;
			if(!string.IsNullOrEmpty(postData))
			{
				//
				data = System.Text.Encoding.UTF8.GetBytes(postData);
			}

			UnityWebRequest request = UWebRequestCreator.PostRequest(uri, data, downloader);

			return request;
		}

		public static UnityWebRequest PostRequest(string uri, WWWForm formData, DownloadHandler downloader)
		{
			byte[] data = UWebRequestCreator.FormDataToByte(formData);
			UnityWebRequest request = UWebRequestCreator.PostRequest(uri, data, downloader);
			//set request headers
			if(formData != null)
				UWebRequestCreator.SetupRequestHeaders(request, formData.headers);

			return request;
		}

		private static byte[] FormDataToByte(WWWForm formData)
		{
			byte[] data = null;
			if(formData != null)
			{
				data = formData.data;
				if(data.Length == 0)
				{
					data = null;
				}
			}

			return data;
		}
		private static void SetupRequestHeaders(UnityWebRequest request, Dictionary<string, string> headers)
		{
			foreach(KeyValuePair<string, string> header in headers)
			{
				request.SetRequestHeader(header.Key, header.Value);
			}
		}

		//Post Json 
		public static UnityWebRequest PostJson(string uri, string jsonString, DownloadHandler downloader)
		{
			UnityWebRequest request = UWebRequestCreator.PostRequest(uri, jsonString, downloader);
			request.uploadHandler.contentType = "application/json; charset=utf-8";

			return request;
		}

	}
}

