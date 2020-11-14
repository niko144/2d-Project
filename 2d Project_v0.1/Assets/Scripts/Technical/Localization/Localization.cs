using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localization
{
    private static string[,] wordlist;
    public static string language = "da";

    private static string[] row;

    public static void InitializeData(string langCode)
    {
        Printer.Print("init");

        language = langCode;
        TextAsset localizationData;
        localizationData = Resources.Load<TextAsset>("LocalizationData/localizationData");
        string[] line = localizationData.text.Split(new char[] { '\n' });

        for (int y = 0; y < line.Length - 1; y++)
        {
            row = line[y].Split(new char[] { ';' });
            wordlist = new string[row.Length, line.Length];
        }

        for (int y = 0; y < line.Length - 1; y++)
        {
            row = line[y].Split(new char[] { ';' });
            for (int x = 0; x < row.Length; x++)
            {
                wordlist[x, y] = row[x];
            }
        }
    }

    public static string GetStringForLanguage(string textToGet)
    {
        int x = -1;
        int y = -1;

        for (int i = 0; i < wordlist.GetLength(0); i++)
        {
            if (wordlist[i, 0].Contains(language))
            {
                x = i;
            }
        }

        for (int i = 0; i < wordlist.GetLength(1); i++)
        {
            if (wordlist[0, i] == textToGet)
            {
                y = i;
            }
        }

        if (x > -1 && y > -1)
        {
            return wordlist[x, y];
        }
        else
        {
            Printer.Throw("Localization data for the given string or language could not be found. Make sure that the text string and language code exists in the localization ressource file. NOTE: The input is case-sensitive!");
            return textToGet;
        }
    }
}