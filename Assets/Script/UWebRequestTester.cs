using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UWebRequest;

public class UWebRequestTester : MonoBehaviour {
	public string testURL;

	[SerializeField]
	Text textResponse;
	[SerializeField]
	Text textResponse2;
	[SerializeField]
	Text textUrl;

	public JSONRequest testJsonRequest;

	// Use this for initialization
	void Start () {
		textUrl.text = this.testURL;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SendRequest()
	{
		textUrl.text = this.testURL;
		//send web request
//		StartCoroutine(GetWebRequest(testURL));//, UnityWebRequest.kHttpVerbGET));

		//
//		StartCoroutine(PostJsonRequest(this.testURL, this.testJsonRequest, onResponseRecevied));
		//
		StartCoroutine(PostFormRequest(this.testURL, response=> {
			this.textResponse.text = response.GetResponseData();
		}));

		StartCoroutine(PostJsonRequest(this.testURL, this.testJsonRequest, response=> {
			this.textResponse2.text = response.GetResponseData();
		}));
	}
	/*----------------------------------------------------------------------------------*/
	private void onResponseRecevied<T>(UWebResponse<T> response)
	{
		//
//		Debug.LogFormat("Web request {0} succes.\nResponse: {1}", url, response.ToString());
		if(response.isError)
		{

			Debug.LogFormat("request error {0}", response.ErrorMsg);
			return;
		}
		Debug.LogFormat("response {0} status code {1}", response.RequestURL, response.ResponseCode);
		if(response.ResponseCode != 200)
		{
			return;
		}
		string data = (string)(object)response.GetResponseData();
		this.textResponse.text = data;
	}

	/*------------------------------------------------------------------------------*/
	IEnumerator PostJsonRequest(string url, object data, WebResponseHandler<string> onResponse)
	{
		var jsonString = JsonUtility.ToJson(data);
//		var req = CreatePostJSONRequest(url, data);
//		var req = UnityWebRequest.Post(url, jsonString);
//		req.downloadHandler = new DownloadHandlerJson();
//		req.method = "POST";
		var req = UWebRequest.UWebRequestCreator.PostJson(url, jsonString, new DownloadHandlerJson());//
		yield return req.Send();
		//
		if(req.isError)
		{
			//Handle Error
			Debug.LogFormat("request {0} error {1}", url, req.error);
//			yield break;
		}
		if(req.responseCode != 200)
		{
			Debug.LogFormat("request {0} web error {1}", url, req.responseCode);
//			yield break;
		}
		//
		var jsonStr = DownloadHandlerJson.GetContent(req);
		//
		var response = new UWebResponse<string>(url, req.responseCode, req.error, jsonStr);
		onResponse(response);
		Debug.LogFormat("Web request {0} succes.\nResponse: {1}", url, jsonStr.ToString());
//		this.textResponse.text = jsonStr;
		//

	}
	IEnumerator PostFormRequest(string url, WebResponseHandler<string> onResponse)
	{
		WWWForm form = new WWWForm();
		form.AddField("username", "Test001");
		form.AddField("name", "Tester 001");
		form.AddField("action", "PostRequest");

		var req = UWebRequest.UWebRequestCreator.PostRequest(url, form, new DownloadHandlerBuffer());//UnityWebRequest.Post(url, form);

		yield return req.Send();
		var strResponse = req.downloadHandler.text;
		var response = new UWebResponse<string>(url, req.responseCode, req.error, strResponse);
		Debug.LogFormat("Web request {0} succes.\nResponse: {1}", url, strResponse);
		onResponse(response);
	}
	IEnumerator SendWebRequest(string url, string method)
	{
		//Create Request
		//UnityWebRequest.Get(url);
		var request = CreateGetRequest(url);//new UnityWebRequest(url, method);
		byte[] data = new byte[1];
		//request.uploadHandler = new UploadHandlerRaw(data);
		//request.downloadHandler = new DownloadHandlerBuffer();
		//
		Debug.Log("Request content: "+ request.GetRequestHeader("Accept"));//request.uploadHandler.contentType);
		//request send
		yield return request.Send();
		//downloadhandler.text would change!!!
		//Web Response
		var uwResponse = new UWebResponse<string>(url, request.responseCode,
			request.error, request.downloadHandler.text);
		if(request.isError)
		{
			//Request Error
			Debug.Log(""+ request.error);
			//Handle Error
			//show error msg to TextUI
			textResponse.text = request.error;
		}
		//Log response
		var response = ResponseToString(request.downloadHandler.data);
		Debug.Log("Web request succes.\n Response: "+ response);
		
		//Handle Response
		//show response to TextUI
		this.textResponse.text = response;
	}
	IEnumerator GetWebRequest(string url)
	{
		UWebResponse<string> response;
//		using(var request = CreateGetRequest(url))
//		{
//			//request send
//			yield return request.Send();
//			//Web Response
//			response = new UWebResponse(request.responseCode,
//				request.downloadHandler.data, request.error);
			//
//		}
		//
		var req = CreateGetRequest(url);
		yield return req.Send();
		//Web Response

//		response = new UWebResponse(req.responseCode,
//			req.downloadHandler.data, req.error);
		//
		if(req.isError)
		{
			//Handle error
			Debug.Log(string.Format("request {0} error {1}", url, req.error));
			yield break;
		}
		//
		string strResponse = DownloadHandlerBuffer.GetContent(req);//response.ToString();
		Debug.LogFormat("Web request {0} succes.\nResponse: {1}", url, strResponse);
		//show response to TextUI
		this.textResponse.text = strResponse;
	}


	UnityWebRequest CreateGetRequest(string url)
	{
		var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
		//set download handler
		request.downloadHandler = new DownloadHandlerBuffer();
		//
		return request;
	}
	UnityWebRequest CreatePostJSONRequest(string url, object data)
	{
		if(data == null)
			return null;
		//create uploadhandler for post json data
		var uploader = CreateJsonUploadHandler(data);
		//create downloadhandler for recive json
		var downloader = new DownloadHandlerJson();

		var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPUT);
		request.method = "POST";
		request.uploadHandler = uploader;
		request.downloadHandler = downloader;
		//set request header
		request.SetRequestHeader("Accept", "application/json");
//		request.SetRequestHeader("Accept-Charset", "utf-8");

		return request;
	}

	private UploadHandlerRaw CreateJsonUploadHandler(object data)
	{
		if(data == null)
			return null;
		var jsonRaw = JsonToByte(data);
		//
		var uploader = new UploadHandlerRaw(jsonRaw);
		uploader.contentType = "application/json; charset=utf-8";

		return uploader;
	}
//	private DownloadHandlerJson<TResult> CreateDownloadHandlerJson<TResult>()
//	{
//		
//	}

	string ResponseToString(byte[] data)
	{
		//byte data to UTF8 encoding string
		return System.Text.Encoding.UTF8.GetString(data);
	}
	byte[] JsonToByte(object data)
	{
		if(data == null)
			return null;
		var json = JsonUtility.ToJson(data);
		return (System.Text.Encoding.UTF8.GetBytes(json));	
	}

	/*--------------------------------------------------------------*/
	public delegate void WebResponseReceiver(string response);
	public delegate void WebResponseHandler<T>(UWebResponse<T> response);

}
