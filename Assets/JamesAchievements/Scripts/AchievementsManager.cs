using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;



public class AchievementsManager : MonoBehaviourSingleton<AchievementsManager>
{
    [SerializeField] private List<AchivementData> achievements;

    [Header("Achivements UI Config")]


    [SerializeField] private GameObject achivementCellPrefab;
    [SerializeField] private List<Achievement> cells;


    [SerializeField] Dictionary<AchivementRarity, AchivementsRarityColors> achivementsRarities = new Dictionary<AchivementRarity, AchivementsRarityColors>{
        {AchivementRarity.Comun,new AchivementsRarityColors(Color.white, Color.black)},
        {AchivementRarity.Rare,new AchivementsRarityColors(Color.green, Color.white)},
        {AchivementRarity.UltraRare,new AchivementsRarityColors(Color.blue, Color.white)},
        {AchivementRarity.Legendary,new AchivementsRarityColors(Color.magenta, Color.white) }
    };


    [SerializeField] private AudioSource unlockAchievementAudio;

    [Header("Animators")]
    [Min(1)]
    [SerializeField] private float animatorSpeed;
    [SerializeField] private Animator achievementAnimator;
    [SerializeField] private Animator achievementsAnimator;

    [SerializeField] private GameObject aSVContent;

    [SerializeField] private KeyCode achievementsMenuKey;
    [SerializeField] private TextMeshProUGUI achievementText;
    [SerializeField] private Image achievementBackground;
    private bool usingAchievementsMenu;



    /*
     * Se usan PlayerPrefs para guardar los distintos logros desbloqueados, se puede cambiar a cualquier otro sistema de serialización de datos.
     * Se pasa el ScriptableObject del logro
     */

    public void UnlockAchievement(AchivementData ach)
    {
        if (PlayerPrefs.HasKey(ach.achievementID))
            return;
        else
        {
            unlockAchievementAudio.Play();
            achievementBackground.color = achivementsRarities[ach.achivementRarity].rarityBackgroundColor;
            achievementText.color = achivementsRarities[ach.achivementRarity].rarityTextColor;
            achievementText.text = $"<sprite index=0 color={achivementsRarities[ach.achivementRarity].rarityTextColorsHex}>{ach.achievementID}<sprite index=0 color={achivementsRarities[ach.achivementRarity].rarityTextColorsHex}>";
            achievementAnimator.SetTrigger("unlockAchievement");
            PlayerPrefs.SetInt(ach.achievementID, 0);
            for (int i = 0; i < cells.Count; ++i)
            {
                if (PlayerPrefs.HasKey(cells[i].GetAchievementID()))
                {
                    cells[i].Unlock();
                }
            }
        }
    }

    //Delete all achivements obtained
    public void Reset()
    {
        foreach(var a in cells)
        {
            PlayerPrefs.DeleteKey(a.GetAchievementID());
        }
    }

    private void Start()
    {
        achievementsAnimator.SetFloat("speed", animatorSpeed);
        for (int i=0; i<achievements.Count; ++i)
        {
            GameObject g = Instantiate(achivementCellPrefab, aSVContent.transform);
            Achievement a = g.GetComponent<Achievement>();
            if (a == null)
            {
                Debug.LogWarning("The archivement cell prefab should have an Achivement Component");
                return;
            }
            a.Init(achievements[i]);
            cells.Add(a);
        }
    }

    private void Update()
    {
        if (!usingAchievementsMenu && Input.GetKeyUp(achievementsMenuKey))
        {
            ShowAchivements();
            return;
        }
        if (usingAchievementsMenu && Input.GetKeyUp(achievementsMenuKey))
        {

            HideAchivements();
            return;
        }
    }

    private void ShowAchivements()
    {
        Time.timeScale = 0;
        achievementsAnimator.SetTrigger("open");

        StartCoroutine(ExecuteAfterDelay(0, () =>
        {
            usingAchievementsMenu = true;
        }));
    }

    private void HideAchivements()
    {
        Time.timeScale = 1;
        achievementsAnimator.SetTrigger("close");

        StartCoroutine(ExecuteAfterDelay(0, () =>
        {
            usingAchievementsMenu = false;
        }));
    }

    private IEnumerator ExecuteAfterDelay(float delay, System.Action action)
    {
        yield return new WaitForSecondsRealtime(delay);
        action?.Invoke();
    }


}


#if UNITY_EDITOR
    [CustomEditor(typeof(AchievementsManager)), CanEditMultipleObjects]
    public class AchievementsManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        AchievementsManager achievementsManager = (AchievementsManager)target;

        //ZONA MODIFICABLE
        if (GUILayout.Button("Reset"))
            {
                achievementsManager.Reset();
            }
            //FIN ZONA MODIFICIABLE
        }
    }
#endif
