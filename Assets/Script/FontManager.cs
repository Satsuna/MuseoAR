using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;

public class FontManager : MonoBehaviour
{
    [Header("Fonts per Language")]
    public TMP_FontAsset englishFont;
    public TMP_FontAsset japaneseFont;
    public TMP_FontAsset koreanFont;
    public TMP_FontAsset chineseFont;

    [Header("Exceptions (won't be changed)")]
    public TMP_Text exception1;
    public TMP_Text exception2;
    public TMP_Text exception3;

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
        UpdateAllFonts();
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    private void OnLocaleChanged(UnityEngine.Localization.Locale locale)
    {
        UpdateAllFonts();
    }

    private void UpdateAllFonts()
    {
        TMP_FontAsset selectedFont = englishFont;

        switch (LocalizationSettings.SelectedLocale.Identifier.Code)
        {
            case "ja": selectedFont = japaneseFont; break;
            case "ko": selectedFont = koreanFont; break;
            case "zh": selectedFont = chineseFont; break;
        }

        foreach (var text in FindObjectsByType<TMP_Text>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (text == exception1 || text == exception2 || text == exception3)
                continue;

            text.font = selectedFont;
        }
    }
}
