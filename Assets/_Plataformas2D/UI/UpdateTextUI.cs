using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;


public class UpdateTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textUI;

    private void Awake()
    {
        // Fallback if not assigned in the Inspector
        if (textUI == null)
            textUI = GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Updates the text with optional color.
    /// </summary>
    public void UpdateText(string text, Color? color = null)
    {
        textUI.SetText(text);

        // Only apply color if one was provided
        if (color.HasValue)
        {
            textUI.color = color.Value;
        }
    }

    /// <summary>
    /// Overload for float values. Converts the float to string and calls the main UpdateText.
    /// </summary>
    public void UpdateText(float value, Color? color = null)
    {
        UpdateText(value.ToString(), color);
    }
}
