using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using System.Collections;
using TMPro;

public class LanguageDropdownScript : MonoBehaviour
{
    private TMP_Dropdown dropdown;

    void Start()
    {
        // Get the dropdown component
        dropdown = GetComponent<TMP_Dropdown>();
        
        // Add listener for when dropdown value changes
        dropdown.onValueChanged.AddListener(OnLanguageChanged);

        // Optional: Set initial dropdown value based on current locale
        StartCoroutine(InitializeLocale());
    }

    private IEnumerator InitializeLocale()
    {
        // Wait for the localization system to initialize
        yield return LocalizationSettings.InitializationOperation;

        // Get the current locale index and set the dropdown
        int currentLocale = GetLocaleIndex(LocalizationSettings.SelectedLocale.Identifier.Code);
        dropdown.value = currentLocale;
    }

    private void OnLanguageChanged(int index)
    {
        // Change the locale based on dropdown selection
        StartCoroutine(ChangeLocale(index));
    }

    private IEnumerator ChangeLocale(int index)
    {
        // Wait for the localization system to initialize
        yield return LocalizationSettings.InitializationOperation;

        // Change the locale
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        
        // Optional: Save the selected language
        PlayerPrefs.SetInt("SelectedLanguage", index);
        PlayerPrefs.Save();
    }

    private int GetLocaleIndex(string localeCode)
    {
        // Convert locale code to dropdown index
        switch (localeCode.ToLower())
        {
            case "es": return 0; // CASTELLANO
            case "en": return 1; // ENGLISH
            case "ar": return 2; // عربي
            default: return 0;
        }
    }
}