using System;
using System.Collections.Generic;
using System.Text;
using CustomInspector;
using UnityEngine; 

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
        [SerializeReference] private Dictionary<string, string> _localizedText = new Dictionary<string, string>();
        [ReadOnly, SerializeField] private List<SystemLanguage> listLanguagesCvs = new List<SystemLanguage>();
        private SystemLanguage currentLanguage = SystemLanguage.English;
 
        private bool isSetDefaultLanguage = false;

        public override void OnInit() {}


        public void SetLanguage(SystemLanguage languageCode)
        {
            isSetDefaultLanguage = true;
            LoadLocalizationData(languageCode);
            currentLanguage = languageCode;
            Logg.Log($"Set language to: {languageCode}", ColorCustom.Green, 15);
        }


        public void SwitchLanguage(SystemLanguage language)
        {
            SetLanguage(language);
            Main.Observer.Notify("UpdateLanguage");
        }

        public void SetLanguageConfigs()
        {
            SetLanguage(Main.Configs.setting.languageDefault);
        }


        public SystemLanguage GetCurrentLanguage => currentLanguage;
        public SystemLanguage[] GetAllLanguages => listLanguagesCvs.ToArray();


        public string GetKey(string key)
        {
            if (isSetDefaultLanguage == false)
            {
                Logg.LogError("Please set default language first.");
                return "";
            }

            if (_localizedText.TryGetValue(key, out var value))
            {
                return value;
            }

            Logg.LogError($"Key '{key}' not found in localization data.");
            return "";
        }


        #region Private

        private void LoadLocalizationData(SystemLanguage languageCode)
        {
            var path = Main.Configs.path.pathLoadFileCsv;
            if (path.StartsWith("Resources/"))
            {
                path = path["Resources/".Length..];
            }
            
            TextAsset textFile = Resources.Load<TextAsset>(path);
            if (textFile == null)
            {
                Logg.LogError("Not found localization file: " + path);
                return;
            }

            // Parse CSV lines while removing empty lines
            string[] lines = textFile.text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Clear the previous localization data
            _localizedText.Clear();

            // Find the index of the language code in the CSV header
            string[] headers = lines[0].Split(',');
            int languageColumnIndex = Array.IndexOf(headers, languageCode.ToString());

            if (languageColumnIndex == -1)
            {
                Logg.LogError($"Language '{languageCode}' not found in localization file.");
                return;
            }

            // Start from the second line to skip the header
            for (int i = 0; i < lines.Length; i++)
            {
                // Handle CSV values with commas or line breaks inside quotes
                string[] columns = ParseCsvLine(lines[i]);

                if (i == 0)
                {
                    GetListLanguageCsv(columns);
                }
                else
                {
                    GetValueFormLanguage(columns, languageColumnIndex, i);
                }
            }

            Logg.Log($"Load localization data for language: {languageCode}", ColorCustom.Green, 15);
        }

        private void GetValueFormLanguage(string[] columns, int languageColumnIndex, int i)
        {
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

        private void GetListLanguageCsv(string[] columns)
        {
            for (int j = 0; j < columns.Length; j++)
            {
                if (Enum.TryParse(columns[j], out SystemLanguage language))
                {
                    if (!listLanguagesCvs.Contains(language))
                        listLanguagesCvs.Add(language);
                }
            }
        }

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

        #endregion
    }
}