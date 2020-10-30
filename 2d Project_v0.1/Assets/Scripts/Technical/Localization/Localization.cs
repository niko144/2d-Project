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
        //Initialize localization data
        language = langCode;
        TextAsset localizationData;
        localizationData = Resources.Load<TextAsset>("LocalizationData/localizationData");
        string[] line = localizationData.text.Split(new char[] { '\n' });

        //Find row and line sizes and set strings in array accordingly
        for (int y = 0; y < line.Length - 1; y++)
        {
            row = line[y].Split(new char[] { ';' });
            wordlist = new string[row.Length, line.Length];
        }

        //Assign each word to its own dimension in 2D array
        for (int y = 0; y < line.Length - 1; y++)
        {
            row = line[y].Split(new char[] { ';' });
            for (int x = 0; x < row.Length; x++)
            {
                wordlist[x, y] = row[x];
            }
        }
    }

    //Method to search for translated word
    public static string GetStringForLanguage(string textToGet)
    {
        int x = -1;
        int y = -1;

        //Search for the specified target-language on the Y-axis
        for (int i = 0; i < wordlist.GetLength(0); i++)
        {
            if (wordlist[i, 0].Contains(language))
            {
                x = i;
            }
        }

        //Search for the specified text string on the X-axis
        for (int i = 0; i < wordlist.GetLength(1); i++)
        {
            if (wordlist[0, i] == textToGet)
            {
                y = i;
            }
        }

        //Return the word from the search result, if it was found. Otherwise return original string and print error to console
        if (x > -1 && y > -1)
        {
            return wordlist[x, y];
        }
        else
        {
            Debug.LogError("Localization data for the given string or language could not be found. Make sure that the text string and language code exists in the localization ressource file. NOTE: The input is case-sensitive!");
            return textToGet;
        }
    }
}