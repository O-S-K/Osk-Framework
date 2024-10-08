using System;
using System.Collections.Generic;
using System.Text;
using CustomInspector;
using UnityEngine;
using UnityEngine.Serialization;

/*
 *
 *  // gen for AI: https://x.com/LoloelRolo/status/1221259358585217030

Key	                 	French	                            Vietnamese
welcome_message	        Welcome to the game!	            Chào mưn bạn đến với trò chơi!
exit_prompt	            Are you sure you want to exit?	    Bạn có chắc chắn muốn thoát không?

 Main.Localization.SetLanguage("English"); // Or "Vietnamese", etc.
 .text = World.Localization.GetLocalizedString("welcome_message");
*/

namespace OSK
{
    public class LocalizationManager : GameFrameworkComponent
    {
        private Dictionary<string, string> _localizedText = new Dictionary<string, string>();
        public string currentLanguage = "English";
        public string excelFilePath = "Assets/Localization.xlsx";
        public string outputCsvPath = "Assets/_Project/Resources/Localization/Localization";
        public string pathLoadFileCsv = "Localization/Localization";
        private bool isSetDefaultLanguage = false;
        
        
        public void SetLanguageAppSystem()
        {
            SetLanguage(Application.systemLanguage.ToString());
        }

        public void SetLanguage(string languageCode)
        {
            isSetDefaultLanguage = true;
            LoadLocalizationData(languageCode);
            currentLanguage = languageCode;
            Logg.Log($"Set language to: {languageCode}", ColorCustom.Green, 15);

            // debug all keys
            // foreach (var key in _localizedText.Keys)
            // {
            //     Logg.LogFormat("Key: {0} - Value: {1}", key, _localizedText[key], ColorCustom.Green);
            // }
        }

        public void SwitchLanguage(SystemLanguage language)
        {
            SetLanguage(language.ToString());
            UpdateAllText();
        }

        public void SwitchLanguage(string language)
        {
            SetLanguage(language);
            UpdateAllText();
        }

        [Button]
        private void UpdateAllText()
        {
            var texts = FindObjectsOfType<LocalizedText>();
            foreach (var text in texts)
            {
                text.UpdateText();
            }
        }

        public string GetCurrentLanguage()
        {
            return currentLanguage;
        }


        private void LoadLocalizationData(string languageCode)
        {
            TextAsset textFile = Resources.Load<TextAsset>(pathLoadFileCsv);
            if (textFile == null)
            {
                Logg.LogError("Not found localization file: " + pathLoadFileCsv);
                return;
            }

            // Parse CSV lines while removing empty lines
            string[] lines = textFile.text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Clear the previous localization data
            _localizedText.Clear();

            // Find the index of the language code in the CSV header
            string[] headers = lines[0].Split(',');
            int languageColumnIndex = Array.IndexOf(headers, languageCode);

            if (languageColumnIndex == -1)
            {
                Logg.LogError($"Language '{languageCode}' not found in localization file.");
                return;
            }

            // Start from the second line to skip the header
            for (int i = 1; i < lines.Length; i++)
            {
                // Handle CSV values with commas or line breaks inside quotes
                string[] columns = ParseCsvLine(lines[i]);

                // Check if the column exists and ensure the first column (key) is present
                if (columns.Length > languageColumnIndex && !string.IsNullOrWhiteSpace(columns[0]))
                {
                    string key = columns[0].Trim();
                    string value = columns[languageColumnIndex].Trim();

                    // Store the key-value pair in the dictionary
                    _localizedText[key] = value;
                }
                else
                {
                    Logg.LogWarning($"Invalid or missing data at line {i + 1} in localization file.");
                }
            }
            Logg.Log($"Load localization data for language: {languageCode}", ColorCustom.Green, 15);
        }

        // Helper function to parse a CSV line while handling commas inside quotes
        private string[] ParseCsvLine(string line)
        {
            List<string> result = new List<string>();
            bool inQuotes = false;
            StringBuilder currentColumn = new StringBuilder();

            foreach (char c in line)
            {
                if (c == '"')
                {
                    inQuotes = !inQuotes; // Toggle quotes state
                }
                else if (c == ',' && !inQuotes)
                {
                    // End of a column
                    result.Add(currentColumn.ToString());
                    currentColumn.Clear();
                }
                else
                {
                    currentColumn.Append(c); // Append character to current column
                }
            }

            // Add the last column
            result.Add(currentColumn.ToString());
            return result.ToArray();
        }

        private void SetLanguageDefault()
        {   
            SetLanguage("English");
        }

        public string GetKey(string key)
        {
            if (isSetDefaultLanguage == false)
            {
                Logg.LogError("Please set default language first.");
                return "";
            }

            if (_localizedText.TryGetValue(key, out var key1))
            {
                return key1;
            }
            else
            {
                Logg.LogError($"Key '{key}' not found in localization file.");
                return "";
            }
        }
    }
}