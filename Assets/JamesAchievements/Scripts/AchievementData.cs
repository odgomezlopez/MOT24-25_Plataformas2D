using UnityEngine;
using UnityEngine.UIElements;
using static Unity.Collections.AllocatorManager;


[CreateAssetMenu(fileName = "new AchivementData", menuName = "AchivementData")]
public class AchievementData : ScriptableObject
{
    [SerializeField] public string achievementTitle;
    [SerializeField] public AchievementRarity achivementRarity;
    [SerializeField] public Sprite iconImage;

    [SerializeField] public bool isUnlocked = false;
    
    //TODO Make each Achievement have a different image

    private string AchievementStoreID => $"Achievement_{achievementTitle}";

    public void Load()
    {
        isUnlocked = (PlayerPrefs.HasKey(AchievementStoreID));
    }

    public void Unlock()
    {
        isUnlocked = true;
        PlayerPrefs.SetInt(AchievementStoreID, 0);
    }

    public void ResetAchievement()
    {
        PlayerPrefs.DeleteKey(AchievementStoreID);

    }

}

