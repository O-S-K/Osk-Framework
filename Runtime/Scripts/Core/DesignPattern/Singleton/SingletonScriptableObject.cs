using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
	public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
	{
		#region Member Variables

		private static T instance;

		#endregion

		#region Properties

		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					GetInstance(typeof(T).Name);
				}

				return instance;
			}
		}

		#endregion

		#region Private Methods

		private static void GetInstance(string name)
		{
			instance = Resources.Load<T>(name);

			if (instance != null)
			{
				return;
			}

			Debug.Log("Creating Resources/" + name + ".asset");

			instance = ScriptableObject.CreateInstance<T>();

#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				if (!System.IO.Directory.Exists(Application.dataPath + "/Resources"))
				{
					System.IO.Directory.CreateDirectory(Application.dataPath + "/Resources");
				}

				UnityEditor.AssetDatabase.CreateAsset(instance, "Assets/Resources/" + name + ".asset");
				UnityEditor.AssetDatabase.SaveAssets();
			}
#endif
		}

		#endregion
	}
}
