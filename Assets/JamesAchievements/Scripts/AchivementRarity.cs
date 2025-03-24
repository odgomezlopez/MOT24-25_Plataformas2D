using UnityEngine;


public enum AchivementRarity
{
    Comun,
    Rare,
    UltraRare,
    Legendary
}

public class AchivementsRarityColors
{
    public AchivementsRarityColors(Color rarityBackgroundColor, Color rarityTextColor)
    {
        this.rarityBackgroundColor = rarityBackgroundColor;
        this.rarityTextColor = rarityTextColor;
    }

    [Tooltip("Color del fondo para las distintas rarezas")]
    public Color rarityBackgroundColor = Color.white;
    [Tooltip("Color del texto para las distintas rarezas")]
    public Color rarityTextColor = Color.white;
    [Tooltip("Referencia hexadecimal del color de texto para usar en Text Mesh Pro si se usan iconos")]
    public string rarityTextColorsHex => "#"+ColorUtility.ToHtmlStringRGB(rarityTextColor);
}