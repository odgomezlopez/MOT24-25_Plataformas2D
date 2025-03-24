using UnityEngine;


[CreateAssetMenu(fileName = "new AchivementData", menuName = "AchivementData")]
public class AchivementData : ScriptableObject
{
    [SerializeField] public string achievementID;
    [SerializeField] public string achievementTitle;
    [SerializeField] public AchivementRarity achivementRarity;


}

