using UnityEngine;
using System.Collections;

public static class MoneyUtils {

    public static string[] StringUnits = new string[41] { string.Empty,
        "k",
        "m",
        "b",
        "t",
        "q",
        "Q",
        "u",
        "U",
        "s",
        "S",
        "p",
        "P",
        "o",
        "O",
        "n",
        "N",
        "d",
        "D",
        "g",
        "G",
        "h",
        "H",
        "l",
        "L",
        "i",
        "I",
        "j",
        "J",
        "n",
        "N",
        "c",
        "C",
        "x",
        "X",
        "w",
        "W",
        "y",
        "Y",
        "z",
        "Z"
    };

    public static string GetNewIndex(int index)
    {
        char c = (char)(97 + index / 26);
        char c2 = (char)(97 + index % 26);
        return c.ToString() + c2.ToString();
    }

    public static string ToShortString(this double value)
    {
        if (value < 1.0f)
            return "0";
        string s = value.ToString ("#");
        Debug.Log (s);
        if (s == "Ifinity")
            return "Inf";
        int l = s.Length;
        if(l > 3)
        {
            string e = string.Empty;
            e = s.Substring (0, 3);
            int n = l % 3;
            if (n > 0)
                e = (e.Substring (0, n) + "," + s.Substring (n, 3));
            else
                e += ("," + s.Substring (3, 3));
            int n2 = (l - 1) / 3;
            string e2 = string.Empty;
            e2 = ((n2 >= StringUnits.Length) ? GetNewIndex (n2 - StringUnits.Length) : StringUnits [n2]) ;
            return e + e2;
        }
        return s;
    }
}