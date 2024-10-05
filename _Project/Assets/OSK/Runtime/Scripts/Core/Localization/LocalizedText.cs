using System;
using CustomInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OSK
{
    public class LocalizedText : MonoBehaviour
    {
        public string key;
        public bool isUpdateOnStart = true;

        private object textComponent;

        private void Start()
        {
            textComponent = GetComponent<Text>() ??
                            (object)GetComponent<TextMeshPro>() ??
                            (object)GetComponent<TextMeshProUGUI>() ??
                            (object)GetComponent<TextMesh>();

            if (textComponent == null)
            {
                Debug.LogWarning("No suitable text component found on " + gameObject.name);
            }

            if (isUpdateOnStart)
            {
                UpdateText();
            }
        }

#if UNITY_EDITOR
        public SystemLanguage language;
        public bool isOnValidate = false;
        private void OnValidate()
        {
            if (isOnValidate)
            {
                UpdateText();
            }
        }
        
        [Button]
        private void CheckKeyInLocalization()
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
            var path = FindObjectOfType<LocalizationManager>().pathLoadFileCsv;
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
                    Logg.Log($"Key found: {split[0]}", Color.green, 15);
                    return split[1];
                }
            }
            Logg.Log($"Key Not found: {key}", Color.red, 15);
            return  null;
        }
#endif

        public void UpdateText()
        {
            string localizedText = Main.Localization.GetKey(key);

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