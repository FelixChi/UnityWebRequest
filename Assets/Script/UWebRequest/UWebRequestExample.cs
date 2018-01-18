using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UWebRequest;

public class UWebRequestExample : MonoBehaviour
{
	public string exampleUrl;
	public string ExampleUrl
	{
		get {return this.exampleUrl;}
		set 
		{
			if(!string.IsNullOrEmpty(value))
				this.exampleUrl = value;
		}
	}

	[SerializeField]
	UnityEngine.UI.Text texUrl;
	[SerializeField]
	UnityEngine.UI.Text texResponse;

	int requestMethod;

	// Use this for initialization
	void Start ()
	{
		this.texUrl.text = exampleUrl;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void SendExampleRequest()
	{
//		SendGetRequest();
		//
		SendJsonRequest();
	}
	void SendGetRequest()
	{
		StartCoroutine(GetRequestExample(this.exampleUrl, (UWebResponse<string> response) =>{
			if(response.isError || response.ResponseCode != 200)
			{
				//handle error msg
				Debug.LogFormat("{0} request error({1}): {2}",
					response.RequestURL, response.ResponseCode, response.ErrorMsg);
				return;
			}
			//handle success
			this.texResponse.text = response.GetResponseData();
		}));
	}
	void SendJsonRequest()
	{
		JsonExample json = new JsonExample(101, "Json101", 10.1f, 10.00101f, new Vector3(1,0,1));
		string jsonString = JsonUtility.ToJson(json);
		Debug.LogFormat("Post JSON request: {0}", jsonString);
//		Debug.Log("value "+ json.fValue +" "+ json.dValue);
		//
		StartCoroutine(PostRequest(this.exampleUrl, jsonString, WebExampleCallback<string>));
//
//		StartCoroutine(PostRequest(this.exampleUrl, jsonString,(UWebResponse<string> response) =>{
//			if(response.isError || response.ResponseCode != 200)
//			{
//				//handle error msg
//				Debug.LogFormat("{0} request error({1}): {2}",
//					response.RequestURL, response.ResponseCode, response.ErrorMsg);
//				return;
//			}
//			//handle success
//			this.texResponse.text = response.GetResponseData();
//		}));
	}
	/*---------------------------------------------------------------------*/
	//
	IEnumerator GetRequestExample(string url, UWebRequestCallback<string> callback)
	{
		var request = UWebRequestCreator.GetRequest(url, new DownloadHandlerJson());

		yield return request.Send();
		//Response
		string data = DownloadHandlerJson.GetContent(request);
		var response = new UWebResponse<string>(request.url, request.responseCode, request.error, data);
		callback(response);
	}
	//
	IEnumerator PostRequest(string url, string json, UWebRequestCallback<string> callback)
	{
		var request = UWebRequestCreator.PostJson(url, json, new DownloadHandlerJson());

		yield return request.Send();
		//Response
		string data = DownloadHandlerJson.GetContent(request);
		var response = new UWebResponse<string>(request.url, request.responseCode, request.error, data);
		callback(response);
	}

	void WebExampleCallback<T>(UWebResponse<T> response)
	{
		if(response.isError || response.ResponseCode != 200)
		{
			//handle error msg
			Debug.LogFormat("{0} request error({1}): {2}",
				response.RequestURL, response.ResponseCode, response.ErrorMsg);
			return;
		}
		//handle response success
		SetResponseText((string)(object)response.GetResponseData());
		string strJson = response.GetResponseData() as string;
		var dict = MiniJSON.Json.Deserialize(strJson) as Dictionary<string, object>;

	}

	public void SetResponseText(string text)
	{
		this.texResponse.text = text;
	}
}

[System.Serializable]
class JsonExample
{
	public int id;
	public string name;
	public float fValue;
	public double dValue;
	public Vector3 v3Value;

	public JsonExample(int id, string name, float value, double dv, Vector3 v3)
	{
		this.id = id;
		this.name = name;
		this.fValue = value;
		this.v3Value = v3;
		this.dValue = dv;
	}
}

