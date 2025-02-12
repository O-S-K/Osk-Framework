using System;
using UnityEngine;
using System.Collections;
using System.Globalization;
using System.Numerics;
using OSK;
using TMPro;
using UnityEngine.UI;

// https://github.com/DarkNaku/Number
// https://ConvertNumberToWords.com/
public static class CurrencyUtils
{
    private static readonly string[] Suffixes =
    {
        "", "k", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc", "Ud", "Dd", "Td", "Qad", "Qid",
        "Sxd", "Spd", "Od", "Nd", "V", "Uv", "Dv", "Tv", "Qav", "Qiv", "Sxv", "Spv", "Ov", "Nv",
        "Tg", "Utg", "Dtg", "Ttg", "Qatg", "Qitg", "Sxtg", "Sptg", "Otg", "Ntg"
    };

    public static string FormatCurrency<T>(T number, string format = "0.00") where T : struct
    {
        double num = Convert.ToDouble(number);
        return num switch
        {
            >= 1e18 => (num / 1e18).ToString(format) + "E",
            >= 1e15 => (num / 1e15).ToString(format) + "Q",
            >= 1e12 => (num / 1e12).ToString(format) + "T",
            >= 1e9 => (num / 1e9).ToString(format) + "B",
            >= 1e6 => (num / 1e6).ToString(format) + "M",
            >= 1e3 => (num / 1e3).ToString(format) + "K",
            _ => num.ToString(CultureInfo.InvariantCulture)
        };
    }

    public static string FormatCurrency(int number)
    {
        return number switch
        {
            >= 1000000000 => (number / 1000000000f).ToString("0.#") + "B",
            >= 1000000 => (number / 1000000f).ToString("0.#") + "M",
            >= 1000 => (number / 1000f).ToString("0.#") + "K",
            _ => number.ToString()
        };
    }

    public static string FormatCurrency(float number)
    {
        // url: https://en.wikipedia.org/wiki/Names_of_large_numbers
        if (number >= 1000000000)
        {
            return (number / 1000000000).ToString() + "B";
        }

        if (number >= 1000000)
        {
            return (number / 1000000).ToString() + "M";
        }

        if (number >= 100000)
        {
            return (number / 1000).ToString() + "K";
        }

        return number.ToString();
    }
 
    public static IEnumerator FillCurrencyText<T>(TextMeshProUGUI text, T current, T target,
        string format = "0.00", float delay = 0, float time = 1, Action onCompleted = null) where T : struct
    {
        yield return FillTextTo(elapsedTime =>
        {
            T value = LerpValue(current, target, elapsedTime / time);
            text.text =  FormatCurrency(value, format);
        }, delay, time, onCompleted);
    }

    private static IEnumerator FillTextTo(Action<float> updateText, float delay, float time, Action onCompleted)
    {
        yield return new WaitForSeconds(delay);
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            updateText(elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        updateText(elapsedTime);
        onCompleted?.Invoke();
    }

    private static T LerpValue<T>(T a, T b, float t) where T : struct
    {
        if (typeof(T) == typeof(int))
            return (T)(object)Mathf.RoundToInt(Mathf.Lerp(Convert.ToSingle(a), Convert.ToSingle(b), t));
        if (typeof(T) == typeof(float))
            return (T)(object)Mathf.Lerp((float)(object)a, (float)(object)b, t);
        if (typeof(T) == typeof(long))
            return (T)(object)(long)Mathf.Lerp(Convert.ToSingle(a), Convert.ToSingle(b), t);
        throw new ArgumentException("Unsupported type for LerpValue");
    }
}