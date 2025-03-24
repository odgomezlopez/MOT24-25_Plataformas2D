using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Achievement : MonoBehaviour
{

    [Tooltip("Achievement ID")]
    [SerializeField] public AchivementData achivementData;

    [Tooltip("Colors")]
    [SerializeField] private Color lockBackGroundColor = Color.white;
    [SerializeField] private Color lockTextColor = Color.black;


    [SerializeField] private Color unLockBackGroundColor;
    [SerializeField] private Color unLockTextColor;

    [Tooltip("References")]

    [SerializeField] private GameObject description;
    [SerializeField] private Image backgroundImage, image, descriptionBackground;
    [SerializeField] private TextMeshProUGUI descriptionText;
    private bool unlocked;

    public void Init(AchivementData achivementData)
    {
        this.achivementData = achivementData;
        this.unlocked = (PlayerPrefs.HasKey(GetAchievementID()));

        if(unlocked)
        {
            Unlock();
            return;
        }
        else
        {
            Lock();
            return;
        }  
    }

    public string GetAchievementID()
    {
        return achivementData.achievementID;
    }

    public void ManageDescription(bool state)
    {
        //if (!unlocked) return;
        description.SetActive(state);
    }

    public void Lock()
    {
        backgroundImage.color = lockBackGroundColor;
        image.color = lockTextColor;

        descriptionBackground.color = unLockTextColor;
        descriptionText.color = unLockBackGroundColor;
        descriptionText.text = $"({achivementData.achivementRarity.ToString()}) {achivementData.achievementTitle}";
        //unlocked = false;
    }

    public void Unlock()
    {
        backgroundImage.color = unLockBackGroundColor;
        image.color = unLockTextColor;
        descriptionBackground.color = unLockTextColor;
        descriptionText.color = unLockBackGroundColor;
        descriptionText.text = $"({achivementData.achivementRarity.ToString()}) {achivementData.achievementTitle}";
        unlocked = true;
    }
}
