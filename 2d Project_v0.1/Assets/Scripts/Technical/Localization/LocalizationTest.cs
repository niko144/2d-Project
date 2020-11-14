using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationTest : MonoBehaviour
{
    public string wordToGet = "Options";

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Printer.Print(Localization.GetStringForLanguage(wordToGet));
        }
    }
}
