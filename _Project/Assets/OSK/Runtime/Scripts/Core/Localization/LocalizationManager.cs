using System.Collections;
using System.Collections.Generic;
using System.IO;
using ExcelDataReader;
using UnityEngine;

/*
 *
 *  // gen for AI: https://x.com/LoloelRolo/status/1221259358585217030

Key	                 	French	                            Vietnamese
welcome_message	        Welcome to the game!	            Chào mưn bạn đến với trò chơi!
exit_prompt	            Are you sure you want to exit?	    Bạn có chắc chắn muốn thoát không?

 World.Localization.SetLanguage("English"); // Or "Vietnamese", etc.
 .text = World.Localization.GetLocalizedString("welcome_message");
*/

 
public class LocalizationManager : GameFrameworkComponent
{
    [SerializeField] private string pathToExcelFile;
    [SerializeField] private string nameTable;

    private Dictionary<string, string> _localizedText;
    private string _currentLanguage;

    public void LoadLocalizationData(string filePath, string nameTable, string languageCode)
    {
        _localizedText = new Dictionary<string, string>();

        // Load the Excel file
        using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet();
                var sheet = result.Tables[nameTable];

                // First row contains column headers (language codes)
                int languageColumnIndex = -1;
                for (int i = 0; i < sheet.Columns.Count; i++)
                {
                    if (sheet.Rows[0][i].ToString().Equals(languageCode, System.StringComparison.OrdinalIgnoreCase))
                    {
                        languageColumnIndex = i;
                        break;
                    }
                }

                if (languageColumnIndex == -1)
                {
                    Debug.LogError("Language code not found in the Excel file.");
                    return;
                }

                for (int i = 1; i < sheet.Rows.Count; i++)
                {
                    string key = sheet.Rows[i][0].ToString();
                    string value = sheet.Rows[i][languageColumnIndex].ToString();

                    _localizedText[key] = value;
                }
            }
        }
    }

    public string GetLocalizedString(string key)
    {
        if (_localizedText.ContainsKey(key))
        {
            return _localizedText[key];
        }
        else
        {
            Debug.LogWarning($"Localization key '{key}' not found.");
            return key; // Fallback to key
        }
    }

    public void SetLanguage(string languageCode)
    {
        // path/to/your/excel/file.xlsx
        LoadLocalizationData(pathToExcelFile, nameTable, languageCode);
        _currentLanguage = languageCode;
    }

    public string GetCurrentLanguage()
    {
        return _currentLanguage;
    }
}
