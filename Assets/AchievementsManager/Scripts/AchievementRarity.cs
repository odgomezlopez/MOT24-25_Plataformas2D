using System.Collections.Generic;
using UnityEngine;


public enum AchievementRarity
{
    Comun,
    Rare,
    UltraRare,
    Legendary
}


[System.Serializable]
public class AchivementsRarityColors
{
    // Constructor for convenience if you like to set them in code
    public AchivementsRarityColors(
        // unlocked
        Color unlockedBackgroundColor, Color unlockedTextColor,
        // locked
        Color lockedBackgroundColor, Color lockedTextColor,
        // hover (unlocked)
        Color hoverUnlockedBackgroundColor, Color hoverUnlockedTextColor,
        // hover (locked)
        Color hoverLockedBackgroundColor, Color hoverLockedTextColor
    )
    {
        this.unlockedBackgroundColor = unlockedBackgroundColor;
        this.unlockedTextColor = unlockedTextColor;
        this.lockedBackgroundColor = lockedBackgroundColor;
        this.lockedTextColor = lockedTextColor;
        this.hoverUnlockedBackgroundColor = hoverUnlockedBackgroundColor;
        this.hoverUnlockedTextColor = hoverUnlockedTextColor;
        this.hoverLockedBackgroundColor = hoverLockedBackgroundColor;
        this.hoverLockedTextColor = hoverLockedTextColor;
    }

    [Header("Unlocked Colors")]
    public Color unlockedBackgroundColor = Color.white;
    public Color unlockedTextColor = Color.black;

    [Header("Locked Colors")]
    public Color lockedBackgroundColor = Color.gray;
    public Color lockedTextColor = Color.gray;

    [Header("Hover (Unlocked)")]
    public Color hoverUnlockedBackgroundColor = new Color(1f, 1f, 1f, 0.7f);
    public Color hoverUnlockedTextColor = new Color(0f, 0f, 0f, 0.8f);

    [Header("Hover (Locked)")]
    public Color hoverLockedBackgroundColor = new Color(0.5f, 0.5f, 0.5f, 0.7f);
    public Color hoverLockedTextColor = new Color(0.2f, 0.2f, 0.2f, 1f);

    [Header("If needed for TMP Icons")]
    [Tooltip("Hex reference for the UNLOCKED text color (commonly used with <sprite> tags in TMP)")]
    public string rarityTextColorsHex => "#" + ColorUtility.ToHtmlStringRGB(unlockedTextColor);
}


[System.Serializable]
public class AchivementUIProvider
{
    [SerializeField] public Color descriptionTextColor = Color.white;
    [SerializeField] public Color descriptionTextSecondaryColor = Color.gray;
    [SerializeField] public float descriptionTextSize = 40;
    [SerializeField] public Color descriptionBackgroundColor = Color.black;
    
    [SerializeField]
    private AchivementsRarityColors comun = new AchivementsRarityColors(
        // UNLOCKED            // LOCKED            // HOVER UNLOCKED         // HOVER LOCKED
        Color.white, Color.black,
        Color.gray, Color.black,
        new Color(1f, 1f, 1f, 0.7f), new Color(0f, 0f, 0f, 0.8f),
        new Color(0.5f, 0.5f, 0.5f, 0.7f), new Color(0.2f, 0.2f, 0.2f, 1f)
    );

    [SerializeField]
    private AchivementsRarityColors rare = new AchivementsRarityColors(
        Color.green, Color.white,
        new Color(0.3f, 0.4f, 0.3f), Color.gray,
        new Color(0.5f, 1f, 0.5f, 0.6f), Color.white,
        new Color(0.35f, 0.35f, 0.35f, 0.7f), new Color(0.2f, 0.2f, 0.2f, 1f)
    );

    [SerializeField]
    private AchivementsRarityColors ultraRare = new AchivementsRarityColors(
        Color.blue, Color.white,
        new Color(0.2f, 0.2f, 0.4f), Color.gray,
        new Color(0.4f, 0.4f, 1f, 0.6f), Color.white,
        new Color(0.4f, 0.4f, 0.4f, 0.7f), new Color(0.2f, 0.2f, 0.2f, 1f)
    );

    [SerializeField]
    private AchivementsRarityColors legendary = new AchivementsRarityColors(
        Color.magenta, Color.white,
        new Color(0.4f, 0.2f, 0.4f), Color.gray,
        new Color(1f, 0.5f, 1f, 0.6f), Color.white,
        new Color(0.4f, 0.4f, 0.4f, 0.7f), new Color(0.2f, 0.2f, 0.2f, 1f)
    );

    public AchivementsRarityColors GetColorByRarity(AchievementRarity rarity)
    {
        switch (rarity)
        {
            case AchievementRarity.Comun:
                return comun;
            case AchievementRarity.Rare:
                return rare;
            case AchievementRarity.UltraRare:
                return ultraRare;
            case AchievementRarity.Legendary:
                return legendary;
            default:
                return comun; // fallback
        }
    }

}