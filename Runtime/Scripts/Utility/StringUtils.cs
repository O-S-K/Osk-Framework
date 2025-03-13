using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OSK
{
    public static class StringUtils
    {
        public static string[] Split(string strValue, string splitValue)
        {
            return strValue.Split(new string[] { splitValue }, StringSplitOptions.None);
        }
        public static string[] SplitParams(string strValue)
        {
            return strValue.Split(new char[] { ',' }, StringSplitOptions.None);
        }

        public static List<int[]> GetNumbersFromString(string input)
        {
            List<int[]> resultList = new List<int[]>(); // List to store arrays
            string[] parts = input.Split(',');

            foreach (string part in parts)
            {
                string cleaned = System.Text.RegularExpressions.Regex.Replace(part, @"[^0-9]", "");
                string[] numberStrings = cleaned.Split('n', 'c');
                List<int> numbers = new List<int>();
                foreach (string numString in numberStrings)
                {
                    if (int.TryParse(numString, out int number))
                    {
                        numbers.Add(number);
                    }
                    else
                    {
                        Console.WriteLine($"Cannot convert '{numString}' to integer.");
                    }
                }
                resultList.Add(numbers.ToArray());
            }

            return resultList;
        }

        // public static string[] GetNumbersFromString(string input)
        // {
        //     //  Remove all non-digit characters
        //     string cleaned = System.Text.RegularExpressions.Regex.Replace(input, @"[^0-9,]", "");
        //     string[] numbers = cleaned.Split(',');
        //     return numbers;
        // }

        public static string Shuffle(string characters, int length)
        {
            char[] stringChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                stringChars[i] = characters[UnityEngine.Random.Range(0, characters.Length)];
            }
            return new string(stringChars);
        }
        
        public static string Shuffle(string input)
        {
            char[] characters = input.ToCharArray();
            for (int i = 0; i < characters.Length; i++)
            {
                char temp = characters[i];
                int randomIndex = UnityEngine.Random.Range(0, characters.Length);
                characters[i] = characters[randomIndex];
                characters[randomIndex] = temp;
            }
            return new string(characters);
        }

        public static int ConvertExcelToInt(string input, int numFail = 0)
        {
            string cleanedInput = Regex.Replace(input, @"[^\d.-]", "");
            if (float.TryParse(cleanedInput, out float floatResult))
            {
                return Mathf.RoundToInt(floatResult);
            }
            else
            {
                Debug.LogWarning($"Unable to convert '{cleanedInput}' to a number.");
                return numFail; 
            }
        }

        public static float ConvertExcelToFloat(string input, int numFail = 0)
        {
            string cleanedInput = Regex.Replace(input, @"[^\d.-]", "");

            // Try to parse the cleaned string to a float first
            if (float.TryParse(cleanedInput, out float floatResult))
            {
                return floatResult;
            }
            else
            {
                Debug.LogWarning($"Unable to convert '{cleanedInput}' to a number.");
                return numFail; 
            }
        }

        public static string ShortenString(string input, int length)
        {
            return (input.Length > length) ? input.Substring(0, length) + "..." : input;
        }
    }
}