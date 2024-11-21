using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OSK
{
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private string key;
        [SerializeField] private bool isUpdateOnStart = true;
        [SerializeField, ReadOnly] private SystemLanguage currentLanguage;
        private object textComponent;

        private void Awake()
        {
            textComponent = GetComponent<Text>() ??
                            (object)GetComponent<TextMeshPro>() ??
                            (object)GetComponent<TextMeshProUGUI>() ??
                            (object)GetComponent<TextMesh>();

            if (textComponent == null)
            {
                Logg.LogWarning("No suitable text component found on " + gameObject.name);
                return;
            }
        }

        private void Start()
        {
            if (isUpdateOnStart && textComponent != null)
            {
                UpdateText();
            }
             
            Main.Observer.Add("UpdateLanguage", UpdateText);
        }

        private void OnDestroy()
        {
            Main.Observer.Remove("UpdateLanguage", UpdateText);
        }

        private void OnEnable()
        {
            if (Main.Localization == null)
                return;

            if (Main.Localization.IsSetDefaultLanguage)
            {
                if (currentLanguage != Main.Localization.GetCurrentLanguage)
                {
                    currentLanguage = Main.Localization.GetCurrentLanguage;
                    UpdateText();
                }
            }
        }
 

#if UNITY_EDITOR
        // public SystemLanguage language;
        // public bool isOnValidate = false;
        //
        // private void OnValidate()
        // {
        //     if (isOnValidate)
        //     {
        //         UpdateText();
        //     }
        // }

        [Button]
        private void CheckKeyExist()
        {
            if (string.IsNullOrEmpty(key))
            {
                Logg.LogError("Key is empty");
                return;
            }

            CheckKeyOnFile();
        }

        private string CheckKeyOnFile()
        {
            var path = FindObjectOfType<GameConfigs>().Game.path.pathLoadFileCsv;
            var textAsset = Resources.Load<TextAsset>(path);
            if (textAsset == null)
            {
                Logg.LogError("Not found localization file: " + path);
                return null;
            }

            var lines = textAsset.text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var split = line.Split(',');
                if (split[0] == key)
                {
                    Logg.Log($"Key found: {split[0]}", ColorCustom.Green, 15);
                    return split[1];
                }
            }

            Logg.Log($"Key Not found: {key}", ColorCustom.Red, 15);
            return null;
        }
#endif

        private void UpdateText(object data = null)
        {
            if (textComponent == null)
                return;

            string localizedText = Main.Localization.GetKey(key);
            if (string.IsNullOrEmpty(localizedText))
                return;

            if (textComponent is Text uiText)
            {
                uiText.text = localizedText;
            }
            else if (textComponent is TMPro.TMP_Text tmpText)
            {
                tmpText.text = localizedText;
            }
        }

        public void SetText(string text)
        {
            if (textComponent is Text uiText)
            {
                uiText.text = text;
            }
            else if (textComponent is TMPro.TMP_Text tmpText)
            {
                tmpText.text = text;
            }
        }
    }
}