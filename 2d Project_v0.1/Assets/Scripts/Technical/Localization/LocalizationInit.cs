using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationInit : MonoBehaviour
{
    public string language = "da";

    void Start()
    {
        Localization.InitializeData(language);
    }
}
