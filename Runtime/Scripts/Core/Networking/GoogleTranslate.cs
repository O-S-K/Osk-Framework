// This is paid service version of Unity Google Translator script https://gist.github.com/IJEMIN/a48f8f302190044de05e3e3fea342fbd

using System;
using System.Collections; 
using UnityEngine;
using UnityEngine.Networking;

public class GoogleTranslate : MonoBehaviour
{
	private void Awake()
	{
		DoExample();
	}
    
	// Remove this method after understanding how to use.
	private void DoExample()
	{
		TranslateText("en","ko","I'm a real gangster.", (success, translatedText) =>
		{
			if (success)
			{
				Debug.Log(translatedText);
			}
		});
        
		TranslateText("ko","en","나는 리얼 갱스터다.", (success, translatedText) =>
		{
			if (success)
			{
				Debug.Log(translatedText);
			}
		});
	}

	public void TranslateText(string sourceLanguage, string targetLanguage, string sourceText, Action<bool, string> callback)
	{
		StartCoroutine(TranslateTextRoutine(sourceLanguage, targetLanguage, sourceText, callback));
	}

	private static IEnumerator TranslateTextRoutine(string sourceLanguage, string targetLanguage, string sourceText, Action<bool, string> callback)
	{
		var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={sourceLanguage}&tl={targetLanguage}&dt=t&q={UnityWebRequest.EscapeURL(sourceText)}";

		var webRequest = UnityWebRequest.Get(url);
		yield return webRequest.SendWebRequest();

		if (webRequest.isHttpError || webRequest.isNetworkError)
		{
			Debug.LogError(webRequest.error);
			callback.Invoke(false, string.Empty);

			yield break;
		}

 	// 	var parsedTexts = SimpleJSON.JSONNode.Parse(webRequest.downloadHandler.text);
		// var translatedText = parsedTexts[0][0][0];
		//
		// callback.Invoke(true, translatedText);
    }
}
