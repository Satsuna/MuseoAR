using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocaleSelectorCamera : MonoBehaviour
{
    private bool active = false;

    private void Start()
    {
        int savedLocale = PlayerPrefs.GetInt("LocaleKey", 0);
        StartCoroutine(SetLocale(savedLocale));
        Debug.Log("Camera scene locale loaded: " + savedLocale);
    }

    public void ChangeLocale(int localeID)
    {
        if (active) return;
        PlayerPrefs.SetInt("LocaleKey", localeID);
        PlayerPrefs.Save();
        StartCoroutine(SetLocale(localeID));
        Debug.Log("Camera scene locale changed to: " + localeID);
    }

    private IEnumerator SetLocale(int localeID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;

        if (localeID >= 0 && localeID < LocalizationSettings.AvailableLocales.Locales.Count)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        }
        else
        {
            Debug.LogWarning("Locale ID out of range: " + localeID);
        }

        active = false;
    }
}
