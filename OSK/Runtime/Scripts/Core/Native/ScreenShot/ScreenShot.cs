using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace OSK
{
public class ScreenShot : MonoBehaviour
{
    public void TakeScreenShot()
    {
        StartCoroutine(IEScreenShot());
    }

    private IEnumerator IEScreenShot()
    {
        string path = null;
#if UNITY_ANDROID
        path = "/sdcard/DCIM/ScreenShots/";
#endif

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        path = Application.dataPath + "/ScreenShots/";
#endif

        if (!string.IsNullOrEmpty(path))
        {
            yield return new WaitForEndOfFrame();

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            texture.Apply();
            string name = $"ScreenShotImage_{DateTime.Now.ToString("yyyyMMddhhmmss")}.png";
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(path + name, bytes);
            Debug.Log($"ScreenShot saved at {path + name}");
        }
        else
        {
            Debug.LogError("Path is null or empty");
            yield return null;
        }
    }
}
}