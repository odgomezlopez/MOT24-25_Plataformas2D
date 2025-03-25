using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AchievementCellUI : Selectable, ISubmitHandler
{
    [Header("Achievement Data")]
    [SerializeField] private AchievementData achivementData;

    [Header("UI References")]
    [SerializeField] private GameObject description;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private Image descriptionBackground;
    [SerializeField] private TextMeshProUGUI descriptionText;

    // The combined color set for this rarity
    private AchivementsRarityColors rarityColor;
    AchievementsManager manager;

    // Achievement states
    private bool isHovered;

    /// <summary>
    /// Initialize with data + color from provider.
    /// </summary>
    public void Init(AchievementData data, AchievementsManager manager)
    {
        achivementData = data;
        this.manager = manager;
        rarityColor = manager.uiConfigProvider.GetColorByRarity(achivementData.achivementRarity);

        // Check if unlocked from prior session
        achivementData.Load();

        // Start selected or hovered as false
        isHovered = false;

        // Force an initial refresh
        RefreshUI();
    }

    /// <summary>
    /// Force a re-check of our color state and text
    /// based on locked/unlocked, hovered, or selected.
    /// </summary>
    private void RefreshUI()
    {
        if (rarityColor == null) return;

        // Determine which color set to use
        Color bgColor, textColor;

        if (achivementData.isUnlocked)
        {
            if (isHovered)
            {
                // Unlocked + Hovered
                bgColor = rarityColor.hoverUnlockedBackgroundColor;
                textColor = rarityColor.hoverUnlockedTextColor;
                ManageDescription(true);
            }
            else
            {
                // Unlocked + Normal
                bgColor = rarityColor.unlockedBackgroundColor;
                textColor = rarityColor.unlockedTextColor;
                ManageDescription(false);

            }
        }
        else
        {
            // Achievement is locked
            if (isHovered)
            {
                // Locked + Hovered
                bgColor = rarityColor.hoverLockedBackgroundColor;
                textColor = rarityColor.hoverLockedTextColor;
                ManageDescription(true);

            }
            else
            {
                // Locked + Normal
                bgColor = rarityColor.lockedBackgroundColor;
                textColor = rarityColor.lockedTextColor;
                ManageDescription(false);

            }
        }

        // Apply to the UI
        backgroundImage.color = bgColor;
        iconImage.color = textColor;
        iconImage.sprite = achivementData.iconImage;



        descriptionBackground.color = manager.uiConfigProvider.descriptionBackgroundColor;
        descriptionText.color = manager.uiConfigProvider.descriptionTextColor;

        // Update the text
        descriptionText.fontSize = manager.uiConfigProvider.descriptionTextSize;
        string colorHex = "#" + ColorUtility.ToHtmlStringRGB(manager.uiConfigProvider.descriptionTextSecondaryColor);
        if (achivementData.isUnlocked)
            descriptionText.text = $"{achivementData.achievementTitle}<br><size=100%><color={colorHex}>({achivementData.achivementRarity})</color></size> ";
        else
        {
            descriptionText.text = $"{achivementData.achievementTitle}<br><size=100%><color={colorHex}>(No conseguido)</color></size> ";
        }
    }


    // Show or hide the description UI
    public void ManageDescription(bool state)
    {
        // if (!isUnlocked) return; // Uncomment if you'd like description only for unlocked
        description.SetActive(state);
    }

    /// <summary>
    /// Unlock achievement in this UI (and refresh).
    /// </summary>
    public void Unlock()
    {
        RefreshUI();
    }

    /// <summary>
    /// Lock achievement in this UI (and refresh).
    /// </summary>
    public void Lock()
    {
        RefreshUI();
    }

    #region Select Handlers

    // ------------------------------------------
    // Selectable overrides (optional)
    // ------------------------------------------

    /// <summary>
    /// Called when the UI system selects this element (e.g., via keyboard/gamepad).
    /// </summary>
    public override void OnSelect(BaseEventData eventData)
    {
        isHovered = true;
        RefreshUI();

        base.OnSelect(eventData);
    }

    /// <summary>
    /// Called when the UI system deselects this element.
    /// </summary>
    public override void OnDeselect(BaseEventData eventData)
    {
        isHovered = false;
        RefreshUI();

        base.OnDeselect(eventData);
    }

    /// <summary>
    /// Called when the user moves the selection with keyboard or gamepad (e.g. arrow keys).
    /// </summary>
    public override void OnMove(AxisEventData eventData)
    {
        base.OnMove(eventData);
    }

    // ------------------------------------------
    // Handling 'Submit' (equivalent to a button press)
    // ------------------------------------------

    /// <summary>
    /// 'Submit' happens when user presses Enter/Space/Controller "Submit" while this item is selected.
    /// </summary>
    public void OnSubmit(BaseEventData eventData)
    {
        //Debug.Log($"OnSubmit called on {name}");
    }
    #endregion
}
