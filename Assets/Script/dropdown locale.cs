using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class dropdownlocale : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    public string[] optionKeys;

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
        UpdateDropdownOptions();
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    private void OnLocaleChanged(Locale locale)
    {
        UpdateDropdownOptions();
    }

    private void UpdateDropdownOptions()
    {
        dropdown.options.Clear();

        foreach (var key in optionKeys)
        {
            var handle = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("Startup", key);
            handle.Completed += op =>
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData(op.Result));

                // Refresh label after adding
                if (dropdown.options.Count == optionKeys.Length)
                {
                    dropdown.captionText.text = dropdown.options[dropdown.value].text;
                    dropdown.RefreshShownValue();
                }
            };
        }
    }
}
