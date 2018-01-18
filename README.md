# UnityWebRequest
Simple Unity Web Request

# WebRequest Callback
```C#
void UWebRequestCallback<T>(UWebResponse<T> response)
{
  if(response.isError|| response.ResponseCode != 200)
  {
    //Log response error
  }
  //get response data
  string strResponse = response.GetResponseData() as string;
}
# Example
### Make a Get request
```C#
IEnumerator GetRequestExample(string url, UWebRequestCallback<string> callback)
{
    //Create Get Request
		var request = UWebRequestCreator.GetRequest(url, new DownloadHandlerJson());

		yield return request.Send();
		//Response
		string data = DownloadHandlerJson.GetContent(request);
		var response = new UWebResponse<string>(request.url, request.responseCode, request.error, data);
		callback(response);
}
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
```

# Reference
http://dev.twsiyuan.com/2016/09/unity-web-request.html
